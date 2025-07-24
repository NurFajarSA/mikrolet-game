using UnityEngine;

public class PassengerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject passengerPrefab;

    private int passengerCounter = 0;

    private Vector3[] spawnPoints = new Vector3[]
    {
        // North East Passenger
        new Vector3(57.2f, 1, 51),
        new Vector3(156, 1, 62.3f),
        new Vector3(156.9f, 1, 25.8f),
        new Vector3(56.5f, 1, 106.5f),
        new Vector3(61.2f, 1, 140.4f),

        // North West Passenger
        new Vector3(27.5f, 1, 75.8f),
        new Vector3(0.8f, 1, 50.7f),
        new Vector3(-20.3f, 1, 81.8f),
        new Vector3(-4f, 1, 140.4f),
        new Vector3(27.3f, 1, 132f),

        // South East Passenger
        new Vector3(75.3f, 1, 20.2f),
        new Vector3(55.1f, 1, -52.2f),
        new Vector3(55.6f, 1, -5.9f),
        new Vector3(156.6f, 1, -2.2f),
        new Vector3(131.8f, 1, -52.3f),

        // South West Passenger
        new Vector3(-16.7f, 1, -28.6f),
        new Vector3(-62.9f, 1, 17.1f),
        new Vector3(10.9f, 1, 19.7f),
        new Vector3(27.4f, 1, -37.4f),
        new Vector3(-17.7f, 1, -3.1f),
    };

    private Vector3[] destinationPoints = new Vector3[]
    {
        // North East Destination
        new Vector3(119.4f, 0.04f, 83.2f),
        new Vector3(132.3f, 0.04f, 112.8f),
        new Vector3(142.5f, 0.04f, 21.5f),
        new Vector3(178.6f, 0.04f, 48.5f),
        new Vector3(77.9f, 0.04f, 37.2f),

        // North West Destination
        new Vector3(29.8f, 0.04f, 78.7f),
        new Vector3(75.2f, 0.04f, 73f),
        new Vector3(1.6f, 0.04f, 24.3f),
        new Vector3(73.9f, 0.04f, 116.2f),
        new Vector3(62.6f, 0.04f, 24.2f),

        // South East Destination
        new Vector3(200.7f, 0.04f, -73f),
        new Vector3(135.4f, 0.04f, -82f),
        new Vector3(146.5f, 0.04f, -33.2f),
        new Vector3(102, 0.04f, -10),
        new Vector3(204.9f, 0.04f, -14.3f),

        // South West Destination
        new Vector3(-50, 0.04f, -81.4f),
        new Vector3(-9.1f, 0.04f, -56.8f),
        new Vector3(-43.2f, 0.04f, -32.6f),
        new Vector3(-35.6f, 0.04f, -2.2f),
        new Vector3(17.1f, 0.04f, -17)
    };

    void Start()
    {
        // SpawnPassenger();
        SpawnMultiplePassengers(16);
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
