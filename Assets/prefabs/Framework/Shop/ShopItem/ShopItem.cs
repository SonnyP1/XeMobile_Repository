using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI Text;
    [SerializeField] Image Icon;
    private Button _button;
    public Button GetItemButton()
    {
        return _button;
    }

    ShopSystem _shopSystem;
    public WeaponInfo weaponInfo
    {
        get;
        private set;
    }

    internal void Init(WeaponInfo weaponInfo, ShopSystem shopSystem)
    {
        _button = GetComponent<Button>();
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
