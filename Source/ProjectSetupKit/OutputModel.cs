using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectSetupKit
{
    class OutputModel
    {
        public OutputModel()
        {
            this.name = "";
            this.location = "";
            this.fullpath = "";
        }

        public void config(string name, string location)
        {
            this.name = name;
            this.location = location;

            this.fullpath = System.IO.Path.Combine(this.location, this.name);
        }

        public bool isValidTarget()
        {
            return System.IO.Directory.Exists(this.location) && !(System.IO.Directory.Exists(this.fullpath));
        }

        public void install(InputModel input)
        {
            DirectoryCopy(input.Template, this.fullpath, true);
        }

        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(sourceDirName);
            System.IO.DirectoryInfo[] dirs = dir.GetDirectories();

            if (!dir.Exists)
            {
                throw new System.IO.DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            if (!System.IO.Directory.Exists(destDirName))
            {
                System.IO.Directory.CreateDirectory(destDirName);
            }

            System.IO.FileInfo[] files = dir.GetFiles();
            foreach (System.IO.FileInfo file in files)
            {
                string temppath = System.IO.Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            if (copySubDirs)
            {
                foreach (System.IO.DirectoryInfo subdir in dirs)
                {
                    string temppath = System.IO.Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }

        #region Attributes
        string name;
        string location;

        string fullpath;

        #endregion Attributes
    }
}
