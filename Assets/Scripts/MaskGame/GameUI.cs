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
            if (LevelManager.Instance == null) return;
            if (style == null) style = new GUIStyle(); // Safety

            GameState state = LevelManager.Instance.currentState;

            if (state == GameState.Title)
            {
                DrawCenteredText("MASK GAME\n\nPress SPACE to Start", Color.white);
            }
            else if (state == GameState.Playing)
            {
                if (player == null) return;

                string levelInfo = LevelManager.Instance != null ? $"Level: {LevelManager.Instance.GetCurrentLevelIndex()}" : "";
                string msg = $"Mask: {player.currentMask}\n{levelInfo}\nControls: 1=Normal, 2=Heavy, 3=Feather\nArrows/WASD to Move, Space to Jump";

                style.fontSize = 24;
                style.alignment = TextAnchor.UpperLeft;

                // Draw shadow
                style.normal.textColor = Color.black;
                GUI.Label(new Rect(22, 22, 600, 200), msg, style);

                // Draw text
                style.normal.textColor = Color.white;
                GUI.Label(new Rect(20, 20, 600, 200), msg, style);
            }
            else if (state == GameState.Victory)
            {
                DrawCenteredText("YOU WIN!\n\nPress R to Restart", Color.green);
            }
            else if (state == GameState.GameOver)
            {
                DrawCenteredText("GAME OVER\n\nPress R to Restart", Color.red);
            }
        }

        void DrawCenteredText(string text, Color color)
        {
            style.fontSize = 48;
            style.alignment = TextAnchor.MiddleCenter;
            style.normal.textColor = Color.black; // Shadow
            GUI.Label(new Rect(Screen.width/2 - 200 + 2, Screen.height/2 - 100 + 2, 400, 200), text, style);

            style.normal.textColor = color;
            GUI.Label(new Rect(Screen.width/2 - 200, Screen.height/2 - 100, 400, 200), text, style);
        }
    }
}
