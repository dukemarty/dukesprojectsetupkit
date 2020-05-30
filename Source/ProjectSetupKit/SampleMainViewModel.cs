using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ProjectSetupKit
{
    internal class SampleMainViewModel
    {
        public string VersionText => $"v{Assembly.GetExecutingAssembly().GetName().Version}";


        public string ProjectName { get; set; } = "TestProject";

        public string Location { get; set; } = "TestLocation";

        //public CollectionView ProjectTypes => new CollectionView(m_input.ProjectTypes);

        //public string ActiveType
        //{
        //    get { return m_input.CurrentName; }
        //    set
        //    {
        //        m_input.CurrentName = value;
        //        Location = m_input.DefaultLocation;
        //    }
        //}
    }

    internal class SampleSettings
    {
        public string InputBackgroundColor { get; set; } = "#96212626";
        public string WindowBackgroundColor { get; set; } = "#CC131616";
        public string TextColor { get; set; } = "#FF00C8C8";
        public string SymbolBackground { get; set; } = "#CC80A33A";
    }

    internal class SampleDataContext
    {
        public SampleMainViewModel vm { get; set; } = new SampleMainViewModel();

        public SampleSettings settings { get; set; } = new SampleSettings();
    }
}
