using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Broken : MonoBehaviour
{
    Piece[] pieces;

    private void Awake()
    {
        pieces = GetComponentsInChildren<Piece>();
    }

    public void Initialize(Vector3 velocity)
    {
        foreach (var piece in pieces)
        {
            piece.Initialize(velocity);
        }
        Destroy(gameObject, 5f);
    }
}
