using SixRens.Core.占例存取;
using SixRens.Core.壬式生成;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SixRens.UI.MAUI.Pages.Main
{
    internal sealed class BindingCase
    {
        public BindingCase(string caseName, 占例 dCase)
        {
            this.Name = caseName;
            this.Plate = dCase.壬式;
            this.DateTime = dCase.西历时间;
            this.Text = dCase.断语;
        }

        public string Name { get; set; }
        public 壬式 Plate { get; }
        public DateTime? DateTime { get; }
        public string Text { get; set; }
    }
}
