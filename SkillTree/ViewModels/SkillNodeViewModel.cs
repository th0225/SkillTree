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
    }
}