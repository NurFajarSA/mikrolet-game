using UnityEngine;

public class Destination : MonoBehaviour
{
    public int destinationID { get; private set; }
    public Vector3 destination { get; private set; }

    public void Initialize(int id, Vector3 dest)
    {
        destinationID = id;
        destination = dest;
    }
}
