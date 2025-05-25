using UnityEngine;
using System.Collections;

public class SkillActionRunner : MonoBehaviour
{
    public GameObject castBarPrefab;
    public Canvas worldSpaceCanvas;
    private GameObject activeCastBar;
    private Coroutine currentActionCoroutine;

    /// <summary>
    /// Startar en skill action enligt request.
    /// </summary>
    public void StartAction(SkillActionRequest request)
    {
        if (request == null) return;
        if (currentActionCoroutine != null)
        {
            StopCoroutine(currentActionCoroutine);
            if (activeCastBar != null) Destroy(activeCastBar);
        }
        currentActionCoroutine = StartCoroutine(RunAction(request));
    }

    private IEnumerator RunAction(SkillActionRequest request)
    {
        float staminaAtStart = PlayerSkills.Instance.stamina;
        float staminaPercent = staminaAtStart / PlayerSkills.Instance.maxStamina;
        float castTime100 = request.baseCastTime;
        float castTime = castTime100 * (1f + (1f - staminaPercent));
        float elapsed = 0f;
        bool staminaHitZero = false;
        float staminaCostPerSecond = request.staminaCost / castTime;

        // Skapa CastBar om prefab finns
        if (castBarPrefab != null)
        {
            if (worldSpaceCanvas != null)
                activeCastBar = Instantiate(castBarPrefab, transform.position + new Vector3(0, 0.6f, 0), Quaternion.identity, worldSpaceCanvas.transform);
            else
                activeCastBar = Instantiate(castBarPrefab, transform.position + new Vector3(0, 0.6f, 0), Quaternion.identity);
        }

        request.OnStart?.Invoke();

        while (elapsed < castTime)
        {
            float delta = Time.deltaTime;
            elapsed += delta;
            // Dra stamina gradvis
            float before = PlayerSkills.Instance.stamina;
            float used = PlayerSkills.Instance.UseStamina(staminaCostPerSecond * delta);
            float after = PlayerSkills.Instance.stamina;
            if (PlayerSkills.Instance.stamina <= 0f && !staminaHitZero)
                staminaHitZero = true;
            // Uppdatera CastBar-position
            if (activeCastBar != null)
            {
                activeCastBar.transform.position = transform.position + new Vector3(0, 0.6f, 0);
                var castBarScript = activeCastBar.GetComponent<PlayerCastBar>();
                if (castBarScript != null)
                {
                    castBarScript.SetProgress(Mathf.Clamp01(elapsed / castTime));
                }
            }
            yield return null;
        }

        if (activeCastBar != null) Destroy(activeCastBar);

        // Räkna ut skilltick
        float skillFactor = staminaHitZero ? 0.01f : (castTime / castTime100);
        PlayerSkills.Instance.GainSkill(request.skillType, request.baseSkillTick * skillFactor);

        request.OnComplete?.Invoke();
        currentActionCoroutine = null;
    }

    /// <summary>
    /// Avbryter pågående action och stänger castbar.
    /// </summary>
    public void CancelAction()
    {
        if (currentActionCoroutine != null)
        {
            StopCoroutine(currentActionCoroutine);
            currentActionCoroutine = null;
        }
        if (activeCastBar != null) Destroy(activeCastBar);
    }
} 