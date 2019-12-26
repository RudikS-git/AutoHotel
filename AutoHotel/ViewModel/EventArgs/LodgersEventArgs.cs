using AutoHotel.RoomLodger;

namespace AutoHotel.ViewModel.EventArgs
{
    class LodgersEventArgs
    {
        public Administration _admin { get; set; }

        public LodgersEventArgs(Administration admin)
        {
           _admin = admin;
        }
    }
}
