using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CarMovement : MonoBehaviour
{
    // public float verticalInput = 0f;
    // public float horizontalInput = 0f;

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
    public float verticalInput;
    public float horizontalInput;
    private Rigidbody rb;

    [Header("Passenger & Destination Settings")]
    [SerializeField] private DestinationSpawner destinationSpawner;
    private List<int> currentPassengers = new List<int>();
    private List<Vector3> currentDestinations = new List<Vector3>();
    private const int maxPassengers = 4;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
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
        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");

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

            if (passengerScript == null)
            {
                Debug.LogError("Passenger script not found on collided object!");
                return;
            }

            Debug.Log("Triggered with: " + other.gameObject.name + " | ID: " + passengerScript.passengerID + " | Destination: " + passengerScript.destination);

            currentPassengers.Add(passengerScript.passengerID);
            currentDestinations.Add(passengerScript.destination);

            if (PassengerUIManager.Instance != null)
            {
                PassengerUIManager.Instance.AddPassenger(passengerScript.passengerID);
            }

            other.gameObject.SetActive(false);

            if (destinationSpawner != null)
            {
                destinationSpawner.SpawnDestination(passengerScript.destination);
            }
            else
            {
                Debug.LogError("DestinationSpawner reference is missing!");
            }

            Debug.Log($"Passenger {passengerScript.passengerID} picked up! Total passengers: {currentPassengers.Count}/{maxPassengers}");

            if (currentPassengers.Count >= maxPassengers)
            {
                if (PassengerCollisionManager.Instance != null)
                {
                    PassengerCollisionManager.Instance.DisableAllPassengerCollisions();
                    Debug.Log("Angkot is full! Passenger collisions disabled.");
                }
            }
        }
        else if (other.gameObject.CompareTag("Destination"))
        {
            if (currentPassengers.Count > 0)
            {
                int droppedPassengerID = currentPassengers[0];
                currentPassengers.RemoveAt(0);
                currentDestinations.RemoveAt(0);

                if (PassengerUIManager.Instance != null)
                {
                    PassengerUIManager.Instance.RemovePassenger();
                }

                Debug.Log($"Passenger {droppedPassengerID} dropped off! Total passengers: {currentPassengers.Count}/{maxPassengers}");

                if (currentPassengers.Count < maxPassengers)
                {
                    if (PassengerCollisionManager.Instance != null)
                    {
                        PassengerCollisionManager.Instance.EnableAllPassengerCollisions();
                        Debug.Log("Space available! Passenger collisions enabled.");

                        PassengerCollisionManager.Instance.LogCurrentCollisionStates();
                    }
                }
            }

            Destroy(other.gameObject);

            Debug.Log("Passenger dropped off successfully!");
        }
    }

    public int GetCurrentPassengerCount()
    {
        return currentPassengers.Count;
    }

    public bool IsAngkotFull()
    {
        return currentPassengers.Count >= maxPassengers;
    }

    public bool HasPassengers()
    {
        return currentPassengers.Count > 0;
    }

    public List<int> GetCurrentPassengerIDs()
    {
        return new List<int>(currentPassengers);
    }

    public void MuteAllEngineSounds()
    {
        idleSound.volume = 0f;
        forwardSound.volume = 0f;
        reverseSound.volume = 0f;
    }
}