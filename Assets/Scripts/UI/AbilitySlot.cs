using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum AbilityType
{
    passive,
    distraction,
    ability
}

public class AbilitySlot : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    public event Action<AbilityType> OnSlotClicked;

    [SerializeField] AbilityType _type;
    [SerializeField] Sprite _normalSprite;
    [SerializeField] Sprite _hoverSprite;
    [SerializeField] Sprite _activatedSprite;
    [SerializeField] Image _image;
    [SerializeField] AbilityTooltip _tooltip;
    string _slotName;
    string _slotTooltip;
    bool _activated = false;

    public void SetAbilitySlot(string slotName,string slotTooltip,Sprite sprite)
    {
        _slotName = slotName;
        _slotTooltip = slotTooltip;
        _image.sprite = sprite;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_type==AbilityType.passive)
            return;
        OnSlotClicked?.Invoke(_type);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(!_activated)
        {
           // _image.sprite = _hoverSprite;
        }
        _tooltip.ActivateTooltip(_slotName, _slotTooltip);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!_activated)
        {
            //_image.sprite = _normalSprite;
        }
        _tooltip.DeactivateTooltip();
    }
}
