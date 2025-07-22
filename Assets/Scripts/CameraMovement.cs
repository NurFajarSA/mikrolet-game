using UnityEngine;

public class FollowCar : MonoBehaviour
{
    public Rigidbody targetRb;
    public Vector3 offset = new Vector3(0, 5, -10);
    public float smoothTime = 0.1f;

    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        if (targetRb == null) return;

        Vector3 targetPos = targetRb.transform.TransformPoint(offset);

        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);

        transform.LookAt(targetRb.transform.position + Vector3.up * 2f);
    }
}

// using UnityEngine;

// public class FollowCar : MonoBehaviour
// {
//     public Transform target;
//     public Vector3 offset = new Vector3(0, 5, -10);
//     public float smoothTime = 0.1f;

//     private Vector3 velocity = Vector3.zero;

//     void LateUpdate()
//     {
//         if (target == null) return;

//         Vector3 targetPos = target.position + target.rotation * offset;

//         transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);

//         transform.LookAt(target.position + Vector3.up * 2f);
//     }
// }
