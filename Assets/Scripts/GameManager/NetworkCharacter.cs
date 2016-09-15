using UnityEngine;
using System.Collections;

public class NetworkCharacter : Photon.MonoBehaviour
{
    public float speed = 10f;
    public float jumpSpeed = 6f;
    public float rotationSensitivity = 3f;
    public Camera cam;

    [System.NonSerialized]
    public Vector3 direction = Vector3.zero;
    [System.NonSerialized]
    public Vector3 rotation = Vector3.zero;
    [System.NonSerialized]
    public Vector3 cameraRotation = Vector3.zero;
    [System.NonSerialized]
    public bool isJumping = false;

    float verticalVelocity = 0;

    Vector3 realPosition = Vector3.zero;
    Quaternion realRotation = Quaternion.identity;

    Animator anim;

    bool gotFirstUpdate = false;

    CharacterController cc;

    // Shooting
    FXManager fxManager;
    public float fireRate = 0.5f;
    public float damage = 25f;
    float coolddown = 0;

    void Start()
    {
        CacheComponents();
    }

    void Update()
    {
        coolddown -= Time.deltaTime;
    }

    void FixedUpdate()
    {
        if (photonView.isMine)
        {
            DoLocalMovement();
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, realPosition, 0.1f);
            transform.rotation = Quaternion.Lerp(transform.rotation, realRotation, 0.1f);
        }
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        CacheComponents();

        if (stream.isWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(anim.GetFloat("Speed"));
            stream.SendNext(anim.GetBool("Resting"));
        }
        else
        {
            realPosition = (Vector3)stream.ReceiveNext();
            realRotation = (Quaternion)stream.ReceiveNext();
            anim.SetFloat("Speed", (float)stream.ReceiveNext());
            anim.SetBool("Resting", (bool)stream.ReceiveNext());

            if (gotFirstUpdate == false)
            {
                transform.position = realPosition;
                transform.rotation = realRotation;
                gotFirstUpdate = true;
            }
        }
    }

    void CacheComponents()
    {
        if (anim == null)
        {
            anim = GetComponent<Animator>();
            if (anim == null)
                Debug.LogError("Player Prefab have no Animator");

            cc = GetComponent<CharacterController>();
            if (cc == null)
                Debug.LogError("Player Prefab have no CharacterController");

            fxManager = GameObject.FindObjectOfType<FXManager>();
            if (fxManager == null)
                Debug.LogError("Player Prefab have no FxManager");
        }
    }

    void DoLocalMovement()
    {
        Vector3 distance = direction * speed * Time.deltaTime;

        if (isJumping)
        {
            isJumping = false;
            if (cc.isGrounded)
                verticalVelocity = jumpSpeed;
        }

        if (cc.isGrounded && verticalVelocity < 0)
            verticalVelocity = Physics.gravity.y * Time.deltaTime;
        else
            verticalVelocity += Physics.gravity.y * Time.deltaTime;

        distance.y = verticalVelocity * Time.deltaTime;

        anim.SetFloat("Speed", direction.magnitude);

        cc.Move(distance);

        rotation *= rotationSensitivity;
        cameraRotation *= rotationSensitivity;

        transform.Rotate(rotation);


        if (cam != null)
        {
            if (cam.transform.eulerAngles.x - cameraRotation.x > 340 ||
                cam.transform.eulerAngles.x - cameraRotation.x < 30)
            {
                cam.transform.Rotate(-cameraRotation);
            }
        }
    }

    public void FireWeapon(Vector3 orig, Vector3 dir)
    {
        if (coolddown > 0)
            return;

        Debug.Log("Firing!");

        Ray ray = new Ray(orig, dir);
        Transform hitTransform;
        Vector3 hitPoint;

        hitTransform = FindClosestHitObject(ray, out hitPoint);

        if (hitTransform != null)
        {
            Debug.Log("We hit: " + hitTransform.name);
            // DoRicochetEffectAt( hitPoint ); Some Special Effects

            Health h = hitTransform.GetComponent<Health>();

            while (h == null && hitTransform.parent)
            {
                hitTransform = hitTransform.parent;
                h = hitTransform.GetComponent<Health>();
            }

            if (h != null)
            {
                PhotonView pv = h.GetComponent<PhotonView>();
                if (pv == null)
                {
                    Debug.Log("Target have no PhotonView component !");
                }
                else
                {
                    h.GetComponent<PhotonView>().RPC("TakeDamage", PhotonTargets.AllBuffered, damage);
                }
            }
            if (fxManager != null)
            {
                DoGunFX(hitPoint);
            }
        }
        else
        {
            if (fxManager != null)
            {
                hitPoint = Camera.main.transform.position + (Camera.main.transform.forward * 100f);
                DoGunFX(hitPoint);
            }
        }
        coolddown = fireRate;
    }

    Transform FindClosestHitObject(Ray ray, out Vector3 hitPoint)
    {
        RaycastHit[] hits = Physics.RaycastAll(ray);

        Transform closestHit = null;
        float distance = 0;
        hitPoint = Vector3.zero;

        foreach (var hit in hits)
        {
            if (hit.transform != this.transform && (closestHit == null || hit.distance < distance))
            {
                closestHit = hit.transform;
                distance = hit.distance;
                hitPoint = hit.point;
            }
        }

        return closestHit;
    }

    void DoGunFX(Vector3 hitPoint)
    {
        fxManager.GetComponent<PhotonView>().RPC("BulletFX", PhotonTargets.All, Camera.main.transform.position, hitPoint);
    }
}