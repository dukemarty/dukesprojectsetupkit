using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using ProjectSetupKit.Properties;

namespace ProjectSetupKit
{
    /// <summary>
    /// Representation for input data used in installing project template.
    /// </summary>
    class InputModel
    {
        #region Attributes
        private bool isValid = false;
        private string defaultLocation;
        private string inputDirectory;
        #endregion Attributes

        #region Properties
        public bool IsValid { get { return isValid; } }
        public string DefaultLocation { get { return this.defaultLocation; } }
        public string Template { get { return this.inputDirectory; } }
        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <remarks>Tries to load target location and template (location) from config file.</remarks>
        /// <remarks>Postcondition: IsValid==true if a default location and a template input directory could be set, otherwise IsValid==false.</remarks>
        public InputModel()
        {
            this.defaultLocation = "";
            this.inputDirectory = "";

            if (loadConfig())
            {
                this.isValid = true;
            }
            else
            {
                if (System.IO.Directory.Exists(Resources.DefaultTemplateName))
                {
                    this.inputDirectory = Resources.DefaultTemplateName;
                    this.isValid = true;
                }
                else
                {
                    this.isValid = false;
                }
            }
        }

        #region Private methods
        /// <summary>
        /// Load configuration 
        /// </summary>
        /// <returns></returns>
        private bool loadConfig()
        {
            bool res = false;

            if (System.IO.File.Exists(Resources.ConfigFilename))
            {
                readXmlDatafile(Resources.ConfigFilename);

                if (string.IsNullOrWhiteSpace(this.defaultLocation) || string.IsNullOrWhiteSpace(this.inputDirectory))
                {
                    res = insertDefaultValuesIfPossible();
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
        private void readXmlDatafile(string filename)
        {
            string sName = "";
            XmlTextReader textreader = new XmlTextReader(filename);

            textreader.Read();

            System.Console.WriteLine(textreader.Name);
            System.Console.WriteLine(textreader.NodeType);

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
                                this.inputDirectory = textreader.Value.Trim();
                                break;
                            case "defaultlocation":
                                this.defaultLocation = textreader.Value.Trim();
                                break;
                        }
                        break;
                }
            }
        }

        private bool insertDefaultValuesIfPossible()
        {
            bool res = true;

            if (String.IsNullOrWhiteSpace(this.defaultLocation))
            {
                string defaultLocation = Environment.ExpandEnvironmentVariables(Resources.EnvironmentExpressionForDefaultLocation);
                if (System.IO.Directory.Exists(defaultLocation))
                {
                    this.defaultLocation = defaultLocation;
                    res &= true;
                }
                else
                {
                    res &= false;
                }
            }

            if (String.IsNullOrWhiteSpace(this.inputDirectory))
            {
                if (System.IO.Directory.Exists(Resources.DefaultTemplateName))
                {
                    this.inputDirectory = Resources.DefaultTemplateName;
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

    }
}
