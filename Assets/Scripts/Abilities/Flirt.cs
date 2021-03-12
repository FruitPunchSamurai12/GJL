using UnityEngine;

public class Flirt : Ability
{
    [SerializeField]
    LayerMask enemyMask;
    NPC currentFlirtTarget;

    public override void OnTryUnuse()
    {
        if(currentFlirtTarget!=null)
        {
            currentFlirtTarget.Flirt(character);
        }
        Using = false;
        character.RestrictMovement = false;
        character.InSafeZone = false;
        currentFlirtTarget = null;
    }

    protected override void OnTryUse()
    {
        Debug.Log("tried to use");
        var box = GetComponent<InteractBox>();
        var colliders = Physics.OverlapBox(transform.position + transform.forward*box.Offset, box.Size/2f, transform.rotation, enemyMask);
        foreach (var col in colliders)
        {
            var npc = col.GetComponent<NPC>();
            if(npc!=null)
            {
                currentFlirtTarget = npc;
                break;
            }
            else
            {
                currentFlirtTarget = null;
            }
        }
        Debug.Log(currentFlirtTarget);
        if (currentFlirtTarget != null)
        {
            if (!currentFlirtTarget.CanSeeSpecificCharacter(character))
            {
                currentFlirtTarget.Flirt(character);
                Using = true;
                character.RestrictMovement = true;
                character.InSafeZone = true;
            }
        }
    }
}
