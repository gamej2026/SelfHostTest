using UnityEngine;

namespace MaskGame
{
    public class GameUI : MonoBehaviour
    {
        public PlayerController player;
        private GUIStyle style;

        void Start()
        {
            style = new GUIStyle();
            style.fontSize = 24;
            style.normal.textColor = Color.white;
        }

        void OnGUI()
        {
            if (player == null) return;

            string levelInfo = LevelManager.Instance != null ? $"Level: {LevelManager.Instance.GetCurrentLevelIndex()}" : "";
            string msg = $"Mask: {player.currentMask}\n{levelInfo}\nControls: 1=Normal, 2=Heavy, 3=Feather\nArrows/WASD to Move, Space to Jump";

            if (style == null) style = new GUIStyle(); // Safety
            style.fontSize = 24;

            // Draw shadow
            style.normal.textColor = Color.black;
            GUI.Label(new Rect(22, 22, 600, 200), msg, style);

            // Draw text
            style.normal.textColor = Color.white;
            GUI.Label(new Rect(20, 20, 600, 200), msg, style);

            if (player.isWon)
            {
                style.fontSize = 48;
                style.alignment = TextAnchor.MiddleCenter;
                style.normal.textColor = Color.green; // Make it green
                GUI.Label(new Rect(Screen.width/2 - 200, Screen.height/2 - 100, 400, 200), "YOU WIN!", style);
            }
        }
    }
}
