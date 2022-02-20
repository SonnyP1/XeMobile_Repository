using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieBeacon : MonoBehaviour
{
    [SerializeField] GameObject Geo;
    private HealthComponent _healthComp;
    private InGameUI _inGameUI;
    private void Start()
    {
        _inGameUI = FindObjectOfType<InGameUI>();
        _healthComp = GetComponent<HealthComponent>();
        _healthComp.onHealthChanged += HealthChanged; 
    }
    private void HealthChanged(float newValue, float oldValue, float maxValue, GameObject Causer)
    {
        if(newValue == 0)
        {
            UpdateUIOnDeath();
            
        }
    }

    public void UpdateUIOnDeath()
    {
        if (_inGameUI)
        {
            if(FindObjectsOfType<ZombieBeacon>().Length-1 == 0)
            {
                StartCoroutine(WinnerWinnerChickenDinner());
            }
            else
            {
                _inGameUI.SetInfectedAreaLeftText(FindObjectsOfType<ZombieBeacon>().Length - 1);
                Destroy(gameObject);
            }
        }
        else
        {
            _inGameUI = FindObjectOfType<InGameUI>();
            _inGameUI.SetInfectedAreaLeftText(FindObjectsOfType<ZombieBeacon>().Length - 1);
        }
    }

    internal ZombieBeaconSavedData GeneratorSaveData()
    {
        return new ZombieBeaconSavedData(gameObject.name);
    }

    IEnumerator WinnerWinnerChickenDinner()
    {
        Destroy(Geo);
        _inGameUI.SetInfectedAreaLeftText(FindObjectsOfType<ZombieBeacon>().Length - 1);
        Zombie[] zombiesInSceneCurrently = FindObjectsOfType<Zombie>();
        foreach(Zombie zombie in zombiesInSceneCurrently)
        {
            zombie.GetComponent<HealthComponent>().ChangeHealth(-100);
        }
        _inGameUI.ToggleWinScreen();
        yield return new WaitForSeconds(5f);
        GameplayStatics.LoadScene("StartMenu");
    }
}

    [Serializable]
    public struct ZombieBeaconSavedData
    {
        public ZombieBeaconSavedData(string uniqueName)
        {
            UniqueID = uniqueName;
        }

        public string UniqueID;
    }