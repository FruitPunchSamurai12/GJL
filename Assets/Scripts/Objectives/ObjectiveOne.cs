using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveOne : MonoBehaviour
{
    public static bool cleared = false;
    private void OnTriggerEnter(Collider other)
    {
        if (cleared)
            return;
        if(other.GetComponent<Character>())
        {
            cleared = true;
            GameEvents.Instance.ClearedObjective();
        }
    }
}
