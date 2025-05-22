using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class EquipManager : MonoBehaviour
{
    public static EquipManager Instance;
    public AxeSlot axeSlot;
    private ItemData equippedAxe;
    [SerializeField] private InventoryManager inventoryManager;
    [SerializeField] private Image axeIcon;
    [SerializeField] private Sprite defaultAxeSprite;
    [SerializeField] private Sprite brokenAxeSprite;

    private bool isAxeEquipped = false;
    private bool isAxeBroken = false;
    private int axeDurability = 100;
    private const int MAX_DURABILITY = 100;
    private const int DURABILITY_LOSS_PER_USE = 10;
    private Dictionary<ItemData, int> axeDurabilities = new Dictionary<ItemData, int>();

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

    private void Start()
    {
        if (axeIcon != null)
        {
            axeIcon.sprite = defaultAxeSprite;
        }

        // Ladda och equippa unbreakable yxan
        ItemData unbreakableAxe = Resources.Load<ItemData>("Items/UnbreakableAxe");
        if (unbreakableAxe != null)
        {
            EquipAxe(unbreakableAxe);
            Logger.Instance.Log("[EquipManager.Start] Equippade Unbreakable Axe", Logger.LogLevel.Info);
        }
        else
        {
            Logger.Instance.Log("[EquipManager.Start] Kunde inte hitta Unbreakable Axe i Resources", Logger.LogLevel.Warning);
        }
    }

    public bool HasAxeEquipped()
    {
        return isAxeEquipped && !isAxeBroken;
    }

    public bool IsAxeBroken()
    {
        return isAxeBroken;
    }

    public void EquipAxe(ItemData axe)
    {
        if (axe == null) return;
        // Om yxan är trasig, visa notis och förhindra equip
        if (axeDurabilities.ContainsKey(axe) && axeDurabilities[axe] <= 0)
        {
            NotificationManager.Instance?.ShowNotification("Det går inte att equippa en trasig yxa!");
            return;
        }
        // Om vi redan har en yxa equipad, unequippa den först
        if (equippedAxe != null)
        {
            UnequipAxe();
        }
        // Sätt den nya yxan i equip slot först
        axeSlot.SetItem(axe);
        equippedAxe = axe;
        // Uppdatera axeIcon om den finns, annars använd yxans egen ikon
        if (axeIcon != null)
        {
            axeIcon.sprite = axe.icon;
            axeIcon.enabled = true;
        }
        else if (axe.icon != null)
        {
            Logger.Instance.Log("[EquipAxe] axeIcon är null - använder yxans egen ikon", Logger.LogLevel.Warning);
            // Skapa en ny Image-komponent om den inte finns
            axeIcon = gameObject.AddComponent<Image>();
            axeIcon.sprite = axe.icon;
            axeIcon.enabled = true;
        }
        else if (defaultAxeSprite != null)
        {
            Logger.Instance.Log("[EquipAxe] axeIcon och axe.icon är null - använder defaultAxeSprite", Logger.LogLevel.Warning);
            // Skapa en ny Image-komponent om den inte finns
            axeIcon = gameObject.AddComponent<Image>();
            axeIcon.sprite = defaultAxeSprite;
            axeIcon.enabled = true;
        }
        else
        {
            Logger.Instance.Log("[EquipAxe] axeIcon, axe.icon och defaultAxeSprite är null - kan inte uppdatera UI", Logger.LogLevel.Error);
        }
        // Sätt equip-status
        isAxeEquipped = true;
        isAxeBroken = false;
        // Återställ eller sätt ny durability
        if (axeDurabilities.ContainsKey(axe))
        {
            axeDurability = axeDurabilities[axe];
            Logger.Instance.Log($"[EquipAxe] Återställde durability för {axe.itemName} till {axeDurability}", Logger.LogLevel.Info);
        }
        else
        {
            axeDurability = MAX_DURABILITY;
            axeDurabilities[axe] = MAX_DURABILITY;
            Logger.Instance.Log($"[EquipAxe] Satt ny durability för {axe.itemName} till {MAX_DURABILITY}", Logger.LogLevel.Info);
        }
        // Ta bort yxan från inventory om den finns där
        if (InventoryManager.Instance.GetItemQuantity(axe) > 0)
        {
            bool removedFromInventory = InventoryManager.Instance.RemoveItem(axe);
            if (!removedFromInventory)
            {
                Logger.Instance.Log("[EquipAxe] Kunde inte ta bort Axe från inventory - kanske redan equipad", Logger.LogLevel.Warning);
            }
            else
            {
                Logger.Instance.Log("[EquipAxe] Tog bort Axe från inventory", Logger.LogLevel.Info);
            }
        }
        else
        {
            Logger.Instance.Log("[EquipAxe] Axe finns inte i inventory - kanske redan equipad", Logger.LogLevel.Info);
        }
        Logger.Instance.Log("[EquipAxe] Equippade Axe", Logger.LogLevel.Info);
        // Tvinga UI-refresh på alla relevanta slots
        if (axeSlot != null) axeSlot.UpdateUI();
        var allSlots = GameObject.FindObjectsByType<InventorySlot>(FindObjectsSortMode.None);
        foreach (var slot in allSlots)
        {
            if (slot != null && slot.GetItem() != null && slot.GetItem().itemName.Contains("Axe"))
                slot.UpdateUI();
        }
    }

    public void UnequipAxe()
    {
        if (equippedAxe == null)
        {
            Logger.Instance.Log("[UnequipAxe] Försöker unequippa men ingen axe är equipad", Logger.LogLevel.Warning);
            return;
        }

        Logger.Instance.Log($"[UnequipAxe] Börjar unequippa {equippedAxe.itemName}", Logger.LogLevel.Info);
            ItemData axeToUnequip = equippedAxe;
        
        // Spara nuvarande durability innan vi unequippar
        axeDurabilities[axeToUnequip] = axeDurability;
        Logger.Instance.Log($"[UnequipAxe] Sparade durability {axeDurability} för {axeToUnequip.itemName}", Logger.LogLevel.Info);
            
            // Rensa equipment slot och referens först
            axeSlot.ClearSlot();
            equippedAxe = null;
        Logger.Instance.Log($"[UnequipAxe] Rensat axeSlot och equippedAxe referens", Logger.LogLevel.Info);
            
            // Lägg till yxan i inventory sist
            InventoryManager.Instance.AddItem(axeToUnequip);
        Logger.Instance.Log($"[UnequipAxe] Lagt till {axeToUnequip.itemName} i inventory", Logger.LogLevel.Info);
        Logger.Instance.Log($"[UnequipAxe] Antal i inventory: {InventoryManager.Instance.GetItemQuantity(axeToUnequip)}", Logger.LogLevel.Debug);

        isAxeEquipped = false;
        if (axeIcon != null)
        {
            axeIcon.sprite = null;
        }

        // Tvinga UI-refresh på alla relevanta slots
        if (axeSlot != null) axeSlot.UpdateUI();
        var allSlots = GameObject.FindObjectsByType<InventorySlot>(FindObjectsSortMode.None);
        foreach (var slot in allSlots)
        {
            if (slot != null && slot.GetItem() != null && slot.GetItem().itemName.Contains("Axe"))
                slot.UpdateUI();
        }
    }

    public void UnequipAxeToSlot(InventorySlot targetSlot)
    {
        if (!isAxeEquipped || targetSlot == null)
        {
            Logger.Instance.Log("[UnequipAxeToSlot] Ingen yxa är equipped eller targetSlot är null", Logger.LogLevel.Warning);
            return;
        }

        Logger.Instance.Log($"[UnequipAxeToSlot] Unequippar yxa till specifik slot", Logger.LogLevel.Info);
        
        // Använd den ursprungliga yxan
            ItemData axeToUnequip = equippedAxe;

        // Lägg till yxan i inventory först
        InventoryManager.Instance.AddItem(axeToUnequip);
        Logger.Instance.Log($"[UnequipAxeToSlot] Lagt till {axeToUnequip.itemName} i inventory", Logger.LogLevel.Info);

        // Sätt yxan i target slot
        targetSlot.SetItem(axeToUnequip);
            
        // Rensa equipment slot och referens
            axeSlot.ClearSlot();
            equippedAxe = null;
        Logger.Instance.Log($"[UnequipAxeToSlot] Rensat axeSlot och equippedAxe referens", Logger.LogLevel.Info);

        isAxeEquipped = false;
        if (axeIcon != null)
        {
            axeIcon.sprite = null;
        }
    }

    public bool IsAxeEquipped()
    {
        return equippedAxe != null;
    }

    public ItemData GetEquippedAxe()
    {
        return equippedAxe;
    }

    public void UseAxe()
    {
        if (!isAxeEquipped || isAxeBroken) return;

        // Om yxan är unbreakable, minska inte durability
        if (equippedAxe != null && equippedAxe.isUnbreakable)
        {
            return;
        }

        axeDurability -= DURABILITY_LOSS_PER_USE;

        if (axeSlot != null)
            axeSlot.UpdateDurabilityBar();

        if (axeDurability <= 0)
        {
            BreakAxe();
        }
    }

    public void BreakAxe()
    {
        isAxeBroken = true;
        if (equippedAxe != null)
        {
            // Sätt yxans ikon till brokenAxeSprite innan den unequippas
            equippedAxe.icon = brokenAxeSprite;
            if (axeIcon != null)
            {
                axeIcon.sprite = brokenAxeSprite;
            }
            // Markera yxan som trasig (om ItemData har en flagga, annars hantera i dictionary)
            axeDurabilities[equippedAxe] = 0;
            UnequipAxe();
        }
    }

    public void RepairAxe()
    {
        if (!isAxeBroken) return;

        isAxeBroken = false;
        axeDurability = MAX_DURABILITY;
        if (axeIcon != null)
        {
            axeIcon.sprite = defaultAxeSprite;
        }
    }

    public int GetAxeDurability()
    {
        return axeDurability;
    }

    public int GetAxeMaxDurability()
    {
        return MAX_DURABILITY;
    }

    public void UnequipAxeWithoutAddingToInventory()
    {
        if (equippedAxe == null)
        {
            Logger.Instance.Log("[UnequipAxeWithoutAddingToInventory] Ingen yxa är equipped", Logger.LogLevel.Warning);
            return;
        }

        Logger.Instance.Log($"[UnequipAxeWithoutAddingToInventory] Unequippar {equippedAxe.itemName}", Logger.LogLevel.Info);
        
        // Rensa equipment slot och referens
        axeSlot.ClearSlot();
        equippedAxe = null;
        Logger.Instance.Log($"[UnequipAxeWithoutAddingToInventory] Rensat axeSlot och equippedAxe referens", Logger.LogLevel.Info);

        isAxeEquipped = false;
        if (axeIcon != null)
        {
            axeIcon.sprite = null;
        }
    }

    public int GetAxeDurabilityFor(ItemData axe)
    {
        if (axeDurabilities.ContainsKey(axe))
            return axeDurabilities[axe];
        return MAX_DURABILITY;
    }
} 