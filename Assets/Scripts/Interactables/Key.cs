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
        transform.localPosition = Vector3.zero;
    }

    public void Interact(Character character)
    {
        col.enabled = false;
        character.PickUpKey(transform);
    }

    public void EnableCollider() { col.enabled = true; }
}
