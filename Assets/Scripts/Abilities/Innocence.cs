using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Innocence : Ability
{
    public bool insideHouse = false;
    public override void OnTryUnuse()
    {
        Debug.Log("innocence lost");
        character.InSafeZone = false;
        character.halfSpeed = false;
        Using = false;
    }

    public override void OnTryUse()
    {
        if (character.UsingAbility() || insideHouse)
            return;
        Using = true;
        character.InSafeZone = true;
        character.halfSpeed = true;
    }
}
