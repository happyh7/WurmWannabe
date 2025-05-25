using UnityEngine;
using System.Collections;

public class GameTree : MonoBehaviour
{
    [SerializeField] private float maxHP = 10f;
    [SerializeField] private GameObject logPrefab;
    [SerializeField] private int logCount = 3;
    [SerializeField] private float logSpreadRadius = 2f;
    [SerializeField] private float logForce = 5f;
    [SerializeField] private float logUpwardForce = 2f;
    [SerializeField] private float logTorque = 2f;
    [SerializeField] private float logLifetime = 300f; // 5 minuter

    private float currentHP;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    private void Start()
    {
        currentHP = maxHP;
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    public void TakeDamage(float amount)
    {
        currentHP -= amount;
        StartCoroutine(ShakeTree());
        if (currentHP <= 0)
        {
            StartFalling();
        }
    }

    public float GetCurrentHP()
    {
        return currentHP;
    }

    public float GetMaxHP()
    {
        return maxHP;
    }

    public void SetHighlight(bool highlight)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = highlight ? Color.yellow : originalColor;
        }
    }

    private void StartFalling()
    {
        // Spawn logs and destroy tree instantly
        SpawnLogs();
        Destroy(gameObject);
    }

    private void SpawnLogs()
    {
        for (int i = 0; i < logCount; i++)
        {
            Vector2 randomOffset = new Vector2(
                Random.Range(-logSpreadRadius, logSpreadRadius),
                Random.Range(-logSpreadRadius, logSpreadRadius)
            );

            Vector2 spawnPosition = (Vector2)transform.position + randomOffset;
            GameObject log = Instantiate(logPrefab, spawnPosition, Quaternion.Euler(0f, Random.Range(0f, 360f), 0f));
            
            Rigidbody2D rb = log.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 forceDirection = (randomOffset.normalized + Vector2.up * logUpwardForce).normalized;
                rb.AddForce(forceDirection * logForce, ForceMode2D.Impulse);
                rb.AddTorque(Random.Range(-logTorque, logTorque), ForceMode2D.Impulse);
            }

            Destroy(log, logLifetime);
        }
    }

    private IEnumerator ShakeTree()
    {
        if (spriteRenderer != null)
        {
            Vector3 originalPos = transform.position;
            float elapsed = 0f;
            float duration = 0.2f;
            float intensity = 0.1f;

            while (elapsed < duration)
            {
                float x = Random.Range(-1f, 1f) * intensity;
                float y = Random.Range(-1f, 1f) * intensity;
                transform.position = originalPos + new Vector3(x, y, 0);
                elapsed += Time.deltaTime;
                yield return null;
            }

            transform.position = originalPos;
        }
    }
} 