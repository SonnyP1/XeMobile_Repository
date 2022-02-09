using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnHealthChanged(float newValue, float oldValue, float maxValue, GameObject Causer);
public delegate void OnNoHealthLeft(GameObject killer = null);

public class HealthComponent : MonoBehaviour
{
    [SerializeField] float HitPoints = 10;
    [SerializeField] float MaxHitPoints = 10;

    public OnHealthChanged onHealthChanged;
    public OnNoHealthLeft noHealthLeft;

    public void ChangeHealth(float changeAmount, GameObject Causer = null)
    {
        float oldValue = HitPoints;
        HitPoints = Mathf.Clamp(HitPoints + changeAmount,0,MaxHitPoints);
        if(HitPoints <= 0)
        {
            if(noHealthLeft!=null)
            {
                noHealthLeft.Invoke(Causer);
            }
        }
        if (onHealthChanged != null)
        {
            onHealthChanged.Invoke(HitPoints, oldValue, MaxHitPoints, Causer);
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        Weapon attackingWeapon = other.GetComponentInParent<Weapon>();
        if(attackingWeapon!=null)
        {
            ChangeHealth(-(int)(attackingWeapon.GetDamagePerBullet()), attackingWeapon.Owner);
        }
    }

    public void BroadCastCurrentHealth()
    {
        onHealthChanged.Invoke(HitPoints, HitPoints, MaxHitPoints, gameObject);
    }
}
