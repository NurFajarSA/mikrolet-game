using UnityEngine;

public class Passenger : MonoBehaviour
{
    public int passengerID { get; private set; }
    public int destinationID { get; private set; }

    public PassengerType passengerType { get; private set; }
    public Material passengerMaterial { get; private set; }
    public Vector3 destination { get; private set; }

    private bool isColorActivated = false;
    private Material defaultWhiteMaterial;
    private Material[] availableColorMaterials;

    public enum PassengerType
    {
        TukangJamu,
        IbuHamil,
        Pengamen,
        Pesilat,
    }
    
    public void Initialize(int id, Vector3 dest)
    {
        passengerID = id;
        destination = dest;
        
        SetRandomPassengerType();
        destinationID = id;
    }
    
    private void SetRandomPassengerType()
    {
        PassengerType[] types = System.Enum.GetValues(typeof(PassengerType)) as PassengerType[];
        passengerType = types[Random.Range(0, types.Length)];
    }
    
    public void SetAvailableColorMaterials(Material whiteMaterial, Material[] colorMaterials)
    {
        defaultWhiteMaterial = whiteMaterial;
        availableColorMaterials = colorMaterials;

        SetPassengerMaterial(defaultWhiteMaterial);
    }
    
    public void SetPassengerMaterial(Material material)
    {
        passengerMaterial = material;

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

    public void ActivatePassengerColor()
    {
        Material randomColorMaterial = availableColorMaterials[Random.Range(0, availableColorMaterials.Length)];
        SetPassengerMaterial(randomColorMaterial);
        isColorActivated = true;
    }
    
    public string GetPassengerTypeString()
    {
        switch (passengerType)
        {
            case PassengerType.TukangJamu:
                return "Tukang Jamu";
            case PassengerType.IbuHamil:
                return "Ibu Hamil";
            case PassengerType.Pengamen:
                return "Pengamen";
            case PassengerType.Pesilat:
                return "Pesilat";
            default:
                return "Unknown";
        }
    }
}