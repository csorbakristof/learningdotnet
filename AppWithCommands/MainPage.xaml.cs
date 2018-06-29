using AppWithCommands.Model;
using AppWithCommands.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace AppWithCommands
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var textModel = new TextModel();
            var textViewModel = new TextViewModel(textModel);
            myControl.ViewModel = textViewModel;    // TODO: Itt miert nem kellett INotifyPropertyChanged a VM modositasakor?
            var cmd = new AddCommand(textModel);
            this.myControl.SetAddCommand(cmd);
            myControl.PointerPressedCommand = new AddPositionCommand(textModel);
        }
    }
}
