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
        private class InputModel
        {
            public string TypeName { get; set; }
            public string DefaultLocation { get; set; }
            public string InputDirectory { get; set; }
        }

        #region Properties

        public bool IsValid { get; private set; }

        public string CurrentName
        {
            get { return m_activeProject; }
            set { m_activeProject = value; }
        }

        public string DefaultLocation
        {
            get
            {
                if (m_inputModels.ContainsKey(m_activeProject))
                {
                    return m_inputModels[m_activeProject].DefaultLocation;
                }
                else
                {
                    return "";
                }
            }
        }

        public string Template { get { return m_inputModels[m_activeProject].InputDirectory; } }

        public ObservableCollection<string> ProjectTypes
        {
            get{return new ObservableCollection<string>(m_inputModels.Keys);}
        } 
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
                Console.WriteLine("Catched exception when trying to read xml.");
            }

            ProjectSetupKitConfiguration.Project defaultProject = null;
            if (settings != null)
            {
                foreach (var s in settings.Projects.Where(s => Directory.Exists(s.TemplateDirectory.Trim())))
                {
                    m_inputModels[s.Name.Trim()] = new InputModel
                    {
                        TypeName = s.Name.Trim(),
                        DefaultLocation = s.DefaultLocation.Trim(),
                        InputDirectory = s.TemplateDirectory.Trim(),
                    };
                }

                defaultProject = settings.Projects.FirstOrDefault(s => s.IsDefault);
            }

            m_activeProject = defaultProject != null ? defaultProject.Name.Trim() : settings.Projects.First().Name.Trim();
        }

        private bool InsertDefaultValuesIfPossible()
        {
            var res = false;

            var defaultLocation = Environment.ExpandEnvironmentVariables(Resources.EnvironmentExpressionForDefaultLocation);
            var inputDirectory = Resources.DefaultTemplateName;

            if (Directory.Exists(inputDirectory) && Directory.Exists(defaultLocation))
            {
                var name = Path.GetFileNameWithoutExtension(inputDirectory);

                m_inputModels[name] = new InputModel
                {
                    TypeName = name,
                    DefaultLocation = defaultLocation,
                    InputDirectory = inputDirectory,
                };

                res = true;
            }

            return res;
        }
        #endregion Private methods

        #region Attributes

        private string m_activeProject = "";
        private Dictionary<string, InputModel> m_inputModels = new Dictionary<string, InputModel>();

        #endregion Attributes
    }
}
