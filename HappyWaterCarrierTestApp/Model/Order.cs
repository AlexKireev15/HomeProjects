using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HappyWaterCarrierTestApp.Model
{
    public class Order
    {
        public virtual int ID { get; set; }
        public virtual String Counterparty { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual Employee Author { get; set; }
    }
}
