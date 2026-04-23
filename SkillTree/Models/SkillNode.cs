using System;
using System.Collections.Generic;

namespace SkillTree.Models;

public enum SkillStatus
{
    Locked,
    Avaliable,
    Unlocked
}

public enum SkillCategory
{
    Programming,
    Language,
    Design,
    Other
}

public class SkillNode
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public SkillCategory Category { get; set; } = SkillCategory.Other;
    public SkillStatus Status { get; set; } = SkillStatus.Locked;

    public List<string> RequiredNodeIds { get; set; } = [];

    public double X { get; set; }
    public double Y { get; set; }

    public DateTime? UnlockedAt { get; set; }
    public string Notes { get; set; } = string.Empty;
}