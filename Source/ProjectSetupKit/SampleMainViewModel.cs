using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Data;
using static ProjectSetupKit.InputModelSet;

namespace ProjectSetupKit
{
    internal class SampleMainViewModel
    {
        public string VersionText => $"v{Assembly.GetExecutingAssembly().GetName().Version}";


        public string ProjectName { get; set; } = "TestProject";

        public string Location { get; set; } = "TestLocation";

        public ObservableCollection<InputModel> ProjectTypes => new ObservableCollection<InputModel>(new[] {
            new InputModel
            {
                TypeName = "Foo",
                DefaultLocation = "Loc1",
                InputDirectory = "SourceLoc1",
                IconPath = "4137172 - blueprint building construction industry.png,"
            },
            new InputModel
            {
                TypeName = "Bar",
                DefaultLocation = "Loc2",
                InputDirectory = "SourceLoc2",
                IconPath = "4137172 - blueprint building construction industry.png,"
            },
            new InputModel
            {
                TypeName = "uug",
                DefaultLocation = "Loc3",
                InputDirectory = "SourceLoc3",
                IconPath = "4137172 - blueprint building construction industry.png,"
            },
        });

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
