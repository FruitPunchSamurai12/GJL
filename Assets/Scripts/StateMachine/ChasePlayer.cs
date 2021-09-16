using UnityEngine.AI;
using UnityEngine;
using Pathfinding;

public class ChasePlayer : IState
{
    NPC _ai;
    private AIPath _aiPath;
    bool reachedCharacter;
    float _moveSpeed;

    public ChasePlayer(NPC ai, AIPath path, float moveSpeed)
    {
        _ai = ai;
        _aiPath = path;
        
        _moveSpeed = moveSpeed;
    }

    public void OnEnter()
    {
        GameEvents.Instance.FireAIEvent(AIStateEvents.enterAlert);
        _aiPath.enabled = true;
        _aiPath.isStopped = false;
        _aiPath.maxSpeed = _moveSpeed;
        _ai.IgnoreSounds = true;
        reachedCharacter = false;
        GameManager.Instance.CharacterIsBeingChased(_ai.TargetCharacter);
        //AkSoundEngine.PostEvent("Play_NPC_Chase_Begins", _ai.gameObject);
    }

    public void OnExit()
    {
        GameEvents.Instance.FireAIEvent(AIStateEvents.exitAlert);
        _ai.IgnoreSounds = false;
        GameManager.Instance.CharacterIsNoLongerBeingChased(_ai.TargetCharacter);
    }

    public void Tick()
    {
        _ai.transform.LookAt(new Vector3(_ai.TargetCharacter.transform.position.x, 0, _ai.TargetCharacter.transform.position.z));
        //_navMeshAgent.SetDestination(_ai.TargetCharacter.transform.position);
        _ai.SetDestination();
        if (_ai.transform.position.FlatDistance(_ai.TargetCharacter.transform.position) < 1.5)
        {
            _ai.TargetCharacter.BackToCheckPoint();
            reachedCharacter = true;
        }
    }

    public bool ReachedCharacter() { return reachedCharacter; }
    public bool LostCharacter() { return !_ai.CanSeeCharacter(); }
}
