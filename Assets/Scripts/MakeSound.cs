using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeSound : MonoBehaviour
{

    [SerializeField]
    int priority = 1;

    public int Priority => priority;



    public void PlaySound()
    {
        var enemies = GameManager.Instance.GetAllEnemies();
        foreach (var ai in enemies)
        {
            ai.HearSound(this);
        }
    }

}
