namespace ProjectSetupKit
{
    class OutputModel
    {
        public OutputModel()
        {
            m_name = "";
            m_location = "";
            m_fullpath = "";
        }

        public void Config(string name, string location)
        {
            m_name = name;
            m_location = location;

            m_fullpath = System.IO.Path.Combine(m_location, m_name);
        }

        public bool IsValidTarget()
        {
            return System.IO.Directory.Exists(m_location) && !(System.IO.Directory.Exists(m_fullpath));
        }

        public void Install(InputModel input)
        {
            DirectoryCopy(input.Template, m_fullpath, true);
        }

        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            var dir = new System.IO.DirectoryInfo(sourceDirName);
            var dirs = dir.GetDirectories();

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
                var temppath = System.IO.Path.Combine(destDirName, file.Name);
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
        string m_name;
        string m_location;

        string m_fullpath;

        #endregion Attributes
    }
}
