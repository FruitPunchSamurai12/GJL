using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour,IInteractable
{
    private CharacterController _characterController;
    private InteractBox _interactBox;
    private IMover _mover;
    private Rotator _rotator;
    private Animator _animator;
    private Ability[] _ability;
    MakeSound _footstepSound;
    [SerializeField] int characterIndex = 1;
    [SerializeField] Transform pickedUpHeavyItemPosition;
    [SerializeField] Transform pickedUpLightItemPosition;
    [SerializeField] Transform keyPosition;
    [SerializeField] float runSpeed = 7f;
    [SerializeField] float walkSpeed = 5f;
    [SerializeField] float hardcodedYValue;

    public bool halfSpeed = false;
    
    public bool RestrictMovement { get; set; }
    public float Speed { get { return halfSpeed?(Controller.Instance.Walk?walkSpeed:runSpeed)/2: Controller.Instance.Walk ? walkSpeed : runSpeed; } }
    public bool InSafeZone { get; set; }
    public int CharacterIndex => characterIndex;
    
    public AK.Wwise.Event PlayFootsteps;
    public AK.Wwise.Event PlayFlirt;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _interactBox = GetComponent<InteractBox>();
        _mover = new WASDMover(this);
        _rotator = new Rotator(this);
        _animator = GetComponent<Animator>();
        _ability = GetComponents<Ability>();
        _footstepSound = GetComponent<MakeSound>();
    }

    public void BackToCheckPoint()
    {
        _characterController.enabled = false;
        foreach (var ability in _ability)
        {
            ability.OnTryUnuse();
        }
        DropHeldItem();
        var key = keyPosition.GetComponentInChildren<SafeKey>();
        if (key != null)
        {
            key.Drop();
        }
        transform.position = GameManager.Instance.GetCheckpointPosition().position;
        _characterController.enabled = true;
    }



    public void Tick()
    {
        if (!RestrictMovement)
        {
            _mover.Tick();
            _rotator.Tick();
        }
        TryInteract();
        foreach (var ability in _ability)
        {
            ability.Tick();
        }
        Animate();
        _characterController.enabled = false;
        transform.position = new Vector3(transform.position.x,hardcodedYValue,transform.position.z);
        _characterController.enabled = true;
    }
  

    private void Animate()
    {
        Vector3 movementInput = new Vector3(Controller.Instance.Horizontal, 0, Controller.Instance.Vertical).normalized;
        float magnitude = (Controller.Instance.Walk || halfSpeed) ? movementInput.magnitude / 2f : movementInput.magnitude;
        magnitude = RestrictMovement ? 0 : magnitude;
        _animator.SetFloat("Speed", magnitude);
    }

    void TryInteract()
    {
        if (Controller.Instance.Interact)
        {
            Debug.Log("trying to interact");
            if (_interactBox.Interactable != null)
            {
                Debug.Log(_interactBox.Interactable.GetType());
                _interactBox.Interactable.Interact(this);
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
        _interactBox.LookForInteractables();
    }
    //this is horrible T_T
    public void ToggleInteractPrompt()
    {
        if (_interactBox.Interactable == null)
            InteractPrompt.DeactivateInteractPrompt();
        else
        {
            IPickable e = _interactBox.Interactable as IPickable;
            if (e != null)
                InteractPrompt.ActivateInteractPrompt(InteractType.pickUp);
            else
            {
                var d = _interactBox.Interactable as Door;
                if (d != null)
                {
                    if (d.IsLocked)
                    {
                        if (HasKey(false))
                            InteractPrompt.ActivateInteractPrompt(InteractType.unlock);
                    }
                    else
                    {
                        InteractPrompt.ActivateInteractPrompt(InteractType.open);
                    }
                }
                else
                {
                    if (characterIndex == 2)
                    {
                        var npc = _interactBox.Interactable as NPC;
                        if(npc!=null && !npc.CanSeeSpecificCharacter(this))
                        {
                            InteractPrompt.ActivateInteractPrompt(InteractType.flirt);
                        }
                    }
                    else
                    {
                        var c = _interactBox.Interactable as Character;
                        if(c!=null && (c.HasKey(false) || c.HasSafeKey(false)))
                        {
                            InteractPrompt.ActivateInteractPrompt(InteractType.pickUp);
                        }
                        else
                        {
                            var safe = _interactBox.Interactable as Safe;
                            if (HasSafeKey(false))
                                InteractPrompt.ActivateInteractPrompt(InteractType.unlock);
                            else
                                InteractPrompt.DeactivateInteractPrompt();
                        }
                    }
                

                }
            }
        }
    }

    public void PickUpItem(IPickable pickable)
    {
        DropHeldItem();
        GameEvents.Instance.ChangedEquippedItem(true,pickable.Icon);
        pickable.transform.SetParent(pickable.Heavy?pickedUpHeavyItemPosition:pickedUpLightItemPosition);
        pickable.transform.localPosition = Vector3.zero;
        pickable.transform.localRotation = Quaternion.identity;      
    }

    public void PickUpKey(Transform key,Sprite icon)
    {
        key.SetParent(keyPosition);
        key.localPosition = Vector3.zero;
        key.localRotation = Quaternion.identity;
        GameEvents.Instance.ChangedEquippedItem(true,icon);
        //key.PlayKeyPickup();
    }

    private void DropHeldItem()
    {
        var item = pickedUpLightItemPosition.GetComponentInChildren<IPickable>();
        if (item != null)
        {
            item.Drop();
            GameEvents.Instance.ChangedEquippedItem(false,null);
        }
        else
        {
            item = pickedUpHeavyItemPosition.GetComponentInChildren<IPickable>();
            if (item != null)
            {
                item.Drop();
                GameEvents.Instance.ChangedEquippedItem(false,null);
            }
        }
    }



    public void SetInteractable(IInteractable interactable)
    {
       // _interactBox.Interactable = interactable;
    }

    public IInteractable GetInteractable() => _interactBox.Interactable;

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

    public bool HasKey(bool destroyKey)
    {
        var key = keyPosition.GetComponentInChildren<Key>();
        if (key != null)
        {
            if (destroyKey)
                Destroy(key.gameObject);
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool HasSafeKey(bool destroyKey)
    {
        var key = keyPosition.GetComponentInChildren<SafeKey>();
        if (key != null)
        {
            if (destroyKey)
                Destroy(key.gameObject);
            return true;
        }
        else
        {
            return false;
        }
    }



    public bool HasDocument()
    {
        var document = pickedUpLightItemPosition.GetComponentInChildren<Document>();
        if (document != null)
            return true;
        else
            return false;
    }

    public void Interact(Character character)
    {
        foreach(var key in keyPosition.GetComponentsInChildren<Key>())
        {
            key.transform.SetParent(character.keyPosition);
            key.transform.localPosition = Vector3.zero;
            key.transform.localRotation = Quaternion.identity;
        }
    }

    public IPickable LightHeldObject()
    {
        var item = pickedUpLightItemPosition.GetComponentInChildren<IPickable>();
        if (item != null)
            return item;
        else
            return null;
    }

    public GameObject HeavyHeldObject()
    {
        var item = pickedUpHeavyItemPosition.GetComponentInChildren<Transform>();
        if (item != null)
            return item.gameObject;
        else
            return null;
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
            var weapon = pickedUpLightItemPosition.GetComponentInChildren<Weapon>();
            if (weapon != null)
            {
                weapon.StartSwing();
            }
        }
    }

    public void MomDistraction()
    {
        if(_interactBox.Interactable!=null)
        {
            var npc = _interactBox.Interactable as NPC;
            if (npc != null)
            {
                //npc.Interact(this);
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
    public void PlayCharacterFS()
    {
        PlayFootsteps.Post(gameObject);
        if (_footstepSound != null)
        {
            _footstepSound.PlaySound();
        }
    }


   
}
