using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCStateMachine : MonoBehaviour
{
    StateMachine _stateMachine;
    NavMeshAgent _navMeshAgent;

    public Type CurrentStateType => _stateMachine.CurrentState.GetType();
    public event Action<IState> OnEntityStateChanged;

    private void Awake()
    {
        var player = FindObjectOfType<Player>();
        _stateMachine = new StateMachine();
        _stateMachine.OnStateChanged += state => OnEntityStateChanged?.Invoke(state);
        _navMeshAgent = GetComponent<NavMeshAgent>();

        var idle = new Idle();
        var patrol = new Patrol();
        var suspicious = new Suspicious();
        var alert = new Alert();

       
        //_stateMachine.AddTransition(idle, chasePlayer, () => DistanceFlat(_navMeshAgent.transform.position, player.transform.position) < 5f);
        //_stateMachine.AddTransition(chasePlayer, attack, () => DistanceFlat(_navMeshAgent.transform.position, player.transform.position) < 2f);

        _stateMachine.SetState(idle);
    }

    private float DistanceFlat(Vector3 source, Vector3 destination)
    {
        
        return Vector3.Distance(new Vector3(source.x, 0, source.z), new Vector3(destination.x, 0, destination.z));
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
        throw new NotImplementedException();
    }

    public void OnExit()
    {
        throw new NotImplementedException();
    }

    public void Tick()
    {
        throw new NotImplementedException();
    }
}

public class Patrol : IState
{
    public Patrol()
    {

    }

    public void OnEnter()
    {
        throw new NotImplementedException();
    }

    public void OnExit()
    {
        throw new NotImplementedException();
    }

    public void Tick()
    {
        throw new NotImplementedException();
    }
}

public class Suspicious : IState
{
    public Suspicious()
    {

    }

    public void OnEnter()
    {
        throw new NotImplementedException();
    }

    public void OnExit()
    {
        throw new NotImplementedException();
    }

    public void Tick()
    {
        throw new NotImplementedException();
    }
}

public class Alert : IState
{
    public Alert()
    {

    }

    public void OnEnter()
    {
        throw new NotImplementedException();
    }

    public void OnExit()
    {
        throw new NotImplementedException();
    }

    public void Tick()
    {
        throw new NotImplementedException();
    }
}