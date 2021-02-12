using HappyWaterCarrierTestApp.Model;
using System.Collections.ObjectModel;

namespace HappyWaterCarrierTestApp.Utils.Pagination
{
    class ScrollPaginationHelper<T> : PaginationHelper<T>
        where T : Employee
    {
        public ObservableCollection<T> VisibleCollection { get; set; }
        private PaginationBuffer<T> nextBuffer;
        private PaginationBuffer<T> prevBuffer;
        private int Position { get; set; }
        private int DefaultVisibleCount { get; set; }
        public ScrollPaginationHelper(ref ObservableCollection<T> visibleCollection, int visibleCollectionSize = 20, int bufferSize = 20)
        {
            VisibleCollection = visibleCollection;
            DefaultVisibleCount = visibleCollectionSize;
            Position = 0;
            prevBuffer = new PaginationBufferPrev<T>(() => Position, bufferSize);
            nextBuffer = new PaginationBufferNext<T>(() => Position + VisibleCollection.Count, bufferSize);
        }
        public void MoveDown(int steps)
        {
            for(int idx = 0; idx < steps; ++idx)
            {
                if (nextBuffer.IsEmpty())
                    return;
                prevBuffer.PutItem(VisibleCollection[0]);
                VisibleCollection.RemoveAt(0);
                VisibleCollection.Add(nextBuffer.GetItem());
                Position++;
            }
        }

        public void MoveUp(int steps)
        {
            for (int idx = 0; idx < steps; ++idx)
            {
                if (prevBuffer.IsEmpty())
                    return;
                nextBuffer.PutItem(VisibleCollection[VisibleCollection.Count - 1]);
                VisibleCollection.RemoveAt(VisibleCollection.Count - 1);
                VisibleCollection.Insert(0, prevBuffer.GetItem());
                Position--;
            }
        }

        public void Remove(T obj)
        {
            if (VisibleCollection.Contains(obj))
            {
                VisibleCollection.Remove(obj);
                T nextObj = nextBuffer.GetItem();
                if (nextObj != null)
                    VisibleCollection.Add(nextObj);
            }
        }

        public void Put(T e)
        {
            if (DefaultVisibleCount > VisibleCollection.Count)
            {
                VisibleCollection.Add(e);
            }
            else if (nextBuffer.IsEmpty()) //Temporary bad solution
            {
                prevBuffer.PutItem(VisibleCollection[0]);
                VisibleCollection.RemoveAt(0);
                VisibleCollection.Add(e);
                Position++;
            }
            
        }
    }
}
