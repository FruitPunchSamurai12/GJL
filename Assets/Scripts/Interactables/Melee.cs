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
        var targets = Physics.OverlapSphere(hitOrigin.position, radius, enemyLayer);
        foreach (var target in targets)
        {
            NPC enemy = target.GetComponent<NPC>();
            if(enemy!=null)
            {
                //play hit sound
                enemy.GetStunned(true);
            }
        }
    }


    private void FixedUpdate()
    {
        if (activateAttack)
            MakeAttack();
    }


}
