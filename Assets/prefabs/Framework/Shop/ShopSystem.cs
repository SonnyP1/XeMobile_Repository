using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName =("Shop/ShopSystem"))]
public class ShopSystem : ScriptableObject
{
    [SerializeField] Weapon[] weaponOnSale;
    private CreditComponent _playerCreditComponent;
    void Start()
    {
        _playerCreditComponent = FindObjectOfType<CreditComponent>();
    }

    internal Weapon[] GetWeaaponsOnSale()
    {
        return weaponOnSale;
    }

    void Update()
    {
        
    }

    public void PurchaseWeapon(string weaponName)
    {
        if(!hasCreditComponent())
        {
            return;
        }


        foreach (Weapon weapon in weaponOnSale)
        {
            if (weapon.GetWeaponInfo().name == weaponName)
            {
                Player player = FindObjectOfType<Player>();
                if(player != null && CanPurchase(weapon.GetWeaponInfo().WeaponCost))
                {
                    player.AquireNewWeapon(weapon,true);
                    _playerCreditComponent.ChangedCredits(-weapon.GetWeaponInfo().WeaponCost);
                }
            }
        }
    }

    bool CanPurchase(float cost)
    {
        if(!hasCreditComponent())
        {
            return false;
        }

        if(cost > _playerCreditComponent.GetCurrentCredits())
        {
            return false;
        }

        return true;
    }

    bool hasCreditComponent()
    {
        if (_playerCreditComponent == null)
        {
            _playerCreditComponent = FindObjectOfType<CreditComponent>();
        }

        return _playerCreditComponent != null;
    }
}
