using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace ControlLibrary
{
    public class RealtimeViewModel {

        readonly DataCollection dataSource = new DataCollection();

        public DataCollection DataSource { get { return dataSource; } }
        public virtual DateTime MinTime { get; protected set; }
        public virtual DateTime MaxTime { get; protected set; }

        public RealtimeViewModel()
        {
        }

        public void AddPoints(double v1, double v2, double v3)
        {
            DateTime argument = DateTime.Now;
            DateTime minDate = argument.AddSeconds(-120);
            IList<ProcessItem> itemsToInsert = new List<ProcessItem>();
            itemsToInsert.Add(new ProcessItem() { DateAndTime = argument, Process1 = v1, Process2 = v2, Process3 = v3 });
            dataSource.AddRange(itemsToInsert);
            dataSource.RemoveRangeAt(0, dataSource.TakeWhile(item => item.DateAndTime < minDate).Count());
            MinTime = minDate;
            MaxTime = argument;
        }

        public void AddPoints(double v1)
        {
            DateTime argument = DateTime.Now;
            DateTime minDate = argument.AddSeconds(-120);
            IList<ProcessItem> itemsToInsert = new List<ProcessItem>();
            itemsToInsert.Add(new ProcessItem() { DateAndTime = argument, Process1 = v1});
            dataSource.AddRange(itemsToInsert);
            dataSource.RemoveRangeAt(0, dataSource.TakeWhile(item => item.DateAndTime < minDate).Count());
            MinTime = minDate;
            MaxTime = argument;
        }
        /// <summary>
        /// Çå³ýµã
        /// </summary>
        public void ClearPoint()
        {
            dataSource.Clear();
        }
    }

    public struct ProcessItem {
        public DateTime DateAndTime { get; set; }
        public double Process1 { get; set; }
        public double Process2 { get; set; }
        public double Process3 { get; set; }
    }

    public class DataCollection : ObservableCollection<ProcessItem> {
        public void AddRange(IList<ProcessItem> items) {
            foreach(ProcessItem item in items)
                Items.Add(item);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, (IList)items, Items.Count - items.Count));
        }
        public void RemoveRangeAt(int startingIndex, int count) {
            var removedItems = new List<ProcessItem>(count);
            for(int i = 0; i < count; i++) {
                removedItems.Add(Items[startingIndex]);
                Items.RemoveAt(startingIndex);
            }
            if(count > 0)
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, (IList)removedItems, startingIndex));
        }
    }
}
