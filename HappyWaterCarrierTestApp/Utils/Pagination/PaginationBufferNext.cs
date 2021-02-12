using HappyWaterCarrierTestApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                CheckForExtention();
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
        protected override void CheckForExtention()
        {
            if (Buffer.Count < ExtensionLength)
            {
                NHibernateHelper.GetInstance().GetAsync<T>(GetPosition() + Buffer.Count + 1, ExtensionLength, (list) =>
                {

                    Console.WriteLine("Contain:-------------" + GetPosition() + Buffer.Count + 1);
                    foreach (var e in Buffer)
                    {
                        Console.WriteLine(((Employee)Convert.ChangeType(e, typeof(Employee))).ID);
                    }
                    Console.WriteLine("----------------Extention:");
                    foreach (var e in list)
                    {
                        Buffer.AddToBack(e);
                        Console.WriteLine(((Employee)Convert.ChangeType(e, typeof(Employee))).ID);
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
