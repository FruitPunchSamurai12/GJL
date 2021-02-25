using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ww_Listener_Orientation : MonoBehaviour
{
    public GameObject RotationSource;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = RotationSource.transform.rotation;
    }
}
