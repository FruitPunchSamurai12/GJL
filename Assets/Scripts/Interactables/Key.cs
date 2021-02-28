using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour,IInteractable
{
    public bool Heavy => false;

    Collider col;

    private void Awake()
    {
        col = GetComponent<Collider>();
    }

    public void Interact(Character character)
    {
        col.enabled = false;
        character.PickUpKey(transform);
    }
}
