using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeKey : MonoBehaviour, IPickable
{
    
    bool cleared = false;

    Rigidbody rb;
    Collider col;

    public bool Heavy => false;
    [SerializeField]
    Sprite _icon;

    public Sprite Icon => _icon;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        col = GetComponent<Collider>();
        col.enabled = false;
    }

    public void Interact(Character character)
    {
        col.enabled = false;
        rb.isKinematic = true;
        rb.useGravity = false;
        character.PickUpKey(transform,_icon);
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


