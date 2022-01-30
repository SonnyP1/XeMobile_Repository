using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedZombie : Zombie
{
    [SerializeField] Transform AttackSpawnPoint;
    [SerializeField] GameObject Projectile;
    public override void AttackPoint()
    {
       GameObject newProjectile = Instantiate(Projectile,AttackSpawnPoint.position,Quaternion.identity);
        newProjectile.GetComponent<ZombieLobProjectile>().SetOwner(gameObject);
    }
}
