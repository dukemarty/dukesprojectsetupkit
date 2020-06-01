using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using ProjectSetupKit.Properties;

namespace ProjectSetupKit
{
    /// <summary>
    /// Representation for input data used in installing project template.
    /// </summary>
    class InputModelSet
    {
        internal class InputModel
        {
            public string TypeName { get; set; }
            public string DefaultLocation { get; set; }
            public string InputDirectory { get; set; }

            public string IconPath { get; set; }

            public InputModel() { }

            public InputModel(ProjectSetupKitConfiguration.Project project)
            {
                TypeName = project.Name;
                DefaultLocation = project.DefaultLocation;
                InputDirectory = project.TemplateDirectory;
                IconPath = project.IconPath;
            }
        }

        #region Properties

        public bool IsValid { get; private set; }

        public InputModel CurrentModel
        {
            get
            {
                if (!m_inputModels.ContainsKey(m_activeProject)) { return null; }

                return m_inputModels[m_activeProject];
            }
        }

        public string CurrentName
        {
            get { return m_activeProject; }
            set { m_activeProject = value; }
        }

        public string DefaultLocation
        {
            get
            {
                if (!m_inputModels.ContainsKey(m_activeProject)) { return ""; }

                return m_inputModels[m_activeProject].DefaultLocation;
            }
        }

        public string Template { get { return m_inputModels[m_activeProject].InputDirectory; } }

        public ObservableCollection<string> ProjectTypes
        {
            get { return new ObservableCollection<string>(m_inputModels.Keys); }
        }

        public ObservableCollection<InputModel> Projects => new ObservableCollection<InputModel>(m_inputModels.Values);
        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <remarks>Tries to load target location and template (location) from config file.</remarks>
        /// <remarks>Postcondition: IsValid==true if a default location and a template input directory could be set, otherwise IsValid==false.</remarks>
        public InputModelSet()
        {
            IsValid = LoadConfig();
        }

        #region Private methods
        /// <summary>
        /// Load configuration 
        /// </summary>
        /// <returns></returns>
        private bool LoadConfig()
        {
            if (File.Exists(Resources.ConfigFilename))
            {
                ReadXmlDatafile(Resources.ConfigFilename);
            }

            var res = m_inputModels.Any() || InsertDefaultValuesIfPossible();

            return res;
        }

        /// <summary>
        /// Read configuration from xml data file.
        /// </summary>
        /// <param name="filename">name of the configuration file</param>
        private void ReadXmlDatafile(string filename)
        {
            ProjectSetupKitConfiguration settings = null;

            try
            {
                var x = new XmlSerializer(typeof(ProjectSetupKitConfiguration));
                var reader = new StreamReader(filename);
                settings = (ProjectSetupKitConfiguration)x.Deserialize(reader);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Caught exception when trying to read xml: {0}\n{1}", ex.Message, ex.StackTrace);
            }

            if (settings == null) { return; }

            m_inputModels = settings.Projects.Where(p => Directory.Exists(p.TemplateDirectory)).ToDictionary(p => p.Name, p => new InputModel(p));
            var defaultProject = settings.Projects.FirstOrDefault(s => s.IsDefault);
            m_activeProject = defaultProject != null ? defaultProject.Name : settings.Projects.First().Name;
        }

        private bool InsertDefaultValuesIfPossible()
        {
            var defaultLocation = Environment.ExpandEnvironmentVariables(Resources.EnvironmentExpressionForDefaultLocation);
            var inputDirectory = Resources.DefaultTemplateName;

            if (!Directory.Exists(inputDirectory) || !Directory.Exists(defaultLocation)) { return false; }

            var name = Path.GetFileNameWithoutExtension(inputDirectory);
            if (String.IsNullOrWhiteSpace(name)) { return false; }

            m_inputModels[name] = new InputModel
            {
                TypeName = name,
                DefaultLocation = defaultLocation,
                InputDirectory = inputDirectory,
            };

            return true;
        }
        #endregion Private methods

        #region Attributes

        private string m_activeProject = "";
        private Dictionary<string, InputModel> m_inputModels = new Dictionary<string, InputModel>();

        #endregion Attributes
    }
}
