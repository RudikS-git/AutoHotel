using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using AutoHotel.Enums;
using AutoHotel.RoomLodger;
using AutoHotel.ViewModel.Commands;
using AutoHotel.ViewModel.FiltersData;

namespace AutoHotel.ViewModel
{
    class AdvFiltreRoomsViewModel : ViewModelBase
    {
        public ICollectionView customViewRoom { get; set; }

        private FiltreData _filreData;
        private Dictionary<string, bool> validProperties;
        private bool allPropertiesValid = false;

        public ObservableCollection<Room> Rooms { get; set; }

        public int Id
        {
            get
            {
                return _filreData.Room.Id;
            }

            set
            {
                if (_filreData.Room.Id != value)
                {
                    _filreData.Room.Id = value;
                    OnPropertyChanged("Id");
                }
            }
        }

        public int Number
        {
            get
            {
                return _filreData.Room.Number;
            }

            set
            {

                if (_filreData.Room.Number != value)
                {
                    _filreData.Room.Number = value;
                    OnPropertyChanged("Number");
                }
            }
        }

        public TypeRoom TypeRoom
        {
            get
            {
                return _filreData.Room.TypeRoom;
            }

            set
            {
                if (_filreData.Room.TypeRoom != value)
                {
                    _filreData.Room.TypeRoom = value;
                    OnPropertyChanged("TypeRoom");
                }
            }
        }

        public PlaceRoom PlaceRoom
        {
            get
            {
                return _filreData.Room.PlaceRoom;
            }

            set
            {
                if (_filreData.Room.PlaceRoom != value)
                {
                    _filreData.Room.PlaceRoom = value;
                    OnPropertyChanged("PlaceRoom");
                }
            }
        }

        public FeatureRoom FeatureRoom
        {
            get
            {
                return _filreData.Room.FeatureRoom;
            }

            set
            {
                if (_filreData.Room.FeatureRoom != value)
                {
                    _filreData.Room.FeatureRoom = value;
                    OnPropertyChanged("FeatureRoom");
                }
            }
        }

        public bool AllPropertiesValid
        {
            get
            {
                return allPropertiesValid;
            }
            set
            {
                if (allPropertiesValid != value)
                {
                    allPropertiesValid = value;
                    OnPropertyChanged("AllPropertiesValid");
                }
            }
        }

        public bool CheckedNumber
        {
            get
            {
                return _filreData.checkedRooms.CheckedNumber;
            }

            set
            {
                _filreData.checkedRooms.CheckedNumber = value;
                OnPropertyChanged("CheckedNumber");
            }
        }

        public bool CheckedType
        {
            get
            {
                return _filreData.checkedRooms.CheckedType;
            }

            set
            {
                _filreData.checkedRooms.CheckedType = value;
                OnPropertyChanged("CheckedType");
            }
        }

        public bool CheckedPlace
        {
            get
            {
                return _filreData.checkedRooms.CheckedPlace;
            }

            set
            {
                _filreData.checkedRooms.CheckedPlace = value;
                OnPropertyChanged("CheckedPlace");
            }
        }

        public bool CheckedFeature
        {
            get
            {
                return _filreData.checkedRooms.CheckedFeature;
            }

            set
            {
              
                _filreData.checkedRooms.CheckedFeature = value;
                OnPropertyChanged("CheckedFeature");
            }
        }
        //public string Error
        //{
        //    get
        //    {
        //        return (currentRoom as IDataErrorInfo).Error;
        //    }
        //}

        //public string this[string propertyName]
        //{
        //    get
        //    {
        //        string error = (currentRoom as IDataErrorInfo)[propertyName];
        //        validProperties[propertyName] = String.IsNullOrEmpty(error) ? true : false;
        //        ValidateProperties();
        //        CommandManager.InvalidateRequerySuggested();
        //        return error;
        //    }
        //}

        //private void ValidateProperties()
        //{
        //    foreach (bool isValid in validProperties.Values)
        //    {
        //        if (!isValid)
        //        {
        //            AllPropertiesValid = false;
        //            return;
        //        }
        //    }
        //    AllPropertiesValid = true;
        //}

        public AdvFiltreRoomsViewModel(FiltreData filtredata)
        {
            _filreData = filtredata;
            InitConstr();
        }

        private void InitConstr()
        {
            validProperties = new Dictionary<string, bool>();
            validProperties.Add("Number", false);
        }

        private RelayCommand filtre;
        public RelayCommand Filtre
        {
            get
            {
                return filtre ??
                    (filtre = new RelayCommand(obj =>
                    {
                        if (Rooms != null)
                        {
                            customViewRoom.Filter = (it =>
                            {
                                var room = it as Room;

                                if(room != null)
                                {
                                    return 
                                    (CheckedNumber != true ? true : room.Number == Number)
                                    &&
                                    (CheckedType != true ? true : room.TypeRoom == TypeRoom)
                                    &&
                                    (CheckedPlace != true ? true : room.PlaceRoom == PlaceRoom)
                                    &&
                                    (CheckedFeature != true ? true : room.FeatureRoom == FeatureRoom);
                                }
                                else
                                {
                                    return false;
                                }

                            });

                        }
                    }));
            }
        }

        private RelayCommand exit;
        public RelayCommand Exit
        {
            get
            {
                return exit ??
                    (exit = new RelayCommand(obj =>
                    {
                        Close();
                    }));
            }
        }

        private RelayCommand discharge;
        public RelayCommand Discharge
        {
            get
            {
                return discharge ??
                    (discharge = new RelayCommand(obj =>
                    {
                        CheckedNumber = false;
                        CheckedType = false;
                        CheckedPlace = false;
                        CheckedFeature = false;

                        Number = 0;
                        TypeRoom = 0;
                        PlaceRoom = 0;
                        FeatureRoom = 0;

                        customViewRoom.Filter = null;
                    }));
            }
        }
    }
}
