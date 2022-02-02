using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AbilityWidget : MonoBehaviour
{
    [Header("Ability")]
    [SerializeField] Abilities WidgetAbility;
    public Abilities GetAbility()
    {
        return WidgetAbility;
    }

    [Header("UI")]
    [SerializeField] RectTransform BackGroundTransform;
    [SerializeField] Image IconImage;

    [Header("UI Expand Animation")]
    [SerializeField] float ExpandedScale = 2.0f;
    [SerializeField] float HighLightedScale = 2.2f;
    [SerializeField] float TimeToExpanded = 50.0f;
    private Vector3 _desiredSize = new Vector3(1,1,1);
    void Start()
    {
        if(WidgetAbility.GetIcon() != null)
        {
            IconImage.sprite = WidgetAbility.GetIcon();
        }
        WidgetAbility = Instantiate(WidgetAbility);
    }

    void Update()
    {
        BackGroundTransform.localScale = Vector3.Lerp(BackGroundTransform.localScale,_desiredSize, TimeToExpanded * Time.deltaTime);
    }

    internal void SetExpand(bool isExpanded)
    {
        if(isExpanded)
        {
            _desiredSize = new Vector3(1,1,1) * ExpandedScale;
        }
        else
        {
            _desiredSize = new Vector3(1, 1, 1);
        }
    }
    internal void SetHighlighted(bool isHighLighted)
    {
        if(isHighLighted)
        {
            _desiredSize = new Vector3(1, 1, 1) * HighLightedScale;
        }
        else
        {
            _desiredSize = new Vector3(1, 1, 1) * ExpandedScale;
        }
    }
}
