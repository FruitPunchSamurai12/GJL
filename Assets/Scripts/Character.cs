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
    [SerializeField] int characterIndex = 1;
    [SerializeField] LayerMask whatCanWeInteractWith;
    [SerializeField] Transform pickedUpHeavyItemPosition;
    [SerializeField] Transform pickedUpLightItemPosition;
    [SerializeField] float runSpeed = 7f;
    [SerializeField] float walkSpeed = 5f;

    public bool halfSpeed = false;
    
    public bool RestrictMovement { get; set; }
    IInteractable interactableInFrontOfCharacter;
    public float Speed { get { return halfSpeed?(Controller.Instance.Walk?walkSpeed:runSpeed)/2: Controller.Instance.Walk ? walkSpeed : runSpeed; } }
    public bool InSafeZone { get; set; }
    public int CharacterIndex => characterIndex;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        _mover = new WASDMover(this);
        _rotator = new Rotator(this);
        _ability = GetComponent<Ability>();
    }

    public void BackToCheckPoint()
    {
        characterController.enabled = false;
        transform.position = GameManager.Instance.GetCheckpointPosition().position;
        characterController.enabled = true;
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
        DoMelee();
    }

    void DoMelee()
    {
        if(!_ability.Using && Controller.Instance.LeftClick)
        {
            var weapon = pickedUpLightItemPosition.GetComponentInChildren<Melee>();
            if(weapon!=null)
            {
                weapon.StartSwing();
            }
        }
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
        //RaycastHit hit;
        var targets = Physics.OverlapBox(transform.position, transform.localScale, transform.rotation, whatCanWeInteractWith);
        if(targets.Length>0)
        {
            interactableInFrontOfCharacter = targets[0].GetComponent<IInteractable>();
        }
        else
        {
            interactableInFrontOfCharacter = null;
        }
    }

    public void PickUpItem(IPickable pickable)
    {
        DropHeldItem();

        pickable.transform.SetParent(pickable.Heavy?pickedUpHeavyItemPosition:pickedUpLightItemPosition);
        pickable.transform.localPosition = Vector3.zero;
        pickable.transform.localRotation = Quaternion.identity;
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

    public void LeaveSafeZoneAfterDelay(float delay)
    {
        StartCoroutine(LeaveSafeZone(delay));
    }

    IEnumerator LeaveSafeZone(float delay)
    {
        yield return new WaitForSeconds(delay);
        InSafeZone = false;
    }

    private void OnDestroy()
    {
        Controller.Instance.MoveModeTogglePressed -= MoveModeTogglePressed;
    }
}
