using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace ProjectSetupKit
{
    class InputModel
    {
        const string CONFIGFILENAME = "pskconfig.xml";
        const string DEFAULTTEMPLATE = "ProjectSetupKitTemplate";
        string DEFAULTDEFAULTLOCATION = Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%");

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
                if (System.IO.Directory.Exists(DEFAULTTEMPLATE))
                {
                    this.inputDirectory = DEFAULTTEMPLATE;
                    this.isValid = true;
                }
                else
                {
                    this.isValid = false;
                }
            }
        }

        public string getDefaultLocation()
        {
            return this.defaultLocation;
        }

        private bool loadConfig()
        {
            bool res = false;

            if (System.IO.File.Exists(CONFIGFILENAME))
            {
                readXmlDatafile(CONFIGFILENAME);

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
                if (System.IO.Directory.Exists(DEFAULTDEFAULTLOCATION))
                {
                    this.defaultLocation = DEFAULTDEFAULTLOCATION;
                    res &= true;
                }
                else
                {
                    res &= false;
                }
            }

            if (String.IsNullOrWhiteSpace(this.inputDirectory))
            {
                if (System.IO.Directory.Exists(DEFAULTTEMPLATE))
                {
                    this.inputDirectory = DEFAULTTEMPLATE;
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

        string defaultLocation;
        string inputDirectory;
        public string Template { get { return this.inputDirectory; } }
        #endregion Attributes
    }
}
