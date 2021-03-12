using System;
using System.Collections.Generic;
using UnityEngine;

public enum AIStateEvents
{
    enterSuspicious,
    exitSuspicious,
    enterAlert,
    exitAlert
}

public class GameEvents : MonoBehaviour
{
    public event Action<NPC> OnEnemyStunned;
    public event Action OnEnemyEnterSuspicious;
    public event Action OnEnemyExitSuspicious;
    public event Action OnEnemyEnterAlert;
    public event Action OnEnemyExitAlert;
    public event Action OnObjectiveClear;
    public event Action<bool,Sprite> OnItemEquipped;

    public static GameEvents Instance;

    private void Awake()
    {
        if(Instance==null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void EnemyStunned(NPC npc)
    {
        OnEnemyStunned?.Invoke(npc);
    }

    public void ClearedObjective()
    {
        OnObjectiveClear?.Invoke();
    }

    public void FireAIEvent(AIStateEvents stateEvent)
    {
        switch (stateEvent)
        {
            case AIStateEvents.enterSuspicious:
                OnEnemyEnterSuspicious?.Invoke();
                break;
            case AIStateEvents.exitSuspicious:
                OnEnemyExitSuspicious?.Invoke();
                break;
            case AIStateEvents.enterAlert:
                OnEnemyEnterAlert?.Invoke();
                break;
            case AIStateEvents.exitAlert:
                OnEnemyExitAlert?.Invoke();
                break;
        }
    }

    public void ChangedEquippedItem(bool equiped,Sprite icon)
    {
        OnItemEquipped?.Invoke(equiped,icon);
    }
}