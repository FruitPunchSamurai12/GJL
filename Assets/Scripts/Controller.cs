using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public static Controller Instance;

    private void Awake()
    {
        if(Instance==null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public float Vertical => Input.GetAxis("Vertical");
    public float Horizontal => Input.GetAxis("Horizontal");

    public bool LeftClick => Input.GetMouseButton(0);

    public bool PausePressed => Input.GetKeyDown(KeyCode.Escape);

    public Vector2 MousePosition => Input.mousePosition;

    

    public event Action MoveModeTogglePressed;
    public event Action<int> NumericKeyPressed;
    public event Action ToggleKeyPressed;
    private void Update()
    {
        if (MoveModeTogglePressed != null && Input.GetKeyDown(KeyCode.Minus))
            MoveModeTogglePressed?.Invoke();
        for (int i = 0; i < 3; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                NumericKeyPressed?.Invoke(i);
            }
        }
        if (Input.GetKeyDown(KeyCode.Tab))
            ToggleKeyPressed?.Invoke();
    }

    

    public Vector3 GetDirection()
    {
        return new Vector3(Horizontal, 0, Vertical);
    }

}
