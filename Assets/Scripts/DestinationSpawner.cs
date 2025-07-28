using System.Collections.Generic;
using UnityEngine;

public class DestinationSpawner : MonoBehaviour
{
    [SerializeField] private GameObject destinationPrefab;

    [Header("Indicator Settings")]
    [SerializeField] private GameObject destinationIndicatorPrefab;
    [SerializeField] private Transform carTransform;

    [Header("Destination Database")]
    public List<Vector3> destinationLocations = new List<Vector3>();

    private List<Vector3> destinationLocationsNE = new List<Vector3>();
    private List<Vector3> destinationLocationsNW = new List<Vector3>();
    private List<Vector3> destinationLocationsSE = new List<Vector3>();
    private List<Vector3> destinationLocationsSW = new List<Vector3>();

    public enum Area
    {
        NorthEast,
        NorthWest,
        SouthEast,
        SouthWest
    }

    private int destinationCounter = 0;

    private Vector3[] allDestinationPoints = new Vector3[]
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
        InitializeAreaDestinations();
    }

    private void InitializeAreaDestinations()
    {
        for (int i = 0; i < 5; i++)
        {
            destinationLocationsNE.Add(allDestinationPoints[i]);
        }
        for (int i = 5; i < 10; i++)
        {
            destinationLocationsNW.Add(allDestinationPoints[i]);
        }
        for (int i = 10; i < 15; i++)
        {
            destinationLocationsSE.Add(allDestinationPoints[i]);
        }
        for (int i = 15; i < 20; i++)
        {
            destinationLocationsSW.Add(allDestinationPoints[i]);
        }
    }

    public Vector3 GetDestinationFromDifferentArea(Area passengerArea)
    {
        List<Area> availableAreas = new List<Area>();

        for (int i = 0; i < 4; i++)
        {
            Area area = (Area)i;
            if (area != passengerArea && GetAreaDestinations(area).Count > 0)
            {
                availableAreas.Add(area);
            }
        }

        if (availableAreas.Count == 0)
        {
            // Debug.LogWarning("No available destination areas! Using random destination.");
            return allDestinationPoints[Random.Range(0, allDestinationPoints.Length)];
        }

        Area selectedArea = availableAreas[Random.Range(0, availableAreas.Count)];
        List<Vector3> areaDestinations = GetAreaDestinations(selectedArea);

        int randomIndex = Random.Range(0, areaDestinations.Count);
        Vector3 selectedDestination = areaDestinations[randomIndex];
        areaDestinations.RemoveAt(randomIndex);

        destinationLocations.Add(selectedDestination);

        Debug.Log($"Selected destination from {selectedArea} area (passenger was from {passengerArea})");
        return selectedDestination;
    }

    private List<Vector3> GetAreaDestinations(Area area)
    {
        switch (area)
        {
            case Area.NorthEast: return destinationLocationsNE;
            case Area.NorthWest: return destinationLocationsNW;
            case Area.SouthEast: return destinationLocationsSE;
            case Area.SouthWest: return destinationLocationsSW;
            default: return new List<Vector3>();
        }
    }

    public void SpawnDestination(Vector3 destinationPoint, int passengerID, Material passengerMaterial)
    {
        // Create the destination
        GameObject newDestination = Instantiate(destinationPrefab, destinationPoint, Quaternion.identity);
        Destination destinationScript = newDestination.GetComponent<Destination>();

        destinationScript.Initialize(passengerID, destinationPoint);
        destinationScript.SetDestinationMaterial(passengerMaterial);

        // Create the destination indicator
        GameObject indicatorObj = Instantiate(destinationIndicatorPrefab);
        DestinationIndicator indicatorScript = indicatorObj.GetComponent<DestinationIndicator>();

        indicatorScript.Initialize(newDestination.transform, carTransform);
        indicatorScript.SetIndicatorMaterial(passengerMaterial);

        destinationCounter++;
        Area area = DetermineAreaFromLocation(destinationPoint);
        Debug.Log($"Destination {destinationScript.destinationID} spawned at {area}. Active destinations: {destinationLocations.Count}");
    }

    public void ReturnDestinationToPool(int destinationID, Vector3 destinationLocation)
    {
        if (destinationLocations.Contains(destinationLocation))
        {
            destinationLocations.Remove(destinationLocation);
        }

        Area destinationArea = DetermineAreaFromLocation(destinationLocation);
        List<Vector3> areaDestinations = GetAreaDestinations(destinationArea);
        areaDestinations.Add(destinationLocation);

        Debug.Log($"Destination {destinationID} returned to {destinationArea} pool. Active destinations: {destinationLocations.Count}");
    }

    private Area DetermineAreaFromLocation(Vector3 location)
    {
        for (int i = 0; i < allDestinationPoints.Length; i++)
        {
            if (Vector3.Distance(allDestinationPoints[i], location) < 0.1f)
            {
                if (i < 5) return Area.NorthEast;
                else if (i < 10) return Area.NorthWest;
                else if (i < 15) return Area.SouthEast;
                else return Area.SouthWest;
            }
        }
        return Area.NorthEast;
    }
}