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
    public sealed class MappedWrappingCollection<TIn, TOut> : ObservableCollection<TOut>
    {
        public interface IMapper
        {
            public TOut MapTo(TIn obj);
            public TIn MapBack(TOut obj);
        }
        public sealed class MapperWithFunctions : IMapper
        {
            public Func<TIn, TOut> MapToFunction { get; }
            public Func<TOut, TIn> MapBackFunction { get; }
            public MapperWithFunctions(Func<TIn, TOut> mapTo, Func<TOut, TIn> mapBack)
            {
                MapToFunction = mapTo;
                MapBackFunction = mapBack;
            }

            public TOut MapTo(TIn obj)
            {
                return MapToFunction(obj);
            }
            public TIn MapBack(TOut obj)
            {
                return MapBackFunction(obj);
            }
        }

        public IMapper Mapper { get; }
        public IList<TIn> WrappedList { get; }
        public MappedWrappingCollection(IList<TIn> wrappedList, IMapper mapper)
            : base(wrappedList.Select(i => mapper.MapTo(i)))
        {
            this.CollectionChanged += this.OnCollectionChanged;
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                {
                    WrappedList.Insert(e.NewStartingIndex,
                        this.Mapper.MapBack(this[e.NewStartingIndex]));
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
                    WrappedList[e.NewStartingIndex] = 
                        this.Mapper.MapBack(this[e.NewStartingIndex]);
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
