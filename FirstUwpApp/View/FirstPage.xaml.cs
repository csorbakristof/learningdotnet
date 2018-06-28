using Windows.UI.Xaml;
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

        private void Canvas_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            var p = e.GetCurrentPoint(this.Canvas);
            CreateShapeAtLocation(p.Position.X, p.Position.Y);
        }

        private void CreateShapeAtLocation(double x, double y)
        {
            var s = CreateShape();
            this.Canvas.Children.Add(s);
            Canvas.SetLeft(s, x);
            Canvas.SetTop(s, y);
        }

        private Shape CreateShape()
        {
            Shape s = null;
            if (this.ShapeIsCircle.IsChecked ?? false)
                s = new Ellipse() { Width = WidthSlider.Value, Height = HeightSlider.Value };
            else if (this.ShapeIsRectangle.IsChecked ?? false)
                s = new Rectangle() { Width = WidthSlider.Value, Height = HeightSlider.Value };
            else
                return null;
            s.Stroke = new SolidColorBrush(Windows.UI.Colors.Blue);
            s.Fill = new SolidColorBrush(this.Color.Color);
            return s;
        }

    }
}
