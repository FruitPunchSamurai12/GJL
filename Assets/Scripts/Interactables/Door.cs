using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour,IInteractable
{
    public AK.Wwise.Event OpenDoor;
    Animator _animator;
    [SerializeField]
    bool open1;
    [SerializeField]
    bool isLocked;

    public bool IsLocked => isLocked;

    private void Awake()
    {
        _animator = GetComponentInParent<Animator>();
    }
    public void Interact(Character character)
    {
        if(isLocked)
        {
            if (character.HasKey(true))
                isLocked = false;
            else
                return;
        }
        OpenDoor.Post(gameObject);
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
