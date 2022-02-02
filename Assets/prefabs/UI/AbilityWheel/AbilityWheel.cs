using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AbilityWheel : MonoBehaviour,IDragHandler,IPointerDownHandler,IPointerUpHandler
{
    [SerializeField] AbilityWidget[] abilityWidgets;
    private AbilityWidget _closestWidget = null;

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 widgetPos = GetComponent<RectTransform>().position;
        Vector2 wheelCenter = new Vector2(widgetPos.x, widgetPos.y);
        Vector2 DragDir = (eventData.position - wheelCenter).normalized;

        float closestAngle = 360.0f;
      

        foreach (var widget in abilityWidgets)
        {
            Vector3 widgetDir = widget.transform.right;
            Vector2 widgetDir2D = new Vector2(-widgetDir.x, -widgetDir.y);

            float angle = Vector2.Angle(DragDir, widgetDir2D);
            if (angle < closestAngle)
            {
                closestAngle = angle;
                _closestWidget = widget;
            }
            widget.SetHighlighted(false);
        }
        _closestWidget.SetHighlighted(true);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _closestWidget = null;
        foreach (AbilityWidget widget in abilityWidgets)
        {
            widget.SetExpand(true);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(_closestWidget.GetAbility() != null)
        {
            _closestWidget.GetAbility().ActivateAbility();
        }
        else { Debug.Log("NO ABILITY EQUIP!"); }

        foreach (AbilityWidget widget in abilityWidgets)
        {
            widget.SetExpand(false);
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
