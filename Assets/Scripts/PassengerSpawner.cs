using UnityEngine;

public class PassengerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject passengerPrefab;

    private int passengerCounter = 0;

    private Vector3[] spawnPoints = new Vector3[]
    {
        // North East Passenger
        new Vector3(-48, 1, 156.5f),
        new Vector3(-48, 1, 130.5f),
        new Vector3(-48, 1, 78),
        new Vector3(51.5f, 1, 56),
        new Vector3(51.5f, 1, 78),

        // North West Passenger
        new Vector3(-124, 1, 74),
        new Vector3(-101, 1, 152),
        new Vector3(-100, 1, 74),
        new Vector3(-74, 1, 78),
        new Vector3(-74, 1, 160),

        // South East Passenger
        new Vector3(-46, 1, 24),
        new Vector3(-30, 1, -9),
        new Vector3(-30, 1, 47.5f),
        new Vector3(23.5f, 1, 5),
        new Vector3(55, 1, 24),

        // South West Passenger
        new Vector3(-151, 1, 36),
        new Vector3(-111.5f, 1, 5),
        new Vector3(-101, 1, 28),
        new Vector3(-75, 1, -13),
        new Vector3(-74, 1, 51.5f),
    };

    private Vector3[] destinationPoints = new Vector3[]
    {
        // North East Destination
        new Vector3(-63, 0.04f, 113),
        new Vector3(-24, 0.04f, 127.5f),
        new Vector3(-6.5f, 0.04f, 75),
        new Vector3(-6.5f, 0.04f, 150),
        new Vector3(24, 0.04f, 102),

        // North West Destination
        new Vector3(-106, 0.04f, 137),
        new Vector3(-108, 0.04f, 101),
        new Vector3(-88, 0.04f, 77),
        new Vector3(-83, 0.04f, 157.5f),
        new Vector3(-65, 0.04f, 134),

        // South East Destination
        new Vector3(-7, 0.04f, -12),
        new Vector3(10.5f, 0.04f, 24),
        new Vector3(20, 0.04f, 48.5f),
        new Vector3(45, 0.04f, -7.5f),
        new Vector3(52.5f, 0.04f, 35),

        // South West Destination
        new Vector3(-145.5f, 0.04f, -3.5f),
        new Vector3(-98, 0.04f, -12),
        new Vector3(-124, 0.04f, 25),
        new Vector3(-103, 0.04f, 54),
        new Vector3(-148.5f, 0.04f, 56)
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
