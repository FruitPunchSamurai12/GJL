﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cry : Ability
{
    public AK.Wwise.Event BabyCrying;
    bool isCrying = false;
    [SerializeField] float cryRange = 15f;
    //[SerializeField] GameObject cursor;

    public override void OnTryUnuse()
    {
        isCrying = false;
        Using = false;
        character.RestrictMovement = false;
        //cursor.SetActive(false);
        BabyCrying.Stop(gameObject);
    }

    protected override void OnTryUse()
    {
        if (character.UsingAbility())
            return;
        Using = true;
        character.RestrictMovement = true;
        isCrying = true;
        BabyCrying.Post(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (Using)
        {
            //cursor.SetActive(true);
            if(isCrying)
            {
                var enemies = GameManager.Instance.GetAllEnemies();
                foreach (var ai in enemies)
                {
                    ai.BabyCrying(transform.position, cryRange);
                }
            }
           /* else
            {
                if (Controller.Instance.LeftClick)
                {
                    isCrying = true;
                }
            }*/
        }
    }

    private void OnDrawGizmos()
    {
        if(Using)
        {
            Gizmos.color = new Color(1, 0, 0, 0.4f);
            Gizmos.DrawSphere(transform.position, cryRange);
            
        }
    }
}
