using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//scriptable object
public class Abilities : MonoBehaviour 
{
    [Header("UI")]
    [SerializeField] Sprite Icon;
    public Sprite GetIcon()
    {
        return Icon;
    }

    [Header("Cooldown Stats")]
    [SerializeField] float CooldownTime;
    private float _timeRemaining;
    public float GetTimeRemaining()
    {
        return _timeRemaining;
    }

    private Coroutine _abilityCooldownCoroutine = null;

    void Start()
    {

    }
    public void ActivateAbility()
    {
        if(_abilityCooldownCoroutine == null)
        {
            AbilityEffect();
            _timeRemaining = 0;
            _abilityCooldownCoroutine = StartCoroutine(AbilityCooldown());
        }
    }
    virtual public void AbilityEffect()
    {
        Debug.Log("Basic Ability with off");
    }

    private IEnumerator AbilityCooldown()
    {
        float time = 0.0f;
        while(time <= CooldownTime)
        {
            time += Time.deltaTime;
            _timeRemaining = CooldownTime - time;
            yield return null;
        }
        Debug.Log("Cooldown ended");
        _abilityCooldownCoroutine = null;
    }
}
