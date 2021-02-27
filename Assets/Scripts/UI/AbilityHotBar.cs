using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityHotBar : MonoBehaviour
{
    [SerializeField] AbilitySlot _passiveSlot;
    [SerializeField] AbilitySlot _distractionSlot;
    [SerializeField] AbilitySlot _abilitySlot;
    Player _player;

    private void Awake()
    {
        _player = FindObjectOfType<Player>();
    }

    private void OnEnable()
    {
        RegisterSlotsForClickCallback();
        _player.OnCharacterChanged += HandleCharacterChanged;
       HandleCharacterChanged(_player.GetCurrentCharacterTransform().GetComponent<CharacterHUDInformation>());
    }

    private void OnDisable()
    {
        UnregisterSlotsForClickCallback();
        _player.OnCharacterChanged -= HandleCharacterChanged;        
    }


    private void RegisterSlotsForClickCallback()
    {
        _distractionSlot.OnSlotClicked += HandleSlotClicked;
        _abilitySlot.OnSlotClicked += HandleSlotClicked;
    }
    private void UnregisterSlotsForClickCallback()
    {
        _distractionSlot.OnSlotClicked -= HandleSlotClicked;
        _abilitySlot.OnSlotClicked -= HandleSlotClicked;
    }

    private void HandleSlotClicked(AbilityType type)
    {
        _player.HandleAbilityClicked(type);
    }

    private void HandleCharacterChanged(CharacterHUDInformation character)
    {
        _passiveSlot.SetAbilitySlot(character.PassiveName, character.PassiveTooltip);
        _distractionSlot.SetAbilitySlot(character.DistractionName, character.DistractionTooltip);
        _abilitySlot.SetAbilitySlot(character.AbilityName, character.AbilityTooltip);
    }
}
