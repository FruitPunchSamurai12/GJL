using System;
using UnityEngine;

public abstract class Ability:MonoBehaviour
{
    [SerializeField]
    [Range(1, 2)]
    int abilityNumber = 1;
    [SerializeField] protected string animationBool;
    protected Animator animator;
    public bool Using { get; protected set; }
    protected Character character;
    private void Awake()
    {
        character = GetComponent<Character>();
        animator = GetComponent<Animator>();
    }

    public void Tick()
    {
        bool input = abilityNumber == 1 ? Controller.Instance.Ability1 : Controller.Instance.Ability2;
        if(input)
        {
            if (!Using)
                OnTryUse();
            else
                OnTryUnuse();           
        }
        animator.SetBool(animationBool, Using);
    }

    protected abstract void OnTryUse();
    public abstract void OnTryUnuse();
}
