namespace ProjectSetupKit
{
    class OutputModel
    {
        #region Public interface
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

        public void Install(InputModelSet input)
        {
            FileDirHandling.DirectoryCopy(input.Template, m_fullpath, true);
        }
        #endregion Public interface

        #region Attributes
        private string m_name = "";
        private string m_location = "";

        private string m_fullpath = "";

        #endregion Attributes
    }
}
