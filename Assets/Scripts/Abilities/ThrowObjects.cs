using UnityEngine;

public class ThrowObjects : Ability
{
    [SerializeField] GameObject cursor;
    [SerializeField] LayerMask raycastLayer;
    [SerializeField] float throwMinRange = 5f;
    [SerializeField] float throwMaxRange = 20f;
    [SerializeField] float chargeDuration = 2f;
    [SerializeField] float throwTime = 0.5f;

    float chargeTimer = 0;
    float currentThrowRange;
    bool startCharging = false;
    Camera cam;
    Throwable _objectToThrow;
    public AK.Wwise.Event ThrowItem;
    public AK.Wwise.Event ChargeThrow;

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if (Using)
        {
            //Vector3 mousePos = Controller.Instance.MousePosition;          
            Vector2 mousePos = (Vector2)Camera.main.ScreenToViewportPoint(Controller.Instance.MousePosition);
            Vector2 positionOnScreen = Camera.main.WorldToViewportPoint(transform.position);
            Vector2 lookDirection = mousePos - positionOnScreen;
            float targetAngle = Mathf.Atan2(lookDirection.x, lookDirection.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
            _objectToThrow.transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
            //new Vector3(_objectToThrow.transform.position.x,hit.point.y, _objectToThrow.transform.position.z);
            //Vector3 directionalVector = Vector3.ClampMagnitude(mousePoint - origin,currentThrowRange);
            Vector3 point = transform.position + transform.forward * currentThrowRange;
            //Vector3 point = directionalVector.magnitude < currentThrowRange ? hit.point : directionalVector.normalized * currentThrowRange;
            //cursor.SetActive(true);
            //cursor.transform.position = point;
            //cursor.transform.position = point + Vector3.up * 0.1f;

            Vector3 v = CalculateVelocity(point, _objectToThrow.transform.position, throwTime);
            if (startCharging)
            {
                if (Controller.Instance.LeftClickHold)
                {
                    chargeTimer += Time.deltaTime;
                    float percentage = chargeTimer / chargeDuration;
                    percentage = Mathf.Clamp(percentage, 0, 1);
                    currentThrowRange = throwMinRange + (throwMaxRange - throwMinRange) * percentage;
                }
                else if (Controller.Instance.LeftClickRelease)
                {
                    _objectToThrow.Throw(v, chargeTimer >= chargeDuration);
                    _objectToThrow = null;
                    OnTryUnuse();
                }
            }
            else
            {
                if (Controller.Instance.LeftClick)
                {
                    startCharging = true;
                    ChargeThrow.Post(gameObject);
                }
            }
        }
        else
        {
            //cursor.SetActive(false);
        }

    }

    float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }


    public override void OnTryUnuse()
    {
        Using = false;
        cursor.SetActive(false);
        cursor.transform.localPosition = Vector3.zero;
        character.RestrictMovement = false;
    }

    public override void OnTryUse()
    {
        var item = character.LightHeldObject();
        if (item != null)
        {
            _objectToThrow = item as Throwable;
            if (_objectToThrow != null)
            {
                Using = true;
                startCharging = false;
                chargeTimer = 0;
                character.RestrictMovement = true;
                currentThrowRange = throwMinRange;
            }
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