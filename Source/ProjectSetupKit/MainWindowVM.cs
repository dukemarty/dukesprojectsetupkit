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

    /// <summary>
    /// ViewModel for MainWindow class/form.
    /// </summary>
    class MainWindowVM : INotifyPropertyChanged
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="window">assigned window for this vm</param>
        public MainWindowVM(MainWindow window)
        {
            this.window = window;

            this.ChooseLocationCommand = new ChooseLocation(this);

            if (!this.input.IsValid)
            {
                this.window.exitWithError("Input template could not be found. Program will be aborted now!");
            }

            this.ProjectName = "";
            this.Location = this.input.DefaultLocation;
        }

        public void notifyPropertyChanged(string propName)
        {
            if (null != this.PropertyChanged)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        /// <summary>
        /// Install loaded project template to chosen location with user-given project name.
        /// </summary>
        /// <returns>true if the project could be installed, false else</returns>
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

        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion Events

        #region Attributes
        private MainWindow window;


        /// <summary>
        /// container for input data, i.e. project template and config file
        /// </summary>
        private InputModel input = new InputModel();
        /// <summary>
        /// container for output data, i.e. target location and project name
        /// </summary>
        private OutputModel output = new OutputModel();

        /// <summary>
        /// Name of the project which is instantiated from the project template.
        /// </summary>
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

        /// <summary>
        /// Attribute for target location (directory) where the project template is installed.
        /// </summary>
        private string location;

        /// <summary>
        /// Command object to be used with open file button in main window
        /// </summary>
        public ICommand ChooseLocationCommand { get; set; }
        #endregion Attributes

        #region Properties
        /// <summary>
        /// Property for target location (i.e. directory) where the project template shall be installed.
        /// </summary>
        public string Location
        {
            get { return this.location; }
            set
            {
                this.location = value;
                notifyPropertyChanged("Location");
            }
        }

        #endregion Properties
    }
}
