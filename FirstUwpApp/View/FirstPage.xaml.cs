using System;
using Windows.Foundation;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
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
            if (!(ModifyShape.IsChecked ?? false))
            {
                var p = e.GetCurrentPoint(Canvas);
                Shape newShape = CreateShapeAtLocation(p.Position.X, p.Position.Y);
                ShapeConfig.BindShapeForModification(newShape, true);
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
            if (AddCircle.IsChecked ?? false)
                s = new Ellipse() { Width = 10, Height = 10 };
            else if (AddRectangle.IsChecked ?? false)
                s = new Rectangle() { Width = 10, Height = 10 };
            else
                return null;
            s.Stroke = new SolidColorBrush(Windows.UI.Colors.Blue);
            s.Fill = new SolidColorBrush(Windows.UI.Colors.Black);
            return s;
        }


        private Point? InitialOffsetOfMouseMove = null;
        private void Canvas_PointerMoved(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            var p = e.GetCurrentPoint(Canvas);
            if (p.Properties.IsLeftButtonPressed)
            {
                if (e.OriginalSource is Shape s)
                {
                    if (InitialOffsetOfMouseMove == null)
                        InitialOffsetOfMouseMove = new Point(
                            Canvas.GetLeft(s) - p.Position.X,
                            Canvas.GetTop(s) - p.Position.Y);
                    Canvas.SetLeft(s, p.Position.X + (InitialOffsetOfMouseMove?.X ?? 0.0));
                    Canvas.SetTop(s, p.Position.Y + (InitialOffsetOfMouseMove?.Y ?? 0.0));
                }
            }
            else
            {
                InitialOffsetOfMouseMove = null;
            }
        }
    }
}
