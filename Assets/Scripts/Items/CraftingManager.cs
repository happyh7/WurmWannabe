using UnityEngine;

public class CraftingManager : MonoBehaviour
{
    public ItemData stickData;
    public ItemData stoneData;

    public bool CanCraftAxe()
    {
        return InventoryManager.Instance.GetItemQuantity(stickData) >= 1 &&
               InventoryManager.Instance.GetItemQuantity(stoneData) >= 1;
    }

    public void CraftAxe()
    {
        // Kolla om vi har tillräckligt med material
        if (CanCraftAxe())
        {
            // Ta bort materialen
            InventoryManager.Instance.RemoveItem(stoneData);
            InventoryManager.Instance.RemoveItem(stickData);

            // Ladda axe prefab från Resources
            ItemData axeItem = Resources.Load<ItemData>("Items/Axe");
            if (axeItem != null)
            {
                // Skapa en ny instans av yxan varje gång
                ItemData newAxe = ScriptableObject.Instantiate(axeItem);
                newAxe.name = axeItem.name;
                Logger.Instance.Log($"[CraftAxe] Laddade yxa. isStackable: {newAxe.isStackable}", Logger.LogLevel.Info);
                // Lägg till yxan i inventory
                InventoryManager.Instance.AddItem(newAxe);
            }
            else
            {
                Logger.Instance.Log("[CraftAxe] Kunde inte ladda yxa från Resources!", Logger.LogLevel.Error);
            }
        }
        else
        {
            Logger.Instance.Log("[CraftAxe] Du har inte tillräckligt med material för att crafta en yxa.", Logger.LogLevel.Warning);
        }
    }
} 