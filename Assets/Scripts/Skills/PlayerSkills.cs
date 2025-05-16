using UnityEngine;
using System.Collections.Generic;

public class PlayerSkills : MonoBehaviour
{
    public static PlayerSkills Instance;

    [Header("Skillinställningar")]
    public List<SkillData> skills = new List<SkillData>();
    public float stamina = 100f;
    public float maxStamina = 100f;
    public float staminaRegenPerSecond = 5f;
    public bool IsActionInProgress = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        // Initiera alla skills om listan är tom
        if (skills.Count == 0)
        {
            foreach (SkillType type in System.Enum.GetValues(typeof(SkillType)))
            {
                skills.Add(new SkillData(type));
            }
        }
    }

    private void Update()
    {
        // Regenerera stamina bara om ingen action pågår
        if (!IsActionInProgress && stamina < maxStamina)
        {
            stamina += staminaRegenPerSecond * Time.deltaTime;
            if (stamina > maxStamina) stamina = maxStamina;
        }
    }

    public SkillData GetSkill(SkillType type)
    {
        return skills.Find(s => s.skillType == type);
    }

    // Öka en skill med rätt logik
    public void GainSkill(SkillType type, float baseValue = 1.0f)
    {
        SkillData skill = GetSkill(type);
        if (skill == null) return;

        float staminaPercent = stamina / maxStamina;
        float staminaFactor = staminaPercent > 0f ? 0.01f + 0.99f * staminaPercent : 0.01f;
        float gain = baseValue * skill.skillEase * (1f - (skill.value / 100f)) * staminaFactor;
        if (gain < 0.01f) gain = 0.01f; // Minsta tick
        skill.value += gain;
        if (skill.value > 100f) skill.value = 100f;
        skill.lastGainAmount = gain;
        skill.lastGainTime = Time.time;
        // Här kan du lägga till feedback till NotificationPanel
        Debug.Log($"+{gain:F2} {type} (totalt: {skill.value:F2})");
        if (NotificationManager.Instance != null)
        {
            NotificationManager.Instance.ShowNotification($"+{gain:F2} {type}!");
        }
    }

    // Exempel på stamina-kostnad
    public float UseStamina(float amount)
    {
        float used = Mathf.Min(stamina, amount);
        stamina -= used;
        if (stamina < 0) stamina = 0;
        return used;
    }

    public float GetStaminaPercent()
    {
        return stamina / maxStamina;
    }

    public void RecoverStamina(float amount)
    {
        Debug.Log($"PlayerSkills: Recovering {amount} stamina");
        stamina = Mathf.Min(maxStamina, stamina + amount);
        Debug.Log($"PlayerSkills: Stamina now at {stamina}");
    }

    // För att öka passiva skills automatiskt
    public void AddPassiveSkill(SkillType type, float baseValue = 0.2f)
    {
        Debug.Log($"PlayerSkills: Adding passive skill {type}");
        GainSkill(type, baseValue);
    }

    // För att påverka action-tid av skills
    public float GetActionTime(SkillType type, float baseTime)
    {
        SkillData skill = GetSkill(type);
        if (skill == null) return baseTime;

        float time = baseTime * (1f - (skill.value / 200f));
        return Mathf.Max(0.5f, time); // Sätt ett minimum om du vill
    }

    public void AddWoodcuttingSkillTest()
    {
        GainSkill(SkillType.Woodcutting);
    }
} 