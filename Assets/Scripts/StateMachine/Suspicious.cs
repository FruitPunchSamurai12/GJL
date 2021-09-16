using UnityEngine;
using UnityEngine.AI;
using Pathfinding;

public class Suspicious : IState
{
    NPC _ai;
    private AIPath _aiPath;

    float _sweepAngle = 180;
    float _sweepDuration;
    float _sweepRotationSpeed;
    float _idleTime;
    float _idleTimer = 0;
    float _sweepTimer = 0;
    bool justEntered = true;
    private float _timeLerpStarted;

    float _failSafeTimer = 0;
    float _failSafeDuration = 15f;

    Quaternion startRotation;
    Quaternion endRotation;
    bool _sweepStarted = false;

    public Suspicious(NPC ai, AIPath path,float sweepRotationSpeed, float sweepDuration, float idleTime,float moveSpeed)
    {
        _ai = ai;
        _aiPath = path;
        _sweepRotationSpeed = sweepRotationSpeed;
        _sweepDuration = sweepDuration;
        _idleTime = idleTime;
        _aiPath.maxSpeed = moveSpeed;
    }

    public void OnEnter()
    {
        GameEvents.Instance.FireAIEvent(AIStateEvents.enterSuspicious);
        _aiPath.enabled = true;
        _idleTimer = 0;
        _sweepTimer = 0;
        justEntered = true;
        _aiPath.isStopped = true;
        _sweepStarted = false;
        _ai.StopChitChatting();
        _failSafeTimer = 0;
        _ai.ChangeMaterial(2);
        //AkSoundEngine.PostEvent("Play_NPC_Sus_React", _ai.gameObject);
    }

    public void OnExit()
    {
        GameEvents.Instance.FireAIEvent(AIStateEvents.exitSuspicious);
        _aiPath.isStopped = false;
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
                //_navMeshAgent.SetDestination(_ai.Target);
                _ai.SetDestination();
                justEntered = false;
            }
        }
        else
        {
            //var path = new NavMeshPath();
            //bool canReach = _navMeshAgent.CalculatePath(_ai.Target, path);//this doesnt work           
            _failSafeTimer += Time.deltaTime;
            //canReach = _failSafeTimer > _failSafeDuration;
            //Debug.Log("can reach " + canReach);
            if (_ai.ReachedDestination() || _failSafeTimer>_failSafeDuration)
            {
                if(!_sweepStarted)
                {
                    _timeLerpStarted = Time.time;
                    startRotation = _ai.transform.rotation;
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
                Debug.Log("got here");
                _ai.SetAnimatorBool("Move", true);
                //_navMeshAgent.SetDestination(_ai.Target);
                _ai.SetDestination();
            }
            
        }
    }

    public bool SearchedForTooLong() { return _sweepTimer > _sweepDuration; }
}
