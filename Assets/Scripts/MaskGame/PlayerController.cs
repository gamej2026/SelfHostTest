using UnityEngine;

namespace MaskGame
{
    public class PlayerController : MonoBehaviour
    {
        public float moveSpeed = 5f;
        public float jumpForce = 5f;
        public MaskType currentMask = MaskType.Normal;

        public bool isWon = false;

        private Rigidbody rb;
        private Renderer rend;
        private bool isGrounded;

        void Start()
        {
            rb = gameObject.AddComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            rend = GetComponent<Renderer>();
            UpdateMaskVisuals();
        }

        void Update()
        {
            if (isWon) return;

            HandleInput();
            HandleMovement();
        }

        void HandleInput()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) SwitchMask(MaskType.Normal);
            if (Input.GetKeyDown(KeyCode.Alpha2)) SwitchMask(MaskType.Heavy);
            if (Input.GetKeyDown(KeyCode.Alpha3)) SwitchMask(MaskType.Feather);

            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                Jump();
            }
        }

        void SwitchMask(MaskType newMask)
        {
            currentMask = newMask;
            UpdateMaskVisuals();
        }

        void UpdateMaskVisuals()
        {
            if (rend == null) return;

            switch (currentMask)
            {
                case MaskType.Normal:
                    rend.material.color = Color.white;
                    moveSpeed = 6f;
                    jumpForce = 6f;
                    break;
                case MaskType.Heavy:
                    rend.material.color = Color.red;
                    moveSpeed = 3f;
                    jumpForce = 3f;
                    break;
                case MaskType.Feather:
                    rend.material.color = Color.cyan;
                    moveSpeed = 8f;
                    jumpForce = 10f;
                    break;
            }
        }

        void HandleMovement()
        {
            if (rb == null) return;

            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            Vector3 movement = new Vector3(h, 0, v) * moveSpeed;
            // Using velocity for broad compatibility (Unity 6 supports linearVelocity but velocity still works)
            Vector3 newVelocity = new Vector3(movement.x, rb.velocity.y, movement.z);
            rb.velocity = newVelocity;
        }

        void Jump()
        {
            if (rb == null) return;
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }

        void OnCollisionEnter(Collision collision)
        {
            // Simple ground check approximation
            if (collision.contacts[0].point.y < transform.position.y)
            {
                isGrounded = true;
            }

            InteractableBlock block = collision.gameObject.GetComponent<InteractableBlock>();
            if (block != null)
            {
                block.OnHitByPlayer(currentMask);
                if (block.Type == BlockType.Goal)
                {
                    Debug.Log("Goal Reached!");
                    LevelManager.Instance.CompleteLevel();
                }
            }
        }

        void OnTriggerEnter(Collider other)
        {
             InteractableBlock block = other.gameObject.GetComponent<InteractableBlock>();
            if (block != null)
            {
                if (block.Type == BlockType.Goal)
                {
                    Debug.Log("Goal Reached (Trigger)!");
                    LevelManager.Instance.CompleteLevel();
                }
            }
        }

        public void Die()
        {
            Debug.Log("Player Died!");
            // Restart Level
            LevelManager.Instance.RestartLevel();
        }
    }
}
