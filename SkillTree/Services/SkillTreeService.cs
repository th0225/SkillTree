using System;
using System.Collections.Generic;
using System.Linq;
using SkillTree.Models;

namespace SkillTree.Services;

public class SkillTreeService
{
    // 計算狀態
    public void RecalculateStatuses(SkillTreeData tree)
    {
        foreach (var node in tree.Nodes)
        {
            if (node.Status == SkillStatus.Unlocked)
                continue;
            
            node.Status = CanUnlock(node, tree.Nodes)
                ? SkillStatus.Available
                : SkillStatus.Locked;
        }
    }

    // 檢查提前條休
    public bool CanUnlock(SkillNode node, List<SkillNode> allNodes)
    {
        if (node.RequiredNodeIds.Count == 0)
            return true;

        return node.RequiredNodeIds.All(reqId =>
            allNodes.FirstOrDefault(n =>
                n.Id == reqId)?.Status == SkillStatus.Unlocked);
    }

    // 解鎖並重新計算狀態
    public bool TryUnlock(SkillNode node, SkillTreeData tree)
    {
        if (!CanUnlock(node, tree.Nodes))
            return false;

        node.Status = SkillStatus.Unlocked;
        node.UnlockedAt = DateTime.Now;

        RecalculateStatuses(tree);
        return true;
    }

    // 上鎖
    public bool TryLock(SkillNode node, SkillTreeData tree)
    {
        var dependents = tree.Nodes
            .Where(n => n.RequiredNodeIds.Contains(node.Id)
                && n.Status == SkillStatus.Unlocked)
            .ToList();
        
        if (dependents.Count > 0)
            return false;

        node.Status = SkillStatus.Available;
        node.UnlockedAt = null;

        RecalculateStatuses(tree);
        return true;
    }

    // 建立範本資料
    public SkillTreeData CreateSampleTree()
    {
        var csharpBasic = new SkillNode
        {
            Name = "C# 基礎",
            Description = "變數、迴圈、條件判斷",
            Category = SkillCategory.Programming,
            Status = SkillStatus.Available,
            X = 300, Y = 50
        };

        var oop = new SkillNode
        {
            Name = "物件導向",
            Description = "類別、繼承、介面",
            Category = SkillCategory.Programming,
            RequiredNodeIds = [csharpBasic.Id],
            X = 300, Y = 180
        };

        var avalonia = new SkillNode
        {
            Name = "Avalonia UI",
            Description = "跨平台桌面應用",
            Category = SkillCategory.Programming,
            RequiredNodeIds = [oop.Id],
            X = 300, Y = 310
        };

        var tree = new SkillTreeData
        {
            Name = "程式設計之路",
            Description = "從 C# 到跨平台應用",
            Nodes = [csharpBasic, oop, avalonia]
        };

        RecalculateStatuses(tree);
        return tree;
    }
}