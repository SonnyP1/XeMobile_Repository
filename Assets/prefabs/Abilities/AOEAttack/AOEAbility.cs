using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/AOEAbility")]

public class AOEAbility : AbilityBase
{
    [SerializeField] float SphereRadius = 5f;
    [SerializeField] float dmgOverTime = 0.5f;
    [SerializeField] float SphereUpkeepTime = 5f;
    [SerializeField] GameObject effect;

    public override void Init(AbilityComponent ownerAbilityComp)
    {
        base.Init(ownerAbilityComp);
    }

    private void CooldownEnded()
    {
        throw new NotImplementedException();
    }

    public override void ActivateAbility()
    {
        if(CommitAbility())
        {
            ownerComp.StartCoroutine(DmgOvertimeZombie(dmgOverTime, SphereUpkeepTime));
        }
    }

    private IEnumerator DmgOvertimeZombie(float dmg,float maxTime)
    {
        GameObject newEffect = Instantiate(effect,ownerComp.GetComponent<Transform>());
        float currentTime = 0;
        while (currentTime < maxTime)
        {
            currentTime += Time.deltaTime;
            Collider[] colliders = Physics.OverlapSphere(ownerComp.GetComponent<Transform>().position, SphereRadius);
            foreach(Collider collider in colliders)
            {
                Zombie colliderAsZombie = collider.GetComponent<Zombie>();
                if(colliderAsZombie)
                {
                    colliderAsZombie.GetComponent<HealthComponent>().ChangeHealth(-dmg);
                }
            }

            yield return new WaitForEndOfFrame();
        }

        DestroyImmediate(newEffect);
    }
}
