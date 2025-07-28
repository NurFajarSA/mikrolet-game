using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CarMovement : MonoBehaviour
{
    [Header("Speed Settings")]
    public float maxForwardSpeed = 50f;
    public float maxReverseSpeed = 30f;
    public float acceleration = 50f;
    public float braking = 80f;
    public float coastingDrag = 10f;

    [Header("Steering")]
    public float turnSpeed = 20f;

    [Header("Visual")]
    public Transform wheelFL;
    public Transform wheelFR;

    [Header("Audio")]
    public AudioSource idleSound;
    public AudioSource forwardSound;
    public AudioSource reverseSound;
    [Range(0.1f, 1.0f)] public float forwardMaxVolume = 1.0f;
    [Range(0.1f, 2.0f)] public float forwardMaxPitch = 1.0f;
    [Range(0.1f, 1.0f)] public float reverseMaxVolume = 0.5f;
    [Range(0.1f, 2.0f)] public float reverseMaxPitch = 0.6f;

    private float currentSpeed = 0f;
    public float verticalInput = 0f;
    public float horizontalInput = 0f;
    private Rigidbody rb;

    [Header("Passenger & Destination Settings")]
    [SerializeField] private PassengerSpawner passengerSpawner;
    [SerializeField] private DestinationSpawner destinationSpawner;
    
    private List<int> currentPassengers = new List<int>();
    private List<Vector3> currentDestinations = new List<Vector3>();
    private List<Material> currentPassengerMaterials = new List<Material>();
    private List<Passenger.PassengerType> currentPassengerTypes = new List<Passenger.PassengerType>();
    private List<Vector3> currentPassengerPickupLocations = new List<Vector3>();
    
    private const int maxPassengers = 4;

    private int money = 0;
    public TMPro.TextMeshProUGUI moneyText;
    private Vector3 moneyScale;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        
        moneyScale = moneyText.transform.localScale;
        money = 0;
        SetMoneyText();

        if (passengerSpawner == null)
        {
            passengerSpawner = FindFirstObjectByType<PassengerSpawner>();
        }
    }

    void Update()
    {
        if (GameManager.Instance != null && !GameManager.Instance.IsGameStarted())
        {
            Debug.Log("Game belum mulai, input dan audio dimatikan.");

            idleSound.volume = 0f;
            forwardSound.volume = 0f;
            reverseSound.volume = 0f;
            return;
        }
        // verticalInput = Input.GetAxis("Vertical");
        // horizontalInput = Input.GetAxis("Horizontal");

        ApplySteering();
        ApplyWheelVisual();
        UpdateEngineAudio();
    }

    void FixedUpdate()
    {
        ApplyMovement();
    }

    void ApplyMovement()
    {
        float targetMaxSpeed = verticalInput >= 0f ? maxForwardSpeed : maxReverseSpeed;
        float accelerationRate = verticalInput != 0f ? acceleration : coastingDrag;

        float targetSpeed = verticalInput * targetMaxSpeed;

        if (Mathf.Sign(currentSpeed) != Mathf.Sign(targetSpeed) && Mathf.Abs(currentSpeed) > 1f)
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, braking * Time.fixedDeltaTime);
        }
        else
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, accelerationRate * Time.fixedDeltaTime);
        }

        Vector3 desiredVelocity = transform.forward * currentSpeed;
        rb.linearVelocity = new Vector3(desiredVelocity.x, rb.linearVelocity.y, desiredVelocity.z);
    }

    void ApplySteering()
    {
        if (Mathf.Abs(currentSpeed) > 0.1f)
        {
            float steerAmount = horizontalInput * turnSpeed * Time.fixedDeltaTime * Mathf.Sign(currentSpeed);
            Quaternion turnOffset = Quaternion.Euler(0, steerAmount, 0);
            rb.MoveRotation(rb.rotation * turnOffset);
        }
    }

    void ApplyWheelVisual()
    {
        float wheelTurnAngle = horizontalInput * 10f;

        if (wheelFL != null)
            wheelFL.localRotation = Quaternion.Euler(0f, wheelTurnAngle, 0f);
        if (wheelFR != null)
            wheelFR.localRotation = Quaternion.Euler(0f, wheelTurnAngle, 0f);
    }

    void UpdateEngineAudio()
    {
        float speedPercent = Mathf.Abs(currentSpeed) / (currentSpeed >= 0 ? maxForwardSpeed : maxReverseSpeed);

        idleSound.volume = Mathf.Lerp(0.6f, 0f, speedPercent * 4f);

        if (currentSpeed < -0.1f)
        {
            forwardSound.volume = 0f;
            reverseSound.volume = Mathf.Lerp(0.1f, reverseMaxVolume, speedPercent);
            reverseSound.pitch = Mathf.Lerp(0.1f, reverseMaxPitch, speedPercent + Mathf.Sin(Time.time) * 0.1f);
        }
        else if (currentSpeed > 0.1f)
        {
            reverseSound.volume = 0f;
            forwardSound.volume = Mathf.Lerp(0.1f, forwardMaxVolume, speedPercent);
            forwardSound.pitch = Mathf.Lerp(0.3f, forwardMaxPitch, speedPercent + Mathf.Sin(Time.time) * 0.1f);
        }
        else
        {
            forwardSound.volume = 0f;
            reverseSound.volume = 0f;
        }
    }

    /// PASSENGER & DESTINATION INTERACTION
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Passenger"))
        {
            if (currentPassengers.Count >= maxPassengers)
            {
                Debug.Log("Angkot is full! Cannot add more passengers. Ignoring collision.");

                Collider col = other.GetComponent<Collider>();
                if (col != null)
                {
                    col.enabled = false;
                    Debug.Log("Disabled collider of " + other.gameObject.name);
                }

                return;
            }

            Passenger passengerScript = other.gameObject.GetComponent<Passenger>();

            if (!IsAngkotFull())
            {
                passengerScript.ActivatePassengerColor();
            }

            Debug.Log($"Passenger {passengerScript.passengerID} ({passengerScript.GetPassengerTypeString()}) picked up!");

            currentPassengers.Add(passengerScript.passengerID);
            currentDestinations.Add(passengerScript.destination);
            currentPassengerMaterials.Add(passengerScript.passengerMaterial);
            currentPassengerTypes.Add(passengerScript.passengerType);
            currentPassengerPickupLocations.Add(other.transform.position);

            PassengerUIManager.Instance.AddPassenger(passengerScript.passengerID, passengerScript.passengerMaterial);

            other.gameObject.SetActive(false);
            if (destinationSpawner != null)
            {
                destinationSpawner.SpawnDestination(passengerScript.destination, passengerScript.passengerID, passengerScript.passengerMaterial);
            }
            else
            {
                Debug.LogError("DestinationSpawner reference is missing!");
            }

            if (currentPassengers.Count >= maxPassengers)
            {
                PassengerCollisionManager.Instance.DisableAllPassengerCollisions();
                Debug.Log("Angkot is full! Passenger collisions disabled.");
            }
            
            PassengerIndicator[] indicators = Object.FindObjectsByType<PassengerIndicator>(FindObjectsSortMode.None);
            foreach (var indicator in indicators)
            {
                if (indicator != null && indicator.gameObject.activeSelf && indicator.transform != null)
                {
                    if (indicator.transform == other.transform || 
                        (indicator.GetType().GetField("targetPassenger", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(indicator) as Transform) == other.transform)
                    {
                        Destroy(indicator.gameObject);
                    }
                }
            }
        }
        else if (other.gameObject.CompareTag("Destination"))
        {
            if (currentPassengers.Count > 0)
            {
                Destination destinationScript = other.gameObject.GetComponent<Destination>();

                int passengerIndex = -1;
                for (int i = 0; i < currentPassengers.Count; i++)
                {
                    if (destinationScript.CanDropPassenger(currentPassengers[i]))
                    {
                        passengerIndex = i;
                        break;
                    }
                }

                if (passengerIndex == -1)
                {
                    Debug.Log($"No matching passenger for destination {destinationScript.destinationID}");
                    return;
                }

                int droppedPassengerID = currentPassengers[passengerIndex];
                Material droppedPassengerMaterial = currentPassengerMaterials[passengerIndex];
                Vector3 passengerPickupLocation = currentPassengerPickupLocations[passengerIndex];

                currentPassengers.RemoveAt(passengerIndex);
                currentDestinations.RemoveAt(passengerIndex);
                currentPassengerMaterials.RemoveAt(passengerIndex);
                currentPassengerTypes.RemoveAt(passengerIndex);
                currentPassengerPickupLocations.RemoveAt(passengerIndex);

                money += 10;
                SetMoneyText();

                Debug.Log($"Passenger {droppedPassengerID} dropped off! Remaining: {currentPassengers.Count}/{maxPassengers}");

                PassengerUIManager.Instance.RemovePassengerByMaterial(droppedPassengerMaterial);
                destinationSpawner.ReturnDestinationToPool(droppedPassengerID, other.transform.position);
                passengerSpawner.SpawnPassengerAfterDelay(passengerPickupLocation);

                if (currentPassengers.Count < maxPassengers)
                {
                    PassengerCollisionManager.Instance.EnableAllPassengerCollisions();
                    Debug.Log("Space available! Passenger collisions enabled.");

                    PassengerCollisionManager.Instance.LogCurrentCollisionStates();
                }
            }

            Destroy(other.gameObject);
        }
    }

    void SetMoneyText()
    {
        moneyText.text = $"{money}";

        StopAllCoroutines();
        StartCoroutine(Bounce());
    }

    System.Collections.IEnumerator Bounce()
    {
        moneyText.transform.localScale = moneyScale * 1.3f;
        moneyText.color = Color.green;

        yield return new WaitForSeconds(0.3f);

        moneyText.transform.localScale = moneyScale;
        moneyText.color = Color.white;
    }

    public bool IsAngkotFull()
    {
        return currentPassengers.Count >= maxPassengers;
    }

    public void MuteAllEngineSounds()
    {
        idleSound.volume = 0f;
        forwardSound.volume = 0f;
        reverseSound.volume = 0f;
    }
}