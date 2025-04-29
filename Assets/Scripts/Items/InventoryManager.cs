using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    private Dictionary<ItemData, int> items = new Dictionary<ItemData, int>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddItem(ItemData item)
    {
        if (item == null) return;

        Logger.Instance.Log($"[AddItem] Försöker lägga till {item.itemName}. isStackable: {item.isStackable}", Logger.LogLevel.Info);

        // För icke-stackbara items, skapa en ny instans
        if (!item.isStackable)
        {
            // Skapa en ny instans av ItemData för icke-stackbara items
            ItemData newInstance = ScriptableObject.Instantiate(item);
            newInstance.name = item.name; // Kopiera namnet från originalet
            items[newInstance] = 1;
            Logger.Instance.Log($"[AddItem] Lade till ny instans av {item.itemName} i inventory", Logger.LogLevel.Info);
        }
        else if (!items.ContainsKey(item))
        {
            // För stackbara items, använd original-referensen
            items[item] = 1;
            Logger.Instance.Log($"[AddItem] Lade till {item.itemName} i inventory. Totalt: 1", Logger.LogLevel.Info);
        }
        else
        {
            // Om itemet kan stackas och redan finns, öka quantity
            items[item]++;
            Logger.Instance.Log($"[AddItem] Lade till {item.itemName} i inventory. Totalt: {items[item]}", Logger.LogLevel.Info);
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        // Hitta alla inventory slots (exklusive AxeSlot) och sortera dem efter position
        var inventorySlots = FindObjectsByType<InventorySlot>(FindObjectsSortMode.None)
            .Where(slot => !(slot is AxeSlot))
            .OrderBy(slot => slot.transform.GetSiblingIndex())
            .ToList();

        // Skapa en lista över items som inte har en slot
        var unassignedItems = new List<ItemData>();
        foreach (var kvp in items)
        {
            bool itemFound = false;
            foreach (var slot in inventorySlots)
            {
                if (slot.GetItem() == kvp.Key)
                {
                    itemFound = true;
                    break;
                }
            }
            if (!itemFound)
            {
                unassignedItems.Add(kvp.Key);
            }
        }

        // Placera ut oplacerade items i första lediga slot
        foreach (var item in unassignedItems)
        {
            bool itemPlaced = false;
            
            // Om detta item nyligen har unequippats, försök placera det i den valda sloten först
            var targetSlot = inventorySlots.FirstOrDefault(slot => slot.isLastUnequipTarget);
            if (targetSlot != null && targetSlot.GetItem() == null)
            {
                targetSlot.SetItem(item);
                targetSlot.isLastUnequipTarget = false; // Återställ flaggan
                itemPlaced = true;
                Logger.Instance.Log($"[UpdateUI] Placerade {item.itemName} i vald unequip slot", Logger.LogLevel.Info);
            }

            // Om itemet inte placerades i en specifik slot, hitta första lediga slot
            if (!itemPlaced)
            {
                foreach (var slot in inventorySlots)
                {
                    if (slot.GetItem() == null)
                    {
                        slot.SetItem(item);
                        Logger.Instance.Log($"[UpdateUI] Placerade {item.itemName} i första lediga slot", Logger.LogLevel.Info);
                        break;
                    }
                }
            }
        }

        // Ta bort items som inte längre finns i inventory
        foreach (var slot in inventorySlots)
        {
            var currentItem = slot.GetItem();
            if (currentItem != null && !items.ContainsKey(currentItem))
            {
                slot.ClearSlot();
            }
        }
    }

    public Dictionary<ItemData, int> GetItems()
    {
        return items;
    }

    public bool RemoveItem(ItemData item)
    {
        if (items.ContainsKey(item))
        {
            items[item]--;
            if (items[item] <= 0)
                items.Remove(item);
            Logger.Instance.Log($"[RemoveItem] Tog bort {item.itemName} från inventory. Antal kvar: {(items.ContainsKey(item) ? items[item] : 0)}", Logger.LogLevel.Info);
            return true;
        }
        Logger.Instance.Log($"[RemoveItem] Kunde inte ta bort {item.itemName} från inventory - finns inte", Logger.LogLevel.Warning);
        return false;
    }

    public int GetItemQuantity(ItemData item)
    {
        if (items.ContainsKey(item))
        {
            return items[item];
        }
        return 0;
    }

    public void UpdateItemSlot(ItemData item, InventorySlot slot)
    {
        // Denna metod används för att hålla koll på var items finns i UI:t
        // Om det behövs kan du lägga till mer logik här
    }
} 