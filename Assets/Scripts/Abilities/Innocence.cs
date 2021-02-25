using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Innocence : Ability
{
    protected override void OnTryUnuse()
    {
        character.InSafeZone = false;
        character.halfSpeed = false;
    }

    protected override void OnTryUse()
    {
        character.InSafeZone = true;
        character.halfSpeed = true;
    }
}
