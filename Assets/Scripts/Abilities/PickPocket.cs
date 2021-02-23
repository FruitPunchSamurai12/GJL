using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickPocket : Ability
{
    protected override void OnTryUnuse()
    {
      
    }

    protected override void OnTryUse()
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
                }
                else
                {
                    Debug.Log("pickpocket not successful, npc can see you");
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
