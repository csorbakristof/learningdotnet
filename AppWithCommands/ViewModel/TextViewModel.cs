using AppWithCommands.Model;
using System.ComponentModel;

namespace AppWithCommands.ViewModel
{
    public class TextViewModel : INotifyPropertyChanged
    {
        private TextModel model;
        public TextViewModel(TextModel model)
        {
            this.model = model;
            model.PropertyChanged += Model_PropertyChanged;
        }

        private void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Text"));
        }

        public string Text
        {
            get
            {
                return model.Text;
            }
            set
            {
                model.Text = value;
                // Note: change notification is initialized by the model (and forwarded here...)
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
