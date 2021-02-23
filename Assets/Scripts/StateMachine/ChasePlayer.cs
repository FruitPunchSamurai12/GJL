using UnityEngine.AI;

public class ChasePlayer : IState
{
    NPC _ai;
    private NavMeshAgent _navMeshAgent;

    public ChasePlayer(NPC ai, NavMeshAgent agent,float moveSpeed)
    {
        _ai = ai;
        _navMeshAgent = agent;
        
        _navMeshAgent.speed = moveSpeed;
    }

    public void OnEnter()
    {
        _navMeshAgent.enabled = true;
        _navMeshAgent.isStopped = false;
    }

    public void OnExit()
    {
        
    }

    public void Tick()
    {
        _navMeshAgent.SetDestination(_ai.TargetCharacter.transform.position);
    }
}