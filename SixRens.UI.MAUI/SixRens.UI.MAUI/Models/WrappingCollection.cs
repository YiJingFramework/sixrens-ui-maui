using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SixRens.UI.MAUI.Models
{
    public sealed class WrappingCollection<T> : ObservableCollection<T>
    {
        public IList<T> WrappedList { get; }
        public WrappingCollection(IList<T> wrappedList) : base(wrappedList)
        {
            this.CollectionChanged += this.OnCollectionChanged;
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                {
                    WrappedList.Insert(e.NewStartingIndex, this[e.NewStartingIndex]);
                    break;
                }
                case NotifyCollectionChangedAction.Move:
                {
                    var item = WrappedList[e.OldStartingIndex];
                    WrappedList.RemoveAt(e.OldStartingIndex);
                    WrappedList.Insert(e.NewStartingIndex, item);
                    break;
                }
                case NotifyCollectionChangedAction.Remove:
                {
                    WrappedList.RemoveAt(e.OldStartingIndex);
                    break;
                }
                case NotifyCollectionChangedAction.Replace:
                {
                    WrappedList[e.NewStartingIndex] = this[e.NewStartingIndex];
                    break;
                }
                case NotifyCollectionChangedAction.Reset:
                {
                    WrappedList.Clear();
                    break;
                }
                default:
                    throw new NotImplementedException("这应该不会发生");
            }
        }
    }
}
