using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator
{
    private readonly Character _character;

    public Rotator(Character character)
    {
        _character = character;
    }

    public void Tick()
    {
        if (Controller.Instance.GetDirection() != Vector3.zero)
        {
            var rotation = Quaternion.LookRotation(Controller.Instance.GetDirection(), Vector3.up);
            _character.transform.rotation = rotation;
        }
    }
}
