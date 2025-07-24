using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PassengerUIManager : MonoBehaviour
{
    [Header("UI References")]
    public Image[] passengerSlots = new Image[4];
    
    [Header("Colors")]
    public Color emptySlotColor = Color.white;
    public Color occupiedSlotColor = Color.green;
    
    public static PassengerUIManager Instance;

    private Material[] slotMaterials = new Material[4];
    
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
        
        InitializeSlots();
    }
    
    void InitializeSlots()
    {
        for (int i = 0; i < passengerSlots.Length; i++)
        {
            if (passengerSlots[i] != null)
            {
                passengerSlots[i].color = emptySlotColor;
                slotMaterials[i] = null;
            }
        }
    }
    
    public bool AddPassenger(int passengerID, Material passengerMaterial = null)
    {
        for (int i = 0; i < passengerSlots.Length; i++)
        {
            if (passengerSlots[i] != null && passengerSlots[i].color == emptySlotColor)
            {
                if (passengerMaterial != null)
                {
                    passengerSlots[i].color = passengerMaterial.color;
                    slotMaterials[i] = passengerMaterial;
                }
                else
                {
                    passengerSlots[i].color = occupiedSlotColor;
                    slotMaterials[i] = null;
                }
                
                Debug.Log($"Passenger {passengerID} added to UI slot {i}");
                return true;
            }
        }
        
        Debug.Log("All passenger slots are full!");
        return false;
    }
    
    public void RemovePassengerByMaterial(Material targetMaterial)
    {
        if (targetMaterial == null)
        {
            RemovePassenger();
            return;
        }
        
        for (int i = 0; i < passengerSlots.Length; i++)
        {
            if (passengerSlots[i] != null && slotMaterials[i] != null && 
                slotMaterials[i].color == targetMaterial.color)
            {
                passengerSlots[i].color = emptySlotColor;
                slotMaterials[i] = null;
                Debug.Log($"Passenger removed from UI slot {i} by material match");
                return;
            }
        }
        
        Debug.LogWarning("No material match found, using fallback removal");
        RemovePassenger();
    }
    
    public void RemovePassenger()
    {
        for (int i = passengerSlots.Length - 1; i >= 0; i--)
        {
            if (passengerSlots[i] != null && passengerSlots[i].color != emptySlotColor)
            {
                passengerSlots[i].color = emptySlotColor;
                slotMaterials[i] = null;
                Debug.Log($"Passenger removed from UI slot {i}");
                return;
            }
        }
    }
    
    public int GetPassengerCount()
    {
        int count = 0;
        for (int i = 0; i < passengerSlots.Length; i++)
        {
            if (passengerSlots[i] != null && passengerSlots[i].color != emptySlotColor)
            {
                count++;
            }
        }
        return count;
    }
    
    public bool IsFull()
    {
        return GetPassengerCount() >= 4;
    }
    
    public bool IsEmpty()
    {
        return GetPassengerCount() == 0;
    }
}