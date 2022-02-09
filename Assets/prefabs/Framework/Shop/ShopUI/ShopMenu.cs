using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopMenu : MonoBehaviour
{
    [SerializeField] ShopItem shopItemPrefab;
    [SerializeField] GameObject shopPanelObject;
    [SerializeField] ShopSystem shopSystem;
    void Start()
    {
        PopulateItems(shopSystem.GetWeaaponsOnSale());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PopulateItems(Weapon[] weaponOnSale)
    {
        foreach(Weapon weapon in weaponOnSale)
        {
            ShopItem newItem = Instantiate(shopItemPrefab, shopPanelObject.transform);
            newItem.Init(weapon.GetWeaponInfo(), shopSystem);
        }
    }
}
