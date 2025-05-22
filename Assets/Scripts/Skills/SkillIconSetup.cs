using UnityEngine;

public class SkillIconSetup : MonoBehaviour
{
    [System.Serializable]
    public class SkillIconData
    {
        public SkillType skillType;
        public Sprite icon;
        public string displayName;
        public string description;
    }

    public Sprite defaultIcon;
    public SkillIconData[] skillIcons;

    void Awake()
    {
        // Sätt default icon
        SkillData.placeholderIcon = defaultIcon;

        // Sätt upp alla skill icons
        foreach (var skillIcon in skillIcons)
        {
            if (skillIcon.icon != null)
                SkillData.skillIcons[skillIcon.skillType] = skillIcon.icon;
            
            if (!string.IsNullOrEmpty(skillIcon.displayName))
                SkillData.skillNames[skillIcon.skillType] = skillIcon.displayName;
            
            if (!string.IsNullOrEmpty(skillIcon.description))
                SkillData.skillDescriptions[skillIcon.skillType] = skillIcon.description;
        }
    }
} 