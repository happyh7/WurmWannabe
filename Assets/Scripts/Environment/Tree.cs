using UnityEngine;
using System.Collections;

public class Tree : MonoBehaviour
{
    [SerializeField] private int maxHP = 3;
    [SerializeField] private int health = 3;
    [SerializeField] private float shakeDuration = 0.2f;
    [SerializeField] private float shakeIntensity = 0.1f;
    [SerializeField] private GameObject woodPrefab;
    [SerializeField] private int woodDropAmount = 2;

    private Vector3 originalPosition;
    private bool isBeingChopped = false;

    private void Start()
    {
        originalPosition = transform.position;
        health = maxHP;
    }

    public void Chop()
    {
        if (isBeingChopped) return;
        
        StartCoroutine(ShakeTree());
        health--;
        
        if (health <= 0)
        {
            StartCoroutine(FallTree());
        }
    }

    public int GetCurrentHP()
    {
        return health;
    }

    public int GetMaxHP()
    {
        return maxHP;
    }

    public void TakeDamage(int amount)
    {
        health = Mathf.Max(0, health - amount);

        if (health == 0)
        {
            // Spawn 2 wood objects at the tree's position
            for (int i = 0; i < 2; i++)
            {
                Instantiate(woodPrefab, transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
        }
    }

    private IEnumerator ShakeTree()
    {
        isBeingChopped = true;
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeIntensity;
            float y = Random.Range(-1f, 1f) * shakeIntensity;
            transform.position = originalPosition + new Vector3(x, y, 0);
            
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPosition;
        isBeingChopped = false;
    }

    private IEnumerator FallTree()
    {
        // Rotera trädet 90 grader
        float elapsed = 0f;
        float duration = 0.5f;
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(0, 0, 90);

        while (elapsed < duration)
        {
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Spawna ved
        for (int i = 0; i < woodDropAmount; i++)
        {
            Vector3 randomOffset = new Vector3(
                Random.Range(-0.5f, 0.5f),
                Random.Range(-0.5f, 0.5f),
                0
            );
            Instantiate(woodPrefab, transform.position + randomOffset, Quaternion.identity);
        }

        // Ta bort trädet
        Destroy(gameObject);
    }
} 