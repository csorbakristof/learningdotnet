using AppWithCommands.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Foundation;

namespace AppWithCommands.ViewModel
{
    public class AddPositionCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private TextModel TextModel { get; set; }

        public AddPositionCommand(TextModel model)
        {
            this.TextModel = model;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (parameter is Point)
            {
                var p = (Point)parameter;
                TextModel.Text = $"{TextModel.Text}({p.X};{p.Y})";
            }
        }
    }
}
