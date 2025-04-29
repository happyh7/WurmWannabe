#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

public class ItemCreator
{
    [MenuItem("Tools/Create Axe Item")]
    public static void CreateAxeItem()
    {
        // Hitta axe ikonen först
        string iconPath = "Assets/Sprites/Items/axe.png"; // Justera sökvägen till där din axe ikon finns
        Sprite axeSprite = AssetDatabase.LoadAssetAtPath<Sprite>(iconPath);
        
        if (axeSprite == null)
        {
            Debug.LogError("Could not find axe sprite at: " + iconPath);
            return;
        }

        // Kolla om Axe.asset redan finns
        string assetPath = "Assets/Resources/Items/Axe.asset";
        ItemData existingAxe = AssetDatabase.LoadAssetAtPath<ItemData>(assetPath);
        
        if (existingAxe != null)
        {
            // Uppdatera existerande yxa
            existingAxe.itemName = "Axe";
            existingAxe.description = "A sturdy axe for chopping wood";
            existingAxe.isStackable = false;
            existingAxe.icon = axeSprite;
            
            EditorUtility.SetDirty(existingAxe);
            AssetDatabase.SaveAssets();
            Debug.Log("Updated existing Axe item!");
        }
        else
        {
            // Skapa ny ItemData för yxan
            ItemData axe = ScriptableObject.CreateInstance<ItemData>();
            axe.itemName = "Axe";
            axe.description = "A sturdy axe for chopping wood";
            axe.isStackable = false;
            axe.icon = axeSprite;

            // Skapa mappen om den inte finns
            string folderPath = System.IO.Path.GetDirectoryName(assetPath);
            if (!System.IO.Directory.Exists(folderPath))
            {
                System.IO.Directory.CreateDirectory(folderPath);
            }

            // Spara asset-filen
            AssetDatabase.CreateAsset(axe, assetPath);
            AssetDatabase.SaveAssets();
            Debug.Log("Created new Axe item!");
        }
        
        AssetDatabase.Refresh();
    }
}
#endif 