using UnityEngine;

public class Destination : MonoBehaviour
{
    public int destinationID { get; private set; }
    public int correspondingPassengerID { get; private set; }

    public Vector3 destination { get; private set; }
    public Material destinationMaterial { get; private set; }
    
    public void Initialize(int id, Vector3 dest)
    {
        destinationID = id;
        destination = dest;
        correspondingPassengerID = id;
    }
    
    public void SetDestinationMaterial(Material material)
    {
        destinationMaterial = material;
        
        Renderer[] childRenderers = GetComponentsInChildren<Renderer>();
        if (childRenderers.Length > 0)
        {
            foreach (Renderer childRenderer in childRenderers)
            {
                Material materialInstance = new Material(material);
                childRenderer.material = materialInstance;
            }
        }
    }
    
    public bool CanDropPassenger(int passengerID)
    {
        bool canDrop = correspondingPassengerID == passengerID;
        return canDrop;
    }
}