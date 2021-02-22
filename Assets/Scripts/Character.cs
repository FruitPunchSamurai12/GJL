using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private CharacterController characterController;
    private IMover _mover;
    private Rotator _rotator;

    [SerializeField] float speed = 7f;
    bool inSafeZone = false;
    public float Speed { get { return speed; } }
    public bool InSafeZone => inSafeZone;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        _mover = new WASDMover(this);
        _rotator = new Rotator(this);
        Controller.Instance.MoveModeTogglePressed += MoveModeTogglePressed;

    }

    private void MoveModeTogglePressed()
    {
        if (_mover is NavMeshMover)
            _mover = new WASDMover(this);
        else
            _mover = new NavMeshMover(this);
    }


    public void Tick()
    {
        _mover.Tick();
        _rotator.Tick();
    }

    private void OnDestroy()
    {
        Controller.Instance.MoveModeTogglePressed -= MoveModeTogglePressed;
    }
}
