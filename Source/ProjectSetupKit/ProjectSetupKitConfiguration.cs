using System.Collections.Generic;
using System.Xml.Serialization;

namespace ProjectSetupKit
{
    public class ProjectSetupKitConfiguration
    {
        public class Project
        {
            private string m_name;
            private string m_templateDirectory;
            private string m_defaultLocation;

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

            [XmlAttribute("IsDefault")]
            public bool IsDefault { get; set; }
        }

        [XmlElement("Project")]
        public List<Project> Projects { get; set; }
    }
}
