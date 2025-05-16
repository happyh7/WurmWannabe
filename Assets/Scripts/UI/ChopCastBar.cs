using UnityEngine;
using UnityEngine.UI;

public class ChopCastBar : MonoBehaviour
{
    [SerializeField] private Image fillImage;

    private void Awake()
    {
        if (fillImage == null)
        {
            fillImage = GetComponentInChildren<Image>();
        }
    }

    public void SetProgress(float progress)
    {
        fillImage.fillAmount = progress;
    }

    public void SetVisible(bool visible)
    {
        gameObject.SetActive(visible);
    }

    public void UpdateProgress(float progress)
    {
        SetProgress(progress);
    }

    public void Initialize(string label)
    {
        // Om du har en textkomponent för label, sätt den här
        // Annars lämna tom
    }
} 