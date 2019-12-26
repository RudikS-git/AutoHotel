using AutoHotel.RoomLodger;

namespace AutoHotel.ViewModel.EventArgs
{
    class RoomsEventArgs
    {
        public Room _room { get; set; }

        public RoomsEventArgs(Room room)
        {
            _room = room;
        }
    }
}
