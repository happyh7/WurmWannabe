using UnityEngine;
using UnityEditor;
using System.IO;

public class TreeSpriteCreator : MonoBehaviour
{
    private const int WIDTH = 64;
    private const int HEIGHT = 96;
    private const float TRUNK_WIDTH_RATIO = 0.3f;  // Stammens bredd i förhållande till total bredd
    private const float TRUNK_HEIGHT_RATIO = 0.4f; // Stammens höjd i förhållande till total höjd

    [MenuItem("Tools/Create Tree Sprite")]
    public static void CreateTreeSprite()
    {
        // Skapa en ny texture med given storlek
        Texture2D texture = new Texture2D(WIDTH, HEIGHT);
        Color[] pixels = new Color[WIDTH * HEIGHT];

        // Fyll bakgrunden med transparent färg
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = Color.clear;
        }

        // Rita stammen (brun rektangel)
        int trunkWidth = Mathf.RoundToInt(WIDTH * TRUNK_WIDTH_RATIO);
        int trunkHeight = Mathf.RoundToInt(HEIGHT * TRUNK_HEIGHT_RATIO);
        int trunkStartX = (WIDTH - trunkWidth) / 2;
        
        Color trunkColor = new Color(0.45f, 0.27f, 0.07f); // Brun färg
        
        for (int y = 0; y < trunkHeight; y++)
        {
            for (int x = trunkStartX; x < trunkStartX + trunkWidth; x++)
            {
                // Lägg till lite variation i stammens färg
                float colorVariation = Random.Range(-0.05f, 0.05f);
                Color variedTrunkColor = new Color(
                    trunkColor.r + colorVariation,
                    trunkColor.g + colorVariation,
                    trunkColor.b + colorVariation,
                    1
                );
                pixels[y * WIDTH + x] = variedTrunkColor;
            }
        }

        // Rita kronan (grön cirkel med variation)
        Color leafColor = new Color(0.15f, 0.5f, 0.15f); // Mörkgrön färg
        int crownCenterX = WIDTH / 2;
        int crownCenterY = HEIGHT - (HEIGHT - trunkHeight) / 2;
        int crownRadius = Mathf.Min(WIDTH / 2, (HEIGHT - trunkHeight) / 2);

        for (int y = trunkHeight; y < HEIGHT; y++)
        {
            for (int x = 0; x < WIDTH; x++)
            {
                float distance = Mathf.Sqrt(
                    (x - crownCenterX) * (x - crownCenterX) + 
                    (y - crownCenterY) * (y - crownCenterY)
                );

                if (distance < crownRadius)
                {
                    // Lägg till mer variation i kronans färg
                    float colorVariation = Random.Range(-0.15f, 0.15f);
                    Color variedColor = new Color(
                        leafColor.r + colorVariation,
                        leafColor.g + colorVariation,
                        leafColor.b + colorVariation,
                        1
                    );
                    pixels[y * WIDTH + x] = variedColor;
                }
            }
        }

        // Applicera pixlarna till texturen
        texture.SetPixels(pixels);
        texture.Apply();

        // Skapa Environment-mappen om den inte finns
        string dirPath = "Assets/Sprites/Environment";
        if (!Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
            AssetDatabase.Refresh();
        }

        // Spara texturen som en PNG-fil
        string path = "Assets/Sprites/Environment/Tree.png";
        byte[] pngData = texture.EncodeToPNG();
        File.WriteAllBytes(path, pngData);
        AssetDatabase.Refresh();

        // Konfigurera sprite-inställningar
        TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
        if (importer != null)
        {
            importer.textureType = TextureImporterType.Sprite;
            importer.spritePixelsPerUnit = 64;
            importer.filterMode = FilterMode.Point;
            importer.textureCompression = TextureImporterCompression.Uncompressed;
            importer.SaveAndReimport();
        }

        Debug.Log("Tree sprite skapad i: " + path);
    }
} 