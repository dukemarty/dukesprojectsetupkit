using System.Collections.Generic;
using System.Xml.Serialization;

namespace ProjectSetupKit
{
    public class ProjectSetupKitConfiguration
    {
        public class Project
        {
            public string Name { get; set; }
            public string TemplateDirectory { get; set; }
            public string DefaultLocation { get; set; }

            [XmlAttribute("IsDefault")]
            public bool IsDefault { get; set; }
        }

        [XmlElement("Project")]
        public List<Project> Projects { get; set; }
    }
}
