using System.Linq;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using SkillTree.ViewModels;

namespace SkillTree.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
        DataContextChanged += OnDataContextChanged;
    }

    private void OnDataContextChanged(object? sender, System.EventArgs e)
    {
        if (DataContext is MainViewModel vm)
        {
            DrawConnections(vm);
            vm.NodesUpdated += () => DrawConnections(vm);
        }
    }

    private void DrawConnections(MainViewModel vm)
    {
        var canvas = this.FindControl<Canvas>("ConnectionCanvas");
        if (canvas == null) return;

        canvas.Children.Clear();

        foreach (var node in vm.Nodes)
        {
            foreach (var reqId in node.RequiredNodeIds)
            {
                var from = vm.Nodes.FirstOrDefault(n => n.Id == reqId);
                if (from == null) continue;

                // 從節點中心底部連到目標節點中心頂部
                var line = new Line
                {
                    StartPoint = new Avalonia.Point(from.X + 60, from.Y + 60),
                    EndPoint = new Avalonia.Point(node.X + 60, node.Y),
                    Stroke = new SolidColorBrush(Color.Parse("#555577")),
                    StrokeThickness = 1.5
                };

                canvas.Children.Add(line);
            }
        }
    }
}