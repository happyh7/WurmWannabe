using UnityEngine;
using TMPro;

public class TreeInteraction : MonoBehaviour
{
    [SerializeField] private float interactionDistance = 1.5f;
    [SerializeField] private KeyCode interactKey = KeyCode.E;
    [SerializeField] private EquipManager equipManager;
    [SerializeField] private GameObject treeHPBarPrefab;
    [SerializeField] private Canvas worldSpaceCanvas;
    [SerializeField] private GameObject chopCastBarPrefab;
    [SerializeField] private TMP_Text notificationText;
    [SerializeField] private GameObject notificationPanel;
    private float notificationHideTime = 0f;

    private Tree currentTree;
    private Tree lastHighlightedTree;
    private TreeHPBar currentHPBar;
    private float lastChopTime = -10f;
    private float hpBarHideDelay = 2f;
    private float castTimer = 0f;
    private bool isCasting = false;
    private ChopCastBar activeCastBar;
    private Vector3 castBarOffset = new Vector3(0, 1.5f, 0);

    private void Update()
    {
        // Visa/göm notification-panel om det är aktivt
        if (notificationPanel != null && notificationPanel.activeSelf && Time.time > notificationHideTime)
        {
            notificationPanel.SetActive(false);
        }

        // Hitta närmaste träd (görs alltid)
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, interactionDistance);
        float closestDistance = float.MaxValue;
        Tree closestTree = null;
        foreach (Collider2D collider in colliders)
        {
            Tree tree = collider.GetComponent<Tree>();
            if (tree != null)
            {
                float distance = Vector2.Distance(transform.position, collider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTree = tree;
                }
            }
        }

        // Kolla om spelaren har en yxa utrustad
        if (!equipManager.HasAxeEquipped())
        {
            // Om spelaren försöker hugga utan yxa
            if (Input.GetKeyDown(interactKey) && closestTree != null)
            {
                ShowNotification("Du behöver en yxa för att hugga!");
            }
            RemoveHPBarAndHighlight();
            currentTree = null;
            return;
        }

        // Hantera highlight med Quick Outline
        if (closestTree != lastHighlightedTree)
        {
            RemoveHighlight();
            if (closestTree != null)
            {
                var outline = closestTree.GetComponent<Outline>();
                if (outline != null)
                {
                    outline.enabled = true;
                }
            }
            lastHighlightedTree = closestTree;
        }

        currentTree = closestTree;

        // Hantera huggning och HP-bar
        if (currentTree != null && Input.GetKey(interactKey))
        {
            lastChopTime = Time.time;
            if (currentHPBar == null)
            {
                // Skapa HP-bar
                var hpBarGO = Instantiate(treeHPBarPrefab, worldSpaceCanvas.transform);
                currentHPBar = hpBarGO.GetComponent<TreeHPBar>();
                currentHPBar.SetTarget(currentTree.transform);
                currentHPBar.Show();
            }
            // Uppdatera HP-bar
            currentHPBar.SetHP(currentTree.GetCurrentHP(), currentTree.GetMaxHP());
        }
        else if (currentHPBar != null)
        {
            // Dölj HP-bar efter delay om ingen huggning
            if (Time.time - lastChopTime > hpBarHideDelay)
            {
                Destroy(currentHPBar.gameObject);
                currentHPBar = null;
            }
        }

        if (currentTree != null)
        {
            float distance = Vector2.Distance(transform.position, currentTree.transform.position);
            if (distance <= interactionDistance)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    Debug.Log("E tryckt. Försöker hugga. Har yxa: " + (equipManager != null && equipManager.HasAxeEquipped() ? "JA" : "NEJ"));
                    ShowHPBar();
                    isCasting = true;
                    castTimer = 0f;
                    ShowCastBar();
                    Debug.Log("Cast bar SHOWN");
                }
                else if (Input.GetKeyUp(KeyCode.E))
                {
                    isCasting = false;
                    HideCastBar();
                    Debug.Log("Cast bar HIDDEN");
                }

