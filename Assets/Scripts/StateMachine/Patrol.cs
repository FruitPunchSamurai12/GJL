using UnityEngine;
using UnityEngine.AI;
using Pathfinding;

public class Patrol : IState
{
    NPC _ai;
    private AIPath _aiPath;
    Waypoint _waypoint;
    float _waitTime;
    float _moveSpeed;
    float timer = 0;
    public Patrol(NPC ai, AIPath path,Waypoint waypoint,float waitTime,float moveSpeed)
    {
        _ai = ai;
        _aiPath = path;
        _waypoint = waypoint;
        _waitTime = waitTime;
        _moveSpeed = moveSpeed;
    }

    public void OnEnter()
    {
        _ai.ResetHearing();
        _aiPath.enabled = true;
        _aiPath.isStopped = false;
        _aiPath.maxSpeed = _moveSpeed;
        _ai.SetAnimatorBool("Move", true);
        _ai.SetAnimatorBool("Alert", false);
        _ai.ChangeMaterial(1);
        _ai.Target = _waypoint.transform.position;
        _ai.SetDestination();
    }

    public void OnExit()
    {
        
    }

    public void Tick()
    {
        if (_aiPath.transform.position.FlatDistance(_waypoint.transform.position) < 1f)
        {
            if (_waypoint.waitAtWaypoint)
            {
                _ai.SetAnimatorBool("Move", false);
                timer += Time.deltaTime;
                if (timer > _waitTime)
                {
                    _ai.SetAnimatorBool("Move", true);
                    _waypoint = _waypoint.nextWaypoint;
                    _ai.Target = _waypoint.transform.position;
                    timer = 0;
                }
            }
            else
            {
                _waypoint = _waypoint.nextWaypoint;
                _ai.Target = _waypoint.transform.position;
            }
        }
        else
        {
            //_navMeshAgent.SetDestination(_waypoint.transform.position);
            _ai.SetDestination();
        }
    }
}
