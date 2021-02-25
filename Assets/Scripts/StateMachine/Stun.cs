using UnityEngine;
using UnityEngine.AI;

public class Stun : IState
{
    NPC _ai;
    NavMeshAgent _navMeshAgent;

    public Stun(NPC ai,NavMeshAgent agent)
    {
        _ai = ai;
        _navMeshAgent = agent;
    }

    public void OnEnter()
    {
        _ai.IgnoreSounds = true;
        _navMeshAgent.enabled = false;
    }

    public void OnExit()
    {
        _ai.GetStunned(false);
        _ai.IgnoreSounds = false;
    }

    public void Tick()
    {
    }

}