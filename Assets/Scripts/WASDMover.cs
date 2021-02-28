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
    }

    public void Tick()
    {
        Vector3 movementInput = new Vector3(-Controller.Instance.Horizontal, 0, -Controller.Instance.Vertical).normalized;
        Vector3 movement =  movementInput * _character.Speed*Time.deltaTime;
        _characterController.Move(movement);
    }
}
