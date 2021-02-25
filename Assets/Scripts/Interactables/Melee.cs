using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : Throwable
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

    public bool Swinging { get; private set; }
    bool activateAttack = false;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void Swing(int phase)
    {
        if(phase ==0)
        {
            Swinging = true;
        }
        else if(phase == 1)
        {
            activateAttack = true;
        }
        else if(phase ==2)
        {
            activateAttack = false;
        }
        else
        {
            Swinging = false;
            activateAttack = false;
            _animator.SetBool("Swing", false);
        }
    }

    public void StartSwing()
    {
        _animator.SetBool("Swing", true);
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
                enemy.GetStunned(true);
                sound.PlaySound();
                hitSomething = true;
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


}
