using UnityEngine;

namespace MaskGame
{
    public class EnemyController : MonoBehaviour
    {
        public float moveSpeed = 3f;
        public float patrolDistance = 3f;
        public bool moveHorizontal = true; // true: X axis, false: Z axis

        private Vector3 startPos;
        private int direction = 1;

        void Start()
        {
            startPos = transform.position;
            // Visuals
            Renderer r = GetComponent<Renderer>();
            if (r != null) r.material.color = Color.red;
        }

        void Update()
        {
            // Simple ping-pong movement
            float dist = moveHorizontal ? (transform.position.x - startPos.x) : (transform.position.z - startPos.z);

            if (Mathf.Abs(dist) > patrolDistance)
            {
                direction *= -1;
                // Force reset to avoid getting stuck
                // transform.position = startPos + (moveHorizontal ? Vector3.right : Vector3.forward) * (patrolDistance * direction);
            }

            Vector3 moveDir = moveHorizontal ? Vector3.right : Vector3.forward;
            transform.Translate(moveDir * direction * moveSpeed * Time.deltaTime);
        }

        void OnCollisionEnter(Collision other)
        {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                player.Die();
            }
        }
    }
}
