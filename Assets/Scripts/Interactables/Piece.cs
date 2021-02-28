using UnityEngine;

public class Piece:MonoBehaviour
{
    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Initialize(Vector3 velocity)
    {
        rb.velocity = velocity;
        Destroy(gameObject, 5f);
    }
}