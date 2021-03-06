﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Safe : MonoBehaviour, IInteractable
{
    public AK.Wwise.Event OpenDoor;
    public AK.Wwise.Event GrabDocument;
    Animator _animator;  
    bool isLocked = true;
    [SerializeField]
    Document document;

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
        document.Interact(character);
        GrabDocument.Post(gameObject);

    }   
}
