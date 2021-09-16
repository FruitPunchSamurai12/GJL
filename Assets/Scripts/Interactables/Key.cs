using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour,IInteractable
{
    public AK.Wwise.Event PickupKey;
    public bool Heavy => false;


    public Sprite _icon;
    protected Collider col;

    private void Awake()
    {
        col = GetComponent<Collider>();
        transform.localPosition = Vector3.zero;
    }

    public void Interact(Character character)
    {
        col.enabled = false;
        character.PickUpKey(transform,_icon);
    }

    public void PlayKeyPickup()
    {
        PickupKey.Post(gameObject);

    }

    public void EnableCollider() { col.enabled = true; }

    public InteractType GetInteractType(Character character)
    {
        return InteractType.pickUp;
    }
}
