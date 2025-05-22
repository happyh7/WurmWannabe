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
    public Sprite placeholderIcon;
    public string tooltipText;
    private bool pointerOver = false;
    private float hoverTime = 0f;
    private const float tooltipDelay = 0.5f;
    private bool tooltipVisible = false;

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
                // Top center av raden
                Vector3 topCenter = (corners[1] + corners[2]) / 2f;
                Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(null, topCenter);

                // Deklarera tooltipPos här så den alltid finns
                Vector2 tooltipPos = screenPos + new Vector2(0, 30); // 30 pixlar ovanför

                SkillsUIManager skillsUIManager = GetComponentInParent<SkillsUIManager>();
                RectTransform skillsPanelRT = null;
                if (skillsUIManager != null && skillsUIManager.skillsPanel != null)
                {
                    skillsPanelRT = skillsUIManager.skillsPanel.GetComponent<RectTransform>();
                }
                if (skillsPanelRT != null)
                {
                    Vector3[] panelCorners = new Vector3[4];
                    skillsPanelRT.GetWorldCorners(panelCorners);
                    float panelLeft = panelCorners[0].x;
                    float panelRight = panelCorners[2].x;
                    float panelTop = panelCorners[1].y;
                    float panelBottom = panelCorners[0].y;

                    RectTransform tooltipRT = SkillTooltip.instance.tooltipPanel.GetComponent<RectTransform>();
                    float tooltipHeight = tooltipRT.rect.height;
                    float tooltipWidth = tooltipRT.rect.width;

                    float padding = 5f;
                    float verticalOffset = 105f; // Återställ till värde som gav bra placering ovanför raden
                    // Använd screenPos.y istället för rowTopY
                    Vector2 tooltipScreenPos = new Vector2(screenPos.x - tooltipWidth / 2, screenPos.y + verticalOffset);

                    float underOffset = 5f; // Exakt samma luft under raden som ovanför
                    // Om tooltipen sticker utanför panelens topp, placera under raden istället
                    if (tooltipScreenPos.y > panelTop - padding)
                        tooltipScreenPos.y = screenPos.y - tooltipHeight - underOffset;

                    // Clamp X så tooltipen alltid är helt innanför panelen
                    float minX = panelLeft + padding;
                    float maxX = panelRight - tooltipWidth - padding;
                    tooltipScreenPos.x = Mathf.Clamp(tooltipScreenPos.x, minX, maxX);

                    // Debug-logg för verticalOffset och Y-position
                    Debug.Log($"verticalOffset: {verticalOffset}, tooltipScreenPos.y: {tooltipScreenPos.y}, rowTopY: {screenPos.y}");

                    // Konvertera till local position i SkillsPanel
                    Vector2 localPoint;
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(
                        skillsPanelRT, tooltipScreenPos, null, out localPoint);

                    // Sätt localPosition på tooltip-panelen
                    tooltipRT.localPosition = localPoint;

                    // Visa tooltip
                    SkillTooltip.Show(tooltipText, tooltipRT.position);
                    tooltipVisible = true;
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
        skillName.text = SkillData.GetDisplayName(data.skillType);
        progressBar.value = data.value / 100f;
        valueText.text = $"{data.value:F1}/100";
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