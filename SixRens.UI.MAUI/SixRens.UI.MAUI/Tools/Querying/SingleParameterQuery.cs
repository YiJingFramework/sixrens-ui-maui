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
    public class SingleParameterQuery<T> : IDictionary<string, object>
    {
        public string Key { get; }
        public T Value { get; }

        public SingleParameterQuery(T value, string key = "p")
        {
            this.Key = key;
            this.Value = value;
        }

        ICollection<string> IDictionary<string, object>.Keys => new string[] { Key };

        ICollection<object> IDictionary<string, object>.Values => new object[] { Value };

        int ICollection<KeyValuePair<string, object>>.Count => 1;

        bool ICollection<KeyValuePair<string, object>>.IsReadOnly => true;

        object IDictionary<string, object>.this[string key] 
        {
            get
            {
                if (key == this.Key)
                    return Value;
                throw new KeyNotFoundException($"找不到名为 {key} 的键。");
            }
            set => throw new NotSupportedException("此字典只读。"); 
        }

        void IDictionary<string, object>.Add(string key, object value)
        {
            throw new NotSupportedException("此字典只读。");
        }

        bool IDictionary<string, object>.ContainsKey(string key)
        {
            return key == this.Key;
        }

        bool IDictionary<string, object>.Remove(string key)
        {
            throw new NotSupportedException("此字典只读。");
        }

        bool IDictionary<string, object>.TryGetValue(string key, out object value)
        {
            if (key == this.Key)
            {
                value = Value;
                return true;
            }
            value = default;
            return false;
        }

        void ICollection<KeyValuePair<string, object>>.Add(KeyValuePair<string, object> item)
        {
            throw new NotSupportedException("此字典只读。");
        }

        void ICollection<KeyValuePair<string, object>>.Clear()
        {
            throw new NotSupportedException("此字典只读。");
        }

        bool ICollection<KeyValuePair<string, object>>.Contains(KeyValuePair<string, object> item)
        {
            return item.Key == this.Key && item.Value.Equals(this.Value);
        }

        void ICollection<KeyValuePair<string, object>>.CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            new KeyValuePair<string, object>[] {
                new(this.Key, this.Value)
            }.CopyTo(array, arrayIndex);
        }

        bool ICollection<KeyValuePair<string, object>>.Remove(KeyValuePair<string, object> item)
        {
            throw new NotSupportedException("此字典只读。");
        }

        IEnumerator<KeyValuePair<string, object>> IEnumerable<KeyValuePair<string, object>>.GetEnumerator()
        {
            return new List<KeyValuePair<string, object>>(1) {
                new(this.Key, this.Value)
            }.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new KeyValuePair<string, object>[] {
                new(this.Key, this.Value)
            }.GetEnumerator();
        }
    }
}
