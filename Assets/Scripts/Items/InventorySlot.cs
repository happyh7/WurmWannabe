using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InventorySlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler, IDropHandler
{
    public Image icon;  // Reference till Image-komponenten för item icon
    public TMP_Text quantityText;  // Text för att visa antal items
    // Ram/Frame hanteras av separat Image-objekt i prefab
    public bool isLastUnequipTarget = false; // Ny flagga för att markera unequip target

    protected ItemData currentItem;
    protected int quantity;
    protected float lastClickTime;
    protected float doubleClickTimeThreshold = 0.3f; // Tid mellan klick för dubbelklick
    protected CanvasGroup canvasGroup;
    protected RectTransform rectTransform;
    protected GameObject ghostImage;

    protected virtual void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        // Initiera tom slot
        if (quantityText != null)
        {
            quantityText.text = "";
            quantityText.enabled = false;
        }
        if (icon != null)
        {
            icon.sprite = null;
            icon.enabled = true;
        }
    }

    public virtual void SetItem(ItemData item)
    {
        if (item == currentItem)
        {
            Logger.Instance.Log($"[SetItem] Försöker sätta samma item ({item.itemName}) i slot som redan finns där", Logger.LogLevel.Warning);
            return;
        }

        Logger.Instance.Log($"[SetItem] Sätter {item.itemName} i slot", Logger.LogLevel.Info);
        currentItem = item;

        // Om det är ett non-stackable item, sätt quantity till 1
        if (!item.isStackable)
        {
            quantity = 1;
            Logger.Instance.Log($"[SetItem] {item.itemName} är non-stackable, sätter quantity till 1", Logger.LogLevel.Info);
        }
        else
        {
            quantity = InventoryManager.Instance.GetItemQuantity(item);
            Logger.Instance.Log($"[SetItem] {item.itemName} är stackable, hämtar quantity från inventory: {quantity}", Logger.LogLevel.Info);
        }

        UpdateUI();
    }

    public virtual void ClearSlot()
    {
        if (currentItem == null) return; // Om sloten redan är tom, hoppa över

        Logger.Instance.Log($"[ClearSlot] Rensar slot som innehöll: {(currentItem != null ? currentItem.itemName : "inget item")}", Logger.LogLevel.Info);
        currentItem = null;
        if (icon != null)
        {
            icon.sprite = null;
            icon.enabled = true;
        }
        if (quantityText != null)
        {
            quantityText.text = "";
            quantityText.enabled = false;
        }
    }

    public virtual ItemData GetItem()
    {
        return currentItem;
    }

    // Drag and drop funktionalitet
    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        if (currentItem == null) return;

        Logger.Instance.Log($"[OnBeginDrag] Börjar dra {currentItem.itemName}", Logger.LogLevel.Info);
        
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
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        if (ghostImage == null) return;
        ghostImage.transform.position = Input.mousePosition;
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
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

    public virtual void OnDrop(PointerEventData eventData)
    {
        InventorySlot droppedSlot = eventData.pointerDrag.GetComponent<InventorySlot>();
        if (droppedSlot == null) return;

        // Om vi försöker droppa på samma slot, avbryt
        if (droppedSlot == this) return;

        // Om det är en AxeSlot, hantera unequip
        if (droppedSlot is AxeSlot axeSlot)
        {
            Logger.Instance.Log("[OnDrop] Droppar från AxeSlot till InventorySlot", Logger.LogLevel.Info);
            EquipManager.Instance.UnequipAxeToSlot(this);
            return;
        }

        // Om det är en vanlig InventorySlot
        if (droppedSlot is InventorySlot)
        {
            Logger.Instance.Log("[OnDrop] Droppar från InventorySlot till InventorySlot", Logger.LogLevel.Info);
            ItemData droppedItem = droppedSlot.GetItem();
            ItemData currentItem = GetItem();

            // Om båda slots har samma non-stackable item, avbryt
            if (droppedItem != null && currentItem != null && 
                !droppedItem.isStackable && droppedItem == currentItem)
            {
                Logger.Instance.Log("[OnDrop] Försöker droppa samma non-stackable item, avbryter", Logger.LogLevel.Warning);
                return;
            }

            // Om det droppade itemet är non-stackable och target slot har samma item type, avbryt
            if (droppedItem != null && currentItem != null && 
                !droppedItem.isStackable && droppedItem.itemName == currentItem.itemName)
            {
                Logger.Instance.Log("[OnDrop] Kan inte stacka non-stackable items", Logger.LogLevel.Warning);
                return;
            }

            // Rensa båda slots först
            droppedSlot.ClearSlot();
            ClearSlot();

            // Sätt items i nya slots
            if (droppedItem != null)
                SetItem(droppedItem);
            if (currentItem != null)
                droppedSlot.SetItem(currentItem);
        }
    }

    // Dubbelklick funktionalitet
    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (currentItem != null && currentItem.itemName.Contains("Axe"))
        {
            float timeSinceLastClick = Time.time - lastClickTime;
            
            if (timeSinceLastClick <= doubleClickTimeThreshold)
            {
                // Dubbelklick detekterad
                Logger.Instance.Log($"[OnPointerClick] Dubbelklick på {currentItem.itemName} i {GetType().Name}", Logger.LogLevel.Info);
                if (EquipManager.Instance != null)
                {
                    EquipManager.Instance.EquipAxe(currentItem);
                }
            }
            
            lastClickTime = Time.time;
        }
    }

    protected virtual void UpdateUI()
    {
        if (currentItem != null)
        {
            Logger.Instance.Log($"[UpdateUI] Uppdaterar UI för {currentItem.itemName}", Logger.LogLevel.Info);
            
            if (icon != null)
            {
                icon.sprite = currentItem.icon;
                icon.enabled = true;
            }

            if (quantityText != null)
            {
                if (currentItem.isStackable && quantity > 1)
                {
                    quantityText.text = quantity.ToString();
                    quantityText.enabled = true;
                    Logger.Instance.Log($"[UpdateUI] Visar quantity {quantity} för stackable item", Logger.LogLevel.Info);
                }
                else
                {
                    quantityText.text = "";
                    quantityText.enabled = false;
                    Logger.Instance.Log("[UpdateUI] Döljer quantity för non-stackable item eller quantity = 1", Logger.LogLevel.Info);
                }
            }
        }
        else
        {
            Logger.Instance.Log("[UpdateUI] Rensar UI då inget item finns", Logger.LogLevel.Info);
            if (icon != null)
            {
                icon.sprite = null;
                icon.enabled = true;
            }
            if (quantityText != null)
            {
                quantityText.text = "";
                quantityText.enabled = false;
            }
        }
    }
} 