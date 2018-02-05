using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace FoltoCrypt.Classes
{
    class CostColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                SolidColorBrush color;
                var a = (string)value;
                var a1 = a.Split(' ')[0];
                var b = Double.Parse(a1);
                if (b < 0)
                {
                    color = (SolidColorBrush)new BrushConverter().ConvertFromString("#c70000"); 
                }
                else if (b > 0)
                {
                    color = (SolidColorBrush)new BrushConverter().ConvertFromString("#40826d");
                }
                else
                {
                    color = new SolidColorBrush(Colors.Black);
                }
                return color;
            }
            catch
            {
                return new SolidColorBrush(Colors.Black);
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
