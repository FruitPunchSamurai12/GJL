using UnityEngine;
using UnityEngine.AI;

public class WASDMover : IMover
{
    private readonly Character _character;
    private readonly CharacterController _characterController;

    public WASDMover(Character character)
    {
        _character = character;
        _characterController = _character.GetComponent<CharacterController>();
        _character.GetComponent<NavMeshAgent>().enabled = false;
    }

    public void Tick()
    {
        Vector3 movementInput = new Vector3(Controller.Instance.Horizontal, 0, Controller.Instance.Vertical);
        Vector3 movement =  movementInput * _character.Speed;
        _characterController.SimpleMove(movement);
    }
}
