using System.Collections.Generic;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using SkillTree.Models;

namespace SkillTree.ViewModels;

public partial class SkillNodeViewModel(SkillNode model) : ViewModelBase
{
    private readonly SkillNode _model = model;

    public string Id => _model.Id;
    public string Name => _model.Name;
    public string Description => _model.Description;
    public SkillCategory Category => _model.Category;
    public double X => _model.X;
    public double Y => _model.Y;
    public List<string> RequiredNodeIds => _model.RequiredNodeIds;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsUnlocked))]
    [NotifyPropertyChangedFor(nameof(IsAvailable))]
    [NotifyPropertyChangedFor(nameof(IsLocked))]
    private SkillStatus _status = SkillStatus.Locked;

    public bool IsUnlocked => Status == SkillStatus.Unlocked;
    public bool IsAvailable => Status == SkillStatus.Available;
    public bool IsLocked => Status == SkillStatus.Locked;

    public void SyncFromModel()
    {
        Status = _model.Status;
        OnPropertyChanged(nameof(StatusColor));
        OnPropertyChanged(nameof(BorderColor));
        OnPropertyChanged(nameof(BackgroundBrush));
        OnPropertyChanged(nameof(BorderBrush));
    }

    public string StyleClass => _status switch
    {
        SkillStatus.Unlocked => "skill-unlocked",
        SkillStatus.Available => "skill-available",
        _ => "skill_locked"
    };

    public string StatusColor => _status switch
    {
        SkillStatus.Unlocked => "#1a4a2a",
        SkillStatus.Available => "#1a3a5c",
        _ => "#2d2d44"
    };

    public string BorderColor => _status switch
    {
        SkillStatus.Unlocked => "#44aa66",
        SkillStatus.Available => "#4488cc",
        _ => "#555577"
    };

    public IBrush BackgroundBrush => _status switch
    {
        SkillStatus.Unlocked => new SolidColorBrush(Color.Parse("#1a4a2a")),
        SkillStatus.Available => new SolidColorBrush(Color.Parse("#1a3a5c")),
        _ => new SolidColorBrush(Color.Parse("#2d2d44"))
    };

    public IBrush BorderBrush => _status switch
    {
        SkillStatus.Unlocked => new SolidColorBrush(Color.Parse("#44aa66")),
        SkillStatus.Available => new SolidColorBrush(Color.Parse("#4488cc")),
        _ => new SolidColorBrush(Color.Parse("#555577"))
    };
}