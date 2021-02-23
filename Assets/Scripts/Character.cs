using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private CharacterController characterController;
    private IMover _mover;
    private Rotator _rotator;
    private Ability _ability;

    [SerializeField] LayerMask whatCanWeInteractWith;
    [SerializeField] Transform pickedUpHeavyItemPosition;
    [SerializeField] Transform pickedUpLightItemPosition;
    [SerializeField] float speed = 7f;

    public bool inSafeZone = false;
    public bool RestrictMovement { get; set; }
    IInteractable interactableInFrontOfCharacter;
    public float Speed { get { return speed; } }
    public bool InSafeZone => inSafeZone;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        _mover = new WASDMover(this);
        _rotator = new Rotator(this);
        _ability = GetComponent<Ability>();
    }

    private void Start()
    {
         Controller.Instance.MoveModeTogglePressed += MoveModeTogglePressed;
    }

    private void MoveModeTogglePressed()
    {
        if (_mover is NavMeshMover)
            _mover = new WASDMover(this);
        else
            _mover = new NavMeshMover(this);
    }


    public void Tick()
    {
        if (!RestrictMovement)
        {
            _mover.Tick();
            _rotator.Tick();
        }
        Interact();
        _ability.Tick();
    }

    void Interact()
    {
        if (Controller.Instance.Interact)
        {
            Debug.Log("trying to interact");
            if (interactableInFrontOfCharacter != null)
            {
                Debug.Log(interactableInFrontOfCharacter.GetType());
                interactableInFrontOfCharacter.Interact(this);
            }
            else
            {
                Debug.Log("wtf am i doing here");
                DropHeldItem();
            }
        }
    }

    public void LookForInteractables()
    {
        RaycastHit hit;
        if (Physics.BoxCast(transform.position, transform.localScale, transform.forward, out hit, transform.rotation, 2f, whatCanWeInteractWith))
        {
            interactableInFrontOfCharacter = hit.collider.GetComponent<IInteractable>();
            Debug.Log(interactableInFrontOfCharacter);
            
        }
        else
        {
            Debug.Log("nothing");
            interactableInFrontOfCharacter = null;
        }
    }

    public void PickUpItem(IPickable pickable)
    {
        DropHeldItem();

        pickable.transform.SetParent(pickable.Heavy?pickedUpHeavyItemPosition:pickedUpLightItemPosition);
        pickable.transform.localPosition = Vector3.zero;
        var throwable = pickable as Throwable;
        if (throwable != null)
        {
            var throwAbility = _ability as ThrowObjects;
            if (throwAbility != null)
            {
                throwAbility.PickedUpObjectToThrow(throwable);
            }
        }
    }

    private void DropHeldItem()
    {
        var item = pickedUpLightItemPosition.GetComponentInChildren<IPickable>();
        if (item != null)
            item.Drop();
        else
        {
            item = pickedUpHeavyItemPosition.GetComponentInChildren<IPickable>();
            if (item != null)
                item.Drop();
        }
    }


    public void SetInteractable(IInteractable interactable)
    {
        interactableInFrontOfCharacter = interactable;
    }

    public IInteractable GetInteractable() => interactableInFrontOfCharacter;

    private void OnDestroy()
    {
        Controller.Instance.MoveModeTogglePressed -= MoveModeTogglePressed;
    }
}
