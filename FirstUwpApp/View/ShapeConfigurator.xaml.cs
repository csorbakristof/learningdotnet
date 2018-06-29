using FirstUwpApp.View.Converters;
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

        // Note: these controls are private which does not allow data binding...
        public ColorPicker ColorPickerToBindTo => this.ColorPicker;
        public Slider WidthSliderToBindTo => this.WidthSlider;
        public Slider HeightSliderToBindTo => this.HeightSlider;


        private Shape lastDataBindedShape = null;
        private Brush originalStrokeBrush = null;
        private readonly IValueConverter converter = new ColorToBrushConverter();
        readonly Brush SelectionStrokeBrush = new SolidColorBrush(Windows.UI.Colors.Yellow);
        public void BindShapeForModification(Shape shape, bool applyCurrentValues=false)
        {

            if (lastDataBindedShape != null)
            {
                ClearLastBinding(Shape.FillProperty);
                ClearLastBinding(Shape.WidthProperty);
                ClearLastBinding(Shape.HeightProperty);
                lastDataBindedShape.Fill = converter.Convert(ColorPicker.Color, null, null, null) as Brush;
                lastDataBindedShape.Width = WidthSlider.Value;
                lastDataBindedShape.Height = HeightSlider.Value;
                lastDataBindedShape.Stroke = originalStrokeBrush;
            }

            if (!applyCurrentValues)
            {
                ColorPicker.Color = (shape.Fill as SolidColorBrush).Color;
                WidthSlider.Value = shape.Width;
                HeightSlider.Value = shape.Height;
            }

            shape.DataContext = this;
            shape.SetBinding(Shape.FillProperty,
                new Binding() { Path = new PropertyPath("ColorPickerToBindTo.Color"), Converter = converter });
            shape.SetBinding(Shape.WidthProperty,
                new Binding() { Path = new PropertyPath("WidthSliderToBindTo.Value") });
            shape.SetBinding(Shape.HeightProperty,
                new Binding() { Path = new PropertyPath("HeightSliderToBindTo.Value") });
            originalStrokeBrush = shape.Stroke;
            shape.Stroke = new SolidColorBrush(Windows.UI.Colors.Yellow);
            lastDataBindedShape = shape;
        }

        private void ClearLastBinding(DependencyProperty prop)
        {
            lastDataBindedShape.ClearValue(prop);
            lastDataBindedShape.SetBinding(prop, new Binding());
        }
    }
}
