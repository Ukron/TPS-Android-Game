using UnityEngine;
using System.Collections;
using CnControls;

public class GUISizeManager : MonoBehaviour
{
    public float Sizer;
    public float joystickRangeMultiplier = 1;
    public float ButtonSizeMultiplier = 1;
    // Use this for initialization
    void Start()
    {
        Sizer = Screen.width >= Screen.height ? Screen.height / 3.5f : Screen.width / 3.5f;
        Sizer /= 2;

        SimpleJoystick sj = gameObject.GetComponent<SimpleJoystick>();
        SimpleButton sb = gameObject.GetComponent<SimpleButton>();
        if (sj != null)
        {
            gameObject.transform.GetChild(1).GetComponent<RectTransform>().sizeDelta =
                new Vector2(Sizer, Sizer);
            sj.MovementRange = Sizer * joystickRangeMultiplier;
        }
        else if (sb != null)
        {
            Debug.Log("GUISizer Button");
            gameObject.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta =
                new Vector2(Sizer * ButtonSizeMultiplier, Sizer * ButtonSizeMultiplier);
        }
    }
}
