using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float panSpeed = 2f;
    [SerializeField]
    private float timeBeforeReset = 3f;

    private float timer = 0;


    private CinemachineVirtualCamera virtualCamera;
    private Transform cameraTransform;
    private Transform currentTarget;

    private void Awake()
    {
        virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        cameraTransform = virtualCamera.transform;
    }

    public void SetCameraTarget(Transform target)
    {
        timer = 0;
        currentTarget = target;
        virtualCamera.m_Follow = currentTarget;        
    }

    // Update is called once per frame
    void Update()
    {
        if(Controller.Instance.MouseX!=0 || Controller.Instance.MouseY !=0)
        {                     
            virtualCamera.m_Follow = null;            
            timer = 0;
        }
        else
        {
            timer += Time.deltaTime;
            if(timer>timeBeforeReset)
            {
                SetCameraTarget(currentTarget);
            }
        }
        Vector2 mousePos = Controller.Instance.MousePosition;
        PanScreen(mousePos.x,mousePos.y);
    }

    Vector2 PanDirection(float x,float y)
    {
        Vector2 direction = Vector2.zero;
        if (y >= Screen.height * 0.98f)
            direction.y += 1;
        else if (y <= Screen.height * 0.02f)
            direction.y -= 1;
        if (x >= Screen.width * 0.98f)
            direction.x += 1;
        else if (x <= Screen.width * 0.02f)
            direction.x -= 1;
        return direction;
    }

    void PanScreen(float x,float y)
    {
        Vector2 dir2d = PanDirection(x, y);
        Vector3 direction = new Vector3(dir2d.x, 0f, dir2d.y);
        cameraTransform.position = Vector3.Lerp(cameraTransform.position, cameraTransform.position + direction * panSpeed, Time.deltaTime);
    }
}
