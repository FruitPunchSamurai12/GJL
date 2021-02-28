using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour,IInteractable
{
    Animator _animator;
    [SerializeField]
    bool open1;
    [SerializeField]
    bool isLocked;

    private void Awake()
    {
        _animator = GetComponentInParent<Animator>();
    }
    public void Interact(Character character)
    {
        if(isLocked)
        {
            if (character.HasKey())
                isLocked = false;
            else
                return;
        }
        if (open1)
        {
            _animator.SetTrigger("Open");
        }
        else
        {
            _animator.SetTrigger("Open2");
        }
    }

   
}
