using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsideHouse : MonoBehaviour
{
    public static bool cleared = false;
    public bool canClear = true;
    public bool leadsOutside = true;
    private void OnTriggerEnter(Collider other)
    {
        if (leadsOutside)
        {
            var baby = other.GetComponent<Innocence>();
            if (baby != null)
            {
                baby.insideHouse = !baby.insideHouse;
                if (baby.insideHouse)
                    baby.OnTryUnuse();
            }
        }
        if (!canClear || cleared || !ObjectiveOne.cleared)
            return;
        if (other.GetComponent<Character>())
        {
            cleared = true;
            GameEvents.Instance.ClearedObjective();
        }
    }
}
