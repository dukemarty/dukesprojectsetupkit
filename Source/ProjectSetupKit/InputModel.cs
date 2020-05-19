using System;
using System.Xml;
using ProjectSetupKit.Properties;

namespace ProjectSetupKit
{
    /// <summary>
    /// Representation for input data used in installing project template.
    /// </summary>
    class InputModel
    {
        #region Properties
        public bool IsValid { get { return m_isValid; } }

        public string DefaultLocation { get { return m_defaultLocation; } }
        public string Template { get { return m_inputDirectory; } }
        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <remarks>Tries to load target location and template (location) from config file.</remarks>
        /// <remarks>Postcondition: IsValid==true if a default location and a template input directory could be set, otherwise IsValid==false.</remarks>
        public InputModel()
        {
            m_defaultLocation = "";
            m_inputDirectory = "";

            if (LoadConfig())
            {
                m_isValid = true;
            }
            else
            {
                if (System.IO.Directory.Exists(Resources.DefaultTemplateName))
                {
                    m_inputDirectory = Resources.DefaultTemplateName;
                    m_isValid = true;
                }
                else
                {
                    m_isValid = false;
                }
            }
        }

        #region Private methods
        /// <summary>
        /// Load configuration 
        /// </summary>
        /// <returns></returns>
        private bool LoadConfig()
        {
            var res = false;

            if (System.IO.File.Exists(Resources.ConfigFilename))
            {
                ReadXmlDatafile(Resources.ConfigFilename);

                if (string.IsNullOrWhiteSpace(m_defaultLocation) || string.IsNullOrWhiteSpace(m_inputDirectory))
                {
                    res = InsertDefaultValuesIfPossible();
                }
                else
                {
                    res = true;
                }
            }

            return res;
        }

        /// <summary>
        /// Read configuration from xml data file.
        /// </summary>
        /// <param name="filename">name of the configuration file</param>
        private void ReadXmlDatafile(string filename)
        {
            string sName = "";
            var textreader = new XmlTextReader(filename);

            textreader.Read();

            Console.WriteLine(textreader.Name);
            Console.WriteLine(textreader.NodeType);

            while (textreader.Read())
            {
                switch (textreader.NodeType)
                {
                    case XmlNodeType.Element:
                        sName = textreader.Name;
                        break;
                    case XmlNodeType.Text:
                        switch (sName.ToLower())
                        {
                            case "templatedirectory":
                                m_inputDirectory = textreader.Value.Trim();
                                break;
                            case "defaultlocation":
                                m_defaultLocation = textreader.Value.Trim();
                                break;
                        }
                        break;
                }
            }
        }

        private bool InsertDefaultValuesIfPossible()
        {
            var res = true;

            if (String.IsNullOrWhiteSpace(m_defaultLocation))
            {
                var defaultLocation = Environment.ExpandEnvironmentVariables(Resources.EnvironmentExpressionForDefaultLocation);
                if (System.IO.Directory.Exists(defaultLocation))
                {
                    m_defaultLocation = defaultLocation;
                    res &= true;
                }
                else
                {
                    res &= false;
                }
            }

            if (String.IsNullOrWhiteSpace(m_inputDirectory))
            {
                if (System.IO.Directory.Exists(Resources.DefaultTemplateName))
                {
                    m_inputDirectory = Resources.DefaultTemplateName;
                    res &= true;
                }
                else
                {
                    res &= false;
                }
            }

            return res;
        }
        #endregion Private methods

        #region Attributes
        private bool m_isValid = false;
        private string m_defaultLocation;
        private string m_inputDirectory;
        #endregion Attributes
    }
}
