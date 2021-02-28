using UnityEngine;

public class Movable : MonoBehaviour,IPickable
{
    bool heavy = true;
    bool pickedUp = false;
    Collider col;
    Rigidbody rb;
    [SerializeField]
    Sprite _icon;

    bool IPickable.Heavy => heavy;

    public Sprite Icon =>  _icon;

    public AK.Wwise.Event Pickup;
    public AK.Wwise.Event Dropped;



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
        Dropped.Post(gameObject);
    }

    public void Interact(Character character)
    {
        if (pickedUp)
            return;
        rb.useGravity = false;
        col.enabled = false;
        pickedUp = true;
        character.PickUpItem(this);
        Pickup.Post(gameObject);
    }

}
