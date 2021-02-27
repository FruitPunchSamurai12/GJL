using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSlot : MonoBehaviour
{
    [SerializeField]
    Sprite _unselectedSprite;
    [SerializeField]
    Sprite _selectedSprite;
    Image _image;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    public void Select()
    {
        _image.sprite = _selectedSprite;
        _image.rectTransform.sizeDelta = new Vector2(128, 128);
    }
    public void Unselect()
    {
        _image.sprite = _unselectedSprite;
        _image.rectTransform.sizeDelta = new Vector2(64, 64);
    }
}
