using System;
using System.Windows.Data;

using AutoHotel.Enums;

namespace AutoHotel.Converter
{
    class YONConverterIceBox : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var featureRoom = (FeatureRoom)value;

            switch ((int)featureRoom)
            {
                case 3:
                    return @"/AutoHotel;component/Images\y.jpg";
                case 5:
                    return @"/AutoHotel;component/Images\y.jpg";
                case 6:
                    return @"/AutoHotel;component/Images\y.jpg";
                case 7:
                    return @"/AutoHotel;component/Images\y.jpg";
                default:
                    return @"/AutoHotel;component/Images\n.jpg";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
