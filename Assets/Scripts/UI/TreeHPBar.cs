using UnityEngine;
using UnityEngine.UI;

public class TreeHPBar : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    private Transform targetTree;
    private float yOffset = 1.5f;

    public void SetTarget(Transform treeTransform)
    {
        targetTree = treeTransform;
    }

    public void SetHP(float current, float max)
    {
        if (fillImage != null && max > 0)
            fillImage.fillAmount = current / max;
    }

    public void Show() => gameObject.SetActive(true);
    public void Hide() => gameObject.SetActive(false);

    public void Initialize(GameTree tree)
    {
        SetTarget(tree.transform);
        SetHP(tree.GetCurrentHP(), tree.GetMaxHP());
    }

    private void LateUpdate()
    {
        if (targetTree != null)
        {
            // Följ trädet i world space
            transform.position = targetTree.position + Vector3.up * yOffset;
        }
    }
} 