using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Innocence : Ability
{
    public bool insideHouse = false;
    CharacterController controller;
    [SerializeField]
    LayerMask noInnocenceZoneMask;
    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    public override void OnTryUnuse()
    {
        Debug.Log("innocence lost");
        character.InSafeZone = false;
        character.halfSpeed = false;
        Using = false;
    }

    protected override void OnTryUse()
    {
        if (character.UsingAbility() || insideHouse)
            return;
        Using = true;
        character.InSafeZone = true;
        character.halfSpeed = true;
    }

    private void FixedUpdate()
    {
        var collisions = Physics.OverlapSphere(transform.position, controller.radius, noInnocenceZoneMask,QueryTriggerInteraction.Collide);
        insideHouse = false;
        foreach (var col in collisions)
        {
            Debug.Log(col.name);
            if (col.GetComponent<NoInnocenceZone>())
            {
                insideHouse = true;
                if (Using)
                    OnTryUnuse();
                return;
            }
        }
    }
}
