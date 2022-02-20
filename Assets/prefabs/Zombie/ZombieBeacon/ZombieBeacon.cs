using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieBeacon : MonoBehaviour
{
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
            if(_inGameUI)
            {
                _inGameUI.SetInfectedAreaLeftText(FindObjectsOfType<ZombieBeacon>().Length-1);
            }
            Destroy(gameObject);
        }
    }
}
