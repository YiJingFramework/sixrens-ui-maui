using com.nlf.calendar;
using SixRens.Core.年月日时;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YiJingFramework.StemsAndBranches;

namespace SixRens.UI.MAUI.Pages.CaseCreation
{
    public sealed class SelectedDateTime
    {
        public DateTime? WesternDateTime { get; }
        public bool ProvidesTheSun { get; }
        public I年月日时信息 DateTimeInformation { get; }
        public SelectedDateTime(DateTime western)
        {
            this.WesternDateTime = western;
            ProvidesTheSun = true;
            DateTimeInformation = new 真实年月日时(WesternDateTime.Value);
        }
        public SelectedDateTime(I年月日时信息 information, bool providesTheSun = false)
        {
            this.WesternDateTime = null;
            ProvidesTheSun = providesTheSun;
            DateTimeInformation = information.修改信息();
        }
    }
}
