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
        if (item != null)
        {
            currentItem = item;
            if (item.isStackable)
            {
                if (quantityText != null)
                {
                    quantityText.text = InventoryManager.Instance.GetItemQuantity(item).ToString();
                }
            }
            else
            {
                if (quantityText != null)
                {
                    quantityText.text = "";
                }
            }

            // Aktivera durability bar för yxor
            if (item.itemName.Contains("Axe") && durabilityBar != null)
            {
                durabilityBar.gameObject.SetActive(true);
                durabilityBar.SetDurability(EquipManager.Instance.GetAxeDurabilityFor(item), EquipManager.Instance.GetAxeMaxDurability());
            }
            else if (durabilityBar != null)
            {
                durabilityBar.gameObject.SetActive(false);
            }

            UpdateUI();
        }
        else
        {
            if (quantityText != null)
            {
                quantityText.text = "";
            }
            if (durabilityBar != null)
            {
                durabilityBar.gameObject.SetActive(false);
            }
            UpdateUI();
        }
    }

    public virtual void ClearSlot()
    {
        if (currentItem == null) return; // Om sloten redan är tom, hoppa över

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
        if (durabilityBar != null)
        {
            durabilityBar.gameObject.SetActive(false);
        }
        UpdateUI();
    }

    public virtual ItemData GetItem()
    {
        return currentItem;
    }

    // Drag and drop funktionalitet
    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        if (currentItem == null) return;

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
        if (eventData.pointerDrag == null) return;

        InventorySlot fromSlot = eventData.pointerDrag.GetComponent<InventorySlot>();
        if (fromSlot != null)
        {
            // Om vi droppar från AxeSlot
            if (fromSlot is AxeSlot)
            {
                ItemData savedAxe = AxeSlot.DraggedAxeItem;
                if (savedAxe == null)
                {
                    return;
                }

                // Markera denna slot som target för unequip
                this.isLastUnequipTarget = true;

                // Ta bort yxan från handen/AxeSlot (lägger automatiskt till i inventory och placerar i rätt slot)
                EquipManager.Instance.UnequipAxe();

                // Returnera direkt – låt UpdateUI hantera placeringen!
                return;
            }

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
                }
            }
            // Om vi droppar på en slot med samma item och item är stackable
            else if (fromSlot.GetItem() != null && currentItem.itemName == fromSlot.GetItem().itemName && currentItem.isStackable)
            {
                // Lägg till quantity från fromSlot till denna slot
                quantity += fromSlot.quantity;
                fromSlot.ClearSlot();
                UpdateUI();
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
            if (icon != null)
            {
                icon.sprite = currentItem.icon;
                icon.enabled = true;
            }
            if (quantityText != null)
            {
                if (currentItem.isStackable)
                {
                    quantityText.text = InventoryManager.Instance.GetItemQuantity(currentItem).ToString();
                }
                else
                {
                    quantityText.text = "";
                }
            }

            // Uppdatera durability bar för yxor
            if (currentItem.itemName.Contains("Axe") && durabilityBar != null)
            {
                durabilityBar.gameObject.SetActive(true);
                durabilityBar.SetDurability(EquipManager.Instance.GetAxeDurabilityFor(currentItem), EquipManager.Instance.GetAxeMaxDurability());
            }
            else if (durabilityBar != null)
            {
                durabilityBar.gameObject.SetActive(false);
            }
        }
        else
        {
            if (icon != null)
            {
                icon.sprite = null;
                icon.enabled = false;
            }
            if (quantityText != null)
            {
                quantityText.text = "";
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