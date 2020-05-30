using System;
using System.Reflection;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Input;
using System.ComponentModel;

namespace ProjectSetupKit
{
    class ChooseLocation : ICommand
    {
        public ChooseLocation(MainWindowViewModel model)
        {
            m_model = model;
        }


        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged { add { } remove { } }

        public void Execute(object parameter)
        {
            var dialog = new FolderBrowserDialog();

            var result = dialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                m_model.Location = dialog.SelectedPath;
            }

        }

        private readonly MainWindowViewModel m_model;
    }

    /// <summary>
    /// ViewModel for MainWindow class/form.
    /// </summary>
    class MainWindowViewModel : INotifyPropertyChanged
    {
        #region Properties

        public string VersionText => $"v{Assembly.GetExecutingAssembly().GetName().Version}";


        public string ProjectName
        {
            get { return m_projectName; }
            set
            {
                m_projectName = value;
                NotifyPropertyChanged("ProjectName");
            }
        }

        /// <summary>
        /// Property for target location (i.e. directory) where the project template shall be installed.
        /// </summary>
        public string Location
        {
            get { return m_location; }
            set
            {
                m_location = value;
                NotifyPropertyChanged("Location");
            }
        }

        public CollectionView ProjectTypes => new CollectionView(m_input.ProjectTypes);

        public string ActiveType
        {
            get { return m_input.CurrentName; }
            set
            {
                m_input.CurrentName = value;
                Location = m_input.DefaultLocation;
            }
        }
        #endregion Properties

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="window">assigned m_window for this vm</param>
        public MainWindowViewModel(MainWindow window)
        {
            ChooseLocationCommand = new ChooseLocation(this);

            if (!m_input.IsValid)
            {
                window.ExitWithError("Input template could not be found. Program will be aborted now!");
            }

            ProjectName = "";
            Location = m_input.DefaultLocation;
        }

        public void NotifyPropertyChanged(string propName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));

        /// <summary>
        /// Install loaded project template to chosen location with user-given project name.
        /// </summary>
        /// <returns>true if the project could be installed, false else</returns>
        public bool InstallNewProject()
        {
            m_output.Config(ProjectName, Location);

            var res = false;
            if (m_output.IsValidTarget())
            {
                m_output.Install(m_input);
                res = true;
            }

            return res;
        }

        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion Events

        #region Attributes

        /// <summary>
        /// container for input data, i.e. project template and config file
        /// </summary>
        private readonly InputModelSet m_input = new InputModelSet();
        /// <summary>
        /// container for output data, i.e. target location and project name
        /// </summary>
        private readonly OutputModel m_output = new OutputModel();

        /// <summary>
        /// Name of the project which is instantiated from the project template.
        /// </summary>
        private string m_projectName;

        /// <summary>
        /// Attribute for target location (directory) where the project template is installed.
        /// </summary>
        private string m_location;

        /// <summary>
        /// Command object to be used with open file button in main m_window
        /// </summary>
        public ICommand ChooseLocationCommand { get; set; }
        #endregion Attributes
    }
}
