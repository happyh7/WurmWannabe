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
        // Thread-safe singleton-initiering
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (axeIcon != null)
        {
            axeIcon.sprite = defaultAxeSprite;
        }
        // Ladda och equippa unbreakable yxan om den finns
        var unbreakableAxe = Resources.Load<ItemData>("Items/UnbreakableAxe");
        if (unbreakableAxe != null)
        {
            EquipAxe(unbreakableAxe);
        }
    }

    /// <summary>
    /// Returnerar true om en fungerande yxa är equipad.
    /// </summary>
    public bool HasAxeEquipped()
    {
        return equippedAxe != null && isAxeEquipped && !isAxeBroken;
    }

    /// <summary>
    /// Returnerar true om yxan är trasig.
    /// </summary>
    public bool IsAxeBroken()
    {
        return isAxeBroken;
    }

    /// <summary>
    /// Returnerar true om någon yxa är equipad (oavsett skick).
    /// </summary>
    public bool IsAxeEquipped()
    {
        return equippedAxe != null;
    }

    public void EquipAxe(ItemData axe)
    {
        if (axe == null) return;
        // Om yxan är trasig, visa notis och förhindra equip
        if (axeDurabilities.TryGetValue(axe, out int durability) && durability <= 0)
        {
            NotificationManager.Instance?.ShowNotification("Det går inte att equippa en trasig yxa!");
            return;
        }
        // Unequippa befintlig yxa först
        if (equippedAxe != null)
        {
            UnequipAxe();
        }
        // Sätt den nya yxan i equip slot
        axeSlot?.SetItem(axe);
        equippedAxe = axe;
        // Uppdatera axeIcon
        if (axeIcon != null)
        {
            axeIcon.sprite = axe.icon ?? defaultAxeSprite;
            axeIcon.enabled = true;
        }
        // Sätt equip-status
        isAxeEquipped = true;
        isAxeBroken = false;
        // Återställ eller sätt ny durability
        if (axeDurabilities.TryGetValue(axe, out int savedDurability))
        {
            axeDurability = savedDurability;
        }
        else
        {
            axeDurability = MAX_DURABILITY;
            axeDurabilities[axe] = MAX_DURABILITY;
        }
        // Ta bort yxan från inventory om den finns där
        if (InventoryManager.Instance != null && InventoryManager.Instance.GetItemQuantity(axe) > 0)
        {
            InventoryManager.Instance.RemoveItem(axe);
        }
        // Tvinga UI-refresh på alla relevanta slots
        axeSlot?.UpdateUI();
        var allSlots = GameObject.FindObjectsByType<InventorySlot>(FindObjectsSortMode.None);
        foreach (var slot in allSlots)
        {
            if (slot?.GetItem() != null && slot.GetItem().itemName.Contains("Axe"))
                slot.UpdateUI();
        }
    }

    public void UnequipAxe()
    {
        if (equippedAxe == null) return;
        // Spara nuvarande durability innan vi unequippar
        axeDurabilities[equippedAxe] = axeDurability;
        // Rensa equipment slot och referens
        axeSlot?.ClearSlot();
        // Lägg till yxan i inventory sist
        if (InventoryManager.Instance != null)
            InventoryManager.Instance.AddItem(equippedAxe);
        equippedAxe = null;
        isAxeEquipped = false;
        if (axeIcon != null)
        {
            axeIcon.sprite = null;
        }
        // Tvinga UI-refresh på alla relevanta slots
        axeSlot?.UpdateUI();
        var allSlots = GameObject.FindObjectsByType<InventorySlot>(FindObjectsSortMode.None);
        foreach (var slot in allSlots)
        {
            if (slot?.GetItem() != null && slot.GetItem().itemName.Contains("Axe"))
                slot.UpdateUI();
        }
    }

    public void UnequipAxeToSlot(InventorySlot targetSlot)
    {
        if (!isAxeEquipped || targetSlot == null) return;
        ItemData axeToUnequip = equippedAxe;
        if (InventoryManager.Instance != null)
            InventoryManager.Instance.AddItem(axeToUnequip);
        targetSlot.SetItem(axeToUnequip);
        axeSlot.ClearSlot();
        equippedAxe = null;
        isAxeEquipped = false;
        if (axeIcon != null)
        {
            axeIcon.sprite = null;
        }
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
        if (equippedAxe == null) return;
        axeSlot.ClearSlot();
        equippedAxe = null;
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