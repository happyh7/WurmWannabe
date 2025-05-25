using UnityEngine;

public class CraftingManager : MonoBehaviour
{
    public ItemData stickData;
    public ItemData stoneData;

    /// <summary>
    /// Returnerar true om spelaren har tillräckligt med material för att crafta en yxa.
    /// </summary>
    public bool CanCraftAxe()
    {
        if (InventoryManager.Instance == null || stickData == null || stoneData == null) return false;
        return InventoryManager.Instance.GetItemQuantity(stickData) >= 1 &&
               InventoryManager.Instance.GetItemQuantity(stoneData) >= 1;
    }

    /// <summary>
    /// Försöker crafta en yxa och lägger till den i inventory om det går.
    /// </summary>
    public void CraftAxe()
    {
        if (!CanCraftAxe())
        {
            Logger.Instance.Log("[CraftAxe] Du har inte tillräckligt med material för att crafta en yxa.", Logger.LogLevel.Warning);
            return;
        }
        // Ta bort materialen
        InventoryManager.Instance.RemoveItem(stoneData);
        InventoryManager.Instance.RemoveItem(stickData);
        // Ladda axe prefab från Resources
        ItemData axeItem = Resources.Load<ItemData>("Items/Axe");
        if (axeItem != null)
        {
            // Skapa en ny instans av yxan för att undvika att dela referens
            ItemData newAxe = ScriptableObject.Instantiate(axeItem);
            newAxe.name = "Axe";
            newAxe.itemName = "Axe";
            newAxe.icon = axeItem.icon;
            newAxe.description = axeItem.description;
            newAxe.isStackable = axeItem.isStackable;
            InventoryManager.Instance.AddItem(newAxe);
        }
        else
        {
            Logger.Instance.Log("[CraftAxe] Kunde inte ladda yxa från Resources!", Logger.LogLevel.Error);
        }
    }
} 