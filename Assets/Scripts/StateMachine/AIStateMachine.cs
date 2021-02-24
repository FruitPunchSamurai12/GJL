using UnityEngine;
using UnityEngine.AI;
using System;


public class AIStateMachine : MonoBehaviour
{
    [SerializeField] bool startInIdle = false;
    [SerializeField] Transform[] waypoints;
    [SerializeField] float timeWaitingBetweenWaypoints = 2f;

    [SerializeField] float timeBeforeActingWhenSuspicious = 1f;
    [SerializeField] float timeInvestigatingWhenSuspicious = 5f;
    [SerializeField] float timeBeforeActingWhenAlerted = 0.5f;
    [SerializeField] float timeInvestigatingWhenAlerted = 15f;
    [SerializeField] float alertMoveSpeed = 10f;
    [SerializeField] float investigationRange = 5f;
    [SerializeField] float stunDuration = 5f;
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
        var idle = new Idle();
        var talk = new Talk(_ai, _navMeshAgent, _ai.MoveSpeed);
        var patrol = new Patrol(_ai,_navMeshAgent, waypoints, timeWaitingBetweenWaypoints, _ai.MoveSpeed);
        var sus = new Suspicious(_ai, _navMeshAgent, investigationRange, timeInvestigatingWhenSuspicious, timeBeforeActingWhenSuspicious, _ai.MoveSpeed);
        var alert = new Alert(_ai, _navMeshAgent, investigationRange, timeInvestigatingWhenAlerted, timeBeforeActingWhenAlerted, alertMoveSpeed);
        var chase = new ChasePlayer(_ai, _navMeshAgent, alertMoveSpeed);
        var stun = new Stun(_ai,_navMeshAgent, stunDuration);
        var sentry = new Sentry(_ai, _navMeshAgent, sentryRotationDuration, sentryWaitTime, sentryAngle);
        _stateMachine.AddTransition(idle, sus, _ai.CanSeeCharacter);
        _stateMachine.AddTransition(idle, sus, _ai.HeardSomething);
        _stateMachine.AddTransition(patrol, sus, _ai.CanSeeCharacter);
        _stateMachine.AddTransition(patrol, sus, _ai.HeardSomething);
        _stateMachine.AddTransition(sentry, sus, _ai.CanSeeCharacter);
        _stateMachine.AddTransition(sentry, sus, _ai.HeardSomething);
        _stateMachine.AddTransition(sus, alert, _ai.HasNoticedPlayer);
        _stateMachine.AddTransition(sus, patrol, sus.SearchedForTooLong);
        _stateMachine.AddTransition(alert, chase, _ai.HasDiscoveredPlayer);
        _stateMachine.AddTransition(alert, patrol, alert.SearchedForTooLong);
        _stateMachine.AddTransition(idle, talk, _ai.IsChitChatting);
        _stateMachine.AddTransition(patrol, talk, _ai.IsChitChatting);
        _stateMachine.AddTransition(sus, talk, _ai.IsChitChatting);
        _stateMachine.AddTransition(talk, sus, _ai.CanSeeCharacter);
        _stateMachine.AddTransition(talk, sus, _ai.HeardSomething);
        _stateMachine.AddAnyTransition(stun, _ai.IsStunned);
        _stateMachine.AddTransition(stun, alert, stun.StunTimeRunOut);

        if (startInIdle)
        {
            _stateMachine.SetState(sentry);
        }
        else
        {
            _stateMachine.AddTransition(talk, patrol, _ai.StoppedChitChatting);
            _stateMachine.SetState(patrol);
        }
    }

    private void Update()
    {
        _stateMachine.Tick();
    }

}
