using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private CharacterController _characterController;
    private IMover _mover;
    private Rotator _rotator;
    private Animator _animator;
    private Ability[] _ability;
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
        _characterController = GetComponent<CharacterController>();
        _mover = new WASDMover(this);
        _rotator = new Rotator(this);
        _animator = GetComponent<Animator>();
        _ability = GetComponents<Ability>();
    }

    public void BackToCheckPoint()
    {
        _characterController.enabled = false;
        transform.position = GameManager.Instance.GetCheckpointPosition().position;
        foreach (var ability in _ability)
        {
            ability.OnTryUnuse();
        }
        _characterController.enabled = true;
    }



    public void Tick()
    {
        if (!RestrictMovement)
        {
            _mover.Tick();
            _rotator.Tick();
        }
        Interact();
        foreach (var ability in _ability)
        {
            ability.Tick();
        }
        DoMelee();
        Animate();
    }

    void DoMelee()
    {
        bool usingAbility = false;
        foreach (var ability in _ability)
        {
            if(ability.Using)
            {
                usingAbility = true;
                break;
            }
        }
        if (!usingAbility && Controller.Instance.LeftClick)
        {
            var weapon = pickedUpLightItemPosition.GetComponentInChildren<Melee>();
            if(weapon!=null)
            {
                weapon.StartSwing();
            }
        }
    }

   

    private void Animate()
    {
        Vector3 movementInput = new Vector3(Controller.Instance.Horizontal, 0, Controller.Instance.Vertical).normalized;
        float magnitude = (Controller.Instance.Walk || halfSpeed) ? movementInput.magnitude / 2f : movementInput.magnitude;
        magnitude = RestrictMovement ? 0 : magnitude;
        _animator.SetFloat("Speed", magnitude);
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
        GameEvents.Instance.ChangedEquippedItem(true);
        pickable.transform.SetParent(pickable.Heavy?pickedUpHeavyItemPosition:pickedUpLightItemPosition);
        pickable.transform.localPosition = Vector3.zero;
        pickable.transform.localRotation = Quaternion.identity;
        var throwable = pickable as Throwable;
        if (throwable != null)
        {
            foreach (var ability in _ability)
            {               
                var throwAbility = ability as ThrowObjects;
                if (throwAbility != null)
                {
                    throwAbility.PickedUpObjectToThrow(throwable);
                }
            }
        }
    }

    private void DropHeldItem()
    {
        var item = pickedUpLightItemPosition.GetComponentInChildren<IPickable>();
        if (item != null)
        {
            item.Drop();
            GameEvents.Instance.ChangedEquippedItem(false);
        }
        else
        {
            item = pickedUpHeavyItemPosition.GetComponentInChildren<IPickable>();
            if (item != null)
            {
                item.Drop();
                GameEvents.Instance.ChangedEquippedItem(false);
            }
        }
    }

    public void Flirt(bool flirtOn)
    {
        _animator.SetBool("Flirt", flirtOn);
        RestrictMovement = flirtOn;
        if (flirtOn)
        {
            InSafeZone = true;
        }
        else
        {
            LeaveSafeZoneAfterDelay(2f);
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

    public bool UsingAbility()
    {
        foreach (var ability in _ability)
        {
            if (ability.Using)
                return true;
        }
        return false;
    }

    //crappy functions for use for the hotbar

    public void DadDistraction()
    {
        if(_ability[0].Using)
        {
            _ability[0].OnTryUnuse();
        }
        else
        {
            _ability[0].OnTryUnuse();
        }
    }

    public void DadMelee()
    {
        bool usingAbility = false;
        foreach (var ability in _ability)
        {
            if (ability.Using)
            {
                usingAbility = true;
                break;
            }
        }
        if (!usingAbility)
        {
            var weapon = pickedUpLightItemPosition.GetComponentInChildren<Melee>();
            if (weapon != null)
            {
                weapon.StartSwing();
            }
        }
    }

    public void MomDistraction()
    {
        if(interactableInFrontOfCharacter!=null)
        {
            var npc = interactableInFrontOfCharacter as NPC;
            if (npc != null)
            {
                npc.Interact(this);
            }
        }
    }

    public void MomPickpocket()
    {
        _ability[0].OnTryUse();
    }

    public void BabyCry()
    {
        foreach (var ability in _ability)
        {
            var cry = ability as Cry;
            if(cry!=null)
            {
                if (cry.Using)
                    cry.OnTryUnuse();
                else
                    cry.OnTryUse();
            }
        }
    }

    public void BabyInnocence()
    {
        foreach (var ability in _ability)
        {
            var innocence = ability as Innocence;
            if (innocence != null)
            {
                if (innocence.Using)
                    innocence.OnTryUnuse();
                else
                    innocence.OnTryUse();
            }
        }
    }
}
