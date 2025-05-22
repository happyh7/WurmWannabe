using System;

public class SkillActionRequest
{
    public float baseCastTime;
    public float baseSkillTick;
    public SkillType skillType;
    public float staminaCost;
    public Action OnStart;
    public Action OnComplete;
    public Action OnCancel;

    public SkillActionRequest(float baseCastTime, float baseSkillTick, SkillType skillType, float staminaCost,
        Action onStart = null, Action onComplete = null, Action onCancel = null)
    {
        this.baseCastTime = baseCastTime;
        this.baseSkillTick = baseSkillTick;
        this.skillType = skillType;
        this.staminaCost = staminaCost;
        this.OnStart = onStart;
        this.OnComplete = onComplete;
        this.OnCancel = onCancel;
    }
} 