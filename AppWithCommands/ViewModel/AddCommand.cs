using AppWithCommands.Model;
using System;
using System.Windows.Input;

namespace AppWithCommands.ViewModel
{
    class AddCommand : ICommand
    {
#pragma warning disable 0067
        public event EventHandler CanExecuteChanged;
#pragma warning restore 0067

        // Model the command is working on...
        private TextModel TextModel { get; set; }

        public AddCommand(TextModel model)
        {
            this.TextModel = model;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            TextModel.Text = $"{TextModel.Text}(add)";
        }
    }
}
