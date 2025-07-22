using UnityEngine;

public class Passenger : MonoBehaviour
{
    public int passengerID { get; private set; }
    public Vector3 destination { get; private set; }

    public void Initialize(int id, Vector3 dest)
    {
        passengerID = id;
        destination = dest;
    }
}
