using UnityEngine;

namespace MaskGame
{
    public enum BlockType
    {
        Wall,
        Breakable, // Requires Heavy
        Goal
    }

    public class InteractableBlock : MonoBehaviour
    {
        public BlockType Type;

        public void OnHitByPlayer(MaskType playerMask)
        {
            if (Type == BlockType.Breakable && playerMask == MaskType.Heavy)
            {
                // Break effect
                if (ParticleManager.Instance != null)
                {
                    Renderer r = GetComponent<Renderer>();
                    ParticleManager.Instance.SpawnBreakEffect(transform.position, r != null ? r.material.color : Color.gray);
                }
                if (CameraFollow.Instance != null)
                {
                    CameraFollow.Instance.Shake(0.2f, 0.3f);
                }
                Destroy(gameObject);
            }
        }
    }
}
