using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;   // đối tượng cần theo dõi (player)
    public float smoothSpeed = 0.125f; // tốc độ mượt
    public Vector3 offset;     // khoảng cách giữa camera và player

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, transform.position.z);
    }
}


