using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum InteractType
{
    flirt,
    pickUp,
    unlock,
    open
}

public class InteractPrompt : MonoBehaviour
{
     static bool interactableInFront = false;
     static InteractType interactableType;

    Image _image;
    [SerializeField]
    Sprite _flirtSprite;
    [SerializeField]
    Sprite _pickUpSprite;
    [SerializeField]
    Sprite _unlockSprite;
    [SerializeField]
    Sprite _openSprite;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    public static void ActivateInteractPrompt(InteractType type)
    {
        interactableInFront = true;
        interactableType = type;
    }

    public static void DeactivateInteractPrompt()
    {
        interactableInFront = false;
    }

    private void Update()
    {
        if(interactableInFront)
        {
            _image.enabled = true;
            switch (interactableType)
            {
                case InteractType.flirt:
                    _image.sprite = _flirtSprite;
                    break;
                case InteractType.pickUp:
                    _image.sprite = _pickUpSprite;
                    break;
                case InteractType.unlock:
                    _image.sprite = _unlockSprite;
                    break;
                case InteractType.open:
                    _image.sprite = _openSprite;
                    break;
            }
        }
        else
        {
            _image.enabled = false;
        }



    }
}