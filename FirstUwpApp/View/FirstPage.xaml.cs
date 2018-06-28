using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace FirstUwpApp.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
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
