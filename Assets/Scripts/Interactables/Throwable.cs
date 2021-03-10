using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour,IPickable
{
    [SerializeField] bool heavyItem;
    protected bool _pickedUp = false;
    protected bool _thrown = false;
    bool _maxPower = false;
    Collider col;
    protected MakeSound sound;
    protected Rigidbody rb;
    protected Vector3 _velocity;

    public bool Heavy => heavyItem;
    [SerializeField]
    Sprite _icon;

    public Sprite Icon => _icon;

    public AK.Wwise.Event ItemThrow;
    public AK.Wwise.Event ItemPickup;
    public AK.Wwise.Event ItemDrop;

    

    private void Awake()
    {
        col = GetComponent<Collider>();
        sound = GetComponent<MakeSound>();
        rb = GetComponent<Rigidbody>();        
    }

    public void Interact(Character character)
    {
        if (_pickedUp)
            return;
        col.enabled = false;
        _pickedUp = true;
        rb.isKinematic = true;
        rb.useGravity = false;
        character.PickUpItem(this);
        ItemPickup.Post(gameObject);

    }

    public void Drop()
    {
        transform.SetParent(null);
        _pickedUp = false;
        rb.isKinematic = false;
        rb.useGravity = true;
        col.enabled = true;
        ItemDrop.Post(gameObject);
    }

    public void Throw(Vector3 velocity,bool maxPower)
    {
        ItemThrow.Post(gameObject);
        transform.SetParent(null);
        rb.isKinematic = false;
        rb.useGravity = true;
        rb.velocity = velocity;
        //rb.AddTorque(velocity);
        col.enabled = true;
        _thrown = true;
        _maxPower = maxPower;
        var destructible = GetComponent<Destructible>();
        if(destructible!=null)
        {
            destructible.controlDestructionFromAnotherScipt = true;
        }
    }

    private void FixedUpdate()
    {
        _velocity = rb.velocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(_thrown)
        {
            if(_maxPower)
            {
                var npc = collision.collider.GetComponent<NPC>();
                Debug.Log(npc);
                if (npc != null)
                {                    
                    npc.GetStunned(_velocity/4f);
                }
                else
                {
                    var destructible = collision.collider.GetComponent<Destructible>();
                    if (destructible != null)
                    {
                        destructible.Destruct(_velocity);
                    }
                }
            }
            var destructibleSelf = GetComponent<Destructible>();
            if (destructibleSelf != null)
            {
                Debug.Log("destructible called from throwable");
                destructibleSelf.Destruct(_velocity);
                destructibleSelf.controlDestructionFromAnotherScipt = false;
            }
            _pickedUp = false;
            _thrown = false;
        }
    }
}
