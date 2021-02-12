using HappyWaterCarrierTestApp.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace HappyWaterCarrierTestApp.Utils.Converters
{
    public class SingleEnumConverter : ConvertorBase<EnumConverter>
    {
        private IEnumerable<string> GetDescriptions(Type t)
        {
            if(!t.IsEnum)
                throw new ArgumentException($"{nameof(t)} must be an enum type");

            var descriptions = new List<string>();
            foreach(var fieldInfo in t.GetFields())
            {
                var attrs = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), true);
                if(attrs != null && attrs.Length > 0)
                {
                    descriptions.Add(((DescriptionAttribute)attrs[0]).Description);
                }
            }
            
            return descriptions;
        }
        private string GetDescriptionByEnum(object value)
        {
            var attrs = value.GetType().GetField(((Enum)value).ToString()).GetCustomAttributes(typeof(DescriptionAttribute), true);
            if(attrs != null && attrs.Length > 0)
            {
                return ((DescriptionAttribute)attrs[0]).Description;
            }
            throw new ArgumentException("The value '" + value.ToString() + "' is not supported");
        }
        private object GetEnumValueByDescription(string value, Type enumType)
        {
            if (value == null)
                return null;
            foreach(Enum val in Enum.GetValues(enumType))
            {
                FieldInfo fi = enumType.GetField(val.ToString());
                var attrs = fi.GetCustomAttributes(typeof(DescriptionAttribute), true);
                if(attrs != null && attrs.Length > 0)
                {
                    if (((DescriptionAttribute)attrs[0]).Description.Equals(value))
                        return val;
                }
            }
            throw new ArgumentException("The value '" + value + "' is not supported");
        }
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return GetDescriptionByEnum(value);
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return GetEnumValueByDescription((string)value, targetType);
        }
    }
}
