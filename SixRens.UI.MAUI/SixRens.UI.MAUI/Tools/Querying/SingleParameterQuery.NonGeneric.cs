using Java.Util;
using SixRens.UI.MAUI.Tools.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SixRens.UI.MAUI.Tools.Querying
{
    public class SingleParameterQuery : SingleParameterQuery<object>
    {
        public SingleParameterQuery(object value, string key = "p")
            : base(value, key)
        { }

        public static SingleParameterQuery Create(object value, string key = "p")
        {
            return new(value, key);
        }
        public static SingleParameterQuery<T> Create<T>(T value, string key = "p")
        {
            return new(value, key);
        }
    }
}
