using CnControls;
using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerControllerOld : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private float rotationSensitivity = 3f;
    [SerializeField]
    private float jumpSpeed = 5f;

    private PlayerMotor motor;

    private SimpleJoystick leftJoystick;
    private SimpleJoystick rightJoystick;

    private float groundDistance = 0;
    private float verticalVelocity = 0;
    private Vector3 velocity = Vector3.zero;
    private Vector3 rotation = Vector3.zero;
    private Vector3 cameraRotation = Vector3.zero;

    void Start()
    {
        motor = GetComponent<PlayerMotor>();

        //foreach (var item in GameObject.FindObjectsOfType<SimpleJoystick>())
        //{
        //    if (item.transform.name == "LeftVirtualJoystick")
        //        leftJoystick = item;
        //    if (item.transform.name == "RightVirtualJoystick")
        //        rightJoystick = item;
        //}
    }


    void Update()
    {
        float xMov = CnInputManager.GetAxis("Horizontal");
        float zMov = CnInputManager.GetAxis("Vertical");

        //float xMov = Input.GetAxis("Horizontal");
        //float zMov = Input.GetAxis("Vertical");

        Vector3 movHorizontal = transform.right * xMov;
        Vector3 movVertical = transform.forward * zMov;

        // final movement vector
        velocity = (movHorizontal + movVertical).normalized * speed;



        // rotation
        float yRot = CnInputManager.GetAxis("Mouse X"); //rightJoystick.Horizontal();
        rotation = new Vector3(0f, yRot, 0f) * rotationSensitivity;


        // Camera rotation
        float xRot = CnInputManager.GetAxis("Mouse Y"); //rightJoystick.Vertical();
        cameraRotation = new Vector3(xRot, 0f, 0f) * rotationSensitivity;


        if (IsGrounded() && Input.GetButton("Jump"))
            verticalVelocity = jumpSpeed;
    }
    void FixedUpdate()
    {
        motor.Move(velocity);
        motor.Rotate(rotation);
        motor.RotateCamera(cameraRotation);

        if (IsGrounded() && verticalVelocity < 0)
        {
            verticalVelocity = Physics.gravity.y * Time.deltaTime;
        }
        else
        {
            verticalVelocity += Physics.gravity.y * Time.deltaTime;
        }
        velocity.y += verticalVelocity * Time.deltaTime;
    }


    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, groundDistance + 0.1f);
    }
}
