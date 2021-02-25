using UnityEngine.AI;

public class Idle : IState
{
    NavMeshAgent _navMeshAgent;
    public Idle(NavMeshAgent navMeshAgent)
    {
        _navMeshAgent = navMeshAgent;
    }

    public void OnEnter()
    {
        _navMeshAgent.enabled = false;
    }

    public void OnExit()
    {
        
    }

    public void Tick()
    {
    }
}
