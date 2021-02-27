using UnityEngine;
using UnityEngine.AI;

public class Suspicious : IState
{
    NPC _ai;
    private NavMeshAgent _navMeshAgent;

    float _sweepAngle = 180;
    float _sweepDuration;
    float _sweepRotationSpeed;
    float _idleTime;
    float _idleTimer = 0;
    float _sweepTimer = 0;
    bool justEntered = true;
    private float _timeLerpStarted;

    Quaternion startRotation;
    Quaternion endRotation;
    bool _sweepStarted = false;

    public Suspicious(NPC ai,NavMeshAgent agent,float sweepRotationSpeed, float sweepDuration, float idleTime,float moveSpeed)
    {
        _ai = ai;
        _navMeshAgent = agent;
        _sweepRotationSpeed = sweepRotationSpeed;
        _sweepDuration = sweepDuration;
        _idleTime = idleTime;
        _navMeshAgent.speed = moveSpeed;
    }

    public void OnEnter()
    {
        GameEvents.Instance.FireAIEvent(AIStateEvents.enterSuspicious);
        _navMeshAgent.enabled = true;
        _idleTimer = 0;
        _sweepTimer = 0;
        justEntered = true;
        _navMeshAgent.isStopped = true;
        _sweepStarted = false;
        _ai.StopChitChatting();
        _ai.ChangeMaterial(2);
    }

    public void OnExit()
    {
        GameEvents.Instance.FireAIEvent(AIStateEvents.exitSuspicious);
        _navMeshAgent.isStopped = false;
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
            if (_ai.ReachedDestination())
            {
                if(!_sweepStarted)
                {
                    _timeLerpStarted = Time.time;
                    startRotation = _ai.transform.rotation;
                    //Vector3 endDirection = Quaternion.Euler(0, _sweepAngle, 0) * _ai.transform.forward;
                    //endRotation = Quaternion.FromToRotation(_ai.transform.forward, endDirection);
                    endRotation = startRotation * Quaternion.Euler(Vector3.up * _sweepAngle);
                    _sweepStarted = true;
                    _ai.SetAnimatorBool("Move", false);
                }
                else
                {
                    float timeSinceStarted = Time.time - _timeLerpStarted;
                    float percentageComplete = timeSinceStarted / _sweepRotationSpeed;
                    _ai.transform.rotation = Quaternion.Lerp(startRotation, endRotation, percentageComplete);
                    if (percentageComplete > 1)
                    {
                        var temp = startRotation;
                        startRotation = endRotation;
                        endRotation = temp;
                        _timeLerpStarted = Time.time;
                    }
                    
                }
                _sweepTimer += Time.deltaTime;
            }
            else
            {
                _ai.SetAnimatorBool("Move", true);
                _navMeshAgent.SetDestination(_ai.Target);
            }
            
        }
    }

    public bool SearchedForTooLong() { return _sweepTimer > _sweepDuration; }
}
