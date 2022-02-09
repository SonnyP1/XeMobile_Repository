using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnNewAbilityInitialized(AbilityBase newAbility);
public delegate void OnStaminaUpdated(float newValue);

public class AbilityComponent : MonoBehaviour
{
    [SerializeField] float StaminaLevel;
    [SerializeField] float MaxStaminaLevel;
    [SerializeField] float StaminaDrainRate;
    [SerializeField] float StaminaDrainDelay = 2f;

    [SerializeField] AbilityBase[] abilities;

    public event OnNewAbilityInitialized onNewAbilityInitialized;
    public event OnStaminaUpdated onStaminaUpdated;

    private Coroutine _staminaDrainingCoroutine;

    void Start()
    {
        for (int i = 0; i < abilities.Length; i++)
        {
            abilities[i] = Instantiate(abilities[i]);
            abilities[i].Init(this);
            onNewAbilityInitialized?.Invoke(abilities[i]);
        }
        _staminaDrainingCoroutine = StartCoroutine(StaminaDrainingCoroutine());
    }

    public void ChangeStamina(float changeAmount)
    {
        if (changeAmount > 0)
        {
            if (_staminaDrainingCoroutine != null)
            {
                StopCoroutine(_staminaDrainingCoroutine);
                _staminaDrainingCoroutine = StartCoroutine(StaminaDrainingCoroutine());
            }
        }
        StaminaLevel = Mathf.Clamp(StaminaLevel + changeAmount, 0, MaxStaminaLevel);
        onStaminaUpdated?.Invoke(StaminaLevel);
    }
    IEnumerator StaminaDrainingCoroutine()
    {
        yield return new WaitForSeconds(StaminaDrainDelay);
        while (StaminaLevel > 0)
        {
            StaminaLevel -= Mathf.Clamp(StaminaDrainRate * Time.deltaTime,0, MaxStaminaLevel);
            onStaminaUpdated?.Invoke(StaminaLevel);
            yield return new WaitForEndOfFrame();
        }
        StaminaLevel = Mathf.Clamp(StaminaLevel, 0, MaxStaminaLevel);
    }

    internal int GetStaminaLevel()
    {
        return (int)StaminaLevel;
    }
}
