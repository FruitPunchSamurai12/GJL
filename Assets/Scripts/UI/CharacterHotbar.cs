using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHotbar : MonoBehaviour
{
    CharacterSlot[] _slots;
    int _currentIndex = 0;

    private void OnEnable()
    {
        Controller.Instance.NumericKeyPressed += NumericKeyPressed;
        Controller.Instance.ToggleKeyPressed += ToggleKeyPressed;
        _slots = GetComponentsInChildren<CharacterSlot>();
    }
    private void OnDisable()
    {
        Controller.Instance.NumericKeyPressed -= NumericKeyPressed;
        Controller.Instance.ToggleKeyPressed -= ToggleKeyPressed;
    }


    private void NumericKeyPressed(int index)
    {
        if (_slots.Length <= index || index < 0)
        {
            return;
        }
        _currentIndex = index;
        SelectCurrentSlot();
    }
    private void ToggleKeyPressed()
    {
        _currentIndex++;
        if (_currentIndex >= _slots.Length)
            _currentIndex = 0;
        SelectCurrentSlot();
    }

    private void SelectCurrentSlot()
    {
        for (int i = 0; i < _slots.Length; i++)
        {
            if (i == _currentIndex)
            {
                _slots[_currentIndex].Select();
            }
            else
            {
                _slots[i].Unselect();
            }
        }
    }

}
