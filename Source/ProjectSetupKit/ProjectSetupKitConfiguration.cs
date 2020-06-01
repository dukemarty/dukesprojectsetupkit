using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace ProjectSetupKit
{
    public class ProjectSetupKitConfiguration
    {
        #region Constants

        private static readonly ProjectSetupKitConfiguration DefaultSettings = new ProjectSetupKitConfiguration
        {
            Projects = new List<Project>
            {
                new Project
                {
                    Name ="Local project",
                    TemplateDirectory=@"ProjectSetupKitTemplates\LocalProject",
                    IconPath = null,
                    IsDefault = true,
                },
                new Project
                {
                    Name ="IBN",
                    TemplateDirectory=@"ProjectSetupKitTemplates\IBN-Notizen",
                    IconPath = "ProjectSetupTe",
                    IsDefault = false,
                }
            },
        };

        #endregion Constants

        #region Properties
        public class Project
        {
            private string m_name;
            private string m_templateDirectory;
            private string m_defaultLocation;
            private string m_iconPath;

            public string Name
            {
                get { return m_name.Trim(); }
                set { m_name = value; }
            }

            public string TemplateDirectory
            {
                get { return m_templateDirectory.Trim(); }
                set { m_templateDirectory = value; }
            }

            public string DefaultLocation
            {
                get { return m_defaultLocation.Trim(); }
                set { m_defaultLocation = value; }
            }

            public string IconPath
            {
                get { return m_iconPath?.Trim(); }
                set { m_iconPath = value; }
            }

            [XmlAttribute("IsDefault")]
            public bool IsDefault { get; set; }

            public Project() { }

            public Project(Project orig)
            {
                m_name = orig.m_name;
                m_templateDirectory = orig.m_templateDirectory;
                m_defaultLocation = orig.m_defaultLocation;
                m_iconPath = orig.m_iconPath;
            }
        }

        [XmlElement("Project")]
        public List<Project> Projects { get; set; }
        #endregion Properties

        #region Public interface

        public ProjectSetupKitConfiguration() { }

        public ProjectSetupKitConfiguration(ProjectSetupKitConfiguration orig)
        {
            Projects = orig.Projects.Select(p => new Project(p)).ToList();
        }

        public void StoreToFile(string filename)
        {
            try
            {
                var x = new XmlSerializer(typeof(ProjectSetupKitConfiguration));
                var writer = new StreamWriter(filename);
                x.Serialize(writer, this);
            }
            catch (Exception ex)
            {
                //Logger.Error(ex, $"Caught exception when trying to write xml: {ex.Message}");
            }
        }

        public static ProjectSetupKitConfiguration LoadOrDefaultSettings(string filename)
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
                //Logger.Error(ex, $"Caught exception when trying to read xml: {ex.Message}");
            }

            if (settings == null)
            {
                settings = new ProjectSetupKitConfiguration(DefaultSettings);
                settings.StoreToFile(filename);
            }

            return settings;
        }
        #endregion Public interface

    }
}
