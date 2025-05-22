using UnityEngine;
using TMPro;

public class SkillTooltip : MonoBehaviour
{
    public static SkillTooltip instance;
    public GameObject tooltipPanel;
    public TextMeshProUGUI tooltipText;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        if (tooltipPanel != null)
            tooltipPanel.SetActive(false);
    }

    public static void Show(string text, Vector2 position)
    {
        if (instance == null || instance.tooltipPanel == null) return;

        instance.tooltipText.text = text;
        instance.tooltipPanel.transform.position = position;
        instance.tooltipPanel.SetActive(true);
    }

    public static void Hide()
    {
        if (instance == null || instance.tooltipPanel == null) return;
        instance.tooltipPanel.SetActive(false);
    }
} 