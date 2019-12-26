using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Windows.Data;

using AutoHotel.Enums;

namespace AutoHotel.Converter
{
    public class EnumToStringTypeRoom : IValueConverter
    {
        private List<KeyValuePair<string, TypeRoom>> typeRoomList;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (typeRoomList != null) return null;

            typeRoomList = new List<KeyValuePair<string, TypeRoom>>();
            foreach (TypeRoom level in Enum.GetValues(typeof(TypeRoom)))
            {
                string Description;
                FieldInfo fieldInfo = level.GetType().GetField(level.ToString());
                DescriptionAttribute[] attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attributes != null && attributes.Length > 0)
                {
                    Description = attributes[0].Description;
                }
                else
                {
                    Description = string.Empty;
                }

                var TypeKeyValue = new KeyValuePair<string, TypeRoom>(Description, level);
                typeRoomList.Add(TypeKeyValue);
            }
            return typeRoomList;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
