using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour,IPickable
{
    [SerializeField] bool heavyItem;
    protected bool pickedUp = false;
    protected bool throwed = false;
    Collider col;
    MakeSound sound;
    Rigidbody rb;

    public bool Heavy => heavyItem;

    private void Awake()
    {
        col = GetComponent<Collider>();
        sound = GetComponent<MakeSound>();
        sound.enabled = false;
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    public void Interact(Character character)
    {
        if (pickedUp)
            return;
        col.enabled = false;
        pickedUp = true;
        rb.isKinematic = true;
        rb.useGravity = false;
        character.PickUpItem(this);
    }

    public void Drop()
    {
        transform.SetParent(null);
        pickedUp = false;
        rb.isKinematic = false;
        rb.useGravity = true;
        col.enabled = true;
    }

    public void Throw(Vector3 velocity)
    {
        transform.SetParent(null);
        rb.isKinematic = false;
        rb.useGravity = true;
        rb.velocity = velocity;
        //rb.AddTorque(velocity);
        col.enabled = true;
        throwed = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(throwed)
        {
            col.enabled = false;
            rb.isKinematic = true;
            rb.velocity = Vector3.zero;
            sound.enabled = true;
        }
    }
}
