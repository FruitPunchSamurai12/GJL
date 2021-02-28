using UnityEngine;
using UnityEngine.AI;

public class Patrol : IState
{
    NPC _ai;
    private NavMeshAgent _navMeshAgent;
    Waypoint _waypoint;
    float _waitTime;
    float _moveSpeed;
    float timer = 0;
    public Patrol(NPC ai, NavMeshAgent agent,Waypoint waypoint,float waitTime,float moveSpeed)
    {
        _ai = ai;
        _navMeshAgent = agent;
        _waypoint = waypoint;
        _waitTime = waitTime;
        _moveSpeed = moveSpeed;
    }

    public void OnEnter()
    {
        _ai.ResetHearing();
        _navMeshAgent.enabled = true;
        _navMeshAgent.isStopped = false;
        _navMeshAgent.speed = _moveSpeed;
        _ai.SetAnimatorBool("Move", true);
        _ai.SetAnimatorBool("Alert", false);
        _ai.ChangeMaterial(1);
    }

    public void OnExit()
    {
        
    }

    public void Tick()
    {
        if (_navMeshAgent.transform.position.FlatDistance(_waypoint.transform.position) < 1f)
        {
            if (_waypoint.waitAtWaypoint)
            {
                _ai.SetAnimatorBool("Move", false);
                timer += Time.deltaTime;
                if (timer > _waitTime)
                {
                    _ai.SetAnimatorBool("Move", true);
                    _waypoint = _waypoint.nextWaypoint;
                    timer = 0;
                }
            }
            else
            {
                _waypoint = _waypoint.nextWaypoint;
            }
        }
        else
        {
            _navMeshAgent.SetDestination(_waypoint.transform.position);
        }
    }
}
