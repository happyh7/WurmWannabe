using UnityEngine;

public class StaminaBarManager : MonoBehaviour
{
    public GameObject staminaBarPrefab;
    private GameObject staminaBarInstance;
    public Canvas worldSpaceCanvas;
    public float yOffset = 1.2f;

    private void Start()
    {
        if (worldSpaceCanvas != null)
        {
            staminaBarInstance = Instantiate(staminaBarPrefab, transform.position + new Vector3(0, yOffset, 0), Quaternion.identity, worldSpaceCanvas.transform);
            staminaBarInstance.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        }
    }

    private void Update()
    {
        if (staminaBarInstance != null)
        {
            staminaBarInstance.transform.position = transform.position + new Vector3(0, yOffset, 0);
        }
    }
} 