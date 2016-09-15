using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    private Image bgImage;
    private Image joystickImage;
    private Vector3 inputVector;

    private void Start()
    {
        bgImage = GetComponent<Image>();
        RectTransform bgRt = GetComponent<RectTransform>();
        RectTransform joystickRt = transform.GetChild(0).GetComponent<RectTransform>();

        int rtSize = Screen.width >= Screen.height ? Screen.height / 3 : Screen.width / 3;

        bgRt.sizeDelta = new Vector2(rtSize, rtSize);
        joystickRt.sizeDelta = new Vector2(rtSize / 2, rtSize / 2);

        joystickImage = transform.GetChild(0).GetComponent<Image>();
    }

    public virtual void OnDrag(PointerEventData ped)
    {
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            bgImage.rectTransform, ped.position, ped.pressEventCamera, out pos))
        {
            pos.x = (pos.x / bgImage.rectTransform.sizeDelta.x);
            pos.y = (pos.y / bgImage.rectTransform.sizeDelta.y);

            float multiplier = -1 + (bgImage.rectTransform.anchorMin.x * 2);

            inputVector = new Vector3(pos.x * 2 + multiplier, 0, pos.y * 2 - 1);
            inputVector = inputVector.magnitude > 1.0f ? inputVector.normalized : inputVector;

            joystickImage.rectTransform.anchoredPosition = new Vector3(
                inputVector.x * (bgImage.rectTransform.sizeDelta.x / 2.5f),
                inputVector.z * (bgImage.rectTransform.sizeDelta.y / 2.5f));
        }
    }
    public virtual void OnPointerDown(PointerEventData ped)
    {
        Debug.Log(123);
        OnDrag(ped);
    }
    public virtual void OnPointerUp(PointerEventData ped)
    {
        inputVector = Vector3.zero;
        joystickImage.rectTransform.anchoredPosition = Vector3.zero;
    }

    public float Horizontal()
    {
        if (inputVector.x != 0)
            return inputVector.x;
        else
            return Input.GetAxis("Horizontal");
    }
    public float Vertical()
    {
        if (inputVector.z != 0)
            return inputVector.z;
        else
            return Input.GetAxis("Vertical");
    }

}
