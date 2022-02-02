using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthRegen : Abilities
{
    [Header("Stats")]
    [SerializeField] int HealingAmount = 1;
    [SerializeField] float HealthRegenRate = 1.0f;
    [SerializeField] int HealingTotalAmount = 5;

    [Header("Player")]
    private GameObject _player;
    private HealthComponent _playerHealthComp;

    [Header("Other")]
    private Coroutine _healthCoroutine;
    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        if(_player == null)
        {
            Debug.Log("Player does not exist for health regen ability");
        }
        _playerHealthComp = _player.GetComponent<HealthComponent>();
    }
    public override void AbilityEffect()
    {
        _healthCoroutine = StartCoroutine(HealthRegenFunctionality());
        Debug.Log("Start Healing Regen");
    }

    private IEnumerator HealthRegenFunctionality()
    {
        int count = 0;
        while(count < HealingTotalAmount)
        {
            Debug.Log("HEAL");
            _playerHealthComp.HealHealth(HealingAmount,gameObject);
            yield return new WaitForSeconds(HealthRegenRate);
            count++;
        }
        Debug.Log("STOP HEALING");
        StopCoroutine(_healthCoroutine);
        
    }

}
