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
        if (destinationPrefab == null)
        {
            Debug.LogError("DestinationSpawner: destinationPrefab is not assigned!");
        }
        else
        {
            Renderer[] prefabRenderers = destinationPrefab.GetComponentsInChildren<Renderer>();
            if (prefabRenderers.Length == 0)
            {
                Debug.LogError("DestinationSpawner: destinationPrefab does not have Renderer components in children!");
            }
        }

        if (destinationIndicatorPrefab == null)
        {
            Debug.LogError("DestinationSpawner: Destination indicator prefab is not assigned!");
        }

        if (carTransform == null)
        {
            Debug.LogError("DestinationSpawner: Car transform is not assigned!");
        }
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
                Debug.Log("Destination indicator spawned for destination at " + destinationPoint);
            }
            else
            {
                Debug.LogError("DestinationIndicator component not found on indicator prefab!");
                Destroy(indicatorObj);
            }
        }

        destinationCounter++;
        Debug.Log($"Destination {passengerID} spawned at {destinationPoint}");
    }
}