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

    public float Vertical => Input.GetAxisRaw("Vertical");
    public float Horizontal => Input.GetAxisRaw("Horizontal");

    public bool LeftClick => Input.GetMouseButtonDown(0);
    public bool LeftClickHold => Input.GetMouseButton(0);
    public bool LeftClickRelease => Input.GetMouseButtonUp(0);

    public bool AbilityInput => Input.GetKeyDown(KeyCode.Space);
    public bool DistractionInput => Input.GetKeyDown(KeyCode.Q);

    public bool Interact => Input.GetKeyDown(KeyCode.E);

    public bool PausePressed => Input.GetKeyDown(KeyCode.Escape);

    public bool Walk => Input.GetKey(KeyCode.LeftShift);

    public Vector2 MousePosition => Input.mousePosition;

    public float MouseX => Input.GetAxis("Mouse X");
    public float MouseY => Input.GetAxis("Mouse Y");
    

    public event Action<int> NumericKeyPressed;
    public event Action ToggleKeyPressed;
    private void Update()
    {
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
        return new Vector3(-Horizontal, 0, -Vertical);
    }

}
