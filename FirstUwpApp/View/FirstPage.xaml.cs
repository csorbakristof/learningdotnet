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
            var type = this.Type.SelectedValue as ComboBoxItem;
            switch (type.Content)
            {
                case "Polygon":
                    return new Rectangle() { Width = 50, Height = 50 };
                case "Circle":
                    return new Ellipse() { Width = 50, Height = 50 };
            }
            return null;
        }
    }
}
