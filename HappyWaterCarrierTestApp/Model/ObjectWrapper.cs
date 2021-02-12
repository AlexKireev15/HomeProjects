using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HappyWaterCarrierTestApp.Model
{
    class ObjectWrapper<T> : INotifyPropertyChanged
    {
        private T Object { get; }
        public ObjectWrapper(T obj)
        {
            Object = obj;
        }
        public static List<ObjectWrapper<T>> WrapList(List<T> items)
        {
            List<ObjectWrapper<T>> wrappedList = new List<ObjectWrapper<T>>();
            foreach (var item in items)
            {
                wrappedList.Add(new ObjectWrapper<T>(item));
            }
            return wrappedList;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
