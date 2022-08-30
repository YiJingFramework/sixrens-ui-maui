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
    public sealed class BirthInformation
    {
        public 性别 Gender { get; }
        public EarthlyBranch Birth { get; }
        public EarthlyBranch? Age { get; }
        public BirthInformation(性别 gender, EarthlyBranch birth, EarthlyBranch? age)
        {
            this.Gender = gender;
            this.Birth = birth;
            this.Age = age;
        }

        public override int GetHashCode()
        {
            // 即使所有字段相同，也不可以视为一个对象，否则会导致删除功能出现问题。
            return base.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            // 即使所有字段相同，也不可以视为一个对象，否则会导致删除功能出现问题。
            return base.Equals(obj);
        }

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
