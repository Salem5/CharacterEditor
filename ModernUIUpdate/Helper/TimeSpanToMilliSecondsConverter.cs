using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace CharacterEditor.Helper
{
    public class TimeSpanToMilliSecondsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value.GetType() == typeof(TimeSpan))
            {
                if (targetType == typeof(string))
                {
                    return ((TimeSpan)value).TotalMilliseconds.ToString();
                }
                else if (targetType == typeof(int))
                {
                    return ((TimeSpan)value).TotalMilliseconds;
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value.GetType() == typeof(string))
            {
                try
                {
                    return TimeSpan.FromMilliseconds(int.Parse(value as string));
                }
                catch (Exception)
                {
                    Debug.WriteLine("Non integer parsable value passed to timespantomillisecondsconverter.");
                    return null;                    
                }
            }
            else if (value.GetType() == typeof(int))
            {
                return TimeSpan.FromMilliseconds((int)value);
            }

            return null;
        }
    }
}
