using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MakeSound : MonoBehaviour
{
    [SerializeField]
    AudioClip soundClip;
    [SerializeField]
    int priority = 1;
    AudioSource audioSource;

    public int Priority => priority;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound()
    {
        audioSource.PlayOneShot(soundClip);
        var enemies = GameManager.Instance.GetAllEnemies();
        foreach (var ai in enemies)
        {
            ai.HearSound(this);
        }
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
        var enemies = GameManager.Instance.GetAllEnemies();
        foreach (var ai in enemies)
        {
            ai.HearSound(this);
        }
    }
}
