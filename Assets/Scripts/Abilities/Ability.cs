using System;
using UnityEngine;

public abstract class Ability:MonoBehaviour
{
    [SerializeField]
    AbilityType abilityType;
    [SerializeField] protected string animationBool;
    protected Animator animator;
    public bool Using { get; protected set; }
    protected Character character;
    public AbilityType GetAbilityType => abilityType;
    private void Awake()
    {
        character = GetComponent<Character>();
        animator = GetComponent<Animator>();
    }

    public void Tick()
    {
        bool input = abilityType == AbilityType.ability ? Controller.Instance.AbilityInput : Controller.Instance.DistractionInput;
        if(input)
        {
            AbilityInputPressed();          
        }
        animator.SetBool(animationBool, Using);
    }

    public void AbilityInputPressed()
    {
        if (!Using)
            OnTryUse();
        else
            OnTryUnuse();
    }

    protected abstract void OnTryUse();
    public abstract void OnTryUnuse();
}
