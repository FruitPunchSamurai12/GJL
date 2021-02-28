using UnityEngine;
using UnityEngine.AI;
using System;

public enum StartState
{
    idle,
    sentry,
    patrol
}
public class AIStateMachine : MonoBehaviour
{
    [SerializeField] Waypoint waypoint;
    [SerializeField] float timeWaitingBetweenWaypoints = 2f;
    [SerializeField] StartState startState;
    [SerializeField] float timeBeforeActingWhenSuspicious = 1f;
    [SerializeField] float sweepDurationWhenSuspicious = 5f;
    [SerializeField] float sweepRotationSpeedWhenSuspicious = 2.5f;
    [SerializeField] float timeBeforeActingWhenAlerted = 0.5f;
    [SerializeField] float timeInvestigatingWhenAlerted = 15f;
    [SerializeField] float alertMoveSpeed = 10f;
    [SerializeField] float investigationRange = 5f;
    [SerializeField] float sentryRotationDuration = 4f;
    [SerializeField] float sentryWaitTime = 1f;
    [SerializeField] float sentryAngle = 90;

    StateMachine _stateMachine;
    NavMeshAgent _navMeshAgent;
    NPC _ai;
    public Type CurrentStateType => _stateMachine.CurrentState.GetType();
    public event Action<IState> OnAIStateChanged;

    private void Awake()
    {
        var player = FindObjectOfType<Player>();
        _stateMachine = new StateMachine();
        _stateMachine.OnStateChanged += state => OnAIStateChanged?.Invoke(state);
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _ai = GetComponent<NPC>();
        _navMeshAgent.speed = _ai.MoveSpeed;
        var idle = new Idle(_ai,_navMeshAgent);
        var talk = new Talk(_ai, _navMeshAgent, _ai.MoveSpeed);
        var patrol = new Patrol(_ai,_navMeshAgent, waypoint, timeWaitingBetweenWaypoints, _ai.MoveSpeed);
        var sus = new Suspicious(_ai, _navMeshAgent, sweepRotationSpeedWhenSuspicious, sweepDurationWhenSuspicious, timeBeforeActingWhenSuspicious, _ai.MoveSpeed);
        var alert = new Alert(_ai, _navMeshAgent, investigationRange, timeInvestigatingWhenAlerted, timeBeforeActingWhenAlerted, alertMoveSpeed);
        var chase = new ChasePlayer(_ai, _navMeshAgent, alertMoveSpeed);
        var stun = new Stun(_ai,_navMeshAgent);
        var sentry = new Sentry(_ai, _navMeshAgent, sentryRotationDuration, sentryWaitTime, sentryAngle);
        var reset = new Reset(_ai, _navMeshAgent, _ai.MoveSpeed);
        _stateMachine.AddTransition(idle, sus, _ai.CanSeeCharacter);
        _stateMachine.AddTransition(idle, sus, _ai.HeardSomething);
        _stateMachine.AddTransition(idle, talk, _ai.IsChitChatting);
        _stateMachine.AddTransition(idle, alert, _ai.CanSeeKnockedOutNPCs);
        _stateMachine.AddTransition(patrol, sus, _ai.CanSeeCharacter);
        _stateMachine.AddTransition(patrol, sus, _ai.HeardSomething);
        _stateMachine.AddTransition(patrol, talk, _ai.IsChitChatting);
        _stateMachine.AddTransition(patrol, alert, _ai.CanSeeKnockedOutNPCs);
        _stateMachine.AddTransition(sentry, sus, _ai.CanSeeCharacter);
        _stateMachine.AddTransition(sentry, sus, _ai.HeardSomething);
        _stateMachine.AddTransition(sentry, talk, _ai.IsChitChatting);
        _stateMachine.AddTransition(sentry, alert, _ai.CanSeeKnockedOutNPCs);
        _stateMachine.AddTransition(sus, alert, _ai.HasNoticedPlayer);        
        _stateMachine.AddTransition(sus, talk, _ai.IsChitChatting);
        _stateMachine.AddTransition(sus, alert, _ai.CanSeeKnockedOutNPCs);
        _stateMachine.AddTransition(alert, chase, _ai.HasDiscoveredPlayer);
        _stateMachine.AddTransition(talk, sus, _ai.CanSeeCharacter);
        _stateMachine.AddTransition(talk, sus, _ai.HeardSomething);
        _stateMachine.AddTransition(talk, sus, _ai.StoppedChitChatting);
        _stateMachine.AddTransition(talk, alert, _ai.CanSeeKnockedOutNPCs);
        _stateMachine.AddTransition(reset, sus, _ai.CanSeeCharacter);
        _stateMachine.AddTransition(reset, sus, _ai.HeardSomething);
        _stateMachine.AddTransition(reset, talk, _ai.IsChitChatting);
        _stateMachine.AddAnyTransition(stun, _ai.IsStunned);


        if (startState==StartState.idle)
        {
            _stateMachine.AddTransition(sus, reset, sus.SearchedForTooLong);
            _stateMachine.AddTransition(alert, reset, alert.SearchedForTooLong);
            _stateMachine.AddTransition(chase, reset, chase.ReachedCharacter);
            _stateMachine.AddTransition(reset, idle, _ai.ReachedDestination);
            _stateMachine.SetState(idle);
        }
        else if (startState == StartState.patrol)
        {
            _stateMachine.AddTransition(sus, patrol, sus.SearchedForTooLong);
            _stateMachine.AddTransition(alert, patrol, alert.SearchedForTooLong);
            _stateMachine.AddTransition(chase, patrol, chase.ReachedCharacter);
            _stateMachine.SetState(patrol);
        }
        else if (startState == StartState.sentry)
        {
            _stateMachine.AddTransition(sus, reset, sus.SearchedForTooLong);
            _stateMachine.AddTransition(alert, reset, alert.SearchedForTooLong);
            _stateMachine.AddTransition(chase, reset, chase.ReachedCharacter);
            _stateMachine.AddTransition(reset, sentry, _ai.ReachedDestination);
            _stateMachine.SetState(sentry);
        }
        _stateMachine.AddTransition(chase, alert, chase.LostCharacter);
    }

    private void Update()
    {
        _stateMachine.Tick();
       
    }

}
