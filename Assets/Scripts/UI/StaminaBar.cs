using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    public Slider staminaSlider;
    public Image fillImage;
    public CanvasGroup canvasGroup;

    private void Start()
    {
        staminaSlider.value = 1f;
    }

    private void Update()
    {
        if (PlayerSkills.Instance != null)
        {
            float staminaPercent = PlayerSkills.Instance.GetStaminaPercent();
            staminaSlider.value = staminaPercent;

            // Visa/dölj hela baren med CanvasGroup
            if (canvasGroup != null)
            {
                canvasGroup.alpha = (staminaPercent < 1f) ? 1f : 0f;
            }

            // Ändra färg baserat på stamina-nivå
            if (staminaPercent <= 0f)
            {
                fillImage.color = Color.gray;
            }
            else if (staminaPercent <= 0.5f)
            {
                fillImage.color = Color.Lerp(Color.red, Color.yellow, staminaPercent * 2f);
            }
            else
            {
                fillImage.color = Color.Lerp(Color.yellow, Color.green, (staminaPercent - 0.5f) * 2f);
            }
        }
    }
} 