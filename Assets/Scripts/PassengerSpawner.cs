using UnityEngine;

public class PassengerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject passengerPrefab;
    
    [Header("Passenger Materials")]
    [SerializeField] private Material[] passengerMaterials;

    private int passengerCounter = 0;

    private Vector3[] spawnPoints = new Vector3[]
    {
        // North East Passenger
        new Vector3(-43.8f, 1, 80),
        new Vector3(55, 1, 91.3f),
        new Vector3(55.89999f, 1, 54.8f),
        new Vector3(-44.5f, 1, 135.5f),
        new Vector3(-39.8f, 1, 169.4f),

        // North West Passenger
        new Vector3(-73.5f, 1, 104.8f),
        new Vector3(-100.2f, 1, 79.7f),
        new Vector3(-121.3f, 1, 110.8f),
        new Vector3(-105f, 1, 169.4f),
        new Vector3(-73.7f, 1, 161f),

        // South East Passenger
        new Vector3(-24.7f, 1, 49.2f),
        new Vector3(-45.9f, 1, -23.2f),
        new Vector3(-45.4f, 1, 23.1f),
        new Vector3(55.60001f, 1, 26.8f),
        new Vector3(30.8f, 1, -23.3f),

        // South West Passenger
        new Vector3(-117.7f, 1, 0.3999996f),
        new Vector3(-163.9f, 1, 46.1f),
        new Vector3(-90.1f, 1, 48.7f),
        new Vector3(-73.6f, 1, -8.400002f),
        new Vector3(-118.7f, 1, 25.9f),
    };

    private Vector3[] destinationPoints = new Vector3[]
    {
        // North East Destination
        new Vector3(-29.1f, 0.04f, 139.2f),
        new Vector3(-16.2f, 0.04f, 168.8f),
        new Vector3(-6f, 0.04f, 77.5f),
        new Vector3(30.10001f, 0.04f, 104.5f),
        new Vector3(-70.6f, 0.04f, 93.2f),

        // North West Destination
        new Vector3(-118.7f, 0.04f, 134.7f),
        new Vector3(-73.3f, 0.04f, 129f),
        new Vector3(-146.9f, 0.04f, 80.3f),
        new Vector3(-74.6f, 0.04f, 172.2f),
        new Vector3(-85.9f, 0.04f, 80.2f),

        // South East Destination
        new Vector3(52.2f, 0.04f, -17f),
        new Vector3(-13.10001f, 0.04f, -26f),
        new Vector3(-2f, 0.04f, 22.8f),
        new Vector3(-46.5f, 0.04f, 46f),
        new Vector3(56.39999f, 0.04f, 41.7f),

        // South West Destination
        new Vector3(-98.5f, 0.04f, -25.4f),
        new Vector3(-139.4f, 0.04f, -0.7999992f),
        new Vector3(-105.3f, 0.04f, 23.4f),
        new Vector3(-112.9f, 0.04f, 53.8f),
        new Vector3(-165.6f, 0.04f, 39f)
    };

    void Start()
    {
       if (passengerMaterials == null || passengerMaterials.Length == 0)
        {
            Debug.LogWarning("PassengerSpawner: No passenger materials assigned! Using default materials.");
        }
        
        // SpawnPassenger();
        SpawnMultiplePassengers(20);
    }

    void SpawnPassenger()
    {
        int randIndex = Random.Range(0, spawnPoints.Length);
        int randDestIndex = Random.Range(0, destinationPoints.Length);

        GameObject newPassenger = Instantiate(passengerPrefab, spawnPoints[randIndex], Quaternion.identity);
        Passenger passengerScript = newPassenger.GetComponent<Passenger>();

        passengerScript.Initialize(passengerCounter, destinationPoints[randDestIndex]);
        
        SetRandomMaterialForPassenger(passengerScript);
        passengerCounter++;
    }

    void SpawnMultiplePassengers(int count)
    {
        Debug.Log("Spawning " + count + " passengers");

        System.Collections.Generic.List<int> availableIndexes = new System.Collections.Generic.List<int>();
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            availableIndexes.Add(i);
        }

        for (int i = 0; i < count && availableIndexes.Count > 0; i++)
        {
            int randListIndex = Random.Range(0, availableIndexes.Count);
            int spawnIndex = availableIndexes[randListIndex];
            availableIndexes.RemoveAt(randListIndex);

            int destIndex = spawnIndex < destinationPoints.Length ? spawnIndex : Random.Range(0, destinationPoints.Length);

            GameObject newPassenger = Instantiate(passengerPrefab, spawnPoints[spawnIndex], Quaternion.identity);
            Passenger passengerScript = newPassenger.GetComponent<Passenger>();
            passengerScript.Initialize(passengerCounter, destinationPoints[destIndex]);
            
            SetRandomMaterialForPassenger(passengerScript);
            passengerCounter++;
        }
    }
    
    private void SetRandomMaterialForPassenger(Passenger passenger)
    {
        if (passengerMaterials != null && passengerMaterials.Length > 0)
        {
            Material randomMaterial = passengerMaterials[Random.Range(0, passengerMaterials.Length)];
            passenger.SetPassengerMaterial(randomMaterial);
        }
    }
}