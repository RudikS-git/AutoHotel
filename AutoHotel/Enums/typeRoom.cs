using System.ComponentModel;

namespace AutoHotel.Enums
{
    public enum TypeRoom
    {
        [Description("Стандарт")]
        STANDART,
        [Description("Полулюкс")]
        JUNIORSUITE,
        [Description("Люкс")]
        SUITE,
        [Description("Президентский")]
        PRESIDENT
    }
}
