using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace ProjectSetupKit
{
    class ChooseLocation : ICommand
    {
        public ChooseLocation(MainWindowVM model)
        {
            this.model = model;
        }


        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged { add { } remove { } }

        public void Execute(object parameter)
        {

        }

        MainWindowVM model;
    }

    class MainWindowVM
    {
        public MainWindowVM(MainWindow window)
        {
            this.window = window;

            this.ChooseLocationCommand = new ChooseLocation(this);
        }


        public bool installNewProject()
        {
            return true;
        }

        #region Attributes
        MainWindow window;

        public string Name { get; set; }
        public string Location { get; set; }

        public ICommand ChooseLocationCommand { get; set; }
        #endregion Attributes
    }
}
