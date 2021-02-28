using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    [SerializeField]
    SpinnySpin _spinnySpin;


    void Start()
    {
        _spinnySpin.BeginSpinning();
    }
    public void OnClickExit()
    {
        Application.Quit();
    }

    public void OnClickBack()
    {

    }

}
