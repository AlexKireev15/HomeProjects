using HappyWaterCarrierTestApp.Model;
using System;

namespace HappyWaterCarrierTestApp.Utils.Pagination
{
    class PaginationBufferNext<T> : PaginationBuffer<T>
        where T : class
    {
        public PaginationBufferNext(Func<int> getPosition, int length) : base(getPosition, length) 
        {
            NHibernateHelper.GetInstance().GetAsync<T>(getPosition(), length, (list) => {
                foreach (var e in list)
                    Buffer.AddToBack(e);
            });
        }

        public override T GetItem()
        {
            lock (Buffer)
            {
                CheckForExtention(Buffer.Count);
                if (Buffer.Count == 0)
                    return null;
                T item = Buffer.RemoveFromFront();
                return item;
            }
        }
        public override void PutItem(T item)
        {
            lock (Buffer)
            {
                Buffer.AddToFront(item);
                CheckForReduce();
            }
        }
        protected override void CheckForExtention(int bufferLength)
        {
            if (bufferLength < ExtensionLength)
            {
                int extensionPosition = GetPosition() + bufferLength;
                NHibernateHelper.GetInstance().GetAsync<T>(extensionPosition, ExtensionLength, (list) =>
                {
                    foreach (var e in list)
                    {
                        Buffer.AddToBack(e);
                    }
                });
            }
        }
        private void CheckForReduce()
        {

            if (Buffer.Count > Length)
                Buffer.RemoveRange(Length, Buffer.Count - Length);
        }

    }
}
