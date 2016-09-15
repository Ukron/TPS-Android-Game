using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour
{
    [SerializeField]
    private Camera cam;

    private Vector3 velocity = Vector3.zero;
    private Vector3 rotation = Vector3.zero;
    private Vector3 cameraRotation = Vector3.zero;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // get movement vector
    public void Move(Vector3 _velocity)
    {
        velocity = _velocity;
    }
    // get rotation vector
    public void Rotate(Vector3 _rotation)
    {
        rotation = _rotation;
    }
    // get camera rotation vector
    public void RotateCamera(Vector3 _cameraRotation)
    {
        cameraRotation = _cameraRotation;
    }

    void FixedUpdate()
    {
        PerformMovement();
        PerformRotation();
    }

    void PerformMovement()
    {
        if (velocity != Vector3.zero)
        {
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        }
    }
    void PerformRotation()
    {
        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));

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
