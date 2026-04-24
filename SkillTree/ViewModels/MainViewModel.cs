using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Linq;
using SkillTree.Models;
using SkillTree.Services;
using System;

namespace SkillTree.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private readonly SkillTreeService _service = new();
    private readonly SkillTreeData _currentTree;
    public int UnlockedCount => Nodes.Count(n => n.IsUnlocked);
    public int TotalCount => Nodes.Count;
    public event Action? NodesUpdated;

    public ObservableCollection<SkillNodeViewModel> Nodes { get; } = [];

    [ObservableProperty]
    private SkillNodeViewModel? _selectedNode;

    public MainViewModel()
    {
        _currentTree = _service.CreateSampleTree();
        LoadNodes();
    }

    private void LoadNodes()
    {
        Nodes.Clear();
        foreach (var node in _currentTree.Nodes)
        {
            var vm = new SkillNodeViewModel(node);
            vm.SyncFromModel();
            Nodes.Add(vm);
        }
    }

    [RelayCommand]
    private void ToggleNode(SkillNodeViewModel nodeVm)
    {
        var model = _currentTree.Nodes.First(n => n.Id == nodeVm.Id);

        if (model.Status == SkillStatus.Unlocked)
            _service.TryLock(model, _currentTree);
        else
            _service.TryUnlock(model, _currentTree);
        
        foreach (var vm in Nodes)
            vm.SyncFromModel();

        OnPropertyChanged(nameof(UnlockedCount));
        NodesUpdated?.Invoke();
    }
}
