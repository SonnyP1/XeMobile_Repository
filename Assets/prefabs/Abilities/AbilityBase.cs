using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnCooldownUpdated(float cooldownPercentage);

public abstract class AbilityBase : ScriptableObject
{
    [SerializeField] float CooldownTime = 5f;
    [SerializeField] Sprite Icon;
    [SerializeField] int AbilityLevel = 1;
    private float cooldownPercentage = 0.0f;

    public OnCooldownUpdated onCooldownUpdated;
    public float GetCooldownPercentage()
    {
        return cooldownPercentage;
    }

    public AbilityComponent ownerComp
    {
        get;
        private set;
    }


    public Sprite GetIcon()
    {
        return Icon;
    }
    public bool IsOnCooldown
    {
        private set;
        get;
    }

    public virtual void Init(AbilityComponent ownerAbilityComp)
    {
        ownerComp = ownerAbilityComp;
        IsOnCooldown = false;
    }

    bool CanCast()
    {
        return !IsOnCooldown && ownerComp.GetStaminaLevel() >= AbilityLevel;
    }

    public abstract void ActivateAbility();

    protected bool CommitAbility()
    {
        if (CanCast())
        {
            StartCooldown();

            return true;
        }
        return false;
    }

    private void StartCooldown()
    {
        ownerComp.StartCoroutine(CooldownCoroutine());
        ownerComp.StartCoroutine(CooldownTimer());
    }

    private IEnumerator CooldownCoroutine()
    {
        IsOnCooldown = true;
        yield return new WaitForSeconds(CooldownTime);
        IsOnCooldown = false;
        yield return null;
    }
    private IEnumerator CooldownTimer()
    {
        float time = 0;
        while(time < CooldownTime)
        {
            time += Time.deltaTime;
            cooldownPercentage = Mathf.Clamp(1-(time / CooldownTime),0,1);
            yield return null;
        }
    }
    public int GetLevel()
    {
        return AbilityLevel;
    }
}
