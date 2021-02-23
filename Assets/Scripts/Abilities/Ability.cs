using UnityEngine;

public abstract class Ability:MonoBehaviour
{
    public bool Using { get; protected set; }
    protected Character character;
    private void Awake()
    {
        character = GetComponent<Character>();
    }

    public void Tick()
    {
        if(Controller.Instance.Ability)
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
