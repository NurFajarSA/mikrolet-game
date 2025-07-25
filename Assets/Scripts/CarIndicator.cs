using UnityEngine;

public class CarIndicator : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0f, 32f, 0f);

    void LateUpdate()
    {
        transform.position = target.position + offset;
        transform.rotation = Quaternion.Euler(0f, target.eulerAngles.y, 0f);
    }
}
