using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    public GameObject slotPrefab;  // Prefab för inventory slots
    public Transform gridContainer;  // Reference till GridLayoutGroup
    public GameObject inventoryPanel;  // Huvudpanelen för inventory
    public int slotCount = 20; // Antal slots i inventoryt (t.ex. 5x4)
    
    private List<InventorySlot> slots = new List<InventorySlot>();
    private bool isInventoryOpen = false;

    void Start()
    {
        // Skapa inventory slots
        for (int i = 0; i < slotCount; i++)
        {
            GameObject newSlot = Instantiate(slotPrefab, gridContainer);
            newSlot.name = $"Inventory Slot {i + 1}"; // Ge sloten ett meningsfullt namn
            InventorySlot slotScript = newSlot.GetComponent<InventorySlot>();
            slots.Add(slotScript);
            
            // Säkerställ att sloten är tom från början
            slotScript.ClearSlot();
        }

        // Stäng inventory vid start
        inventoryPanel.SetActive(false);
    }

    void Update()
    {
        // Öppna/stäng inventory med I-tangenten
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }

        if (isInventoryOpen && InventoryManager.Instance != null)
        {
            UpdateInventoryDisplay();
        }
    }

    void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen;
        inventoryPanel.SetActive(isInventoryOpen);
    }

    void UpdateInventoryDisplay()
    {
        if (InventoryManager.Instance == null) return;

        var items = InventoryManager.Instance.GetItems();
        
        // Uppdatera bara quantity för existerande items och ta bort items som inte längre finns
        foreach (var slot in slots)
        {
            // Skippa AxeSlot och slots som nyligen har fått ett item via drag & drop
            if (slot is AxeSlot || slot.isLastUnequipTarget) continue;
            
            ItemData currentItem = slot.GetItem();
            if (currentItem != null)
            {
                if (!items.ContainsKey(currentItem))
                {
                    // Ta bort item om det inte längre finns i inventory
                    slot.ClearSlot();
                }
                // Vi behöver inte uppdatera SetItem här eftersom det redan finns i sloten
            }
        }

        // Lägg till nya items i tomma slots
        foreach (var kvp in items)
        {
            // Kolla om itemet redan finns i någon slot
            bool itemExists = false;
            foreach (var slot in slots)
            {
                // Skippa AxeSlot och slots som nyligen har fått ett item via drag & drop
                if (slot is AxeSlot || slot.isLastUnequipTarget) continue;
                
                if (slot.GetItem() == kvp.Key)
                {
                    itemExists = true;
                    break;
                }
            }

            // Om itemet inte finns i någon slot, hitta en tom slot att lägga det i
            if (!itemExists)
            {
                foreach (var slot in slots)
                {
                    // Skippa AxeSlot och slots som nyligen har fått ett item via drag & drop
                    if (slot is AxeSlot || slot.isLastUnequipTarget) continue;
                    
                    if (slot.GetItem() == null)
                    {
                        slot.SetItem(kvp.Key);
                        break;
                    }
                }
            }
        }
    }
} 