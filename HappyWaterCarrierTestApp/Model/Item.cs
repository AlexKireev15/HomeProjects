using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HappyWaterCarrierTestApp.Model
{
    class Item
    {
        public virtual int ID { get; set; }
        public virtual Order Order { get; set; }
        public virtual String Name { get; set; }
        public virtual int Count { get; set; }
        public virtual float Price { get; set; }
    }
}
