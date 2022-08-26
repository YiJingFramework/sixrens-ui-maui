using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SixRens.UI.MAUI.Pages.CaseCreation
{
    internal sealed record ShowingDateTime(SelectedDateTime DateTime)
    {
        public string DisplayString
        {
            get
            {
                StringBuilder stringBuilder = new();
                _ = stringBuilder.AppendLine(
                    $"{DateTime.DateTimeInformation.年干:C}{DateTime.DateTimeInformation.年支:C}年 " +
                    $"{DateTime.DateTimeInformation.月干:C}{DateTime.DateTimeInformation.月支:C}月 " +
                    $"{DateTime.DateTimeInformation.日干:C}{DateTime.DateTimeInformation.日支:C}日 " +
                    $"{DateTime.DateTimeInformation.时干:C}{DateTime.DateTimeInformation.时支:C}时");
                if (DateTime.WesternDateTime.HasValue)
                    _ = stringBuilder.Append($"西历：{DateTime.WesternDateTime.Value:yyyy/MM/dd HH:mm}");
                else
                    _ = stringBuilder.Append($"西历：未指定");
                return stringBuilder.ToString();
            }
        }
    }
}
