using CommunityToolkit.Mvvm.Collections;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Controls;
using SixRens.Api;
using SixRens.Core.插件管理.插件包管理;
using SixRens.Core.插件管理.预设管理;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SixRens.Core.插件管理.预设管理.预设;

namespace SixRens.UI.MAUI.Models
{
    public sealed class ObservablePreset : ObservableObject
    {
        public 预设 BindingPreset { get; }

        public ObservablePreset(预设 bindingPreset)
        {
            this.BindingPreset = bindingPreset;
            this.神煞插件 = new(bindingPreset.神煞插件);
            课体插件 = new(bindingPreset.神煞插件);
            参考插件 = new(bindingPreset.参考插件);
            神煞启用 = new(bindingPreset.神煞启用);
            课体启用 = new(bindingPreset.课体启用);
            神煞禁用 = new(bindingPreset.神煞禁用);
            课体禁用 = new(bindingPreset.课体禁用);
        }

        public Guid? 三传插件
        {
            get => BindingPreset.三传插件;
            set
            {
                OnPropertyChanging();
                BindingPreset.三传插件 = value;
                OnPropertyChanged();
            }
        }

        public Guid? 天将插件
        {
            get => BindingPreset.天将插件;
            set
            {
                OnPropertyChanging();
                BindingPreset.天将插件 = value;
                OnPropertyChanged();
            }
        }

        public WrappingCollection<Guid> 神煞插件 { get; }

        public WrappingCollection<Guid> 课体插件 { get; }

        public WrappingCollection<Guid> 参考插件 { get; }

        public WrappingCollection<实体题目和所属插件识别码> 神煞启用 { get; }

        public WrappingCollection<实体题目和所属插件识别码> 课体启用 { get; }

        public WrappingCollection<实体题目和所属插件识别码> 神煞禁用 { get; }

        public WrappingCollection<实体题目和所属插件识别码> 课体禁用 { get; }
    }
}
