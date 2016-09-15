using UnityEngine;
using System.Collections;
using CnControls;

public class PlayerMovementBackup : MonoBehaviour
{
    [SerializeField]
    private Camera cam;
    [SerializeField]
    private float speed = 10f;
    [SerializeField]
    private float rotationSensitivity = 3f;
    [SerializeField]
    private float JumpSpeed = 20f;


    Vector3 direction = Vector3.zero;
    Vector3 rotation = Vector3.zero;
    Vector3 cameraRotation = Vector3.zero;
    private float verticalVelocity = 0;

    CharacterController cc;
    Animator anim;

    // Use this for initialization
    void Start()
    {
        cc = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        direction = transform.rotation * new Vector3(CnInputManager.GetAxis("Horizontal"), 0, CnInputManager.GetAxis("Vertical"));

        if (direction.magnitude > 1)
            direction = direction.normalized;

        rotation = new Vector3(0f, CnInputManager.GetAxis("Mouse X"), 0f) * rotationSensitivity;
        cameraRotation = new Vector3(CnInputManager.GetAxis("Mouse Y"), 0f, 0f) * rotationSensitivity;

        anim.SetFloat("Speed", CnInputManager.GetAxis("Vertical"));

        if (cc.isGrounded && CnInputManager.GetButton("Jump"))
            verticalVelocity = JumpSpeed;
    }

    void FixedUpdate()
    {
        Vector3 distance = direction * speed * Time.deltaTime;

        if (cc.isGrounded && verticalVelocity < 0)
            verticalVelocity = Physics.gravity.y * Time.deltaTime;
        else
            verticalVelocity += Physics.gravity.y * Time.deltaTime;

        distance.y = verticalVelocity * Time.deltaTime;

        cc.Move(distance);
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
}