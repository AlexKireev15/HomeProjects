using HappyWaterCarrierTestApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HappyWaterCarrierTestApp.Utils.Pagination
{
    interface PaginationHelper<T> where T : class
    {
        void MoveDown(int steps);
        void MoveUp(int steps);
        void Remove(T obj);
        void Put(T e);
    }
}
