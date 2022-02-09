using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI Text;
    [SerializeField] Image Icon;

    ShopSystem _shopSystem;
    public WeaponInfo weaponInfo
    {
        get;
        private set;
    }

    internal void Init(WeaponInfo weaponInfo, ShopSystem shopSystem)
    {
        _shopSystem = shopSystem;
        this.weaponInfo = weaponInfo;
        Icon.sprite = weaponInfo.Icon;
        Text.text = $"{weaponInfo.name}\n" +
             $"Rate: {weaponInfo.ShootSpeed}\n" +
             $"Damage: {weaponInfo.DamagePerBullet}\n" +
             $"Cost: {weaponInfo.WeaponCost}\n";
    }


    public void Purchase()
    {
        _shopSystem.PurchaseWeapon(weaponInfo.name);
    }
}
