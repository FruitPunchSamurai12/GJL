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
        var patrol = new Patrol(_navMeshAgent,waypoints,timeWaitingBetweenWaypoints,_ai.MoveSpeed);
        var sus = new Suspicious(_ai,_navMeshAgent, investigationRange, timeInvestigatingWhenSuspicious,timeBeforeActingWhenSuspicious,_ai.MoveSpeed);
        var alert = new Alert(_ai, _navMeshAgent, investigationRange, timeInvestigatingWhenAlerted, timeBeforeActingWhenAlerted,alertMoveSpeed);
        var chase = new ChasePlayer(_ai,_navMeshAgent,alertMoveSpeed);

        _stateMachine.AddTransition(idle, sus, _ai.CanSeeCharacter);
        _stateMachine.AddTransition(idle, sus, _ai.HeardSomething);
        _stateMachine.AddTransition(patrol, sus, _ai.CanSeeCharacter);
        _stateMachine.AddTransition(patrol, sus, _ai.HeardSomething);
        _stateMachine.AddTransition(sus,alert,_ai.HasNoticedPlayer);
        _stateMachine.AddTransition(sus, patrol, sus.SearchedForTooLong);
        _stateMachine.AddTransition(alert, chase, _ai.HasDiscoveredPlayer);
        _stateMachine.AddTransition(alert, patrol, alert.SearchedForTooLong);

        if (startInIdle)
            _stateMachine.SetState(idle);
        else
            _stateMachine.SetState(patrol);
    }


    private void Update()
    {
        _stateMachine.Tick();
    }

}

public class Idle : IState
{
    public Idle()
    {

    }

    public void OnEnter()
    {
        
    }

    public void OnExit()
    {
        
    }

    public void Tick()
    {
        
    }
}


public class Patrol : IState
{
    private NavMeshAgent _navMeshAgent;
    Transform[] _waypoints;
    int _waypointIndex = 0;
    float _waitTime;
    float timer = 0;
    public Patrol(NavMeshAgent agent,Transform[] waypoints,float waitTime,float moveSpeed)
    {
        _navMeshAgent = agent;
        _waypoints = waypoints;
        _waypointIndex = UnityEngine.Random.Range(0, _waypoints.Length - 1);
        _waitTime = waitTime;
        _navMeshAgent.speed = moveSpeed;
    }

    public void OnEnter()
    {
        _navMeshAgent.enabled = true;
    }

    public void OnExit()
    {
        
    }

    public void Tick()
    {
        if (_navMeshAgent.transform.position.FlatDistance(_waypoints[_waypointIndex].position) < 2f)
        {
            timer += Time.deltaTime;
            if(timer>_waitTime)
            {
                int previousIndex = _waypointIndex;
                do
                {
                    _waypointIndex = UnityEngine.Random.Range(0, _waypoints.Length - 1);
                } while (previousIndex == _waypointIndex);
                timer = 0;
            }
        }
        else
        {
            _navMeshAgent.SetDestination(_waypoints[_waypointIndex].position);
        }
    }
}


public class Suspicious : IState
{
    NPC _ai;
    private NavMeshAgent _navMeshAgent;

    float _searchDistance;
    float _searchDuration;
    float _idleTime;
    float _idleTimer = 0;
    float _totalTimer = 0;
    bool justEntered = true;
    public Suspicious(NPC ai,NavMeshAgent agent,float searchDistance,float searchDuration,float idleTime,float moveSpeed)
    {
        _ai = ai;
        _navMeshAgent = agent;
        _searchDistance = searchDistance;
        _searchDuration = searchDuration;
        _idleTime = idleTime;
        _navMeshAgent.speed = moveSpeed;
    }

    public void OnEnter()
    {
        _idleTimer = 0;
        _totalTimer = 0;
        justEntered = true;
    }

    public void OnExit()
    {
        
    }

    public void Tick()
    {
        _ai.CanSeeCharacter();
        if (justEntered)
        {
            _idleTimer += Time.deltaTime;
            if (_idleTimer > _idleTime)
            {
                _idleTimer = 0;
                _navMeshAgent.SetDestination(_ai.Target);
                justEntered = false;
            }
        }
        else
        {
            if (_navMeshAgent.transform.position.FlatDistance(_ai.Target) < 2f)
            {
                _idleTimer += Time.deltaTime;
                if (_idleTimer > _idleTime)
                {
                    _ai.RandomTargetCloseToOriginalTarget(_searchDistance);
                }
            }
            else
            {
                _idleTimer = 0;
                _navMeshAgent.SetDestination(_ai.Target);
            }
            _totalTimer += Time.deltaTime;
        }
    }

    public bool SearchedForTooLong() { return _totalTimer > _searchDuration; }
}


public class Alert : IState
{
    NPC _ai;
    private NavMeshAgent _navMeshAgent;

    float _searchDistance;
    float _searchDuration;
    float _idleTime;
    float _idleTimer = 0;
    float _totalTimer = 0;
    bool justEntered = true;
    public Alert(NPC ai, NavMeshAgent agent, float searchDistance, float searchDuration, float idleTime,float moveSpeed)
    {
        _ai = ai;
        _navMeshAgent = agent;
        _searchDistance = searchDistance;
        _searchDuration = searchDuration;
        _idleTime = idleTime;
        _navMeshAgent.speed = moveSpeed;
    }


    public void OnEnter()
    {
        _idleTimer = 0;
        _totalTimer = 0;
        justEntered = true;
    }

    public void OnExit()
    {
        _ai.ResetHearing();
    }

    public void Tick()
    {
        _ai.CanSeeCharacter();
        if (justEntered)
        {
            _idleTimer += Time.deltaTime;
            if (_idleTimer > _idleTime)
            {
                _idleTimer = 0;
                _navMeshAgent.SetDestination(_ai.Target);
                justEntered = false;
            }
        }
        else
        {
            if (_navMeshAgent.transform.position.FlatDistance(_ai.Target) < 2f)
            {
                _idleTimer += Time.deltaTime;
                if (_idleTimer > _idleTime)
                {
                    _ai.RandomTargetCloseToOriginalTarget(_searchDistance);
                }
            }
            else
            {
                _idleTimer = 0;
                _navMeshAgent.SetDestination(_ai.Target);
            }
            _totalTimer += Time.deltaTime;
        }
    }

    public bool SearchedForTooLong() { return _totalTimer > _searchDuration; }
}


public class ChasePlayer : IState
{
    NPC _ai;
    private NavMeshAgent _navMeshAgent;

    public ChasePlayer(NPC ai, NavMeshAgent agent,float moveSpeed)
    {
        _ai = ai;
        _navMeshAgent = agent;
        _navMeshAgent.speed = moveSpeed;
    }

    public void OnEnter()
    {
        
    }

    public void OnExit()
    {
        
    }

    public void Tick()
    {
        _navMeshAgent.SetDestination(_ai.TargetCharacter.transform.position);
    }
}