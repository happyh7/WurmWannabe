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

    /// <summary>
    /// Returnerar SkillData för angiven typ.
    /// </summary>
    public SkillData GetSkill(SkillType type)
    {
        return skills.Find(s => s.skillType == type);
    }

    /// <summary>
    /// Ökar en skill med rätt logik.
    /// </summary>
    public void GainSkill(SkillType type, float baseValue = 1.0f)
    {
        SkillData skill = GetSkill(type);
        if (skill == null) return;
        float gain = baseValue;
        skill.value += gain;
        if (skill.value > 100f) skill.value = 100f;
        skill.lastGainAmount = gain;
        skill.lastGainTime = Time.time;
        NotificationManager.Instance?.ShowNotification($"+{gain:F2} {type}!");
    }

    /// <summary>
    /// Drar stamina och returnerar mängden som faktiskt användes.
    /// </summary>
    public float UseStamina(float amount)
    {
        float used = Mathf.Min(stamina, amount);
        stamina -= used;
        if (stamina < 0) stamina = 0;
        return used;
    }

    /// <summary>
    /// Returnerar stamina i procent (0-1).
    /// </summary>
    public float GetStaminaPercent()
    {
        return stamina / maxStamina;
    }

    /// <summary>
    /// Återställer stamina med angivet belopp.
    /// </summary>
    public void RecoverStamina(float amount)
    {
        stamina = Mathf.Min(maxStamina, stamina + amount);
    }

    // För att öka passiva skills automatiskt
    public void AddPassiveSkill(SkillType type, float baseValue = 0.2f)
    {
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