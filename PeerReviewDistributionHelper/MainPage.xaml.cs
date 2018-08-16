using PeerReviewCommonLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace PeerReviewDistributionHelper
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            Reviews = new ObservableCollection<Review>();
            Supervisions = new ObservableCollection<Supervision>();
            this.InitializeComponent();
        }

        private void LoadSupervisionButton_Click(object sender, RoutedEventArgs e)
        {
            Supervisions.Add(new Supervision());

        }

        private void LoadReviewsButton_Click(object sender, RoutedEventArgs e)
        {
            Reviews.Add(new Review());
        }

        public ObservableCollection<Review> Reviews { get; set; }
        public ObservableCollection<Supervision> Supervisions { get; set; }
    }
}
