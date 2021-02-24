using UnityEngine.AI;
using UnityEngine;

public class Sentry : IState
{
    NPC _ai;
    private NavMeshAgent _navMeshAgent;
    private float _waitTime;
    private float _waitTimer;
    private float _timeLerpStarted;
    private float _lerpDuration;
    Quaternion startRotation;
    Quaternion endRotation;

    public Sentry(NPC ai, NavMeshAgent agent, float rotationDuration,float waitTime,float angle)
    {
        _ai = ai;
        _navMeshAgent = agent;
        _lerpDuration = rotationDuration;
        _waitTime = waitTime;
        startRotation = _ai.transform.rotation;
        Vector3 endDirection =  Quaternion.Euler(0, angle, 0) * _ai.transform.forward;
        endRotation = Quaternion.FromToRotation(_ai.transform.forward,endDirection);
    }

    public void OnEnter()
    {
        _navMeshAgent.enabled = false;
        _timeLerpStarted = Time.time;
        _waitTimer = 0;
    }

    public void OnExit()
    {

    }

    public void Tick()
    {
        if (_waitTimer > _waitTime)
        {
            float timeSinceStarted = Time.time - _timeLerpStarted;
            float percentageComplete = timeSinceStarted / _lerpDuration;
            _ai.transform.rotation = Quaternion.Lerp(startRotation, endRotation, percentageComplete);
            if(percentageComplete>1)
            {
                var temp = startRotation;
                startRotation = endRotation;
                endRotation = temp;
                _waitTimer = 0;
            }
        }
        else
        {
            _waitTimer += Time.deltaTime;
            if( _waitTimer > _waitTime)
            {
                _timeLerpStarted = Time.time;
            }
        }
    }
}