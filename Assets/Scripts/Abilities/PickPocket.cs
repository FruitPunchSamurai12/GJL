using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickPocket : Ability
{
    [SerializeField]
    LayerMask enemyMask;
    public override void OnTryUnuse()
    {
      
    }

    protected override void OnTryUse()
    {
        var box = GetComponent<InteractBox>();
        var colliders = Physics.OverlapBox(transform.position + transform.forward + box.Center, box.Size / 2f, transform.rotation, enemyMask);
        foreach (var col in colliders)
        {
            var npc = col.GetComponent<NPC>();
            if (npc != null)
            {
                if (!npc.CanSeeSpecificCharacter(character))
                {
                    npc.StealItem(character);
                    Using = true;
                    StartCoroutine(AnimationDelay());
                    break;
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
