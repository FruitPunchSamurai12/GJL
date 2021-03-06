﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Throwable
{
    [SerializeField]
    LayerMask enemyLayer;
    [SerializeField]
    float radius = 3f;
    [SerializeField]
    Transform hitOrigin;
    [SerializeField]
    float power = 10f;
    Animator _animator;

    bool activateAttack = false;
    public AK.Wwise.Event PanSwing;
    public AK.Wwise.Event PanHit;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void Swing(int phase)
    {
        if(phase ==0)
        {
            activateAttack = true;
        }
        else if(phase ==1)
        {
            activateAttack = false;     
        }
        else
        {
            _animator.SetBool("Swing", false);
        }
    }

    public void StartSwing()
    {
        _animator.SetBool("Swing", true);
        PanSwing.Post(gameObject);
    }

    void MakeAttack()
    {
        bool hitSomething = false;
        Vector3 hitVelocity = Vector3.zero;
        var targets = Physics.OverlapSphere(hitOrigin.position, radius, enemyLayer);
        foreach (var target in targets)
        {
            NPC enemy = target.GetComponent<NPC>();
            hitVelocity = (target.transform.position - hitOrigin.position).normalized * power;
            if(enemy!=null)
            {
                //play hit sound
                enemy.GetStunned(hitVelocity);
                sound.PlaySound();
                hitSomething = true;
                PanHit.Post(gameObject);
            }
            else
            {
                var destructible = target.GetComponent<Destructible>();
                if(destructible!=null)
                {
                    destructible.Destruct(hitVelocity);
                    hitSomething = true;
                    sound.PlaySound();
                }
            }
        }
        if(hitSomething)
        {
            var destructibleSelf = GetComponent<Destructible>();
            if (destructibleSelf != null)
            {
                Debug.Log("destructible called from throwable");
                destructibleSelf.Destruct(hitVelocity);
            }
        }
    }


    private void FixedUpdate()
    {
        _velocity = rb.velocity;
        if (activateAttack)
            MakeAttack();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(hitOrigin.position, radius);
    }
}
