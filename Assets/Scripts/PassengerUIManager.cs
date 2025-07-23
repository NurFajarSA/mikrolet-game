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
            }
        }
    }
    
    public bool AddPassenger(int passengerID)
    {
        for (int i = 0; i < passengerSlots.Length; i++)
        {
            if (passengerSlots[i] != null && passengerSlots[i].color == emptySlotColor)
            {
                passengerSlots[i].color = occupiedSlotColor;
                Debug.Log($"Passenger {passengerID} added to slot {i}");
                return true;
            }
        }
        
        Debug.Log("All passenger slots are full!");
        return false;
    }
    
    public void RemovePassenger()
    {
        for (int i = passengerSlots.Length - 1; i >= 0; i--)
        {
            if (passengerSlots[i] != null && passengerSlots[i].color == occupiedSlotColor)
            {
                passengerSlots[i].color = emptySlotColor;
                Debug.Log($"Passenger removed from slot {i}");
                return;
            }
        }
    }
    
    public int GetPassengerCount()
    {
        int count = 0;
        for (int i = 0; i < passengerSlots.Length; i++)
        {
            if (passengerSlots[i] != null && passengerSlots[i].color == occupiedSlotColor)
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