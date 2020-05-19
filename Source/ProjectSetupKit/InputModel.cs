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

        #region Attributes
        bool isValid = false;
        public bool IsValid { get { return isValid; } }

        private string defaultLocation;
        public string DefaultLocation { get { return this.defaultLocation; } }

        string inputDirectory;
        public string Template { get { return this.inputDirectory; } }
        #endregion Attributes
    }
}
