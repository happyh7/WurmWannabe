using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class CastTimeBonusEntry
{
    public float castTime; // t.ex. 0.5, 1.0, 1.5, ... (castTime / baseCastTime)
    public float bonus;    // t.ex. 0.3, 1.0, 1.05, ...
}

public class SkillActionRunner : MonoBehaviour
{
    public GameObject castBarPrefab;
    public Canvas worldSpaceCanvas;
    private GameObject activeCastBar;
    private Coroutine currentActionCoroutine;
    private bool isCancelled = false;

    [Header("Skill Bonus Settings")]
    public List<CastTimeBonusEntry> castTimeBonusTable = new List<CastTimeBonusEntry>();
    public AnimationCurve castTimeToSkillBonus = AnimationCurve.Linear(1f, 1f, 1.5f, 1.5f); // Default: 1x vid normal, 1.5x vid 1.5x casttime
    public float baseCastTime = 2.0f; // Standard cast-tid vid full stamina

    private void OnValidate()
    {
        // Uppdatera kurvan automatiskt när du ändrar listan i Inspectorn
        if (castTimeBonusTable != null && castTimeBonusTable.Count > 0)
        {
            Keyframe[] keys = new Keyframe[castTimeBonusTable.Count];
            for (int i = 0; i < castTimeBonusTable.Count; i++)
            {
                keys[i] = new Keyframe(castTimeBonusTable[i].castTime, castTimeBonusTable[i].bonus);
            }
            castTimeToSkillBonus.keys = keys;
        }
    }

    /// <summary>
    /// Startar en skill action enligt request.
    /// </summary>
    public void StartAction(SkillActionRequest request)
    {
        isCancelled = false;
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
        float staminaPercentStart = staminaAtStart / PlayerSkills.Instance.maxStamina;
        float staminaPercent = staminaPercentStart;
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
            if (isCancelled)
            {
                if (activeCastBar != null) Destroy(activeCastBar);
                currentActionCoroutine = null;
                yield break;
            }
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

        if (isCancelled)
        {
            if (activeCastBar != null) Destroy(activeCastBar);
            currentActionCoroutine = null;
            yield break;
        }

        if (activeCastBar != null) Destroy(activeCastBar);

        // Räkna ut skilltick
        float skillValue = PlayerSkills.Instance.GetSkill(request.skillType)?.value ?? 0f;
        float skillTickNoBonus = 0f;
        float skillTick = 0f;
        float bonusSkillGain = 0f;
        if (skillValue == 0f) {
            skillTick = 1.0f;
            skillTickNoBonus = 1.0f;
            bonusSkillGain = 0f;
        } else {
            float baseSkill = 0.15f;
            float decay = 0.96f;
            if (request.skillType == SkillType.Woodcutting)
            {
                baseSkill = 0.15f;
                decay = 0.96f;
            }
            skillTickNoBonus = baseSkill * Mathf.Pow(decay, skillValue * 1.2f);
            // --- NYTT: Beräkna bonus utifrån castTime ---
            float castTimeRatio = castTime / baseCastTime;
            float skillBonus = castTimeToSkillBonus.Evaluate(castTimeRatio);
            if (staminaHitZero)
                skillBonus = 0.01f;
            skillTick = skillTickNoBonus * skillBonus;
            bonusSkillGain = skillTick - skillTickNoBonus;
        }
        PlayerSkills.Instance.GainSkill(request.skillType, skillTick);

        // --- NYTT: Visa detaljerat meddelande i chatten ---
        float newTotal = PlayerSkills.Instance.GetSkill(request.skillType)?.value ?? 0f;
        string skillName = SkillData.GetDisplayName(request.skillType);
        string message = $"+{skillTick:F8} {skillName}! Total: {newTotal:F8} (Skill bonus: x{(skillTick/skillTickNoBonus):F2})";
        NotificationManager.Instance?.ShowNotification(message);
        // --- SLUT NYTT ---

        request.OnComplete?.Invoke();
        currentActionCoroutine = null;
    }

    /// <summary>
    /// Avbryter pågående action och stänger castbar.
    /// </summary>
    public void CancelAction()
    {
        isCancelled = true;
        if (currentActionCoroutine != null)
        {
            StopCoroutine(currentActionCoroutine);
            currentActionCoroutine = null;
        }
        if (activeCastBar != null) Destroy(activeCastBar);
    }
} 