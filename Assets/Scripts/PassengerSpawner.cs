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
        new Vector3(-70.48529f, 1.137f, 123.9953f),
        new Vector3(-29.50529f, 1.137f, 122.5853f),
        new Vector3(-52.87529f, 1.137f, 171.7053f),
        new Vector3(-119.1953f, 1.137f, 196.4853f),
        new Vector3(-99.46529f, 1.137f, 164.9553f),

        // North West Passenger
        new Vector3(-134.7653f, 1.137f, 140.3653f),
        new Vector3(-166.8053f, 1.137f, 125.9453f),
        new Vector3(-200.2553f, 1.137f, 112.2853f),
        new Vector3(-167.3853f, 1.137f, 165.5553f),
        new Vector3(-144.8253f, 1.137f, 196.4153f),

        // South East Passenger
        new Vector3(-97.59529f, 1.137f, 107.3553f),
        new Vector3(-127.9753f, 1.137f, 65.2553f),
        new Vector3(-76.48529f, 1.137f, 81.3353f),
        new Vector3(-47.07529f, 1.137f, 51.28531f),
        new Vector3(-45.21529f, 1.137f, 107.4453f),

        // South West Passenger
        new Vector3(-144.0953f, 1.137f, 100.8553f),
        new Vector3(-193.3153f, 1.137f, 95.06531f),
        new Vector3(-145.6953f, 1.137f, 50.2653f),
        new Vector3(-187.8153f, 1.137f, 61.84531f),
        new Vector3(-156.7953f, 1.137f, 82.8793f),
    };

    private Vector3[] destinationPoints = new Vector3[]
    {
        // North East Destination
        new Vector3(-104.6953f, 0.1838198f, 189.9553f),
        new Vector3(-123.7953f, 0.1838198f, 171.0943f),
        new Vector3(-35.67529f, 0.1838198f, 146.5053f),
        new Vector3(-99.49529f, 0.1838198f, 141.7553f),
        new Vector3(-78.19529f, 0.1838198f, 108.0653f),

        // North West Destination
        new Vector3(-158.2553f, 0.1838198f, 182.6953f),
        new Vector3(-133.7353f, 0.1838198f, 161.5453f),
        new Vector3(-188.0153f, 0.1838198f, 143.3153f),
        new Vector3(-137.1953f, 0.1838198f, 120.1853f),
        new Vector3(-150.0453f, 0.1838198f, 146.5053f),

        // South East Destination
        new Vector3(-111.6353f, 0.1838198f, 113.7953f),
        new Vector3(-56.99529f, 0.1838198f, 113.5553f),
        new Vector3(-117.0953f, 0.1838198f, 83.95531f),
        new Vector3(-73.99529f, 0.1838198f, 51.3553f),
        new Vector3(-30.15529f, 0.1838198f, 82.30531f),

        // South West Destination
        new Vector3(-131.0053f, 0.1838198f, 50.9953f),
        new Vector3(-176.0053f, 0.1838198f, 51.0853f),
        new Vector3(-168.1153f, 0.1838198f, 88.9653f),
        new Vector3(-153.7953f, 0.1838198f, 110.7853f),
        new Vector3(-194.3153f, 0.1838198f, 118.1553f),
    };

    void Start()
    {
       if (passengerMaterials == null || passengerMaterials.Length == 0)
        {
            Debug.LogWarning("PassengerSpawner: No passenger materials assigned! Using default materials.");
        }
        
        // SpawnPassenger();
        SpawnMultiplePassengers(6);
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