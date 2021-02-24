using UnityEngine;

public class Movable : MonoBehaviour,IPickable
{
    bool heavy = true;
    bool pickedUp = false;
    Collider col;
    Rigidbody rb;

    bool IPickable.Heavy => heavy;



    private void Awake()
    {
        col = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
    }

    public void Drop()
    {
        transform.SetParent(null);
        pickedUp = false;
        rb.isKinematic = false;
        rb.useGravity = true;
        col.enabled = true;
    }

    public void Interact(Character character)
    {
        if (pickedUp)
            return;
        rb.useGravity = false;
        col.enabled = false;
        pickedUp = true;
        character.PickUpItem(this);
    }

}