                if (isCasting)
                {
                    castTimer += Time.deltaTime;
                    float progress = Mathf.Clamp01(castTimer / 1f);
                    if (activeCastBar != null)
                    {
                        activeCastBar.SetProgress(progress);
                        activeCastBar.transform.position = transform.position + castBarOffset;
                    }
                    Debug.Log("Casting... Timer: " + castTimer);

                    if (castTimer >= 1f)
                    {
                        Debug.Log("Cast klar! Försöker skada träd.");
                        int prevHP = currentTree.GetCurrentHP();
                        currentTree.TakeDamage(1);
                        EquipManager.Instance.UseAxe();
                        castTimer = 0f;
                        Debug.Log("Tree took damage! HP nu: " + currentTree.GetCurrentHP());
                        // Uppdatera HP-bar direkt efter skada
                        if (currentHPBar != null)
                        {
                            currentHPBar.SetHP(currentTree.GetCurrentHP(), currentTree.GetMaxHP());
                        }
                        // Om trädet dog, städa UI
                        if (currentTree.GetCurrentHP() <= 0)
                        {
                            if (currentHPBar != null)
                            {
                                currentHPBar.SetHP(0, currentTree.GetMaxHP());
                                Destroy(currentHPBar.gameObject);
                                currentHPBar = null;
                            }
                            HideCastBar();
                            currentTree = null;
                        }
                    }
                }
            }
            else
            {
                HideHPBar();
                isCasting = false;
                HideCastBar();
                Debug.Log("För långt bort från träd, avbryter hugg.");
            }
        }
    }

    private void RemoveHighlight()
    {
        if (lastHighlightedTree != null)
        {
            var outline = lastHighlightedTree.GetComponent<Outline>();
            if (outline != null)
            {
                outline.enabled = false;
            }
            lastHighlightedTree = null;
        }
    }

    private void RemoveHPBarAndHighlight()
    {
        if (currentHPBar != null)
        {
            Destroy(currentHPBar.gameObject);
            currentHPBar = null;
        }
        RemoveHighlight();
    }

    private void ShowHPBar()
    {
        if (currentHPBar == null)
        {
            // Skapa HP-bar
            var hpBarGO = Instantiate(treeHPBarPrefab, worldSpaceCanvas.transform);
            currentHPBar = hpBarGO.GetComponent<TreeHPBar>();
            currentHPBar.SetTarget(currentTree.transform);
            currentHPBar.Show();
        }
        // Uppdatera HP-bar
        currentHPBar.SetHP(currentTree.GetCurrentHP(), currentTree.GetMaxHP());
    }

    private void HideHPBar()
    {
        if (currentHPBar != null)
        {
            Destroy(currentHPBar.gameObject);
            currentHPBar = null;
        }
    }

    private void ShowCastBar()
    {
        if (activeCastBar == null && chopCastBarPrefab != null)
        {
            GameObject barObj = Instantiate(chopCastBarPrefab, worldSpaceCanvas.transform);
            activeCastBar = barObj.GetComponent<ChopCastBar>();
            activeCastBar.transform.position = transform.position + castBarOffset;
            activeCastBar.SetProgress(0f);
            activeCastBar.SetVisible(true);
        }
        else if (activeCastBar != null)
        {
            activeCastBar.SetVisible(true);
            activeCastBar.SetProgress(0f);
        }
    }

    private void HideCastBar()
    {
        if (activeCastBar != null)
        {
            Destroy(activeCastBar.gameObject);
            activeCastBar = null;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<Tree>() != null)
        {
            HideHPBar();
            isCasting = false;
            HideCastBar();
            currentTree = null;
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Rita en cirkel som visar interaktionsavståndet
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionDistance);
    }

    private void ShowNotification(string message, float duration = 2f)
    {
        if (notificationText != null && notificationPanel != null)
        {
            notificationText.text = message;
            notificationText.enabled = true;
            notificationPanel.SetActive(true);
            notificationHideTime = Time.time + duration;
        }
    }
} 