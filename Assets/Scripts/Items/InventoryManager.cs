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
        if (item == null)
        {
            Logger.Instance.Log("[AddItem] Försöker lägga till null item!", Logger.LogLevel.Error);
            return;
        }

        Logger.Instance.Log($"[AddItem] Lägger till {item.itemName} (isStackable: {item.isStackable})", Logger.LogLevel.Info);

        // Om itemet är stackable, uppdatera bara kvantiteten
        if (item.isStackable)
        {
            if (items.ContainsKey(item))
            {
                items[item]++;
            }
            else
            {
                items[item] = 1;
            }
        }
        else
        {
            // För non-stackable items, lägg till den befintliga instansen
            if (!items.ContainsKey(item))
            {
                items[item] = 1;
            }
        }

        // Uppdatera UI
        UpdateUI();
    }

    // UpdateUI behöver inte längre hantera placering av nya items, bara synka UI med items-dictionary
    private void UpdateUI()
    {
        var inventorySlots = FindObjectsByType<InventorySlot>(FindObjectsSortMode.None)
            .Where(slot => !(slot is AxeSlot))
            .OrderBy(slot => slot.transform.GetSiblingIndex())
            .ToList();

        // Synka alla slots med items-dictionary
        var itemsList = items.Keys.ToList();
        int itemIndex = 0;
        foreach (var slot in inventorySlots)
        {
            if (itemIndex < itemsList.Count)
            {
                var item = itemsList[itemIndex];
                if (item != null)
                {
                    slot.SetItem(item);
                    itemIndex++;
                }
            }
            else
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
        Logger.Instance.Log($"[InventoryManager.RemoveItem] Försöker ta bort item: {(item != null ? item.itemName : "null")}", Logger.LogLevel.Info);
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
        // Om itemet är en yxa, ta bort det från inventory och lägg till det i den nya sloten
        if (item != null && item.itemName.Contains("Axe"))
        {
            if (items.ContainsKey(item))
            {
                items.Remove(item);
                Logger.Instance.Log($"[UpdateItemSlot] Tog bort {item.itemName} från inventory", Logger.LogLevel.Info);
            }
        }
    }
} 