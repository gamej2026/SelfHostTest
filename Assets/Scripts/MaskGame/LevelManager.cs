using UnityEngine;

namespace MaskGame
{
    public class LevelManager : MonoBehaviour
    {
        // Simple text-based level layout
        // P: Player Start
        // #: Wall
        // .: Ground
        // B: Breakable
        // G: Goal
        private string[] levelMap = new string[]
        {
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
        };

        public void GenerateLevel(PlayerController playerRef)
        {
            float tileSize = 1.0f;

            // Create a parent object for the level
            GameObject levelRoot = new GameObject("LevelRoot");

            // Create Ground Plane
            GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Plane);
            floor.name = "Floor";
            floor.transform.parent = levelRoot.transform;
            floor.transform.localScale = new Vector3(3, 1, 3); // 30x30 roughly
            floor.transform.position = new Vector3(10, -0.5f, 6);
            floor.GetComponent<Renderer>().material.color = Color.gray;

            for (int z = 0; z < levelMap.Length; z++)
            {
                string row = levelMap[z];
                // Invert Z so the map reads top-down in world space
                int zPos = levelMap.Length - 1 - z;

                for (int x = 0; x < row.Length; x++)
                {
                    char tile = row[x];
                    Vector3 pos = new Vector3(x * tileSize, 0.5f, zPos * tileSize);

                    switch (tile)
                    {
                        case '#':
                            CreateCube(pos, Color.black, BlockType.Wall, levelRoot.transform);
                            break;
                        case 'B':
                            CreateCube(pos, new Color(0.6f, 0.3f, 0.0f), BlockType.Breakable, levelRoot.transform); // Brown
                            break;
                        case 'G':
                            CreateCube(pos, Color.yellow, BlockType.Goal, levelRoot.transform);
                            break;
                        case 'P':
                            // Set player position
                            playerRef.transform.position = pos + Vector3.up * 1.0f;
                            break;
                    }
                }
            }
        }

        private void CreateCube(Vector3 position, Color color, BlockType type, Transform parent)
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = position;
            cube.transform.parent = parent;

            Renderer r = cube.GetComponent<Renderer>();
            r.material.color = color;

            InteractableBlock block = cube.AddComponent<InteractableBlock>();
            block.Type = type;

            if (type == BlockType.Goal)
            {
                cube.transform.localScale = Vector3.one * 0.8f;
                // Make goal trigger?
                cube.GetComponent<Collider>().isTrigger = true;
            }
        }
    }
}
