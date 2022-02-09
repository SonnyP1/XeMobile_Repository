using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Abilities/SpeedUp")]

public class SpeedUpAbility : AbilityBase
{
    [SerializeField] float MultiplyWalkingSpeed = 2.0f;
    private MovementComponent _movementComp;
    public override void Init(AbilityComponent ownerAbilityComp)
    {
        base.Init(ownerAbilityComp);
        _movementComp = ownerComp.GetComponent<MovementComponent>();
    }
    public override void ActivateAbility()
    {
        if(CommitAbility())
        {
            ownerComp.StartCoroutine(SpeedUpCoroutine());
        }
    }

    private IEnumerator SpeedUpCoroutine()
    {
        while (IsOnCooldown)
        {
            yield return new WaitForEndOfFrame();
            _movementComp.SetModifiedWalkingSpeed(MultiplyWalkingSpeed);
        }
        _movementComp.SetModifiedWalkingSpeed(1f);
        yield return new WaitForEndOfFrame();
    }
}
