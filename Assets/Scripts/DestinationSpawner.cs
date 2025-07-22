using UnityEngine;

public class DestinationSpawner : MonoBehaviour
{
    [SerializeField] private GameObject destinationPrefab;
    private int destinationCounter = 0;

    void Start()
    {
        // Initialize if needed
    }

    void Update()
    {
        // Update logic if needed
    }

    // Changed to public (non-static) method
    public void SpawnDestination(Vector3 destinationPoint)
    {
        GameObject newDestination = Instantiate(destinationPrefab, destinationPoint, Quaternion.identity);
        Destination destinationScript = newDestination.GetComponent<Destination>();

        destinationScript.Initialize(destinationCounter, destinationPoint);
        destinationCounter++;
        
        Debug.Log($"Destination {destinationCounter} spawned at {destinationPoint}");
    }
    
    // Optional: Method to spawn destination at specific position with custom settings
    public GameObject SpawnDestinationWithReturn(Vector3 destinationPoint)
    {
        GameObject newDestination = Instantiate(destinationPrefab, destinationPoint, Quaternion.identity);
        Destination destinationScript = newDestination.GetComponent<Destination>();

        destinationScript.Initialize(destinationCounter, destinationPoint);
        destinationCounter++;
        
        Debug.Log($"Destination {destinationCounter} spawned at {destinationPoint}");
        
        return newDestination;
    }
}