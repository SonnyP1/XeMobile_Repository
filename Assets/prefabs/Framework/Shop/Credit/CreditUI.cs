using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreditUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI CreditAmountText;

    public void UpdateCreditAmount(float newValue)
    {
        CreditAmountText.text = newValue.ToString();
    }
}
