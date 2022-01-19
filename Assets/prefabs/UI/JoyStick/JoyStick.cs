using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStick : MonoBehaviour , IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField] RectTransform handleTransform;
    [SerializeField] RectTransform backGroundTransform;
    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Drag");
        Vector2 DragPos = eventData.position;
        Vector2 BgPos = backGroundTransform.position;

        Debug.DrawLine(DragPos,BgPos);

        handleTransform.localPosition = Vector2.ClampMagnitude(DragPos - BgPos,backGroundTransform.rect.width/2);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("PointerDown");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("PointerUp");
        handleTransform.position = backGroundTransform.position;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
