using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinnySpin : MonoBehaviour
{
    [SerializeField]
    float rotateSpeed = 10f;
    bool beginSpin = false;

    // Update is called once per frame
    void Update()
    {
        if(beginSpin)
        {
            transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);
        }
    }

    public void BeginSpinning()
    {
        beginSpin = true;
    }
}
