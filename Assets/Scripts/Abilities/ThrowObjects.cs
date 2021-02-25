using UnityEngine;

public class ThrowObjects : Ability
{
    [SerializeField] GameObject cursor;
    [SerializeField] LayerMask raycastLayer;
    [SerializeField] float throwMinRange = 5f;
    [SerializeField] float throwMaxRange = 20f;
    [SerializeField] float chargeDuration = 2f;

    float chargeTimer = 0;
    float currentThrowRange;
    bool startCharging = false;
    Camera cam;
    Throwable _objectToThrow;

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if(Using)
        {
            Vector3 mousePos = cam.ScreenToWorldPoint(Controller.Instance.MousePosition);
            Ray camRay = cam.ScreenPointToRay(Controller.Instance.MousePosition);
            RaycastHit hit;
            if(Physics.Raycast(camRay,out hit,100f,raycastLayer))
            {
                Vector3 origin = new Vector3(_objectToThrow.transform.position.x,hit.point.y, _objectToThrow.transform.position.z);
                Vector3 directionalVector = hit.point - origin;
                Vector3 point = directionalVector.magnitude < currentThrowRange ? hit.point : directionalVector.normalized * currentThrowRange;
                cursor.SetActive(true);
                cursor.transform.position = point + Vector3.up * 0.1f;

                Vector3 v = CalculateVelocity(point, _objectToThrow.transform.position, 1f);
                if(startCharging)
                {
                    if(Controller.Instance.LeftClickHold)
                    {
                        chargeTimer += Time.deltaTime;
                        float percentage = chargeTimer / chargeDuration;
                        percentage = Mathf.Clamp(percentage, 0, 1);
                        currentThrowRange = throwMinRange + (throwMaxRange - throwMinRange) * percentage;
                    }
                    else if(Controller.Instance.LeftClickRelease)
                    {
                        _objectToThrow.Throw(v,chargeTimer>=chargeDuration);
                        _objectToThrow = null;
                        OnTryUnuse();
                    }
                }
                else
                {
                    if(Controller.Instance.LeftClick)
                    {
                        startCharging = true;
                    }
                }
            }
            else
            {
                cursor.SetActive(false);
            }
        }
    }

    public void PickedUpObjectToThrow(Throwable objToThrow)
    {
        _objectToThrow = objToThrow;
        Debug.Log("ability armed");
    }

    protected override void OnTryUnuse()
    {
        Using = false;
        cursor.SetActive(false);
        character.RestrictMovement = false;
    }

    protected override void OnTryUse()
    {
        if(_objectToThrow!=null)
        {
            Using = true;
            startCharging = false;
            chargeTimer = 0;
            character.RestrictMovement = true;
        }
    }

    Vector3 CalculateVelocity(Vector3 target, Vector3 origin, float time)
    {
        Vector3 distance = target - origin;
        Vector3 distanceXZ = distance;
        distanceXZ.y = 0f;

        float Sy = distance.y;
        float Sxz = distanceXZ.magnitude;

        float Vxz = Sxz / time;
        float Vy = Sy / time + 0.5f * Mathf.Abs(Physics.gravity.y) * time;

        Vector3 result = distanceXZ.normalized * Vxz;
        result.y = Vy;

        return result;  
    }
}