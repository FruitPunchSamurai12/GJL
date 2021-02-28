using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Safe : MonoBehaviour, IInteractable
{
    public AK.Wwise.Event OpenDoor;
    Animator _animator;  
    bool isLocked = true;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    public void Interact(Character character)
    {
        if (isLocked)
        {
            if (character.HasSafeKey(true))
                isLocked = false;
            else
                return;
        }
        OpenDoor.Post(gameObject);
        _animator.SetTrigger("Open");

    }   
}
