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

        public ColorPicker ColorPickerToBindTo => this.ColorPicker;

        private async void Canvas_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (!(this.ModifyShape.IsChecked ?? false))
            {
                var p = e.GetCurrentPoint(this.Canvas);
                CreateShapeAtLocation(p.Position.X, p.Position.Y);
            }
            else
            {
                await BindSelectedShapeForModification(e.OriginalSource);
            }
        }

        Shape lastDataBindedShape = null;
        private readonly IValueConverter converter = new ColorToBrushConverter();
        private async Task BindSelectedShapeForModification(object originalSource)
        {
            if (!(originalSource is Shape))
            {
                var dialog = new MessageDialog("A shape has to be selected to modify...");
                await dialog.ShowAsync();
                return;
            }

            if (lastDataBindedShape != null)
            {
                lastDataBindedShape.ClearValue(Shape.FillProperty);
                lastDataBindedShape.SetBinding(Shape.FillProperty, new Binding());
                lastDataBindedShape.Fill = converter.Convert(this.ColorPicker.Color, null, null, null) as Brush;
            }

            Shape s = originalSource as Shape;

            this.ColorPicker.Color = (s.Fill as SolidColorBrush).Color;

            s.DataContext = this;
            Binding bColor = new Binding() { Path = new PropertyPath("ColorPickerToBindTo.Color"), Converter = converter };
            s.SetBinding(Shape.FillProperty, bColor);
            lastDataBindedShape = s;
        }

        private void CreateShapeAtLocation(double x, double y)
        {
            var s = CreateShape();
            if (s != null)
            {
                Canvas.Children.Add(s);
                Canvas.SetLeft(s, x);
                Canvas.SetTop(s, y);
            }
        }

        private Shape CreateShape()
        {
            Shape s = null;
            if (this.AddCircle.IsChecked ?? false)
                s = new Ellipse() { Width = WidthSlider.Value, Height = HeightSlider.Value };
            else if (this.AddRectangle.IsChecked ?? false)
                s = new Rectangle() { Width = WidthSlider.Value, Height = HeightSlider.Value };
            else
                return null;
            s.Stroke = new SolidColorBrush(Windows.UI.Colors.Blue);
            s.Fill = new SolidColorBrush(this.ColorPicker.Color);
            return s;
        }

    }
}
