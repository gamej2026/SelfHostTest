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
                Destroy(gameObject);
            }
        }
    }
}
