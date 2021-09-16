using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickPocket : Ability
{
    [SerializeField]
    LayerMask enemyMask;

    InteractBox box;


    protected override void Awake()
    {
        base.Awake();
        box = GetComponent<InteractBox>();
    }

    public override void OnTryUnuse()
    {
      
    }

    protected override void OnTryUse()
    {
        if (box.Interactable != null)
        {
            var npc = box.Interactable as NPC;
            if (npc != null)
            {
                if (!npc.CanSeeSpecificCharacter(character))
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
