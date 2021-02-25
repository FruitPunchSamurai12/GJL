using System;
using UnityEngine;

public abstract class Ability:MonoBehaviour
{
    [SerializeField]
    [Range(1, 2)]
    int abilityNumber = 1;
    public bool Using { get; protected set; }
    protected Character character;
    private void Awake()
    {
        character = GetComponent<Character>();
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
    }

    protected abstract void OnTryUse();
    protected abstract void OnTryUnuse();
}
