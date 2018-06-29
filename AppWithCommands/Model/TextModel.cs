using System.ComponentModel;

namespace AppWithCommands.Model
{
    public class TextModel : INotifyPropertyChanged
    {
        private string text;
        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                if (text != value)
                {
                    text = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Text"));
                }
            }
        }

        public TextModel()
        {
            text = "(d)";
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
