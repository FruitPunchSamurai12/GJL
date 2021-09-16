using UnityEngine;
using UnityEngine.AI;
using Pathfinding;

public class Reset:IState
{
    private NPC _ai;
    private AIPath _aiPath;
    Vector3 startPosition;
    Quaternion startRotation;

    public Reset(NPC ai, AIPath path, float moveSpeed)
    {
        _ai = ai;
        _aiPath = path;
        _aiPath.maxSpeed = moveSpeed;
        startPosition = _ai.transform.position;
        startRotation = _ai.transform.rotation;
    }

    public void OnEnter()
    {
        _ai.ResetHearing();
        _aiPath.enabled = true;
        _aiPath.isStopped = false;
        _ai.Target = startPosition;
        _ai.SetAnimatorBool("Move", true);
        _ai.SetAnimatorBool("Alert", false);
        _ai.ChangeMaterial(1);
        //AkSoundEngine.PostEvent("Play_NPC_Sus_Reset", _ai.gameObject);
    }

    public void OnExit()
    {
        _ai.transform.rotation = startRotation;
    }

    public void Tick()
    {
        //_navMeshAgent.SetDestination(_ai.Target);
        _ai.SetDestination();
    }
}