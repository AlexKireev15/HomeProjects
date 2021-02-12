using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HappyWaterCarrierTestApp.Model
{
    public class Employee
    {
        public virtual int ID { get; set; }
        public virtual string Surname { get; set; }
        public virtual string Name { get; set; }
        public virtual string Midname { get; set; }
        public virtual DateTime Birthday { get; set; }
        public virtual Sex Sex { get; set; }
        public virtual Unit Unit { get; set; }

        public Employee(Employee other)
        {
            if (other == null)
                return;
            ID = other.ID;
            if(other.Surname != null)
                Surname = (string)other.Surname.Clone();
            if (other.Name != null)
                Name = (string)other.Name.Clone();
            if (other.Midname != null)
                Midname = (string)other.Midname.Clone();
            if (other.Birthday != null)
                Birthday = new DateTime(other.Birthday.Ticks);
            Sex = other.Sex;
            if(other.Unit != null)
                Unit = new Unit(other.Unit);
        }
        public Employee()
        {

        }
    }
}
