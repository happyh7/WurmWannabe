using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class AxeSlot : InventorySlot
{
    public static ItemData DraggedAxeItem = null;

    public override void OnBeginDrag(PointerEventData eventData)
    {
        if (currentItem == null) return;
        Logger.Instance.Log($"[AxeSlot.OnBeginDrag] Börjar dra {currentItem.itemName}", Logger.LogLevel.Info);
        DraggedAxeItem = currentItem;
            base.OnBeginDrag(eventData);
    }

    public override void OnDrag(PointerEventData eventData)
    {
        if (ghostImage == null) return;
        ghostImage.transform.position = Input.mousePosition;
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        DraggedAxeItem = null;
        // Visa original ikonen igen och återaktivera raycast
        icon.enabled = true;
        canvasGroup.blocksRaycasts = true;
        
        // Ta bort ghost
        if (ghostImage != null)
                    {
            Destroy(ghostImage);
            ghostImage = null;
        }
    }

    public override void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;

        InventorySlot droppedSlot = eventData.pointerDrag.GetComponent<InventorySlot>();
        if (droppedSlot == null) return;

        // Om vi försöker droppa på samma slot, avbryt
        if (droppedSlot == this) return;

        // Om det är en InventorySlot med en yxa
        if (droppedSlot is InventorySlot && droppedSlot.GetItem()?.itemName.Contains("Axe") == true)
        {
            Logger.Instance.Log("[AxeSlot.OnDrop] Droppar Axe på AxeSlot", Logger.LogLevel.Info);
            
            // Spara referensen till yxan innan vi equipar
            ItemData axeToEquip = droppedSlot.GetItem();
            
            // Rensa den ursprungliga slotten först
            droppedSlot.ClearSlot();
            
            // Equippa yxan
            EquipManager.Instance.EquipAxe(axeToEquip);
            return;
        }
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (GetItem() != null)
        {
            float timeSinceLastClick = Time.time - lastClickTime;
            
            if (timeSinceLastClick <= doubleClickTimeThreshold)
            {
                // Double click detected - unequip the axe to first available slot
                Logger.Instance.Log($"[AxeSlot.OnPointerClick] Dubbelklick på {GetItem().itemName}, unequippar", Logger.LogLevel.Info);
                EquipManager.Instance.UnequipAxe();
            }
            
            lastClickTime = Time.time;
        }
    }

    public override void SetItem(ItemData item)
    {
        if (item == null)
        {
            Logger.Instance.Log("[AxeSlot.SetItem] Försöker sätta null item i AxeSlot", Logger.LogLevel.Error);
            return;
        }

        if (!item.itemName.Contains("Axe"))
        {
            Logger.Instance.Log($"[AxeSlot.SetItem] Försöker sätta icke-yxa ({item.itemName}) i AxeSlot", Logger.LogLevel.Warning);
            return;
        }

        Logger.Instance.Log($"[AxeSlot.SetItem] Sätter {item.itemName} i AxeSlot", Logger.LogLevel.Info);
        base.SetItem(item);
        if (item != null && item.itemName.Contains("Axe"))
        {
            if (durabilityBar != null)
            {
                durabilityBar.gameObject.SetActive(true);
                durabilityBar.SetDurability(EquipManager.Instance.GetAxeDurability(), EquipManager.Instance.GetAxeMaxDurability());
            }
        }
        else if (durabilityBar != null)
        {
            durabilityBar.gameObject.SetActive(false);
        }
    }

    public override void ClearSlot()
    {
        if (currentItem == null) return;
        Logger.Instance.Log($"[AxeSlot.ClearSlot] Rensar AxeSlot som innehöll: {currentItem.itemName}", Logger.LogLevel.Info);
        if (durabilityBar != null)
        {
            durabilityBar.gameObject.SetActive(false);
        }
        base.ClearSlot();
    }

    public new void UpdateDurabilityBar()
    {
        if (durabilityBar != null && currentItem != null)
        {
            durabilityBar.gameObject.SetActive(true);
            durabilityBar.SetDurability(EquipManager.Instance.GetAxeDurability(), EquipManager.Instance.GetAxeMaxDurability());
        }
        else if (durabilityBar != null)
        {
            durabilityBar.gameObject.SetActive(false);
        }
    }
} 