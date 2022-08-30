using SixRens.Api.实体;
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
    public sealed record BirthInformation(性别 Gender, EarthlyBranch Birth, EarthlyBranch? Age)
    {
        public string DisplayString
        {
            get
            {
                var result = $"{Birth:C}命{Gender}";
                if (Age.HasValue)
                    result = $"{Age.Value:C}年{result}";
                return result;
            }
        }

        public 年命 ToCoreInformation(I年月日时信息 time)
        {
            if (this.Age.HasValue)
                return new(this.Gender, this.Birth, this.Age.Value);
            return new(this.Gender, this.Birth, time);
        }
    }
}
