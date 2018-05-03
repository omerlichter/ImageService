using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace ImageServiceGUI.Convertor
{
    class ConnectionToBackgroundConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (targetType != typeof(Brush))
            {
                throw new InvalidOperationException("Must convert to a brush!");
            }
            bool connected = (bool)value;
            if (connected)
            {
                return Brushes.White;
            }
            return Brushes.Gray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(Brush))
            {
                throw new InvalidOperationException("Must convert to a brush!");
            }
            decimal price = (decimal)value;
            return price > 45 ? Brushes.Red : Brushes.Transparent;
        }
    }
}
