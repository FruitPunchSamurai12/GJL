using UnityEngine.AI;

public class Idle : IState
{
    NPC _ai;
    NavMeshAgent _navMeshAgent;
    public Idle(NPC ai,NavMeshAgent navMeshAgent)
    {
        _ai = ai;
        _navMeshAgent = navMeshAgent;
    }

    public void OnEnter()
    {
        _navMeshAgent.enabled = false;
        _ai.SetAnimatorBool("Move", false);
        _ai.ChangeMaterial(1);
    }

    public void OnExit()
    {
        
    }

    public void Tick()
    {
        _ai.CanSeeCharacter();
    }
}
