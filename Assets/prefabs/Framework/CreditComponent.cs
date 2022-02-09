using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnCreditUpdated(float newValue);

public class CreditComponent : MonoBehaviour
{
    [SerializeField] float Credits = 0f;
    [SerializeField] float MaxCredit = 2000.0f;
    private CreditUI _creditUI;

    public event OnCreditUpdated onCreditUpdated;

    private void Start()
    {
        _creditUI = FindObjectOfType<CreditUI>();
        _creditUI.UpdateCreditAmount(Credits);
        if(_creditUI == null)
        {
            Debug.LogWarning("Credit UI was not found!");
        }
    }
    public void ChangedCredits(float changeAmount)
    {

        Credits = Mathf.Clamp(Credits + changeAmount, 0, MaxCredit);
        _creditUI.UpdateCreditAmount(Credits);
        if(onCreditUpdated != null)
        {
            onCreditUpdated.Invoke(Credits);
        }
    }
}
