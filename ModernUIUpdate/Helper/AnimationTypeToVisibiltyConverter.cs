using CharacterEditor.Models;
using CharacterEditor.ViewModels;
using CharacterModelLib.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace CharacterEditor.Helper
{
    public class AnimationTypeToVisibiltyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool reverse = (parameter != null && parameter.GetType() == typeof(string) && Boolean.Parse(parameter as String));

            if (value != null && value.GetType() == typeof(SimpleAnimation))
            {                
                if (reverse)
                {
                    return Visibility.Collapsed;    
                }
                return Visibility.Visible;
            }

            if (reverse)
            {
                return Visibility.Visible;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
