using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PassengerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject passengerPrefab;

    [Header("Passenger Materials")]
    [SerializeField] private Material whiteMaterial;
    [SerializeField] private Material[] colorMaterials;

    [Header("Indicator Settings")]
    [SerializeField] private GameObject passengerIndicatorPrefab;
    [SerializeField] private Transform carTransform;

    // TAMBAH INI: Database locations untuk setiap area
    [Header("Location Database")]
    public List<Vector3> passengerLocations = new List<Vector3>();
    
    // TAMBAH INI: Area-based locations untuk passenger
    private List<Vector3> passengerLocationsNE = new List<Vector3>();
    private List<Vector3> passengerLocationsNW = new List<Vector3>();
    private List<Vector3> passengerLocationsSE = new List<Vector3>();
    private List<Vector3> passengerLocationsSW = new List<Vector3>();

    private int passengerCounter = 0;

    // UBAH INI: Original spawn points sebagai pool locations
    private Vector3[] allPassengerSpawnPoints = new Vector3[]
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

    // TAMBAH INI: Area enum untuk identifikasi
    public enum Area
    {
        NorthEast,
        NorthWest,
        SouthEast,
        SouthWest
    }

    void Start()
    {
        // TAMBAH INI: Initialize area-based location pools
        InitializeAreaLocations();
        
        // UBAH INI: Spawn 6 passengers instead of 8
        SpawnMultiplePassengers(6);
    }

    // TAMBAH INI: Initialize locations by area
    private void InitializeAreaLocations()
    {
        // NE: indices 0-4
        for (int i = 0; i < 5; i++)
        {
            passengerLocationsNE.Add(allPassengerSpawnPoints[i]);
        }

        // NW: indices 5-9
        for (int i = 5; i < 10; i++)
        {
            passengerLocationsNW.Add(allPassengerSpawnPoints[i]);
        }

        // SE: indices 10-14
        for (int i = 10; i < 15; i++)
        {
            passengerLocationsSE.Add(allPassengerSpawnPoints[i]);
        }

        // SW: indices 15-19
        for (int i = 15; i < 20; i++)
        {
            passengerLocationsSW.Add(allPassengerSpawnPoints[i]);
        }

        Debug.Log($"Initialized passenger locations: NE={passengerLocationsNE.Count}, NW={passengerLocationsNW.Count}, SE={passengerLocationsSE.Count}, SW={passengerLocationsSW.Count}");
    }

    // UBAH INI: Modified spawn multiple passengers
    void SpawnMultiplePassengers(int count)
    {
        int passengersPerArea = count / 4;
        int remainingPassengers = count % 4;

        // Spawn evenly across areas
        SpawnPassengersFromArea(Area.NorthEast, passengersPerArea + (remainingPassengers > 0 ? 1 : 0));
        if (remainingPassengers > 0) remainingPassengers--;

        SpawnPassengersFromArea(Area.NorthWest, passengersPerArea + (remainingPassengers > 0 ? 1 : 0));
        if (remainingPassengers > 0) remainingPassengers--;

        SpawnPassengersFromArea(Area.SouthEast, passengersPerArea + (remainingPassengers > 0 ? 1 : 0));
        if (remainingPassengers > 0) remainingPassengers--;

        SpawnPassengersFromArea(Area.SouthWest, passengersPerArea);

        Debug.Log($"Spawned {count} passengers. Current passenger locations in use: {passengerLocations.Count}");
    }

    // TAMBAH INI: Spawn passengers from specific area
    private void SpawnPassengersFromArea(Area area, int count)
    {
        List<Vector3> areaLocations = GetAreaLocations(area);
        
        for (int i = 0; i < count && areaLocations.Count > 0; i++)
        {
            int randomIndex = Random.Range(0, areaLocations.Count);
            Vector3 spawnPos = areaLocations[randomIndex];

            // Remove from area pool and add to active locations
            areaLocations.RemoveAt(randomIndex);
            passengerLocations.Add(spawnPos);

            // Get destination from different areas
            Vector3 destinationPos = GetDestinationFromDifferentArea(area);

            GameObject newPassenger = Instantiate(passengerPrefab, spawnPos, Quaternion.identity);
            Passenger passengerScript = newPassenger.GetComponent<Passenger>();
            passengerScript.Initialize(passengerCounter, destinationPos);

            SetMaterialsForPassenger(passengerScript);
            SpawnPassengerIndicator(newPassenger.transform);

            Debug.Log($"Spawned passenger {passengerCounter} from {area} area at {spawnPos}");
            passengerCounter++;
        }
    }

    // TAMBAH INI: Get area locations by enum
    private List<Vector3> GetAreaLocations(Area area)
    {
        switch (area)
        {
            case Area.NorthEast: return passengerLocationsNE;
            case Area.NorthWest: return passengerLocationsNW;
            case Area.SouthEast: return passengerLocationsSE;
            case Area.SouthWest: return passengerLocationsSW;
            default: return new List<Vector3>();
        }
    }

    private Vector3 GetDestinationFromDifferentArea(Area passengerArea)
    {
        DestinationSpawner destSpawner = FindFirstObjectByType<DestinationSpawner>();
        if (destSpawner != null)
        {
            return destSpawner.GetDestinationFromDifferentArea((DestinationSpawner.Area)passengerArea);
        }

        Debug.LogError("DestinationSpawner not found!");
        return Vector3.zero;
    }

    public void SpawnPassengerAfterDelay(Vector3 droppedPassengerLocation)
    {
        StartCoroutine(DelayedSpawnPassenger(droppedPassengerLocation));
    }

    private IEnumerator DelayedSpawnPassenger(Vector3 droppedLocation)
    {
        yield return new WaitForSeconds(5f);

        Area locationArea = DetermineAreaFromLocation(droppedLocation);
        List<Vector3> areaLocations = GetAreaLocations(locationArea);
        areaLocations.Add(droppedLocation);

        if (passengerLocations.Contains(droppedLocation))
        {
            passengerLocations.Remove(droppedLocation);
        }

        SpawnRandomPassenger();

        Debug.Log($"Spawned new passenger after 5 seconds. Location {droppedLocation} returned to {locationArea} pool.");
    }

    private void SpawnRandomPassenger()
    {
        List<Area> availableAreas = new List<Area>();
        
        if (passengerLocationsNE.Count > 0) availableAreas.Add(Area.NorthEast);
        if (passengerLocationsNW.Count > 0) availableAreas.Add(Area.NorthWest);
        if (passengerLocationsSE.Count > 0) availableAreas.Add(Area.SouthEast);
        if (passengerLocationsSW.Count > 0) availableAreas.Add(Area.SouthWest);

        if (availableAreas.Count > 0)
        {
            Area randomArea = availableAreas[Random.Range(0, availableAreas.Count)];
            SpawnPassengersFromArea(randomArea, 1);
        }
        else
        {
            Debug.LogWarning("No available locations to spawn new passenger!");
        }
    }

    private Area DetermineAreaFromLocation(Vector3 location)
    {
        for (int i = 0; i < allPassengerSpawnPoints.Length; i++)
        {
            if (Vector3.Distance(allPassengerSpawnPoints[i], location) < 0.1f)
            {
                if (i < 5) return Area.NorthEast;
                else if (i < 10) return Area.NorthWest;
                else if (i < 15) return Area.SouthEast;
                else return Area.SouthWest;
            }
        }
        return Area.NorthEast;
    }

    private void SetMaterialsForPassenger(Passenger passenger)
    {
        passenger.SetAvailableColorMaterials(whiteMaterial, colorMaterials);
    }
    
    private void SpawnPassengerIndicator(Transform passengerTransform)
    {
        GameObject indicator = Instantiate(passengerIndicatorPrefab);
        PassengerIndicator indicatorScript = indicator.GetComponent<PassengerIndicator>();
        
        indicatorScript.Initialize(passengerTransform, carTransform);
    }
}