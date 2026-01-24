using UnityEngine;

public class RotateCube : MonoBehaviour
{
    // 회전 속도 (degrees per second)
    public float rotationSpeed = 50f;

    void Update()
    {
        // Y축을 중심으로 회전
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
