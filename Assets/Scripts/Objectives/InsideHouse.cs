using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsideHouse : MonoBehaviour
{
    public static bool cleared = false;
    public bool canClear = true;
    private void OnTriggerEnter(Collider other)
    {       
        if (!canClear || cleared || !ObjectiveOne.cleared)
            return;
        if (other.GetComponent<Character>())
        {
            cleared = true;
            GameEvents.Instance.ClearedObjective();
        }
    }
}
