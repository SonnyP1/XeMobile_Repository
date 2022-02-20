using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    [SerializeField] GameObject InGameMenu;
    [SerializeField] GameObject PauseMenu;
    [SerializeField] Image WeaponIcon;
    [SerializeField] Image ProgressBar;
    [SerializeField] Canvas ShopMenuUI;
    [SerializeField] Canvas WinMenu;
    [SerializeField] TextMeshProUGUI InfectedAreaLeft;
    public void SetPlayerHealth(float percent)
    {
        ProgressBar.material.SetFloat("_Progress", percent);
    }

    public void SetInfectedAreaLeftText(int amountLeft)
    {
        InfectedAreaLeft.text = amountLeft.ToString();
    }
    public void PauseGame()
    {
        if(Time.timeScale == 0)
        {
            SwichToInGameMenu();
        }
        else
        {
            SwitchToPauseMenu();
        }
    }
    public void SwichToInGameMenu()
    {
        InGameMenu.SetActive(true);
        PauseMenu.SetActive(false);
        GameplayStatics.UnPauseGame();
    }

    public void SwitchToPauseMenu()
    {
        PauseMenu.SetActive(true);
        InGameMenu.SetActive(false);
        GameplayStatics.PauseGame();
       
    }

    public void ToggleShopMenu()
    {
        if(ShopMenuUI.enabled == true)
        {
            ShopMenuUI.enabled = false;
        }
        else
        {
            ShopMenuUI.enabled = true;
        }
    }

    public void ToggleWinScreen()
    {
        WinMenu.enabled = true;
    }

    private void Start()
    {
        WinMenu.enabled = false;
        StartCoroutine(WaitToUpdateCounterUI());
        SwichToInGameMenu();
        HealthComponent PlayerHealthComp = FindObjectOfType<Player>().GetComponent<HealthComponent>();
        SetInfectedAreaLeftText(FindObjectsOfType<ZombieBeacon>().Length);
        PlayerHealthComp.onHealthChanged += PlayerHealthChanged;
        PlayerHealthComp.BroadCastCurrentHealth();
    }

    private void PlayerHealthChanged(float newValue, float oldValue, float maxValue, GameObject Causer)
    {
        SetPlayerHealth((float)newValue/(float)maxValue);
    }

    private void Update()
    {
        
    }

    public void SwichedWeaponTo(Weapon EquipedWeapon)
    {
        WeaponIcon.sprite = EquipedWeapon.GetWeaponIcon();
    }

    IEnumerator WaitToUpdateCounterUI()
    {
        yield return new WaitForSeconds(0.1f);
        SetInfectedAreaLeftText(FindObjectsOfType<ZombieBeacon>().Length);
    }
}
