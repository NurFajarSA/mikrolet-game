using UnityEngine;

public class DestinationIndicator : MonoBehaviour
{
    private Transform carTransform;
    private Transform targetDestination;
    private Renderer indicatorRenderer;

    private const float MINIMAP_RADIUS = 18f;
    private const float INDICATOR_HEIGHT = 23f;

    public void Initialize(Transform destination, Transform car)
    {
        targetDestination = destination;
        carTransform = car;
    }

    void Awake()
    {
        indicatorRenderer = GetComponent<Renderer>();
    }

    void LateUpdate()
    {
        if (targetDestination == null || carTransform == null)
        {
            Destroy(gameObject);
            return;
        }

        UpdateIndicatorPosition();
    }

    private void UpdateIndicatorPosition()
    {
        Vector3 destinationPos = targetDestination.position;
        Vector3 carPos = carTransform.position;

        float deltaX = destinationPos.x - carPos.x;
        float deltaZ = destinationPos.z - carPos.z;

        Vector3 indicatorPos;

        if (Mathf.Abs(deltaX) <= MINIMAP_RADIUS && Mathf.Abs(deltaZ) <= MINIMAP_RADIUS)
        {
            indicatorPos = new Vector3(destinationPos.x, INDICATOR_HEIGHT, destinationPos.z);
        }
        else
        {
            float clampedX = carPos.x + Mathf.Clamp(deltaX, -MINIMAP_RADIUS, MINIMAP_RADIUS);
            float clampedZ = carPos.z + Mathf.Clamp(deltaZ, -MINIMAP_RADIUS, MINIMAP_RADIUS);

            indicatorPos = new Vector3(clampedX, INDICATOR_HEIGHT, clampedZ);
        }

        transform.position = indicatorPos;
    }
    
    public void SetIndicatorMaterial(Material mat)
    {
        indicatorRenderer.material = mat;
    }
}