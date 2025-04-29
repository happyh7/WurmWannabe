using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AxeSlot : InventorySlot
{
    public override void OnBeginDrag(PointerEventData eventData)
    {
        if (currentItem == null) return;

        Logger.Instance.Log("[AxeSlot.OnBeginDrag] Börjar dra Axe", Logger.LogLevel.Info);
        
        // Skapa en ghost image
        ghostImage = new GameObject("Ghost");
        ghostImage.transform.SetParent(transform.root);
        ghostImage.transform.position = transform.position;
        
        // Kopiera ikonen och sätt storlek
        Image ghostSprite = ghostImage.AddComponent<Image>();
        RectTransform ghostRect = ghostImage.GetComponent<RectTransform>();
        ghostRect.sizeDelta = GetComponent<RectTransform>().sizeDelta;
        
        ghostSprite.sprite = icon.sprite;
        ghostSprite.raycastTarget = false;
        ghostSprite.color = new Color(1, 1, 1, 0.5f); // Halvgenomskinlig
        
        // Dölj original ikonen och blockera raycast
        icon.enabled = false;
        canvasGroup.blocksRaycasts = false;

        // Sätt denna slot som den som dras
        eventData.pointerDrag = gameObject;
    }

    public override void OnDrag(PointerEventData eventData)
    {
        if (ghostImage == null) return;
        ghostImage.transform.position = Input.mousePosition;
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        Logger.Instance.Log("[AxeSlot.OnEndDrag] Släpper Axe", Logger.LogLevel.Info);
        
        // Visa original ikonen igen och återaktivera raycast
        icon.enabled = true;
        canvasGroup.blocksRaycasts = true;
        
        // Hitta den slot som vi släppte på
        GameObject dropObject = eventData.pointerCurrentRaycast.gameObject;
        if (dropObject != null)
        {
            InventorySlot targetSlot = dropObject.GetComponent<InventorySlot>();
            if (targetSlot != null && !(targetSlot is AxeSlot))
            {
                Logger.Instance.Log("[AxeSlot.OnEndDrag] Hittade target slot, unequippar Axe", Logger.LogLevel.Info);
                // Unequippa yxan till den hittade slotten
                EquipManager.Instance.UnequipAxeToSlot(targetSlot);
            }
        }
        
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

        // Om det är en AxeSlot, avbryt
        if (droppedSlot is AxeSlot) return;

        // Om det är en InventorySlot med en yxa
        if (droppedSlot is InventorySlot && droppedSlot.GetItem()?.itemName.Contains("Axe") == true)
        {
            Logger.Instance.Log("[AxeSlot.OnDrop] Droppar Axe på AxeSlot", Logger.LogLevel.Info);
            // Equippa yxan
            EquipManager.Instance.EquipAxe(droppedSlot.GetItem());
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
} 