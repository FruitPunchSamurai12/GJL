using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickPocket : Ability
{
    public override void OnTryUnuse()
    {
      
    }

    public override void OnTryUse()
    {
        var interactable = character.GetInteractable();
        if(interactable!=null)
        {
            var npc = interactable as NPC;
            if(npc!=null)
            {
                if(!npc.CanSeeSpecificCharacter(character))
                {
                    npc.StealItem(character);
                    Using = true;
                    StartCoroutine(AnimationDelay());
                }
                else
                {
                    Using = false;
                    Debug.Log("pickpocket not successful, npc can see you");
                }
            }
        }
    }

    IEnumerator AnimationDelay()
    {
        yield return new WaitForSeconds(0.3f);
        Using = false;
    }

}
