using UnityEngine;

public class PassengerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject passengerPrefab;

    private int passengerCounter = 0;

    private Vector3[] spawnPoints = new Vector3[]
    {
        // North East Passenger
        new Vector3(-48, 1, 78),
        new Vector3(-48, 1, 130.5f),
        new Vector3(-48, 1, 156.5f),
        new Vector3(51.5f, 1, 78),
        new Vector3(51.5f, 1, 56),

        // North West Passenger
        new Vector3(-74, 1, 78),
        new Vector3(-74, 1, 160),
        new Vector3(-100, 1, 74),
        new Vector3(-101, 1, 152),
        new Vector3(-124, 1, 74),

        // South East Passenger
        new Vector3(-30, 1, 47.5f),
        new Vector3(-30, 1, -9),
        new Vector3(-46, 1, 24),
        new Vector3(23.5f, 1, 5),
        new Vector3(55, 1, 24),

        // South West Passenger
        new Vector3(-74, 1, 51.5f),
        new Vector3(-75, 1, -13),
        new Vector3(-101, 1, 28),
        new Vector3(-111.5f, 1, 5),
        new Vector3(-151, 1, 36),
    };

    private Vector3[] destinationPoints = new Vector3[]
    {
        new Vector3(25, 0.16f, -35),
        new Vector3(26, 0.16f, 20),
        new Vector3(-17, 0.16f, 1)
    };

    void Start()
    {
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

            int randDestIndex = Random.Range(0, destinationPoints.Length);

            GameObject newPassenger = Instantiate(passengerPrefab, spawnPoints[spawnIndex], Quaternion.identity);
            Passenger passengerScript = newPassenger.GetComponent<Passenger>();
            passengerScript.Initialize(passengerCounter, destinationPoints[randDestIndex]);
            passengerCounter++;
        }
    }
}
