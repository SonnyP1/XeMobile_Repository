using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Abilities/LifeSteal")]

public class LifeStealAbility : AbilityBase
{
    [SerializeField] float EffectTime = 10f;
    [SerializeField] GameObject effect;

    private HealthComponent _playerHealthComp;
    public override void Init(AbilityComponent ownerAbilityComp)
    {
        base.Init(ownerAbilityComp);
        _playerHealthComp = ownerComp.GetComponent<HealthComponent>();
    }


    public override void ActivateAbility()
    {
        if(CommitAbility())
        {
            Zombie[] zombiesInScene = FindObjectsOfType<Zombie>();
            if (zombiesInScene == null)
            {
                return;
            }

            foreach(Zombie zombie in zombiesInScene)
            {
                zombie.GetComponent<HealthComponent>().onHealthChanged += LifeSteal;
            }

            ownerComp.StartCoroutine(LifeStealTimer(EffectTime));
        }
    }

    private IEnumerator LifeStealTimer(float maxTime)
    {
        GameObject newEffect = Instantiate(effect,ownerComp.GetComponent<Transform>());
        float currentTime = 0;
        while (currentTime < maxTime)
        {
            currentTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        DestroyImmediate(newEffect);
        LifeStealEffectEnded();
    }
    private void LifeStealEffectEnded()
    {
        Zombie[] zombiesInScene = FindObjectsOfType<Zombie>();
        if (zombiesInScene == null)
        {
            return;
        }

        foreach (Zombie zombie in zombiesInScene)
        {
            zombie.GetComponent<HealthComponent>().onHealthChanged -= LifeSteal;
        }
    }

    private void LifeSteal(float newValue, float oldValue, float maxValue, GameObject Causer)
    {
        _playerHealthComp.ChangeHealth(oldValue-newValue);
    }
}
