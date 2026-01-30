using UnityEngine;

namespace MaskGame
{
    public static class TextureGen
    {
        public static Texture2D CreateCheckerTexture(Color c1, Color c2, int size = 64)
        {
            Texture2D tex = new Texture2D(size, size);
            tex.filterMode = FilterMode.Point;

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    bool isWhite = (x / (size/4) + y / (size/4)) % 2 == 0;
                    tex.SetPixel(x, y, isWhite ? c1 : c2);
                }
            }
            tex.Apply();
            return tex;
        }

        public static Texture2D CreateSolidTexture(Color c, int size = 8)
        {
            Texture2D tex = new Texture2D(size, size);
            for (int i = 0; i < size * size; i++)
            {
                tex.SetPixel(i % size, i / size, c);
            }
            tex.Apply();
            return tex;
        }

        public static Texture2D CreateBrickTexture(Color brickColor, Color mortarColor, int size = 64)
        {
            Texture2D tex = new Texture2D(size, size);
            tex.filterMode = FilterMode.Point;

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    // Simple brick pattern logic
                    // Rows are roughly size/4 high
                    int row = y / (size / 4);
                    bool isMortar = false;

                    // Horizontal mortar
                    if (y % (size / 4) < 2) isMortar = true;

                    // Vertical mortar (staggered)
                    int xOffset = (row % 2 == 0) ? 0 : (size / 4);
                    if ((x + xOffset) % (size / 2) < 2) isMortar = true;

                    tex.SetPixel(x, y, isMortar ? mortarColor : brickColor);
                }
            }
            tex.Apply();
            return tex;
        }
    }
}
