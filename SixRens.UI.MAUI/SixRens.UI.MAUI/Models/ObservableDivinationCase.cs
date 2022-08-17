using CommunityToolkit.Mvvm.ComponentModel;
using SixRens.Core.占例存取;
using SixRens.Core.壬式生成;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SixRens.UI.MAUI.Models
{
    public sealed partial class ObservableDivinationCase : ObservableObject
    {
        public ObservableDivinationCase(占例 dCase, bool makeCopy = false)
        {
            this.BindingCase = makeCopy ? 占例.反序列化(dCase.序列化()) : dCase;
        }

        public 占例 BindingCase { get; }

        public string Text
        {
            get => BindingCase.断语;
            set
            {
                this.OnPropertyChanging();
                BindingCase.断语 = value;
                this.OnPropertyChanged();
            }
        }

        public DateTime? Time => BindingCase.西历时间;

        public 壬式 Plate => BindingCase.壬式;
    }
}
