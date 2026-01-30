using UnityEngine;
using System.Collections.Generic;

namespace MaskGame
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance;
        public GameState currentState = GameState.Title;

        public PlayerController currentPlayer;
        private int currentLevelIndex = 0;
        private GameObject currentLevelRoot;

        // Level Data
        // P: Player, G: Goal, #: Wall, B: Breakable, E: Enemy (Horizontal), Z: Enemy (Vertical)
        private List<string[]> levels = new List<string[]>();

        void Awake()
        {
            Instance = this;
            InitializeLevels();
        }

        void Update()
        {
            if (currentState == GameState.Title)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    StartGame();
                }
            }
            else if (currentState == GameState.GameOver || currentState == GameState.Victory)
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    StartGame();
                }
            }
        }

        public void StartGame()
        {
            currentLevelIndex = 0;
            currentState = GameState.Playing;
            GenerateCurrentLevel();
        }

        void InitializeLevels()
        {
            // Level 1: Tutorial
            levels.Add(new string[] {
                "####################",
                "#P.................#",
                "#####.###.##########",
                "#.......#..........#",
                "#.BBBB..#..BBBB....#",
                "#.B..B..#..B.......#",
                "#.BBBB..#..BBBB....#",
                "#.......#..........#",
                "#########.##########",
                "#..................#",
                "#..BBBBB#####..G...#",
                "#..................#",
                "####################"
            });

            // Level 2: Enemies & Challenge
            levels.Add(new string[] {
                "####################",
                "#P.................#",
                "#.###.###.###.###..#",
                "#...E...#...E...#..#",
                "#######.#######.#..#",
                "#.......#..........#",
                "#.B.B.B.#.BBBBBB...#",
                "#.B.B.B.#..........#",
                "#.......#...Z......#",
                "#########.#######.##",
                "#.........#........#",
                "#..BBBB...#...G....#",
                "####################"
            });
        }

        public void GenerateCurrentLevel()
        {
            if (currentLevelRoot != null) Destroy(currentLevelRoot);

            currentLevelRoot = new GameObject($"Level_{currentLevelIndex}");

            // Textures
            Texture2D wallTex = TextureGen.CreateBrickTexture(Color.gray, Color.black);
            Texture2D floorTex = TextureGen.CreateCheckerTexture(new Color(0.8f, 0.8f, 0.8f), new Color(0.5f, 0.5f, 0.5f));
            Texture2D breakableTex = TextureGen.CreateBrickTexture(new Color(0.6f, 0.3f, 0.0f), Color.black);

            // Ground
            GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Plane);
            floor.name = "Floor";
            floor.transform.parent = currentLevelRoot.transform;
            floor.transform.localScale = new Vector3(3, 1, 3);
            floor.transform.position = new Vector3(10, -0.5f, 6);
            floor.GetComponent<Renderer>().material.mainTexture = floorTex;

            string[] map = levels[currentLevelIndex % levels.Count];
            float tileSize = 1.0f;

            for (int z = 0; z < map.Length; z++)
            {
                string row = map[z];
                int zPos = map.Length - 1 - z;

                for (int x = 0; x < row.Length; x++)
                {
                    char tile = row[x];
                    Vector3 pos = new Vector3(x * tileSize, 0.5f, zPos * tileSize);

                    switch (tile)
                    {
                        case '#':
                            CreateCube(pos, wallTex, BlockType.Wall, currentLevelRoot.transform);
                            break;
                        case 'B':
                            CreateCube(pos, breakableTex, BlockType.Breakable, currentLevelRoot.transform);
                            break;
                        case 'G':
                            CreateCube(pos, null, BlockType.Goal, currentLevelRoot.transform, Color.yellow);
                            break;
                        case 'P':
                            if(currentPlayer)
                            {
                                currentPlayer.transform.position = pos + Vector3.up * 1.0f;
                                // Reset velocity
                                Rigidbody rb = currentPlayer.GetComponent<Rigidbody>();
                                if(rb) {
                                    rb.velocity = Vector3.zero;
                                    rb.angularVelocity = Vector3.zero;
                                }
                            }
                            break;
                        case 'E': // Horizontal Enemy
                            CreateEnemy(pos, true, currentLevelRoot.transform);
                            break;
                        case 'Z': // Vertical Enemy
                            CreateEnemy(pos, false, currentLevelRoot.transform);
                            break;
                    }
                }
            }
        }

        private void CreateCube(Vector3 position, Texture2D tex, BlockType type, Transform parent, Color? color = null)
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = position;
            cube.transform.parent = parent;

            Renderer r = cube.GetComponent<Renderer>();
            if (tex != null) r.material.mainTexture = tex;
            if (color.HasValue) r.material.color = color.Value;

            InteractableBlock block = cube.AddComponent<InteractableBlock>();
            block.Type = type;

            if (type == BlockType.Goal)
            {
                cube.transform.localScale = Vector3.one * 0.8f;
                cube.GetComponent<Collider>().isTrigger = true;
            }
        }

        private void CreateEnemy(Vector3 position, bool horizontal, Transform parent)
        {
             GameObject enemy = GameObject.CreatePrimitive(PrimitiveType.Sphere);
             enemy.name = "Enemy";
             enemy.transform.position = position;
             enemy.transform.parent = parent;

             EnemyController ec = enemy.AddComponent<EnemyController>();
             ec.moveHorizontal = horizontal;
        }

        public void CompleteLevel()
        {
            Debug.Log($"Completed Level {currentLevelIndex}");
            currentLevelIndex++;
            if (currentLevelIndex >= levels.Count)
            {
                Debug.Log("ALL LEVELS COMPLETED");
                currentState = GameState.Victory;
                if (currentLevelRoot != null) Destroy(currentLevelRoot);
            }
            else
            {
                GenerateCurrentLevel();
            }
        }

        public void GameOver()
        {
            currentState = GameState.GameOver;
            if (currentLevelRoot != null) Destroy(currentLevelRoot);
        }

        public void RestartLevel()
        {
             Debug.Log("Restarting Level...");
             GenerateCurrentLevel();
        }

        public int GetCurrentLevelIndex()
        {
            return currentLevelIndex + 1;
        }
    }
}
