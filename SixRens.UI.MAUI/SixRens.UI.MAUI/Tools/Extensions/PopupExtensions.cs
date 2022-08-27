using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SixRens.UI.MAUI.Tools.Extensions
{
    public static class PopupExtensions
    {
        public static void ShowPopup<TCurrent, TNew>(this TCurrent currentPopup, TNew newPopup) 
            where TCurrent : Popup where TNew : Popup
        {
            CreatePopup(currentPopup, newPopup);
        }

        public static Task<object> ShowPopupAsync<TCurrent, TNew>(
            this TCurrent currentPopup, TNew newPopup)
            where TCurrent : Popup where TNew : Popup
        {
            CreatePopup(currentPopup, newPopup);
            return newPopup.Result;
        }

        private static void CreatePopup<TCurrent, TNew>(TCurrent currentPopup, TNew newPopup)
            where TCurrent : Popup where TNew : Popup
        {
            IMauiContext mauiContext = GetMauiContext(currentPopup);
            newPopup.Parent = currentPopup;
            newPopup.ToHandler(mauiContext).Invoke("OnOpened");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static IMauiContext GetMauiContext<TPopup>(TPopup popup)
            where TPopup : Popup
        {
            return popup.Handler?.MauiContext 
                ?? throw new InvalidOperationException("Could not locate MauiContext");
        }
    }
}
