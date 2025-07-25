using UnityEngine;

public class PassengerIndicator : MonoBehaviour
{
    private Transform carTransform;
    private Transform targetPassenger;

    private const float MINIMAP_RADIUS = 18f;
    private const float INDICATOR_HEIGHT = 22f;

    public void Initialize(Transform passenger, Transform car)
    {
        targetPassenger = passenger;
        carTransform = car;
    }

    void LateUpdate()
    {
        if (targetPassenger == null || carTransform == null)
        {
            Destroy(gameObject);
            return;
        }

        UpdateIndicatorPosition();
    }

    private void UpdateIndicatorPosition()
    {
        Vector3 passengerPos = targetPassenger.position;
        Vector3 carPos = carTransform.position;
        
        float deltaX = passengerPos.x - carPos.x;
        float deltaZ = passengerPos.z - carPos.z;
        
        Vector3 indicatorPos;
        
        if (Mathf.Abs(deltaX) <= MINIMAP_RADIUS && Mathf.Abs(deltaZ) <= MINIMAP_RADIUS)
        {
            indicatorPos = new Vector3(passengerPos.x, INDICATOR_HEIGHT, passengerPos.z);
        }
        else
        {
            float clampedX = carPos.x + Mathf.Clamp(deltaX, -MINIMAP_RADIUS, MINIMAP_RADIUS);
            float clampedZ = carPos.z + Mathf.Clamp(deltaZ, -MINIMAP_RADIUS, MINIMAP_RADIUS);
            
            indicatorPos = new Vector3(clampedX, INDICATOR_HEIGHT, clampedZ);
        }
        
        transform.position = indicatorPos;
    }
}
