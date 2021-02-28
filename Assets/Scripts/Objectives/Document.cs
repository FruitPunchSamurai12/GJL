using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Document : MonoBehaviour,IPickable
{
    public bool Heavy => false;
    bool cleared = false;

    Collider col;
    Rigidbody rb;

    private void Awake()
    {
        col = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
    }

    public void Interact(Character character)
    {
        col.enabled = false;
        rb.isKinematic = true;
        rb.useGravity = false;
        character.PickUpItem(this);
        if (!cleared)
        {
            cleared = true;
            GameEvents.Instance.ClearedObjective();
        }
    }

    public void Drop()
    {
        transform.SetParent(null);
        col.enabled = true;
        rb.isKinematic = false;
        rb.useGravity = true;
    }

}
