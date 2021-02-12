using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace HappyWaterCarrierTestApp.Utils.Converters
{
    public abstract class ConvertorBase<T> : MarkupExtension, IValueConverter
        where T : class, new()
    {
        public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);
        public abstract object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture);
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
