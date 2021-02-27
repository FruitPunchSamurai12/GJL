﻿using UnityEngine;
using UnityEngine.UI;

public class ItemBox : MonoBehaviour
{
    [SerializeField]
    Sprite equippedSprite;
    [SerializeField]
    Sprite unequippedSprite;

    Image _image;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    private void OnEnable()
    {
        GameEvents.Instance.OnItemEquipped += EquipItem;
    }

    private void OnDisable()
    {
        GameEvents.Instance.OnItemEquipped -= EquipItem;
    }

    public void EquipItem(bool equipped)
    {
        if(equipped)
        {
            _image.sprite = equippedSprite;
        }
        else
        {
            _image.sprite = unequippedSprite;
        }
    }

}
