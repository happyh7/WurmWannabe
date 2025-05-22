using UnityEngine;
using UnityEngine.UI;

public class SkillCategoryPanelUI : MonoBehaviour
{
    public GameObject content; // Dra in Content-objektet här i Inspector
    public Button expandButton; // Dra in ExpandButton här
    public Image expandIcon; // (valfritt) Dra in Image-komponenten på knappen om du vill byta ikon
    public Sprite expandedSprite; // (valfritt) ikon för expanderad
    public Sprite collapsedSprite; // (valfritt) ikon för kollapsad

    private bool expanded = false;
    private LayoutElement layoutElement;

    private void Awake()
    {
        layoutElement = GetComponent<LayoutElement>();
        if (layoutElement == null)
            layoutElement = gameObject.AddComponent<LayoutElement>();
    }

    private void Start()
    {
        if (expandButton != null)
            expandButton.onClick.AddListener(ToggleExpand);
        UpdateUI();
    }

    public void ToggleExpand()
    {
        expanded = !expanded;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (content != null)
            content.SetActive(expanded);

        // Räkna ut höjd
        float headingHeight = 30f; // Rubrikradens höjd
        float skillLineHeight = 30f; // SkillLine: 20 + 5 spacing över och under
        float spacing = 5f; // Samma som i Vertical Layout Group på Content
        int skillCount = expanded ? content.transform.childCount : 0;
        float totalHeight = headingHeight + (skillCount > 0 ? (skillCount * skillLineHeight) + ((skillCount - 1) * spacing) : 0);

        layoutElement.preferredHeight = totalHeight;

        // Tvinga layout update på parent
        LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
        var parent = transform.parent;
        if (parent != null)
            LayoutRebuilder.ForceRebuildLayoutImmediate(parent as RectTransform);

        Debug.Log($"{gameObject.name} UpdateUI: Content active: {content.activeSelf}, Content childCount: {content.transform.childCount}, Content LayoutGroup: {content.GetComponent<LayoutGroup>() != null}, Content anchoring: {content.GetComponent<RectTransform>().anchorMin} - {content.GetComponent<RectTransform>().anchorMax}, Content sizeDelta: {content.GetComponent<RectTransform>().sizeDelta}, Content preferredHeight: {content.GetComponent<LayoutElement>()?.preferredHeight}");

        // DEBUG: Logga storlek och antal skills
        Debug.Log($"{gameObject.name} expanded: {expanded}, height: {totalHeight}, Content children: {content.transform.childCount}");

        if (expandIcon != null)
        {
            if (expanded && expandedSprite != null)
                expandIcon.sprite = expandedSprite;
            else if (!expanded && collapsedSprite != null)
                expandIcon.sprite = collapsedSprite;
        }
    }
} 