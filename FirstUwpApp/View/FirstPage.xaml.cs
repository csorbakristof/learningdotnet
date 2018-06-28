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

        private void CreateShapeButton_Click(object sender, RoutedEventArgs e)
        {
            var s = CreateShape();
            s.Stroke = new SolidColorBrush(Windows.UI.Colors.Blue);
            s.Fill = new SolidColorBrush(this.Color.Color);
            this.Canvas.Children.Add(s);
            Canvas.SetLeft(s, 10);
            Canvas.SetTop(s, 10);
        }

        private Shape CreateShape()
        {
            if (this.ShapeIsCircle.IsChecked ?? false)
                return new Ellipse() { Width = WidthSlider.Value, Height = HeightSlider.Value };
            else if (this.ShapeIsRectangle.IsChecked ?? false)
                return new Rectangle() { Width = WidthSlider.Value, Height = HeightSlider.Value };
            return null;
        }
    }
}
