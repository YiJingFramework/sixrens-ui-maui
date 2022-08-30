using com.nlf.calendar;
using SixRens.Core.壬式生成;
using SixRens.Core.年月日时;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YiJingFramework.StemsAndBranches;

namespace SixRens.UI.MAUI.Pages.CaseCreation
{
    public sealed class SelectedBirthInformationList
    {
        public BirthInformation PlateOwners { get; }
        public IReadOnlyList<BirthInformation> Others { get; }
        public SelectedBirthInformationList(
            BirthInformation plateOwners, 
            IEnumerable<BirthInformation> others)
        {
            this.PlateOwners = plateOwners;
            this.Others = others.ToArray();
        }
    }
}
