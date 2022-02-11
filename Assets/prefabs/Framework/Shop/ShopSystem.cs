using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName =("Shop/ShopSystem"))]
public class ShopSystem : ScriptableObject
{
    [SerializeField] Weapon[] weaponOnSale;
    private CreditComponent _playerCreditComponent;
    private ShopMenu _shopMenu;
    private Player _playerComponent;
    void Start()
    {

    }

    public void InitializeShopSystem()
    {
        _playerCreditComponent = FindObjectOfType<CreditComponent>();
        _shopMenu = FindObjectOfType<ShopMenu>();
        _playerComponent = _playerCreditComponent.GetComponent<Player>();
        _playerCreditComponent.onCreditUpdated += CheckIfItemIsBuyable;
        CheckIfItemIsBuyable(_playerCreditComponent.GetCurrentCredits());
    }

    private void CheckIfItemIsBuyable(float newValue)
    {
        if (_shopMenu != null)
        {
            foreach (ShopItem item in _shopMenu.GetShopItem())
            {
                if (!CanPurchase(item.weaponInfo.WeaponCost))
                {
                    item.GetItemButton().interactable = false;
                    item.GetItemButton().colors.normalColor.Equals(Color.red);
                }
                else
                {
                    item.GetItemButton().interactable = true;
                    item.GetItemButton().colors.normalColor.Equals(Color.white);
                }

                if(DoesPlayerHaveItem(item))
                {
                    item.GetItemButton().interactable = false;
                    item.GetItemButton().colors.normalColor.Equals(Color.red);
                }
            }
        }
    }

    private bool DoesPlayerHaveItem(ShopItem item)
    {
        if(_playerComponent != null && _playerComponent.GetWeaponList() != null)
        {
            foreach(Weapon playerWeapons in _playerComponent.GetWeaponList())
            {
                if(item.weaponInfo.name == playerWeapons.GetWeaponInfo().name)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        Debug.Log("Player Item Check Fail");
        return false;
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
