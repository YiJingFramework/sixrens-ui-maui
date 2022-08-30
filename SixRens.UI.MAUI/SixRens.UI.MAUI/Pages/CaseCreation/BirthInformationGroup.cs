using com.nlf.calendar;
using SixRens.Core.壬式生成;
using SixRens.Core.年月日时;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YiJingFramework.StemsAndBranches;
using static SixRens.UI.MAUI.Pages.CaseCreation.BirthInformationGroup;

namespace SixRens.UI.MAUI.Pages.CaseCreation
{
    public sealed class BirthInformationGroup : ObservableCollection<BirthInformation>
    {
        public string GroupName { get; }

        public BirthInformationGroup(string groupName)
        {
            this.GroupName = groupName;
        }
    }
}
