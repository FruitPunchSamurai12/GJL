using UnityEngine;
using UnityEngine.AI;
using Pathfinding;

public class Stun : IState
{
    NPC _ai;
    AIPath _aiPath;

    public Stun(NPC ai, AIPath path)
    {
        _ai = ai;
        _aiPath = path;
    }

    public void OnEnter()
    {
        _ai.IgnoreSounds = true;
        _aiPath.enabled = false;
        _ai.SetAnimatorBool("Alert", false);
        _ai.SetAnimatorBool("Move", false);
        _ai.ChangeMaterial(1);
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