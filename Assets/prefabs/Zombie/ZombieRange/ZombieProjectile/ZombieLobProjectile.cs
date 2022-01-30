using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class ZombieLobProjectile : ZombieProjectile
{
    [SerializeField] GameObject ExplosionEffect;
    [SerializeField] SphereCollider ExplosionHitBox;
    [SerializeField] float HeightOfArcLaunch = 3f;
    [SerializeField] LayerMask LayerToIgnore;
    private float gravity = -9.8f;
    private GameObject _effect = null;

    public ZombieLobProjectile(GameObject owner) : base(owner)
    {

    }

    public override void ProjectileBehavior()
    {
        if (GetTarget() != null)
        {
            GetRigidbody().velocity = ShootAtTarget();
        }
        else
        {
            Debug.Log("Target doesnt exist");
        }
    }

    public Vector3 ShootAtTarget()
    {
        if (GetTarget() == null)
        {
            return Vector3.zero;
        }
        //Variable gravity doesnt work??? it zero for some reason

        Vector3 TargetPos = GetTarget().transform.position;
        Vector3 PLPos = transform.position;
        float displacementY = TargetPos.y - PLPos.y;
        Vector3 displacementXZ = new Vector3(TargetPos.x - PLPos.x, 0, TargetPos.z - PLPos.z);

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * -9.81f * HeightOfArcLaunch);
        Vector3 velocityXZ = displacementXZ / (Mathf.Sqrt(-2 * HeightOfArcLaunch / -9.8f) + Mathf.Sqrt(2 * (displacementY - HeightOfArcLaunch) / -9.8f));

        return velocityXZ + velocityY;
    }
    

    void OnCollisionEnter(Collision other)
    {
        if(LayerMask.Equals(other.gameObject.layer, LayerToIgnore))
        {
            return;
        }
        if (_effect == null)
        {
            Quaternion rotToSpawn = Quaternion.FromToRotation(transform.up, other.contacts[0].normal);
            _effect = Instantiate(ExplosionEffect, gameObject.transform.position, rotToSpawn);
            //Debug.Log(_effect.transform.rotation);
            Destroy(gameObject);
        }
    }
}
