using ExcelReaderStandardLibrary;
using PeerReviewCommonLib;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Threading.Tasks;
using System.Linq;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace PeerReviewDistributionHelper
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public ObservableCollection<Review> Reviews { get; set; }
        public ObservableCollection<Supervision> Supervisions { get; set; }

        public MainPage()
        {
            Reviews = new ObservableCollection<Review>();
            Supervisions = new ObservableCollection<Supervision>();
            this.InitializeComponent();
        }

        #region Loading supervisions
        private async void LoadSupervisionButton_Click(object sender, RoutedEventArgs e)
        {
            AddSupervisions(await ChooseFileAndLoadIntoDictionaryList(1,1));
        }

        private void AddSupervisions(List<Dictionary<string, string>> dictList)
        {
            foreach (var d in dictList)
            {
                Supervisions.Add(new Supervision()
                {
                    AdvisorName = d["Konzulens"],
                    StudentName = d["Hallgató neve"],
                    StudentNeptunCode = d["Hallg. nept"],
                    StudentEmail = null
                });
            }
        }
        #endregion

        #region Loading reviews
        private async void LoadReviewsButton_Click(object sender, RoutedEventArgs e)
        {
            AddReviews(await ChooseFileAndLoadIntoDictionaryList(0,1));
        }

        private void AddReviews(List<Dictionary<string, string>> dictList)
        {
            foreach (var d in dictList)
            {
                Reviews.Add(new Review()
                {
                    PresenterEmail = d["PresenterEmail"],
                    ReviewerNeptunCode = d["ReviewerNKod"],
                    OverallScore = int.Parse(d["Score"]),
                    Text = d["Text"]
                });
            }
        }
        #endregion

        #region Xls picking and loading helpers
        private async Task<List<Dictionary<string,string>>> ChooseFileAndLoadIntoDictionaryList(int worksheetIndex, int headerRowIndex)
        {
            var file = await PickFileToOpen();
            return LoadXlsIntoDictionaryList(file, worksheetIndex, headerRowIndex);
        }

        private async Task<StorageFile> PickFileToOpen()
        {
            var fop = new Windows.Storage.Pickers.FileOpenPicker();
            fop.FileTypeFilter.Add("*");
            return await fop.PickSingleFileAsync();
        }

        private List<Dictionary<string, string>> LoadXlsIntoDictionaryList(StorageFile file, int worksheetIndex, int headerRowIndex)
        {
            var stream = file.OpenStreamForReadAsync().Result;
            var reader = new Excel2Dict();
            return reader.Read(stream, worksheetIndex, headerRowIndex);
        }
        #endregion

        private async void CreateStudentEmailsButton_Click(object sender, RoutedEventArgs e)
        {
            var allPresenterEmails = Reviews.Select(r => r.PresenterEmail).Distinct();
            var supervisionLookup = new SupervisionLookupBase();
            foreach (var s in Supervisions)
                supervisionLookup.AddIfNew(s.StudentNeptunCode, s);
            var allReviews = Reviews.ToList();
            foreach (var presenterEmail in allPresenterEmails)
            {
                var email = Review.GetCollectedPresenterEmail(presenterEmail, allReviews, supervisionLookup);
                await Windows.ApplicationModel.Email.EmailManager.ShowComposeNewEmailAsync(email);
            }
        }

        private void CreateAdvisorEmailsButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
