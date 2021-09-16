using UnityEngine;

public class Flirt : Ability
{
    [SerializeField]
    LayerMask enemyMask;
    NPC currentFlirtTarget;
    InteractBox box;


    protected override void Awake()
    {
        base.Awake();
        box = GetComponent<InteractBox>();
    }

    public override void OnTryUnuse()
    {
        if(currentFlirtTarget!=null)
        {
            currentFlirtTarget.Interact(character);
        }
        Using = false;
        character.RestrictMovement = false;
        character.InSafeZone = false;
        currentFlirtTarget = null;
    }

    protected override void OnTryUse()
    {
        if(box.Interactable !=null)
        {
            var npc = box.Interactable as NPC;
            if(npc!=null)
            {
                currentFlirtTarget = npc;
            }
            else
            {
                currentFlirtTarget = null;
            }
        }
        else
        {
            currentFlirtTarget = null;
        }
        if (currentFlirtTarget != null)
        {
            if (!currentFlirtTarget.CanSeeSpecificCharacter(character))
            {
                currentFlirtTarget.Interact(character);
                Using = true;
                character.RestrictMovement = true;
                character.InSafeZone = true;
            }
        }    
    }
}
