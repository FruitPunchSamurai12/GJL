using UnityEngine.AI;

public class Talk : IState
{
    NPC _ai;
    private NavMeshAgent _navMeshAgent;
    public Talk(NPC ai, NavMeshAgent agent, float moveSpeed)
    {
        _ai = ai;
        _navMeshAgent = agent;       
        _navMeshAgent.speed = moveSpeed;
    }

    public void OnEnter()
    {
        _ai.ResetHearing();
        _navMeshAgent.enabled = true;
        _ai.SetAnimatorBool("Move", false);
        _ai.ChangeMaterial(1);
    }

    public void OnExit()
    {
        _navMeshAgent.enabled = false;
    }

    public void Tick()
    {
        _navMeshAgent.SetDestination(_ai.Target);
    }
}
