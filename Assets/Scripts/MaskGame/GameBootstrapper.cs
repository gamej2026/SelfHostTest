using UnityEngine;

namespace MaskGame
{
    public class GameBootstrapper
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void Init()
        {
            Debug.Log("Bootstrapping Mask Game...");
            Time.timeScale = 1.0f;

            // 1. Setup Camera
            Camera mainCam = Camera.main;
            if (mainCam == null)
            {
                GameObject camObj = new GameObject("Main Camera");
                mainCam = camObj.AddComponent<Camera>();
                camObj.tag = "MainCamera";
            }
            // Ensure Light
            if (GameObject.FindObjectOfType<Light>() == null)
            {
                GameObject lightObj = new GameObject("Directional Light");
                Light light = lightObj.AddComponent<Light>();
                light.type = LightType.Directional;
                lightObj.transform.rotation = Quaternion.Euler(50, -30, 0);
            }

            // 2. Create Player
            GameObject playerObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            playerObj.name = "Player";
            // Ensure it has a collider (CreatePrimitive does this)
            // Ensure Rigidbody is added by PlayerController (it does this)
            PlayerController player = playerObj.AddComponent<PlayerController>();

            // 3. Create Level Manager
            GameObject levelMgrObj = new GameObject("LevelManager");
            LevelManager levelManager = levelMgrObj.AddComponent<LevelManager>();
            levelManager.currentPlayer = player;
            levelManager.GenerateCurrentLevel(); // Uses the new multi-level system

            // 4. Setup Camera Follow
            CameraFollow camFollow = mainCam.gameObject.AddComponent<CameraFollow>();
            camFollow.target = playerObj.transform;

            // 5. Setup UI
            GameObject uiObj = new GameObject("GameUI");
            GameUI gui = uiObj.AddComponent<GameUI>();
            gui.player = player;

            Debug.Log("Mask Game Initialized.");
        }
    }
}
