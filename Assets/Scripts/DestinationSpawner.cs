using UnityEngine;

public class DestinationSpawner : MonoBehaviour
{
    [SerializeField] private GameObject destinationPrefab;

    [Header("Indicator Settings")]
    [SerializeField] private GameObject destinationIndicatorPrefab;
    [SerializeField] private Transform carTransform;

    private int destinationCounter = 0;

    void Start()
    {
        // Initialize the spawner if needed
    }

    void Update()
    {
        // Update logic if needed
    }

    public void SpawnDestination(Vector3 destinationPoint, int passengerID, Material passengerMaterial)
    {
        GameObject newDestination = Instantiate(destinationPrefab, destinationPoint, Quaternion.identity);
        Destination destinationScript = newDestination.GetComponent<Destination>();

        destinationScript.Initialize(passengerID, destinationPoint);

        if (passengerMaterial != null)
        {
            destinationScript.SetDestinationMaterial(passengerMaterial);
        }

        if (destinationIndicatorPrefab != null && carTransform != null)
        {
            GameObject indicatorObj = Instantiate(destinationIndicatorPrefab);
            DestinationIndicator indicatorScript = indicatorObj.GetComponent<DestinationIndicator>();

            if (indicatorScript != null)
            {
                indicatorScript.Initialize(newDestination.transform, carTransform);
                indicatorScript.SetIndicatorMaterial(passengerMaterial);
            }
        }

        destinationCounter++;
        Debug.Log($"Destination {passengerID} spawned at {destinationPoint}");
    }
}