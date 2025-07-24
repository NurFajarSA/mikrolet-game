using UnityEngine;

public class DestinationSpawner : MonoBehaviour
{
    [SerializeField] private GameObject destinationPrefab;
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
        
        destinationCounter++;
        
        Debug.Log($"Destination {passengerID} spawned at {destinationPoint}");
    }

    // Optional: Method to spawn destination at specific position with custom settings
    public GameObject SpawnDestinationWithReturn(Vector3 destinationPoint, int passengerID, Material passengerMaterial)
    {
        GameObject newDestination = Instantiate(destinationPrefab, destinationPoint, Quaternion.identity);
        Destination destinationScript = newDestination.GetComponent<Destination>();

        destinationScript.Initialize(passengerID, destinationPoint);
        
        if (passengerMaterial != null)
        {
            destinationScript.SetDestinationMaterial(passengerMaterial);
        }
        
        destinationCounter++;
        Debug.Log($"Destination {passengerID} spawned at {destinationPoint}");
        
        return newDestination;
    }
}