﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace ImageServiceGUI.controls.Convertor
{
    class LogStatusToColorConvertor : IValueConverter
    {
        /// <summary>
        /// convert log status to color
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(Brush))
            {
                throw new InvalidOperationException("Must convert to a brush!");
            }
            string status = value.ToString();
            object brush;
            switch(status)
            {
                case "INFO": brush = Brushes.GreenYellow; break;
                case "FAIL": brush = Brushes.Red; break;
                case "WARNING": brush = Brushes.Yellow; break;
                default: brush = Brushes.Gray; break;
            }
            return brush;
        }

        /// <summary>
        /// convert log status to color
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
