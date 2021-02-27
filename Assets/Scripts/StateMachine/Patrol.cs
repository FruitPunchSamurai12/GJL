using UnityEngine;
using UnityEngine.AI;

public class Patrol : IState
{
    NPC _ai;
    private NavMeshAgent _navMeshAgent;
    Transform[] _waypoints;
    int _waypointIndex = 0;
    float _waitTime;
    float timer = 0;
    public Patrol(NPC ai, NavMeshAgent agent,Transform[] waypoints,float waitTime,float moveSpeed)
    {
        _ai = ai;
        _navMeshAgent = agent;
        _waypoints = waypoints;
        _waypointIndex = UnityEngine.Random.Range(0, _waypoints.Length - 1);
        _waitTime = waitTime;
        _navMeshAgent.speed = moveSpeed;
    }

    public void OnEnter()
    {
        _ai.ResetHearing();
        _navMeshAgent.enabled = true;
        _navMeshAgent.isStopped = false;
        _ai.SetAnimatorBool("Move", true);
        _ai.ChangeMaterial(1);
    }

    public void OnExit()
    {
        
    }

    public void Tick()
    {
        if (_navMeshAgent.transform.position.FlatDistance(_waypoints[_waypointIndex].position) < 2f)
        {
            timer += Time.deltaTime;
            if(timer>_waitTime)
            {
                int previousIndex = _waypointIndex;
                do
                {
                    _waypointIndex = UnityEngine.Random.Range(0, _waypoints.Length );
                } while (previousIndex == _waypointIndex);
                timer = 0;
            }
        }
        else
        {
            _navMeshAgent.SetDestination(_waypoints[_waypointIndex].position);
        }
    }
}
