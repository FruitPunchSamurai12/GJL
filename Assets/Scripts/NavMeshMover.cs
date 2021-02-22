using UnityEngine;
using UnityEngine.AI;

public class NavMeshMover : IMover
{
    private readonly Character _character;
    private readonly NavMeshAgent _navmeshAgent;

    public NavMeshMover(Character character)
    {
        _character = character;
        _navmeshAgent = _character.GetComponent<NavMeshAgent>();
        _navmeshAgent.enabled = true;

    }
    public void Tick()
    {
        if (Controller.Instance.LeftClick)
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Controller.Instance.MousePosition), out var hitInfo))
            {
                _navmeshAgent.SetDestination(hitInfo.point);
            }
        }
    }
}
