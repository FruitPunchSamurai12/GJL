using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Destructible : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField]
    float _velocityThreshold;
    Vector3 _velocity;
    public bool controlDestructionFromAnotherScipt = false;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        _velocity = rb.velocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (controlDestructionFromAnotherScipt)
            return;
        Destruct(_velocity);
    }

    public void Destruct(Vector3 velocity)
    {

        if (velocity.magnitude > _velocityThreshold)
        {
            Destroy(gameObject);
        }
    }
}
