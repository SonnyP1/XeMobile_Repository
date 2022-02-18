using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Abilities/Nuke")]

public class NukeAbility : AbilityBase
{
    [SerializeField] float nukeDmgAmount;
    [SerializeField] float nukeRadius;
    [SerializeField] GameObject effect;
    public override void ActivateAbility()
    {
        if(CommitAbility())
        {
            Instantiate(effect,ownerComp.GetComponent<Transform>());
            Collider[] colliders = Physics.OverlapSphere(ownerComp.GetComponent<Transform>().position, nukeRadius);
            foreach (Collider collider in colliders)
            {
                Zombie colliderAsZombie = collider.GetComponent<Zombie>();
                if (colliderAsZombie)
                {
                    colliderAsZombie.GetComponent<HealthComponent>().ChangeHealth(-nukeDmgAmount);
                }
            }
        }
    }


}
