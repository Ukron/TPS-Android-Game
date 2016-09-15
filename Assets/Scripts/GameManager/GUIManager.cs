using UnityEngine;
using System.Collections;
using CnControls;

public class GUIManager : MonoBehaviour
{
    private SimpleJoystick[] joysticks;

    // Use this for initialization
    void Start()
    {
        joysticks = GameObject.FindObjectsOfType<SimpleJoystick>();

        float joystickSize = Screen.width >= Screen.height ? Screen.height / 3f : Screen.width / 3f;

        foreach (var item in joysticks)
        {
            if (!item.enabled)
            {
                Debug.Log("Joystick");
                item.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(joystickSize, joystickSize);
                item.transform.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(joystickSize, joystickSize);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
