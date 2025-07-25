using UnityEngine;

public class PassengerCollisionManager : MonoBehaviour
{
    public static PassengerCollisionManager Instance;
    private Passenger[] allPassengers;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        allPassengers = FindObjectsByType<Passenger>(FindObjectsSortMode.None);
    }

    public void DisableAllPassengerCollisions()
    {
        foreach (var passenger in allPassengers)
        {
            if (passenger != null && passenger.gameObject.activeInHierarchy)
            {
                Collider col = passenger.GetComponent<Collider>();
                if (col != null)
                    col.enabled = false;
            }
        }
    }

    public void EnableAllPassengerCollisions()
    {
        foreach (var passenger in allPassengers)
        {
            if (passenger != null && passenger.gameObject.activeInHierarchy)
            {
                Collider col = passenger.GetComponent<Collider>();
                if (col != null)
                    col.enabled = true;
            }
        }
    }

    public void LogCurrentCollisionStates()
    {
        int enabledCount = 0;
        foreach (var passenger in allPassengers)
        {
            if (passenger != null && passenger.GetComponent<Collider>().enabled)
                enabledCount++;
        }
    }
}
