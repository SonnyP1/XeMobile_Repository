using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField] RectTransform handleTransform;
    [SerializeField] RectTransform backGroundTransform;
    Vector2 JoyStickInput;
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
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        handleTransform.position = backGroundTransform.position;
        JoyStickInput = new Vector2(0, 0);
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
