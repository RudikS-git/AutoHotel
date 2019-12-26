using System.ComponentModel;

namespace AutoHotel.Enums
{
    public enum FeatureRoom
    {
        [Description("Отсутствуют")]
        none,
        [Description("Кондиционер")]
        cond,
        [Description("Балкон")]
        balc,
        [Description("Холодильник")]
        icebox,
        [Description("Кондиционер + Балкон")]
        condBalc,
        [Description("Кондиционер + Холодильник")]
        condIcebox,
        [Description("Балкон + Холодильник")]
        balcIcebox,
        [Description("Балкон + Холодильник + Кондиционер")]
        balcIceboxCond
    }
}
