using Microsoft.Extensions.Primitives;
using SixRens.Core.壬式生成;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SixRens.UI.MAUI.Pages.CaseCreation
{
    internal sealed record ShowingBirthInformationList(SelectedBirthInformationList Information)
    {
        public string DisplayString
        {
            get
            {
                var count = Information.Others.Count;
                if (count is 0)
                    return "未指定年命";


                IEnumerable<BirthInformation> items;
                if (Information.PlateOwners is not null)
                {
                    count++;
                    items = Information.Others.Prepend(Information.PlateOwners);
                }
                else
                {
                    items = Information.Others;
                }

                var strings = items.Take(3).Select(item => $"{item.Birth:C}命{item.Gender}");
                var result = string.Join(' ', strings);

                return count > 3 ? $"{result} 等" : result;
            }
        }
    }
}
