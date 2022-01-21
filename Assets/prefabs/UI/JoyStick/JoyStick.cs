using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStick : MonoBehaviour , IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField] RectTransform handleTransform;
    [SerializeField] RectTransform backGroundTransform;
    [SerializeField] Player PlayerComp;

    private Vector2 handleLocalPostionInVector2;
    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("Drag");
        Vector2 DragPos = eventData.position;
        Vector2 BgPos = backGroundTransform.position;

        Debug.DrawLine(DragPos,BgPos);

        handleTransform.localPosition = Vector2.ClampMagnitude(DragPos - BgPos,backGroundTransform.rect.width/2);
        PlayerComp.MoveInputUpdatedOnMobileInput( handleTransform.localPosition);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log("PointerDown");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //Debug.Log("PointerUp");
        handleTransform.position = backGroundTransform.position;
        PlayerComp.MoveInputUpdatedOnMobileInput(new Vector2(0,0));
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
