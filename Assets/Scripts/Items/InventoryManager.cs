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
            return;
        }

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
            if (!items.ContainsKey(item))
            {
                items[item] = 1;
            }
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        var inventorySlots = FindObjectsByType<InventorySlot>(FindObjectsSortMode.None)
            .Where(slot => !(slot is AxeSlot))
            .OrderBy(slot => slot.transform.GetSiblingIndex())
            .ToList();

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
        if (items.ContainsKey(item))
        {
            items[item]--;
            if (items[item] <= 0)
                items.Remove(item);
            return true;
        }
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
        if (item != null && item.itemName.Contains("Axe"))
        {
            if (items.ContainsKey(item))
            {
                items.Remove(item);
            }
        }
    }
} 