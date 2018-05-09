using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace ImageServiceGUI.controls.Convertor
{
    class ConnectionToBackgroundConvertor : IValueConverter
    {
        /// <summary>
        /// convert bool to color
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns>the color of the background</returns>
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

        /// <summary>
        /// convert from color to bool
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return true;
        }
    }
}
