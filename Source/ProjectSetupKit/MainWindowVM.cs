using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.ComponentModel;

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

    class MainWindowVM : INotifyPropertyChanged
    {
        public MainWindowVM(MainWindow window)
        {
            this.window = window;

            this.ChooseLocationCommand = new ChooseLocation(this);

            if (!this.input.IsValid)
            {
                this.window.exitWithError("Input template could not be found. Program will be aborted now!");
            }

            this.ProjectName = "";
            this.Location = this.input.getDefaultLocation();
        }

        public void notifyPropertyChanged(string propName)
        {
            if (null != this.PropertyChanged)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        public bool installNewProject()
        {
            this.output.config(ProjectName, Location);

            if (this.output.isValidTarget())
            {
                this.output.install(this.input);
                return true;
            }
            else
            {
                return false;
            }
        }

        #region Attributes
        MainWindow window;

        public event PropertyChangedEventHandler PropertyChanged;

        InputModel input = new InputModel();
        OutputModel output = new OutputModel();

        private string projectName;
        public string ProjectName 
        {
            get { return this.projectName; }
            set
            {
                this.projectName = value;
                notifyPropertyChanged("ProjectName");
            }
        }
        private string location;
        public string Location 
        {
            get { return this.location; }
            set
            {
                this.location = value;
                notifyPropertyChanged("Location");
            }
        }

        public ICommand ChooseLocationCommand { get; set; }
        #endregion Attributes
    }
}
