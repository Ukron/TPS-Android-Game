using UnityEngine;
using System.Collections;
using CnControls;

public class JoystickSizeManager : MonoBehaviour
{
    public float joystickSize;
    public float joystickRangeMultiplier = 1;


    // Use this for initialization
    void Start()
    {
        joystickSize = Screen.width >= Screen.height ? Screen.height / 3.5f : Screen.width / 3.5f;
        joystickSize /= 2;

        gameObject.transform.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(joystickSize, joystickSize);
        gameObject.GetComponent<SimpleJoystick>().MovementRange = joystickSize * joystickRangeMultiplier;
    }
}
