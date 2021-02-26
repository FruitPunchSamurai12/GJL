using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Innocence : Ability
{
    public override void OnTryUnuse()
    {
        Debug.Log("innocence lost");
        character.InSafeZone = false;
        character.halfSpeed = false;
        Using = false;
    }

    protected override void OnTryUse()
    {
        if (character.UsingAbility())
            return;
        Using = true;
        character.InSafeZone = true;
        character.halfSpeed = true;
    }
}
