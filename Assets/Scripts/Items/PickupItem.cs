using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public ItemData itemData; // Nytt: referens till ScriptableObject
    private bool playerInRange = false;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            // Lägg till i inventory
            if (InventoryManager.Instance != null && itemData != null)
            {
                InventoryManager.Instance.AddItem(itemData);
            }
            else
            {
                Debug.LogWarning("InventoryManager eller itemData saknas!");
            }
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            // Här kan du visa en pickup-prompt i UI
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            // Dölj pickup-prompt i UI
        }
    }
} 