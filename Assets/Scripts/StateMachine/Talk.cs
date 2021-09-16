using UnityEngine.AI;
using Pathfinding;

public class Talk : IState
{
    NPC _ai;
    private AIPath _aiPath;
    public Talk(NPC ai, AIPath path, float moveSpeed)
    {
        _ai = ai;
        _aiPath = path;
        _aiPath.maxSpeed = moveSpeed;
    }

    public void OnEnter()
    {
        _ai.ResetHearing();
        _aiPath.enabled = true;
        _ai.SetAnimatorBool("Move", false);
        _ai.ChangeMaterial(1);
    }

    public void OnExit()
    {
        _aiPath.enabled = false;
    }

    public void Tick()
    {
        //_navMeshAgent.SetDestination(_ai.Target);
        _ai.SetDestination();
    }
}
