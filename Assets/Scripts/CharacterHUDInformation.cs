using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHUDInformation : MonoBehaviour
{
    [SerializeField] string passiveName;
    [SerializeField] string passiveTooltip;
    [SerializeField] Sprite passiveImage;
    [SerializeField] string distractionName;
    [SerializeField] string distractionTooltip;
    [SerializeField] Sprite distractionImage;
    [SerializeField] string abilityName;
    [SerializeField] string abilityTooltip;
    [SerializeField] Sprite abilityImage;

    public string PassiveName { get => passiveName; }
    public string PassiveTooltip { get => passiveTooltip;  }
    public Sprite PassiveImage { get => passiveImage; }   
    public string DistractionName { get => distractionName; }
    public string DistractionTooltip { get => distractionTooltip;}
    public Sprite DistractionImage { get => distractionImage; }
    public string AbilityName { get => abilityName;}
    public string AbilityTooltip { get => abilityTooltip;}
    public Sprite AbilityImage { get => abilityImage; }
}
