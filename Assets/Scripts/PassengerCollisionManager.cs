using UnityEngine;

public class PassengerCollisionManager : MonoBehaviour
{
    public static PassengerCollisionManager Instance;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void SetPassengerPhysics(GameObject passenger, bool canBePickedUp)
    {
        Collider passengerCollider = passenger.GetComponent<Collider>();
        
        if (passengerCollider != null)
        {
            if (canBePickedUp)
            {
                passengerCollider.isTrigger = true;
            }
            else
            {
                passengerCollider.isTrigger = true;
                
                Rigidbody passengerRb = passenger.GetComponent<Rigidbody>();
                if (passengerRb != null)
                {
                    passengerRb.isKinematic = true;
                }
            }
        }
    }
    
    public void DisableAllPassengerCollisions()
    {
        GameObject[] passengers = GameObject.FindGameObjectsWithTag("Passenger");
        
        foreach (GameObject passenger in passengers)
        {
            SetPassengerPhysics(passenger, false);
        }
    }
    
    public void EnableAllPassengerCollisions()
    {
        GameObject[] passengers = GameObject.FindGameObjectsWithTag("Passenger");
        
        foreach (GameObject passenger in passengers)
        {
            SetPassengerPhysics(passenger, true);
        }
    }
}