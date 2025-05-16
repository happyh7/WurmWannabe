using UnityEngine;

[System.Serializable]
public class SkillData
{
    public SkillType skillType;
    public float value = 1.0f;
    public float skillEase = 1.0f; // Hur lätt denna skill är att träna upp (1.0 = normal, <1 = svårare, >1 = lättare)
    public float lastGainAmount = 0f; // Senaste skilltickens storlek
    public float lastGainTime = 0f;
    // Lägg till fler parametrar vid behov

    public SkillData(SkillType type, float startValue = 1.0f, float ease = 1.0f)
    {
        skillType = type;
        value = startValue;
        skillEase = ease;
    }
} 