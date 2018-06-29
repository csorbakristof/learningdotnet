using FirstUwpApp.ViewModel;
using System;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace FirstUwpApp.View
{
    public sealed partial class FirstPage : Page
    {
        public FirstPage()
        {
            this.InitializeComponent();
        }

        private async void Canvas_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (!(this.ModifyShape.IsChecked ?? false))
            {
                var p = e.GetCurrentPoint(this.Canvas);
                Shape newShape = CreateShapeAtLocation(p.Position.X, p.Position.Y);
                this.ShapeConfig.BindShapeForModification(newShape, true);
            }
            else
            {
                Shape s = e.OriginalSource as Shape;
                if (s==null)
                {
                    var dialog = new MessageDialog("A shape has to be selected to modify...");
                    await dialog.ShowAsync();
                    return;
                }
                this.ShapeConfig.BindShapeForModification(s);
            }
        }

        private Shape CreateShapeAtLocation(double x, double y)
        {
            var s = CreateShape();
            if (s != null)
            {
                Canvas.Children.Add(s);
                Canvas.SetLeft(s, x);
                Canvas.SetTop(s, y);
            }
            return s;
        }

        private Shape CreateShape()
        {
            Shape s = null;
            if (this.AddCircle.IsChecked ?? false)
                s = new Ellipse() { Width = 10, Height = 10 };
            else if (this.AddRectangle.IsChecked ?? false)
                s = new Rectangle() { Width = 10, Height = 10 };
            else
                return null;
            s.Stroke = new SolidColorBrush(Windows.UI.Colors.Blue);
            s.Fill = new SolidColorBrush(Windows.UI.Colors.Black);
            return s;
        }

    }
}
