using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CarMovement : MonoBehaviour
{
    public float verticalInput = 0f;
    public float horizontalInput = 0f;

    [Header("Speed Settings")]
    public float maxForwardSpeed = 100f;
    public float maxReverseSpeed = 30f;
    public float acceleration = 50f;
    public float braking = 80f;
    public float coastingDrag = 10f;

    [Header("Steering")]
    public float turnSpeed = 5f;

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
    // public float verticalInput;
    // public float horizontalInput;
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
    }

    void Update()
    {
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
        float wheelTurnAngle = horizontalInput * 30f;

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
}
