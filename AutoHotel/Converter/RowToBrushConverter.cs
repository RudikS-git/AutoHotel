using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

using AutoHotel.RoomLodger;

namespace AutoHotel.Converter
{
    public class RowToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(Brush))
            {
                return null;
            }

            var r = value as Administration;

            if (r != null && r.DateCheck.Date <= DateTime.Now.Date && r.DateEviction.Date >= DateTime.Now.Date)
            {
                return Brushes.LightGreen;
            }
            else if(DateTime.Now.Date < r.DateEviction.Date)
            {
                return Brushes.Yellow;
            }
            else
            {
                return new SolidColorBrush(Color.FromArgb(200, 255, 153, 153));
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
