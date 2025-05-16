using UnityEngine;
using TMPro;

public class TreeInteraction : MonoBehaviour
{
    [SerializeField] private float interactionDistance = 2f;
    [SerializeField] private KeyCode interactKey = KeyCode.E;
    [SerializeField] private EquipManager equipManager;
    [SerializeField] private GameObject treeHPBarPrefab;
    [SerializeField] private Canvas worldSpaceCanvas;
    [SerializeField] private GameObject chopCastBarPrefab;
    [SerializeField] private float castTime = 2f; // Tid det tar att hugga ett träd
    [SerializeField] private Vector3 castBarOffset = new Vector3(0, 0.6f, 0);

    private GameTree currentTree;
    private GameTree lastHighlightedTree;
    private TreeHPBar currentHPBar;
    private float lastChopTime = -10f;
    private float castTimer = 0f;
    private bool isCasting = false;
    private ChopCastBar activeCastBar;
    private float hpBarAutoHideTime = 0f;
    private float hpBarAutoHideDelay = 2.5f; // sekunder
    private bool wasHoldingE = false;
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        // Hitta närmaste träd (görs alltid)
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, interactionDistance);
        float closestDistance = float.MaxValue;
        GameTree closestTree = null;
        foreach (Collider2D collider in colliders)
        {
            GameTree tree = collider.GetComponent<GameTree>();
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

        // Hantera highlight av träd
        if (closestTree != lastHighlightedTree)
        {
            if (lastHighlightedTree != null)
                lastHighlightedTree.SetHighlight(false);
            if (closestTree != null)
                closestTree.SetHighlight(true);
            lastHighlightedTree = closestTree;
        }

        // Ta bort HPBar och nollställ currentTree direkt när trädet dör
        if (currentTree != null && currentTree.GetCurrentHP() <= 0)
        {
            if (currentHPBar != null)
            {
                Destroy(currentHPBar.gameObject);
                currentHPBar = null;
            }
            currentTree = null;
            CancelChop();

            // Starta direkt ny cast om E hålls och det finns ett annat träd nära
            if (Input.GetKey(interactKey))
            {
                GameTree nextTree = FindClosestAliveTree();
                if (nextTree != null)
                {
                    StartChop(nextTree);
                }
                else
                {
                    // Ingen ny target, dölj castbar och avbryt
                    CancelChop();
                    if (activeCastBar != null)
                    {
                        Destroy(activeCastBar.gameObject);
                        activeCastBar = null;
                    }
                }
            }
        }

        // Auto-hide HPBar om spelaren inte hugger
        if (currentHPBar != null && (currentTree == null || Time.time > hpBarAutoHideTime) && !isCasting)
        {
            Destroy(currentHPBar.gameObject);
            currentHPBar = null;
        }

        bool holdingE = Input.GetKey(interactKey);
        bool justPressedE = Input.GetKeyDown(interactKey);

        // Hantera interaktion med träd och starta cast eller visa varning
        if (closestTree != null && holdingE && !isCasting)
        {
            if (equipManager != null && equipManager.HasAxeEquipped())
            {
                StartChop(closestTree);
            }
            else if (equipManager != null && equipManager.IsAxeBroken() && equipManager.IsAxeEquipped())
            {
                if (justPressedE)
                {
                    ShowNotification("Yxan är i för dåligt skick för att fälla träd!");
                }
            }
            else if (justPressedE && equipManager != null && !equipManager.IsAxeEquipped())
            {
                ShowNotification("Du behöver en yxa för att hugga träd!");
            }
        }

        // Avbryt cast om E släpps
        if (isCasting && !holdingE)
        {
            CancelChop();
        }

        // Hantera pågående huggningsanimation
        if (isCasting)
        {
            castTimer += Time.deltaTime;
            if (activeCastBar != null)
                activeCastBar.UpdateProgress(castTimer / castTime);

            if (castTimer >= castTime)
            {
                CompleteChop();
            }
        }

        // Spara state för nästa frame
        wasHoldingE = holdingE;
    }

    private void StartChop(GameTree tree)
    {
        if (tree == null || tree.GetCurrentHP() <= 0)
        {
            CancelChop();
            if (activeCastBar != null)
            {
                Destroy(activeCastBar.gameObject);
                activeCastBar = null;
            }
            if (currentHPBar != null)
            {
                Destroy(currentHPBar.gameObject);
                currentHPBar = null;
            }
            return;
        }
        Debug.Log("Starting chop on tree");
        currentTree = tree;
        isCasting = true;
        castTimer = 0f;
        if (PlayerSkills.Instance != null) PlayerSkills.Instance.IsActionInProgress = true;

        // Skapa cast bar ovanför spelaren
        if (chopCastBarPrefab != null && worldSpaceCanvas != null)
        {
            GameObject castBarObj = Instantiate(chopCastBarPrefab, transform.position + castBarOffset, Quaternion.identity, worldSpaceCanvas.transform);
            activeCastBar = castBarObj.GetComponent<ChopCastBar>();
            if (activeCastBar != null)
                activeCastBar.Initialize("Hugger...");
        }

        // Ta bort eventuell gammal HPBar
        if (currentHPBar != null)
        {
            Destroy(currentHPBar.gameObject);
            currentHPBar = null;
        }

        // Skapa ny HP bar för det nya trädet
        if (treeHPBarPrefab != null && worldSpaceCanvas != null)
        {
            GameObject hpBarObj = Instantiate(treeHPBarPrefab, tree.transform.position + new Vector3(0, 2f, 0), Quaternion.identity, worldSpaceCanvas.transform);
            currentHPBar = hpBarObj.GetComponent<TreeHPBar>();
            if (currentHPBar != null)
                currentHPBar.Initialize(tree);
        }
        // Sätt auto-hide timer
        hpBarAutoHideTime = Time.time + hpBarAutoHideDelay;
    }

    private void CancelChop()
    {
        isCasting = false;
        castTimer = 0f;
        if (PlayerSkills.Instance != null) PlayerSkills.Instance.IsActionInProgress = false;
        if (activeCastBar != null)
        {
            Destroy(activeCastBar.gameObject);
            activeCastBar = null;
        }
    }

    private void CompleteChop()
    {
        Debug.Log("Completing chop");
        isCasting = false;
        castTimer = 0f;
        if (PlayerSkills.Instance != null) PlayerSkills.Instance.IsActionInProgress = false;

        if (activeCastBar != null)
        {
            Destroy(activeCastBar.gameObject);
            activeCastBar = null;
        }

        // Stamina-kostnad
        if (PlayerSkills.Instance != null)
        {
            float staminaCost = 10f;
            PlayerSkills.Instance.UseStamina(staminaCost);
        }

        if (currentTree != null)
        {
            Debug.Log("Dealing damage to tree");
            currentTree.TakeDamage(1f);

            // Minska yxans hållbarhet
            if (equipManager != null)
            {
                equipManager.UseAxe();
                // Om yxan är trasig efter användning, visa notis och avbryt cast
                if (equipManager.IsAxeBroken())
                {
                    ShowNotification("Din yxa gick sönder!");
                    CancelChop();
                    return;
                }
            }

            // Uppdatera HP-bar
            if (currentHPBar != null)
            {
                currentHPBar.SetHP(currentTree.GetCurrentHP(), currentTree.GetMaxHP());
                // Sätt auto-hide timer
                hpBarAutoHideTime = Time.time + hpBarAutoHideDelay;
                // Om trädet dog, ta bort HP-baren och nollställ currentTree
                if (currentTree.GetCurrentHP() <= 0)
                {
                    Destroy(currentHPBar.gameObject);
                    currentHPBar = null;
                    currentTree = null;
                }
            }

            // Lägg till skill
            if (PlayerSkills.Instance != null)
            {
                Debug.Log("Adding woodcutting skill");
                PlayerSkills.Instance.GainSkill(SkillType.Woodcutting);
            }
            else
            {
                Debug.Log("PlayerSkills instance not found!");
            }
        }

        lastChopTime = Time.time;

        // Starta bara ny cast om det finns ett träd kvar och HP > 0
        if (currentTree != null && currentTree.GetCurrentHP() > 0 && Input.GetKey(interactKey))
        {
            StartChop(currentTree);
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
        Debug.Log($"Showing notification: {message}");
        NotificationManager.Instance?.ShowNotification(message);
    }

    private GameTree FindClosestAliveTree()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, interactionDistance);
        float closestDistance = float.MaxValue;
        GameTree closestAliveTree = null;
        foreach (Collider2D collider in colliders)
        {
            GameTree tree = collider.GetComponent<GameTree>();
            if (tree != null && tree.GetCurrentHP() > 0)
            {
                float distance = Vector2.Distance(transform.position, collider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestAliveTree = tree;
                }
            }
        }
        return closestAliveTree;
    }
} 