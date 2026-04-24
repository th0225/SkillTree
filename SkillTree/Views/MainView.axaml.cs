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

                var x1 = from.X + 60;
                var y1 = from.Y + 60;
                var x2 = node.X + 60;
                var y2 = node.Y;

                // 貝茲曲線控制點
                var cy1 = y1 + (y2 - y1) * 0.5;
                var cy2 = y2 - (y2 - y1) * 0.5;

                // 顏色跟著來源節點狀態走
                var color = from.IsUnlocked
                    ? Color.Parse("#44aa66")
                    : Color.Parse("#555577");

                var path = new Avalonia.Controls.Shapes.Path
                {
                    Stroke = new SolidColorBrush(color),
                    StrokeThickness = 1.5,
                    Fill = Brushes.Transparent,
                    Data = Avalonia.Media.PathGeometry.Parse(
                        $"M {x1} {y1} C {x1} {cy1}, {x2} {cy2}, {x2} {y2}")
                };

                canvas.Children.Add(path);
            }
        }
    }
}