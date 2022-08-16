using SixRens.UI.MAUI.Services.ExceptionHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SixRens.UI.MAUI.Services.SixRens
{
    public sealed class SixRensCore
    {
        readonly ExceptionHandler exceptionHandler;
        public SixRensCore(ExceptionHandler exceptionHandler)
        {
            this.exceptionHandler = exceptionHandler;
        }
    }
}
