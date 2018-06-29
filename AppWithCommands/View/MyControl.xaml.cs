using AppWithCommands.ViewModel;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;

namespace AppWithCommands.View
{
    public sealed partial class MyControl : UserControl
    {
        public MyControl()
        {
            this.InitializeComponent();
        }

        public void SetAddCommand(ICommand cmd)
        {
            this.AddButton.Command = cmd;
        }


        public ICommand PointerPressedCommand { get; set; }

        public TextViewModel ViewModel { get; set; }

        private void Canvas_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            PointerPressedCommand?.Execute(e.GetCurrentPoint(this.Canvas).Position);
            // Note: innen kezdve a PointerPressedCommand-ot már lehet tesztelni. De ehhez minden eseménykezelő belsejét
            //  valami Command pattern mögé kell rakni?
        }
    }
}
