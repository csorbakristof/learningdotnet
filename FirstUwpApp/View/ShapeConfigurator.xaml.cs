using FirstUwpApp.ViewModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace FirstUwpApp.View
{
    public sealed partial class ShapeConfigurator : UserControl
    {
        public ShapeConfigurator()
        {
            this.InitializeComponent();
        }

        public ColorPicker ColorPickerToBindTo => this.ColorPicker;

        Shape lastDataBindedShape = null;
        private readonly IValueConverter converter = new ColorToBrushConverter();
        public void BindShapeForModification(Shape shape, bool applyCurrentValues=false)
        {

            if (lastDataBindedShape != null)
            {
                lastDataBindedShape.ClearValue(Shape.FillProperty);
                lastDataBindedShape.SetBinding(Shape.FillProperty, new Binding());
                lastDataBindedShape.Fill = converter.Convert(this.ColorPicker.Color, null, null, null) as Brush;
            }

            if (!applyCurrentValues)
            {
                this.ColorPicker.Color = (shape.Fill as SolidColorBrush).Color;
            }

            shape.DataContext = this;
            Binding bColor = new Binding() { Path = new PropertyPath("ColorPickerToBindTo.Color"), Converter = converter };
            shape.SetBinding(Shape.FillProperty, bColor);
            lastDataBindedShape = shape;
        }

    }
}
