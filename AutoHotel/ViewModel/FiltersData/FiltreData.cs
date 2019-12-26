using AutoHotel.RoomLodger;

namespace AutoHotel.ViewModel.FiltersData
{
    class FiltreData
    {
        public class CheckedRooms
        {
            public bool CheckedNumber { get; set; }

            public bool CheckedType { get; set; }

            public bool CheckedPlace { get; set; }

            public bool CheckedFeature { get; set; }
        }

        public Room Room { get; set; }
        public CheckedRooms checkedRooms {get; set;}

        public FiltreData()
        {
            Room = new Room();
            checkedRooms = new CheckedRooms();
        }
    }
}