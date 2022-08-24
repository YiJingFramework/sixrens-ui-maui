using SixRens.UI.MAUI.Tools.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SixRens.UI.MAUI.Tools.Querying
{
    public interface IQueryAttributable<T> : IQueryAttributable
    {
        void ApplyQueryParameter(T parameter);
        string QueryKey => "p";
        T DefaultQueryParameter => default(T);

        void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query)
        {
            ApplyQueryParameter((T)query.GetValueOrDefault(QueryKey, DefaultQueryParameter));
        }
    }
}
