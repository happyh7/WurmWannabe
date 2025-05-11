using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

// TEST: Cursor kan nu ändra denna fil?
public class InventorySlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler, IDropHandler
{
    public Image icon;  // Reference till Image-komponenten för item icon
    public TMP_Text quantityText;  // Text för att visa antal items och deras dubbelhakor.
    // Ram/Frame hanteras av separat Image-objekt i prefab
    public bool isLastUnequipTarget = false; // Ny flagga för att markera unequip target
    [SerializeField] protected AxeDurabilityBar durabilityBar;

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
        Logger.Instance.Log($"[InventorySlot.SetItem] Försöker sätta item: {(item != null ? item.itemName : "null")} i slot: {this.name}", Logger.LogLevel.Info);
        if (item == null)
        {
            Logger.Instance.Log("[SetItem] Försöker sätta null item i slot", Logger.LogLevel.Error);
            return;
        }

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
        Logger.Instance.Log($"[InventorySlot.ClearSlot] Försöker rensa slot: {this.name}, innehöll: {(currentItem != null ? currentItem.itemName : "null")}", Logger.LogLevel.Info);
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
        UpdateUI(); // Se till att durability-baren döljs när sloten töms
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
        Logger.Instance.Log($"[InventorySlot.OnDrop] eventData.pointerDrag: {(eventData.pointerDrag != null ? eventData.pointerDrag.name : "null")}, this: {this.name}, currentItem: {(currentItem != null ? currentItem.itemName : "null")}", Logger.LogLevel.Info);
        if (eventData.pointerDrag == null) return;

        InventorySlot fromSlot = eventData.pointerDrag.GetComponent<InventorySlot>();
        if (fromSlot != null)
        {
            // Om vi droppar från AxeSlot
            if (fromSlot is AxeSlot)
            {
                Logger.Instance.Log($"[InventorySlot.OnDrop] Droppar från AxeSlot: {fromSlot.name}, item: {(AxeSlot.DraggedAxeItem != null ? AxeSlot.DraggedAxeItem.itemName : "null")}", Logger.LogLevel.Info);

                ItemData savedAxe = AxeSlot.DraggedAxeItem;
                if (savedAxe == null)
                {
                    Logger.Instance.Log("[OnDrop] Ingen yxa att flytta från AxeSlot (DraggedAxeItem är null)", Logger.LogLevel.Warning);
                    return;
                }

                // Markera denna slot som target för unequip
                this.isLastUnequipTarget = true;

                // Ta bort yxan från handen/AxeSlot (lägger automatiskt till i inventory och placerar i rätt slot)
                EquipManager.Instance.UnequipAxe();

                // Returnera direkt – låt UpdateUI hantera placeringen!
                return;
            }

            Logger.Instance.Log($"[InventorySlot.OnDrop] Droppar från InventorySlot: {fromSlot.name}, item: {(fromSlot.GetItem() != null ? fromSlot.GetItem().itemName : "null")}", Logger.LogLevel.Info);
            // Om vi droppar på samma slot, gör ingenting
            if (fromSlot == this) return;

            // Om vi droppar på en tom slot
            if (currentItem == null)
            {
                // Flytta item från fromSlot till denna slot
                ItemData itemToMove = fromSlot.GetItem();
                if (itemToMove != null)
                {
                    SetItem(itemToMove);
                    fromSlot.ClearSlot();
                    Logger.Instance.Log($"[OnDrop] Flyttade {currentItem.itemName} från slot {fromSlot.name} till {name}", Logger.LogLevel.Info);
                }
            }
            // Om vi droppar på en slot med samma item och item är stackable
            else if (fromSlot.GetItem() != null && currentItem.itemName == fromSlot.GetItem().itemName && currentItem.isStackable)
            {
                // Lägg till quantity från fromSlot till denna slot
                quantity += fromSlot.quantity;
                fromSlot.ClearSlot();
                UpdateUI();
                Logger.Instance.Log($"[OnDrop] Stackade {currentItem.itemName} från slot {fromSlot.name} till {name}", Logger.LogLevel.Info);
            }
            // Om vi droppar på en slot med ett annat item
            else if (fromSlot.GetItem() != null)
            {
                // Byta plats på items
                ItemData tempItem = currentItem;
                ItemData fromItem = fromSlot.GetItem();
                SetItem(fromItem);
                fromSlot.SetItem(tempItem);

                // Tvinga båda slots att uppdatera sitt UI (extra säkerhet)
                UpdateUI();
                fromSlot.UpdateUI();

                Logger.Instance.Log($"[OnDrop] Bytte plats på {currentItem.itemName} och {tempItem.itemName}", Logger.LogLevel.Info);
            }
            return;
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

    public virtual void UpdateUI()
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

            // Durability bar
            if (currentItem.itemName.Contains("Axe") && durabilityBar != null)
            {
                durabilityBar.gameObject.SetActive(true);
                int durability = EquipManager.Instance.GetAxeDurabilityFor(currentItem);
                durabilityBar.SetDurability(durability, EquipManager.Instance.GetAxeMaxDurability());
            }
            else if (durabilityBar != null)
            {
                durabilityBar.gameObject.SetActive(false);
            }
        }
        else
        {
            Logger.Instance.Log("[UpdateUI] Rensar UI då inget item finns", Logger.LogLevel.Info);
            if (icon != null)
            {
                icon.sprite = null;
                icon.enabled = false;
            }
            if (quantityText != null)
            {
                quantityText.text = "";
                quantityText.enabled = false;
            }
            if (durabilityBar != null)
            {
                durabilityBar.gameObject.SetActive(false);
            }
        }
    }

    public void UpdateDurabilityBar()
    {
        if (durabilityBar != null && currentItem != null && currentItem.itemName.Contains("Axe"))
        {
            durabilityBar.SetDurability(EquipManager.Instance.GetAxeDurability(), EquipManager.Instance.GetAxeMaxDurability());
        }
    }
} 