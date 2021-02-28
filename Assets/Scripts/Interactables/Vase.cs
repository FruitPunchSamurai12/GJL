using UnityEngine;

public class Vase:Destructible
{
    [SerializeField]
    SafeKey key;
    [SerializeField]
    Transform keyPos;



    public override void Destruct(Vector3 velocity)
    {
        if(velocity.magnitude>_velocityThreshold)
        {
            key.Drop();
            base.Destruct(velocity);
        }
    }

}