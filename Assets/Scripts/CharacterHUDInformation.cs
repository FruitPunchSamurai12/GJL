using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHUDInformation : MonoBehaviour
{
    [SerializeField] string passiveName;
    [SerializeField] string passiveTooltip;
    [SerializeField] string distractionName;
    [SerializeField] string distractionTooltip;
    [SerializeField] string abilityName;
    [SerializeField] string abilityTooltip;

    public string PassiveName { get => passiveName; }
    public string PassiveTooltip { get => passiveTooltip;  }
    public string DistractionName { get => distractionName; }
    public string DistractionTooltip { get => distractionTooltip;}
    public string AbilityName { get => abilityName;}
    public string AbilityTooltip { get => abilityTooltip;}
}
