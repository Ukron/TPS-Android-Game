using UnityEngine;
using System.Collections;
using CnControls;

public class PlayerController : MonoBehaviour
{
    NetworkCharacter netChar;
    SimpleJoystick shootingStick;

    //CharacterController cc;
    //Animator anim;

    // Use this for initialization
    void Start()
    {
        //cc = GetComponent<CharacterController>();
        //anim = GetComponent<Animator>();

        netChar = GetComponent<NetworkCharacter>();
        SimpleJoystick[] sj = GameObject.FindObjectsOfType<SimpleJoystick>();
        foreach (var joystick in sj)
        {
            if (joystick.name.Equals("ShootJoystick"))
                shootingStick = joystick;
        }
        if (shootingStick == null)
            Debug.LogError("ShootJoystick == null");
    }

    // Update is called once per frame
    void Update()
    {
        netChar.direction = transform.rotation * new Vector3(CnInputManager.GetAxis("Horizontal"), 0, CnInputManager.GetAxis("Vertical"));

        if (netChar.direction.magnitude > 1f)
            netChar.direction = netChar.direction.normalized;

        netChar.rotation = new Vector3(0f, CnInputManager.GetAxis("Mouse X"), 0f);
        netChar.cameraRotation = new Vector3(CnInputManager.GetAxis("Mouse Y"), 0f, 0f);

        if (CnInputManager.GetButton("Jump"))
        {
            netChar.isJumping = true;
        }
        else
        {
            netChar.isJumping = false;
        }
        // Shoot
        if (shootingStick.isPushing)
            netChar.FireWeapon(Camera.main.transform.position, Camera.main.transform.forward);

        //netChar.anim.SetFloat("Speed", CnInputManager.GetAxis("Vertical"));

        //if (cc.isGrounded && CnInputManager.GetButton("Jump"))
        //    verticalVelocity = JumpSpeed;
    }

    //void FixedUpdate()
    //{
    //    Vector3 distance = direction * speed * Time.deltaTime;

    //    if (cc.isGrounded && verticalVelocity < 0)
    //        verticalVelocity = Physics.gravity.y * Time.deltaTime;
    //    else
    //        verticalVelocity += Physics.gravity.y * Time.deltaTime;

    //    distance.y = verticalVelocity * Time.deltaTime;

    //    cc.Move(distance);
    //    transform.Rotate(rotation);

    //    if (cam != null)
    //    {
    //        if (cam.transform.eulerAngles.x - cameraRotation.x > 340 ||
    //            cam.transform.eulerAngles.x - cameraRotation.x < 30)
    //        {
    //            cam.transform.Rotate(-cameraRotation);
    //        }
    //    }
    //}
}