using UnityEngine;
using UnityEngine.UI;

public class AxeDurabilityBar : MonoBehaviour
{
    [SerializeField] private Image fillImage;

    // Colors for full (green), half (yellow), and low (red) durability
    private Color fullColor = new Color(0.2f, 1f, 0.2f); // Green
    private Color midColor = new Color(1f, 1f, 0.2f);    // Yellow
    private Color lowColor = new Color(1f, 0.2f, 0.2f);   // Red

    public void SetDurability(float current, float max)
    {
        float percent = (max > 0) ? Mathf.Clamp01(current / max) : 0f;
        fillImage.fillAmount = percent;
        fillImage.color = GetDurabilityColor(percent);
    }

    private Color GetDurabilityColor(float percent)
    {
        if (percent > 0.6f)
            return Color.Lerp(midColor, fullColor, (percent - 0.6f) / 0.4f); // Greenish
        else if (percent > 0.3f)
            return Color.Lerp(lowColor, midColor, (percent - 0.3f) / 0.3f);  // Yellowish
        else
            return lowColor; // Red
    }
} 