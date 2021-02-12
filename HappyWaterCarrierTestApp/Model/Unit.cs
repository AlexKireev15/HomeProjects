using System;

namespace HappyWaterCarrierTestApp.Model
{
    class Unit
    {
        public virtual int ID { get; set; }
        public virtual string Name { get; set; }
        public virtual Employee Manager { get; set; }

        public Unit(Unit other)
        {
            if (other == null)
                return;
            ID = other.ID;
            Name = (string)Name.Clone();
            Manager = new Employee(other.Manager);
        }
        public Unit()
        {

        }
    }
}
