using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MakeSound : MonoBehaviour
{
    [SerializeField]
    float duration = 1f;
    [SerializeField]
    AudioClip soundClip;
    [SerializeField]
    int priority = 1;
    

    public int Priority => priority;

    private void OnEnable()
    {
        GetComponent<AudioSource>().clip = soundClip;
        Destroy(gameObject, duration);
    }

    private void Update()
    {
        var enemies = GameManager.Instance.GetAllEnemies();
        foreach (var ai in enemies)
        {
            ai.HearSound(this);
        }
    }
}
