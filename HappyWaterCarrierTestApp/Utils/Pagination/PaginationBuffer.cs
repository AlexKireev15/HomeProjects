using HappyWaterCarrierTestApp.Model;
using Nito.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HappyWaterCarrierTestApp.Utils.Pagination
{
    abstract class PaginationBuffer<T> where T : class
    {
        protected Deque<T> Buffer { get; set; }
        protected Func<int> GetPosition { get; set; }
        protected int ExtensionLength { get; }
        public int Length { get; }
        public PaginationBuffer(Func<int> position, int length)
        {
            GetPosition = position;
            Length = length;
            ExtensionLength = length / 2;
            Buffer = new Deque<T>();
        }
        public bool IsEmpty()
        {
            //CheckForExtention();
            return Buffer.Count == 0;
        }
        public abstract T GetItem();
        public abstract void PutItem(T item);
        protected abstract void CheckForExtention();
    }
}
