using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CnControls;

public class PlayerShooting : MonoBehaviour
{
    public float fireRate = 0.5f;
    public float damage = 25f;
    float cooldown = 0;
    SimpleJoystick shootingStick;
    FXManager fxManager;

    void Start()
    {
        SimpleJoystick[] sj = GameObject.FindObjectsOfType<SimpleJoystick>();
        fxManager = GameObject.FindObjectOfType<FXManager>();
        foreach (var joystick in sj)
        {
            if (joystick.name.Equals("ShootJoystick"))
                shootingStick = joystick;
        }
        if (shootingStick == null)
            Debug.LogError("RightStick == null");
        if (fxManager == null)
            Debug.LogError("fxManager == null");
    }

    // Update is called once per frame
    void Update()
    {
        cooldown -= Time.deltaTime;

        if (shootingStick.isPushing)
            Fire();
    }

    void Fire()
    {
        if (cooldown > 0)
            return;

        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward); // изменить начальную точку
        Transform hitTransform;
        Vector3 hitPoint;

        hitTransform = FindClosesHitInfo(ray, out hitPoint);

        if (hitTransform != null)
        {
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
                    Debug.LogError("Fire() - h != null -- pv == null");
                }
                else
                {
                    h.GetComponent<PhotonView>().RPC("TakeDamage", PhotonTargets.AllBuffered, damage);  //  <--  ==  h.TakeDamage(damage);
                }
            }
            if (fxManager != null)
            {
                fxManager.GetComponent<PhotonView>().RPC("BulletFX", PhotonTargets.All, Camera.main.transform.position, hitPoint);
            }
        }
        else
        {
            if (fxManager != null)
            {
                hitPoint = Camera.main.transform.position + (Camera.main.transform.forward * 100f);
                fxManager.GetComponent<PhotonView>().RPC("BulletFX", PhotonTargets.All, Camera.main.transform.position, hitPoint);
            }
        }
        cooldown = fireRate;

    }

    Transform FindClosesHitInfo(Ray ray, out Vector3 hitPoint)
    {
        RaycastHit[] hits = Physics.RaycastAll(ray);

        Transform closesHit = null;
        float distance = 0;
        hitPoint = Vector3.zero;

        foreach (RaycastHit hit in hits)
        {
            if (hit.transform != this.transform && (closesHit == null || hit.distance < distance))
            {
                closesHit = hit.transform;
                distance = hit.distance;
                hitPoint = hit.point;
            }
        }
        return closesHit;
    }
}
