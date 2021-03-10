using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : Ability
{
    [SerializeField]
    float attackRate = 1f;
    float timeOfLastAttack;

    public override void OnTryUnuse()
    {
        
    }

    public override void OnTryUse()
    {
        if (character.UsingAbility())
            return;
        if(Time.time>timeOfLastAttack+attackRate)
        {
            var item = character.LightHeldObject();
            if(item!=null)
            {
                Weapon weapon = item as Weapon;
                if(weapon!=null)
                {
                    weapon.StartSwing();
                    timeOfLastAttack = Time.time;
                    Using = true;
                }
            }
        }
    }

    private void Update()
    {
        if (Using && Time.time > timeOfLastAttack + attackRate)
        {
            Using = false;
        }
    }
}
