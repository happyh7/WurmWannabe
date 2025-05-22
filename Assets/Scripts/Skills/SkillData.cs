using UnityEngine;
using System.Collections.Generic;

public class SkillData
{
    public SkillType skillType;
    public float value;
    public float skillEase = 1.0f; // Hur lätt denna skill är att träna upp (1.0 = normal, <1 = svårare, >1 = lättare)
    public float lastGainAmount = 0f; // Senaste skilltickens storlek
    public float lastGainTime = 0f;
    public SkillCategory category;
    // Lägg till fler parametrar vid behov

    public static Sprite placeholderIcon;
    public static Dictionary<SkillType, Sprite> skillIcons = new Dictionary<SkillType, Sprite>();
    public static Dictionary<SkillType, string> skillNames = new Dictionary<SkillType, string>();
    public static Dictionary<SkillType, string> skillDescriptions = new Dictionary<SkillType, string>();

    public static string GetDisplayName(SkillType type)
    {
        if (skillNames.TryGetValue(type, out string name))
            return name;
        return type.ToString();
    }

    public static Sprite GetIcon(SkillType type)
    {
        if (skillIcons.TryGetValue(type, out Sprite icon))
            return icon;
        return placeholderIcon;
    }

    public static string GetDescription(SkillType type)
    {
        if (skillDescriptions.TryGetValue(type, out string description))
            return description;
        return "No description available.";
    }

    public SkillData(SkillType type, float startValue = 1.0f, float ease = 1.0f)
    {
        skillType = type;
        value = startValue;
        skillEase = ease;
        category = SkillCategoryMapping.GetCategory(type);
    }
}

public static class SkillCategoryMapping
{
    public static SkillCategory GetCategory(SkillType type)
    {
        switch (type)
        {
            case SkillType.Woodcutting:
                return SkillCategory.Extraction;
            case SkillType.Crafting:
                return SkillCategory.Crafting;
            case SkillType.Repairing:
                return SkillCategory.Crafting;
            case SkillType.Strength:
            case SkillType.Stamina:
            case SkillType.Precision:
            case SkillType.Dexterity:
            case SkillType.Endurance:
            case SkillType.Luck:
                return SkillCategory.Passive;
            default:
                return SkillCategory.Passive;
        }
    }
} 