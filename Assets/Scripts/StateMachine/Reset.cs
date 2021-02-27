using UnityEngine;
using UnityEngine.AI;

public class Reset:IState
{
    private NPC _ai;
    private NavMeshAgent _navMeshAgent;
    Vector3 startPosition;
    Quaternion startRotation;

    public Reset(NPC ai, NavMeshAgent agent, float moveSpeed)
    {
        _ai = ai;
        _navMeshAgent = agent;
        _navMeshAgent.speed = moveSpeed;
        startPosition = _ai.transform.position;
        startRotation = _ai.transform.rotation;
    }

    public void OnEnter()
    {
        _ai.ResetHearing();
        _navMeshAgent.enabled = true;
        _navMeshAgent.isStopped = false;
        _ai.Target = startPosition;
        _ai.SetAnimatorBool("Move", true);
        _ai.SetAnimatorBool("Alert", false);
        _ai.ChangeMaterial(1);
    }

    public void OnExit()
    {
        _ai.transform.rotation = startRotation;
    }

    public void Tick()
    {
        _navMeshAgent.SetDestination(_ai.Target);
    }
}