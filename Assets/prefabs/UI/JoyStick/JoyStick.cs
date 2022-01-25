using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JoyStick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField] RectTransform handleTransform;
    [SerializeField] RectTransform backGroundTransform;


    [Header("UI")]
    Image handleImage;
    Image bgImage;
    [SerializeField] Image iconImage;

    Vector2 JoyStickInput;
    Vector3 orginalTransform;
    public Vector2 GetJoyStickInput()
    {
        return JoyStickInput;
    }
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 DragPos = eventData.position;
        Vector2 BgPos = backGroundTransform.position;

        Debug.DrawLine(DragPos,BgPos);

        handleTransform.localPosition = Vector2.ClampMagnitude(DragPos - BgPos,backGroundTransform.rect.width/2);
        JoyStickInput = handleTransform.localPosition;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        transform.position = eventData.position;

        isJoystickVisable(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        handleTransform.position = backGroundTransform.position;
        JoyStickInput = new Vector2(0, 0);
        transform.position = orginalTransform;


        isJoystickVisable(false);
    }

    void Start()
    {
        orginalTransform = transform.position;

        handleImage = handleTransform.gameObject.GetComponent<Image>();
        bgImage = backGroundTransform.gameObject.GetComponent<Image>();

        isJoystickVisable(false);
    }

    private void isJoystickVisable(bool isVisable)
    {
        if(isVisable)
        {
            handleImage.CrossFadeAlpha(255.0f,1.0f, true);
            bgImage.CrossFadeAlpha(255.0f, 1.0f, true);
            iconImage.CrossFadeAlpha(0.0f, 0.0f, true);
        }
        else
        {
            handleImage.CrossFadeAlpha(0.0f, .0f, true);
            bgImage.CrossFadeAlpha(0.0f, .0f, true);
            iconImage.CrossFadeAlpha(255.0f, 0.0f, true);
        }
    }

    void Update()
    {
        
    }
}
