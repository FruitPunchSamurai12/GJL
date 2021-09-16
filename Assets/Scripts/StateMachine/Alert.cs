using UnityEngine;
using UnityEngine.AI;
using Pathfinding;
public class Alert : IState
{
    NPC _ai;
    private AIPath _aiPath;

    float _searchDistance;
    float _searchDuration;
    float _idleTime;
    float _idleTimer = 0;
    float _totalTimer = 0;
    bool justEntered = true;
    float _moveSpeed;

    public Alert(NPC ai, AIPath path, float searchDistance, float searchDuration, float idleTime,float moveSpeed)
    {
        _ai = ai;
        _aiPath = path;
        _searchDistance = searchDistance;
        _searchDuration = searchDuration;
        _idleTime = idleTime;
        _moveSpeed = moveSpeed;
    }


    public void OnEnter()
    {
        GameEvents.Instance.FireAIEvent(AIStateEvents.enterAlert);
        _aiPath.enabled = true;
        _aiPath.maxSpeed = _moveSpeed;
        _idleTimer = 0;
        _totalTimer = 0;
        justEntered = true;
        _aiPath.isStopped = true;
        _ai.SetAnimatorBool("Alert", true);
        _ai.ChangeMaterial(3);
    }

    public void OnExit()
    {
        GameEvents.Instance.FireAIEvent(AIStateEvents.exitAlert);
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
                _aiPath.isStopped = false;
                _idleTimer = 0;
                // _navMeshAgent.SetDestination(_ai.Target);
                _ai.SetDestination();
                justEntered = false;
            }
        }
        else
        {
            if (_ai.ReachedDestination())
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
                //_navMeshAgent.SetDestination(_ai.Target);
                _ai.SetDestination();
            }
            _totalTimer += Time.deltaTime;
        }
    }

    public bool SearchedForTooLong() { return _totalTimer > _searchDuration; }
}
