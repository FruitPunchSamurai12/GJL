using UnityEngine;
using UnityEngine.AI;

public class Alert : IState
{
    NPC _ai;
    private NavMeshAgent _navMeshAgent;

    float _searchDistance;
    float _searchDuration;
    float _idleTime;
    float _idleTimer = 0;
    float _totalTimer = 0;
    bool justEntered = true;
    public Alert(NPC ai, NavMeshAgent agent, float searchDistance, float searchDuration, float idleTime,float moveSpeed)
    {
        _ai = ai;
        _navMeshAgent = agent;
        _searchDistance = searchDistance;
        _searchDuration = searchDuration;
        _idleTime = idleTime;
        _navMeshAgent.speed = moveSpeed;
    }


    public void OnEnter()
    {
        _navMeshAgent.enabled = true;
        _idleTimer = 0;
        _totalTimer = 0;
        justEntered = true;
        _navMeshAgent.isStopped = true;
    }

    public void OnExit()
    {
        _ai.ResetHearing();
    }

    public void Tick()
    {
        _ai.CanSeeCharacter();
        if (justEntered)
        {
            _idleTimer += Time.deltaTime;
            if (_idleTimer > _idleTime)
            {
                _navMeshAgent.isStopped = false;
                _idleTimer = 0;
                _navMeshAgent.SetDestination(_ai.Target);
                justEntered = false;
            }
        }
        else
        {
            if (_navMeshAgent.transform.position.FlatDistance(_ai.Target) < 2f)
            {
                _idleTimer += Time.deltaTime;
                if (_idleTimer > _idleTime)
                {
                    _ai.RandomTargetCloseToOriginalTarget(_searchDistance);
                }
            }
            else
            {
                _idleTimer = 0;
                _navMeshAgent.SetDestination(_ai.Target);
            }
            _totalTimer += Time.deltaTime;
        }
    }

    public bool SearchedForTooLong() { return _totalTimer > _searchDuration; }
}
