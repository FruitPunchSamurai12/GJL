using UnityEngine;
using UnityEngine.AI;

public class Stun : IState
{
    NPC _ai;
    NavMeshAgent _navMeshAgent;
    float _stunDuration;
    float _stunTimer;

    public Stun(NPC ai,NavMeshAgent agent,float stunDuration)
    {
        _ai = ai;
        _navMeshAgent = agent;
        _stunDuration = stunDuration;
    }

    public void OnEnter()
    {
        _navMeshAgent.enabled = false;
        _stunTimer = 0;
    }

    public void OnExit()
    {
        _ai.GetStunned(false);
    }

    public void Tick()
    {
        _stunTimer += Time.deltaTime;
        Debug.Log("stun timer " + _stunTimer);
    }

    public bool StunTimeRunOut() { return _stunTimer > _stunDuration; }
}