using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionHitBox : MonoBehaviour
{
    [SerializeField] float MaxDmg = 2.0f;
    private SphereCollider _hitBox;
    private void Start()
    {
        _hitBox = GetComponent<SphereCollider>();

        Collider[] targets = Physics.OverlapSphere(_hitBox.bounds.center,_hitBox.radius);
        foreach (var target in targets)
        {
            Player targetAsPlayer = target.GetComponent<Player>();
            if (targetAsPlayer)
            {
                target.GetComponent<HealthComponent>().TakeDamage(CalculateDmgBasedOnDistance(targetAsPlayer.transform), gameObject);
            }
        }
        Destroy(gameObject);
    }

    private int CalculateDmgBasedOnDistance(Transform objectToCalculateDmg)
    {
        float areaProximity = (gameObject.transform.position - objectToCalculateDmg.position).magnitude;
        float output = areaProximity / _hitBox.radius;
        float dmg = output * MaxDmg;
        int roundUpOutput = Mathf.RoundToInt(dmg);

        return roundUpOutput;
    }
}
