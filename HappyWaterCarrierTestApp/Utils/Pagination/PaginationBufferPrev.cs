using System;

namespace HappyWaterCarrierTestApp.Utils.Pagination
{
    class PaginationBufferPrev<T> : PaginationBuffer<T>
        where T : class
    {
        public PaginationBufferPrev(Func<int> getPosition, int length) : base(getPosition, length){}

        public override T GetItem()
        {
            CheckForExtention();
            if (Buffer.Count == 0)
                return null;
            T item = Buffer.RemoveFromBack();
            return item;
        }
        public override void PutItem(T item)
        {
            Buffer.AddToBack(item);
            CheckForReduce();
        }
        protected override void CheckForExtention()
        {
            if (Buffer.Count < ExtensionLength)
            {
                NHibernateHelper.GetInstance().GetAsync<T>(GetPosition() - ExtensionLength - Buffer.Count, ExtensionLength, (list) => {
                    for (int idx = list.Count - 1; idx >= 0; --idx)
                        Buffer.AddToFront(list[idx]);
                });
            }
        }
        private void CheckForReduce()
        {
            if (Buffer.Count > Length)
                Buffer.RemoveRange(0, Buffer.Count - Length);
        }

    }
}
