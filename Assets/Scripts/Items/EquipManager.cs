using UnityEngine;

public class EquipManager : MonoBehaviour
{
    public static EquipManager Instance;
    public AxeSlot axeSlot;
    private ItemData equippedAxe;

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

    public void EquipAxe(ItemData axe)
    {
        if (axe == null)
        {
            Logger.Instance.Log("[EquipAxe] Försöker equipa null axe", Logger.LogLevel.Error);
            return;
        }

        if (equippedAxe != null)
        {
            Logger.Instance.Log($"[EquipAxe] Försöker equipa {axe.itemName} men redan har {equippedAxe.itemName} equipad - unequippar först", Logger.LogLevel.Info);
            UnequipAxe();
        }

        // Ta bort yxan från inventory först
        if (!InventoryManager.Instance.RemoveItem(axe))
        {
            Logger.Instance.Log($"[EquipAxe] Kunde inte ta bort {axe.itemName} från inventory", Logger.LogLevel.Error);
            return;
        }

        Logger.Instance.Log($"[EquipAxe] Tog bort {axe.itemName} från inventory", Logger.LogLevel.Info);
        equippedAxe = axe;
        axeSlot.SetItem(axe);
        Logger.Instance.Log($"[EquipAxe] Equippade {axe.itemName}", Logger.LogLevel.Info);
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
        
        // Rensa equipment slot och referens först
        axeSlot.ClearSlot();
        equippedAxe = null;
        Logger.Instance.Log($"[UnequipAxe] Rensat axeSlot och equippedAxe referens", Logger.LogLevel.Info);
        
        // Lägg till yxan i inventory sist
        InventoryManager.Instance.AddItem(axeToUnequip);
        Logger.Instance.Log($"[UnequipAxe] Lagt till {axeToUnequip.itemName} i inventory", Logger.LogLevel.Info);
        Logger.Instance.Log($"[UnequipAxe] Antal i inventory: {InventoryManager.Instance.GetItemQuantity(axeToUnequip)}", Logger.LogLevel.Debug);
    }

    public void UnequipAxeToSlot(InventorySlot targetSlot)
    {
        if (equippedAxe == null)
        {
            Logger.Instance.Log("[UnequipAxeToSlot] Ingen yxa är equipped", Logger.LogLevel.Warning);
            return;
        }

        Logger.Instance.Log($"[UnequipAxeToSlot] Unequippar yxa till specifik slot", Logger.LogLevel.Info);
        
        // Markera target slot
        targetSlot.isLastUnequipTarget = true;
        
        // Lägg till yxan i inventory först
        InventoryManager.Instance.AddItem(equippedAxe);
        
        // Rensa equipped slot
        equippedAxe = null;
        if (axeSlot != null)
        {
            axeSlot.ClearSlot();
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
} 