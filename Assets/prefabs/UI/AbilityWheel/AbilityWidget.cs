using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityWidget : MonoBehaviour
{
    [SerializeField] RectTransform background;
    [SerializeField] RectTransform icon;

    [SerializeField] RectTransform CooldownTransform;

    [SerializeField] float ExpandedScale = 2.0f;
    [SerializeField] float HighLighetedScale = 2.2f;
    [SerializeField] float ScaleSpeed = 20f;

    Vector3 GoalScale = new Vector3(1,1,1);

    AbilityBase ability;

    Material _cooldownMaterial;
    Material _staminaMaterial;
    Image _cooldownImage;
    // Start is called before the first frame update
    void Start()
    {
        _cooldownMaterial = Instantiate(CooldownTransform.GetComponent<Image>().material);
        CooldownTransform.GetComponent<Image>().material = _cooldownMaterial;

        _staminaMaterial = Instantiate(background.GetComponent<Image>().material);
        background.GetComponent<Image>().material = _staminaMaterial;
    }

    // Update is called once per frame
    void Update()
    {
        background.localScale = Vector3.Lerp(background.localScale, GoalScale, ScaleSpeed * Time.deltaTime);

        //Cooldown Stuff
        if(ability != null && ability.IsOnCooldown)
        {
            SetCooldownProgress(ability.GetCooldownPercentage());
        }
        else
        {
            SetCooldownProgress(1);
        }
    }

    void SetCooldownProgress(float progress)
    {
        _cooldownMaterial.SetFloat("_Progress", progress);
    }

    internal void SetStaminaProgress(float progress)
    {
        _staminaMaterial.SetFloat("_Progress", progress);
    }

    internal void SetExpand(bool isExpanded)
    {

        if (isExpanded)
        {
            GoalScale = new Vector3(1, 1, 1) * ExpandedScale;
        }
        else
        {
            if (IsHighlighted())
            {
                if(ability != null)
                { 
                    ability.ActivateAbility();
                }
            }
            GoalScale = new Vector3(1, 1, 1);
        }
    }

    private bool IsHighlighted()
    {
        return GoalScale == new Vector3(1, 1, 1) * HighLighetedScale;
    }

    internal void SetHighlighted(bool isHighLighted)
    {
        if (isHighLighted)
        {
            GoalScale = new Vector3(1, 1, 1) * HighLighetedScale;
        }
        else
        {
            GoalScale = new Vector3(1, 1, 1) * ExpandedScale;
        }
    }

    internal void AssignAbility(AbilityBase newAbility)
    {
        ability = newAbility;
        icon.GetComponent<Image>().sprite = ability.GetIcon();
    }
}