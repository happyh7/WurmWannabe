using UnityEngine;

public class TreeInteraction : MonoBehaviour
{
    [SerializeField] private float interactionDistance = 2f;
    [SerializeField] private KeyCode interactKey = KeyCode.E;
    [SerializeField] private EquipManager equipManager;
    [SerializeField] private GameObject treeHPBarPrefab;
    [SerializeField] private Canvas worldSpaceCanvas;
    [SerializeField] private GameObject castBarPrefab;
    [SerializeField] private float castTime = 2f; // Tid det tar att hugga ett träd
    [SerializeField] private Vector3 castBarOffset = new Vector3(0, 0.6f, 0);
    [SerializeField] private SkillActionRunner skillActionRunner;

    private GameTree currentTree;
    private GameTree lastHighlightedTree;
    private TreeHPBar currentHPBar;
    private float lastChopTime = -10f;
    private float hpBarAutoHideTime = 0f;
    private float hpBarAutoHideDelay = 2.5f; // sekunder
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
                }
            }
        }

        // Auto-hide HPBar om spelaren inte hugger
        if (currentHPBar != null && (currentTree == null || Time.time > hpBarAutoHideTime))
        {
            Destroy(currentHPBar.gameObject);
            currentHPBar = null;
        }

        bool holdingE = Input.GetKey(interactKey);
        bool justPressedE = Input.GetKeyDown(interactKey);

        // Hantera interaktion med träd och starta cast eller visa varning
        if (closestTree != null && holdingE && !PlayerSkills.Instance.IsActionInProgress)
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
        if (PlayerSkills.Instance.IsActionInProgress && !holdingE)
        {
            CancelChop();
        }
    }

    private void StartChop(GameTree tree)
    {
        if (tree == null || tree.GetCurrentHP() <= 0)
        {
            CancelChop();
            if (currentHPBar != null)
            {
                Destroy(currentHPBar.gameObject);
                currentHPBar = null;
            }
            return;
        }
        currentTree = tree;
        PlayerSkills.Instance.IsActionInProgress = true;
        // Skapa HP bar för det nya trädet
        if (currentHPBar != null)
        {
            Destroy(currentHPBar.gameObject);
            currentHPBar = null;
        }
        if (treeHPBarPrefab != null && worldSpaceCanvas != null)
        {
            GameObject hpBarObj = Instantiate(treeHPBarPrefab, tree.transform.position + new Vector3(0, 2f, 0), Quaternion.identity, worldSpaceCanvas.transform);
            currentHPBar = hpBarObj.GetComponent<TreeHPBar>();
            if (currentHPBar != null)
                currentHPBar.Initialize(tree);
        }
        hpBarAutoHideTime = Time.time + hpBarAutoHideDelay;
        // Starta action via SkillActionRunner
        if (skillActionRunner != null)
        {
            var request = new SkillActionRequest(
                baseCastTime: castTime, // Använd Inspector-värdet
                baseSkillTick: 1f,
                skillType: SkillType.Woodcutting,
                staminaCost: 10f,
                onStart: null,
                onComplete: () =>
                {
                    if (currentTree != null)
                    {
                        currentTree.TakeDamage(1f);
                        if (equipManager != null)
                        {
                            equipManager.UseAxe();
                            if (equipManager.IsAxeBroken())
                            {
                                ShowNotification("Din yxa gick sönder!");
                                CancelChop();
                                return;
                            }
                        }
                        if (currentHPBar != null)
                        {
                            currentHPBar.SetHP(currentTree.GetCurrentHP(), currentTree.GetMaxHP());
                            hpBarAutoHideTime = Time.time + hpBarAutoHideDelay;
                            if (currentTree.GetCurrentHP() <= 0)
                            {
                                Destroy(currentHPBar.gameObject);
                                currentHPBar = null;
                                currentTree = null;
                            }
                        }
                    }
                    lastChopTime = Time.time;
                    // Starta ny cast om E hålls och trädet lever
                    if (currentTree != null && currentTree.GetCurrentHP() > 0 && Input.GetKey(interactKey))
                    {
                        StartChop(currentTree);
                    }
                },
                onCancel: () =>
                {
                    PlayerSkills.Instance.IsActionInProgress = false;
                }
            );
            skillActionRunner.StartAction(request);
        }
    }

    private void CancelChop()
    {
        PlayerSkills.Instance.IsActionInProgress = false;
        if (skillActionRunner != null)
        {
            skillActionRunner.CancelAction();
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