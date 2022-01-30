using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieProjectile : MonoBehaviour
{
    [SerializeField] float Speed;
    private Transform _spawnPoint;
    private Rigidbody _rbRigidbody;
    private GameObject _target;
    private GameObject _owner;

    public ZombieProjectile(GameObject owner)
    {
        SetOwner(owner);
    }

    public GameObject GetOwner()
    {
        return _owner;
    }
    public void SetOwner(GameObject newOwner)
    {
        _owner = newOwner;
        if (_owner.GetComponent<BehaviorTree>() != null)
        {
            _owner.GetComponent<BehaviorTree>().GetBlackboardValue("Target", out object target);
            _target = (GameObject)target;
        }
        else
        {
            Debug.Log("BEHAVIORTREE DOES NOT EXIST");
        }
    }
    public GameObject GetTarget()
    {
        return _target;
    }
    
    public Transform GetSpawnPoint()
    {
        return _spawnPoint;
    }
    public Rigidbody GetRigidbody()
    {
        return _rbRigidbody;
    }
    public float GetSpeed()
    {
        return Speed;
    }
    
    void Start()
    {
        _spawnPoint = gameObject.transform;
        _rbRigidbody = GetComponent<Rigidbody>();

        ProjectileBehavior();
    }

    public virtual void ProjectileBehavior()
    {
        _rbRigidbody.AddForce(0,0,Speed);
    }
}
