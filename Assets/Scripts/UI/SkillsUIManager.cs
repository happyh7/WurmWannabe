using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillsUIManager : MonoBehaviour
{
    [Header("Prefabs och föräldrar")]
    public GameObject skillCategoryPanelPrefab;
    public GameObject skillLinePrefab;
    public Transform skillsUIParent;
    [Header("Panels")]
    public GameObject skillsPanel;
    private bool isSkillsOpen = false;

    private Dictionary<SkillCategory, GameObject> categoryPanels = new Dictionary<SkillCategory, GameObject>();
    private List<GameObject> skillLines = new List<GameObject>();

    void Start()
    {
        BuildSkillsUI();
        if (skillsPanel != null)
            skillsPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            ToggleSkillsPanel();
        }
    }

    public void BuildSkillsUI()
    {
        // Rensa ALLA barn till skillsUIParent (Content)
        foreach (Transform child in skillsUIParent)
        {
            Destroy(child.gameObject);
        }
        categoryPanels.Clear();
        skillLines.Clear();

        // Hämta alla skills
        var allSkills = PlayerSkills.Instance.skills;
        // Gruppera efter kategori
        var grouped = new Dictionary<SkillCategory, List<SkillData>>();
        foreach (var skill in allSkills)
        {
            if (!grouped.ContainsKey(skill.category))
                grouped[skill.category] = new List<SkillData>();
            grouped[skill.category].Add(skill);
        }
        // Skapa UI-paneler per kategori
        foreach (var kvp in grouped)
        {
            var catPanel = Instantiate(skillCategoryPanelPrefab, skillsUIParent);
            catPanel.name = kvp.Key + "Panel";
            // Sätt rubrik om panelen har t.ex. en Text eller TMP_Text
            var header = catPanel.GetComponentInChildren<Text>();
            if (header != null) header.text = kvp.Key.ToString();
            var headerTMP = catPanel.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            if (headerTMP != null) headerTMP.text = kvp.Key.ToString();
            // Hitta Content-child (för underskills)
            Transform contentParent = catPanel.transform.Find("Content");
            if (contentParent == null)
                contentParent = catPanel.transform;
            // Sortera skills i bokstavsordning efter display name
            var sortedSkills = kvp.Value;
            sortedSkills.Sort((a, b) => SkillData.GetDisplayName(a.skillType).CompareTo(SkillData.GetDisplayName(b.skillType)));
            // Skapa en rad per skill
            foreach (var skill in sortedSkills)
            {
                var skillLine = Instantiate(skillLinePrefab, contentParent);
                skillLine.name = skill.skillType.ToString() + "Line";
                // Nollställ position och anchoring på skillLine
                RectTransform rt = skillLine.GetComponent<RectTransform>();
                rt.anchoredPosition = Vector2.zero;
                rt.localScale = Vector3.one;
                rt.offsetMin = Vector2.zero;
                rt.offsetMax = Vector2.zero;
                var skillLineUI = skillLine.GetComponent<SkillLineUI>();
                if (skillLineUI != null)
                {
                    skillLineUI.SetSkill(skill);
                }
                skillLines.Add(skillLine);
            }
            categoryPanels[kvp.Key] = catPanel;
        }
    }

    public void ToggleSkillsPanel()
    {
        isSkillsOpen = !isSkillsOpen;
        if (skillsPanel != null)
            skillsPanel.SetActive(isSkillsOpen);
    }
}