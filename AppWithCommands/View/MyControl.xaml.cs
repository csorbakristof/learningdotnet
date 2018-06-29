using AppWithCommands.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace AppWithCommands.View
{
    public sealed partial class MyControl : UserControl, INotifyPropertyChanged
    {
        public MyControl()
        {
            this.InitializeComponent();
        }

        public void SetAddCommand(ICommand cmd)
        {
            this.AddButton.Command = cmd;
        }

        private TextViewModel textViewModel;

        public event PropertyChangedEventHandler PropertyChanged;

        public TextViewModel ViewModel {
            get
            {
                return this.textViewModel;
            }
            set
            {
                if (this.textViewModel != value)
                {
                    this.textViewModel = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ViewModel"));
                }
            }
        }
    }
}
