using UnityEngine;

namespace MaskGame
{
    public class CameraFollow : MonoBehaviour
    {
        public static CameraFollow Instance;
        public Transform target;
        public Vector3 offset = new Vector3(0, 15, -10); // High angle
        public float smoothSpeed = 0.125f;

        private float shakeDuration = 0f;
        private float shakeMagnitude = 0.2f;

        void Awake()
        {
            Instance = this;
        }

        void LateUpdate()
        {
            if (target == null) return;

            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            if (shakeDuration > 0)
            {
                smoothedPosition += Random.insideUnitSphere * shakeMagnitude;
                shakeDuration -= Time.deltaTime;
            }

            transform.position = smoothedPosition;
            transform.LookAt(target);
        }

        public void Shake(float duration, float magnitude)
        {
            shakeDuration = duration;
            shakeMagnitude = magnitude;
        }
    }
}
