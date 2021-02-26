using UnityEngine.AI;
using UnityEngine;

public class ChasePlayer : IState
{
    NPC _ai;
    private NavMeshAgent _navMeshAgent;
    bool reachedCharacter;
    float _moveSpeed;

    public ChasePlayer(NPC ai, NavMeshAgent agent,float moveSpeed)
    {
        _ai = ai;
        _navMeshAgent = agent;
        
        _moveSpeed = moveSpeed;
    }

    public void OnEnter()
    {
        _navMeshAgent.enabled = true;
        _navMeshAgent.isStopped = false;
        _navMeshAgent.speed = _moveSpeed;
        _ai.IgnoreSounds = true;
        reachedCharacter = false;
        GameManager.Instance.CharacterIsBeingChased(_ai.TargetCharacter);
    }

    public void OnExit()
    {
        _ai.IgnoreSounds = false;
        GameManager.Instance.CharacterIsNoLongerBeingChased(_ai.TargetCharacter);
    }

    public void Tick()
    {
        _ai.transform.LookAt(_ai.TargetCharacter.transform);
        _navMeshAgent.SetDestination(_ai.TargetCharacter.transform.position);
        if(_ai.transform.position.FlatDistance(_ai.TargetCharacter.transform.position) < 1.5)
        {
            _ai.TargetCharacter.BackToCheckPoint();
            reachedCharacter = true;
        }
    }

    public bool ReachedCharacter() { return reachedCharacter; }
    public bool LostCharacter() { return !_ai.CanSeeCharacter(); }
}
