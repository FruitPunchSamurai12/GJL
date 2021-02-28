using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Destructible : MonoBehaviour
{
    public AK.Wwise.Event ItemBreak;
    Rigidbody rb;
    [SerializeField]
    protected float _velocityThreshold;
    [SerializeField]
    Broken brokenPrefab;
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

    public virtual void Destruct(Vector3 velocity)
    {

        if (velocity.magnitude > _velocityThreshold)
        {
            ItemBreak.Post(gameObject);
            if(brokenPrefab!=null)
            {
                brokenPrefab.gameObject.SetActive(true);
                brokenPrefab.transform.SetParent(null);
                brokenPrefab.Initialize(velocity);
            }
            Destroy(gameObject);
        }
    }
}
