using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class SkillLineUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image icon;
    public TextMeshProUGUI skillName;
    public Slider progressBar;
    public TextMeshProUGUI valueText;
    public TextMeshProUGUI valueTextInBar;
    public Sprite placeholderIcon;
    public string tooltipText;
    private bool pointerOver = false;
    private float hoverTime = 0f;
    private const float tooltipDelay = 0.5f;
    private bool tooltipVisible = false;
    public SkillType skillType;
    private const float maxTooltipWidth = 300f; // Maxbredd på tooltipen i px

    void OnEnable()
    {
        PlayerSkills.OnSkillValueChanged += OnSkillValueChanged;
    }
    void OnDisable()
    {
        PlayerSkills.OnSkillValueChanged -= OnSkillValueChanged;
    }
    void OnDestroy()
    {
        PlayerSkills.OnSkillValueChanged -= OnSkillValueChanged;
    }
    private void OnSkillValueChanged(SkillType changedType, float newValue)
    {
        if (changedType == skillType)
        {
            // Progress till nästa heltal
            float currentLevel = Mathf.Floor(newValue);
            float progressToNext = newValue - currentLevel;
            progressBar.value = progressToNext; // Slider min=0, max=1

            if (valueTextInBar != null)
                valueTextInBar.text = $"{newValue:F8}";
            if (valueText != null)
                valueText.text = $"{newValue:F1}/100";
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        pointerOver = true;
        hoverTime = 0f;
        tooltipVisible = false;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        pointerOver = false;
        hoverTime = 0f;
        tooltipVisible = false;
        SkillTooltip.Hide();
    }

    void Update()
    {
        if (pointerOver)
        {
            hoverTime += Time.unscaledDeltaTime;
            if (hoverTime >= tooltipDelay && !tooltipVisible)
            {
                if (SkillTooltip.instance == null || SkillTooltip.instance.tooltipPanel == null)
                    return;
                RectTransform rt = GetComponent<RectTransform>();
                Vector3[] corners = new Vector3[4];
                rt.GetWorldCorners(corners);
                // Top center och bottom center av raden
                Vector3 topCenter = (corners[1] + corners[2]) / 2f;
                Vector3 bottomCenter = (corners[0] + corners[3]) / 2f;

                // Hitta SkillsPanel och dess RectTransform
                SkillsUIManager skillsUIManager = GetComponentInParent<SkillsUIManager>();
                RectTransform skillsPanelRT = null;
                Canvas canvas = null;
                if (skillsUIManager != null && skillsUIManager.skillsPanel != null)
                {
                    skillsPanelRT = skillsUIManager.skillsPanel.GetComponent<RectTransform>();
                    canvas = skillsUIManager.skillsPanel.GetComponentInParent<Canvas>();
                }
                if (skillsPanelRT != null && canvas != null)
                {
                    RectTransform tooltipRT = SkillTooltip.instance.tooltipPanel.GetComponent<RectTransform>();
                    float tooltipWidth = 100f; // Fast bredd
                    float tooltipHeight = 40f; // Fast höjd
                    float padding = 5f;

                    // Sätt storlek på tooltip-panelen
                    tooltipRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, tooltipWidth);
                    tooltipRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, tooltipHeight);

                    // Konvertera world -> screen -> local i SkillsPanel
                    Vector2 screenPosTop = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, topCenter);
                    Vector2 screenPosBottom = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, bottomCenter);
                    Vector2 localPointTop;
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(skillsPanelRT, screenPosTop, canvas.worldCamera, out localPointTop);
                    Vector2 localPointBottom;
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(skillsPanelRT, screenPosBottom, canvas.worldCamera, out localPointBottom);

                    // Panelens storlek och pivot
                    float panelWidth = skillsPanelRT.rect.width;
                    float panelHeight = skillsPanelRT.rect.height;
                    float panelPivotX = skillsPanelRT.pivot.x;

                    // Försök placera ovanför
                    float tooltipX = padding - panelWidth * panelPivotX;
                    Vector2 tooltipLocalPos = new Vector2(tooltipX, localPointTop.y + padding);
                    bool fitsAbove = (tooltipLocalPos.y + tooltipHeight <= panelHeight / 2f);
                    if (!fitsAbove)
                    {
                        // Placera under
                        tooltipLocalPos = new Vector2(tooltipX, localPointBottom.y - tooltipHeight - padding);
                        if (tooltipLocalPos.y < -panelHeight / 2f + padding)
                            tooltipLocalPos.y = -panelHeight / 2f + padding;
                    }

                    // Sätt localPosition på tooltip-panelen
                    tooltipRT.SetParent(skillsPanelRT, false); // Se till att den är child till SkillsPanel
                    tooltipRT.localPosition = tooltipLocalPos;
                    // Visa tooltip
                    SkillTooltip.Show(tooltipText, tooltipRT.position);
                    tooltipVisible = true;
                }
            }
            // --- NYTT: Uppdatera maxbredd varje frame när tooltipen är synlig ---
            if (tooltipVisible)
            {
                SkillsUIManager skillsUIManager = GetComponentInParent<SkillsUIManager>();
                RectTransform skillsPanelRT = null;
                if (skillsUIManager != null && skillsUIManager.skillsPanel != null)
                {
                    skillsPanelRT = skillsUIManager.skillsPanel.GetComponent<RectTransform>();
                }
                if (skillsPanelRT != null && SkillTooltip.instance != null && SkillTooltip.instance.tooltipPanel != null)
                {
                    float panelWidth = skillsPanelRT.rect.width;
                    float padding = 5f;
                    RectTransform tooltipRT = SkillTooltip.instance.tooltipPanel.GetComponent<RectTransform>();
                    var layoutElement = tooltipRT.GetComponent<UnityEngine.UI.LayoutElement>();
                    if (layoutElement != null)
                        layoutElement.preferredWidth = Mathf.Min(panelWidth - 2 * padding, maxTooltipWidth);
                }
            }
        }
        else
        {
            tooltipVisible = false;
        }
    }

    public void SetSkill(SkillData data)
    {
        skillType = data.skillType;
        skillName.text = SkillData.GetDisplayName(data.skillType);

        // Progress till nästa heltal
        float currentLevel = Mathf.Floor(data.value);
        float progressToNext = data.value - currentLevel;
        progressBar.value = progressToNext; // Slider min=0, max=1

        valueText.text = $"{data.value:F1}/100";
        if (valueTextInBar != null)
            valueTextInBar.text = $"{data.value:F8}";
        // Sätt rätt ikon
        if (icon != null)
        {
            var skillIcon = SkillData.GetIcon(data.skillType);
            if (skillIcon != null)
                icon.sprite = skillIcon;
            else if (placeholderIcon != null)
                icon.sprite = placeholderIcon;
        }
        tooltipText = SkillData.GetDescription(data.skillType);
    }
} 