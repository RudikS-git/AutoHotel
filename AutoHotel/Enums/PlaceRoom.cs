using System.ComponentModel;

namespace AutoHotel.Enums
{
    public enum PlaceRoom
    {
        [Description("Одноместный")]
        SINGLE = 0,
        [Description("Двухместный")]
        DOUBLE,
        [Description("Трехместный")]
        TRIPLE,
        [Description("Четырехместный")]
        QUADRUPLE
    }
}
