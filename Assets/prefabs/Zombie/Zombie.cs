using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Zombie : Character
{
    [SerializeField] float CreditReward;
    [SerializeField] float StaminaReward;
    private int _assignZombie;
    NavMeshAgent navAgent;
    Animator animator;
    Rigidbody ZombieRigidbody;
    float speed;
    Vector3 previousLocation;


    // Start is called before the first frame update
    public override void Start() 
    {
        navAgent = GetComponent<NavMeshAgent>();
        base.Start();
        animator = GetComponent<Animator>();
        ZombieRigidbody = GetComponent<Rigidbody>();
        previousLocation = transform.position;
        _assignZombie = GetHashCode();
    }

    internal void Attack()
    {
        animator.SetLayerWeight(1, 1);
    }
    public virtual void AttackPoint()
    {

    }

    public void AttackFinished()
    {
        animator.SetLayerWeight(1, 0);
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        float MoveDelta = Vector3.Distance(transform.position, previousLocation);
        speed = MoveDelta / Time.deltaTime;
        previousLocation = transform.position;
        if (animator != null)
        {
            animator.SetFloat("Speed", speed);
        }
        else { Debug.Log("ANIMATOR DOES NOT EXIST"); }
    }

    public override void NoHealthLeft(GameObject killer = null)
    {
        GetComponent<CapsuleCollider>().enabled = false;
        if(killer != null)
        {
            Player killerAsPlayer = killer.GetComponent<Player>();
            if(killerAsPlayer != null)
            {
                killerAsPlayer.GetComponent<CreditComponent>().ChangedCredits(CreditReward);
                killerAsPlayer.GetComponent<AbilityComponent>().ChangeStamina(StaminaReward);
            }
        }

        AIController AIC = GetComponent<AIController>();
        if (AIC != null)
        {
            AIC.StopAIBehavior();
        }
        base.NoHealthLeft();
    }

    internal void UpdateFromSavedData(ZombieSavedData data)
    {
        //Update Pos
        transform.position = data.Pos;

        //Apply Health
        HealthComponent healthComp = GetComponent<HealthComponent>();
        float zombieCurrentHealth = healthComp.GetHitPoints();
        float healthDelta = data.Health - zombieCurrentHealth;
        healthComp.ChangeHealth(healthDelta);

    }

    public ZombieSavedData GeneratorSaveData()
    {
        return new ZombieSavedData(transform.position,
            GetComponent<HealthComponent>().GetHitPoints(),
            gameObject.name
            );
    }
}

[Serializable]
public struct ZombieSavedData
{
    public ZombieSavedData(Vector3 savedZombiePos, float savedZombieHealth,string uniqueName)
    {
        UniqueID = uniqueName;
        Pos = savedZombiePos;
        Health = savedZombieHealth;
    }

    public Vector3 Pos;
    public float Health;
    public string UniqueID;
}

