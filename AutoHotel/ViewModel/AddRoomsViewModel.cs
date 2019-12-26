using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Linq;

using AutoHotel.RoomLodger;
using AutoHotel.ViewModel.Commands;
using AutoHotel.ViewModel.EventArgs;
using AutoHotel.Enums;

namespace AutoHotel.ViewModel
{
    class AddRoomsViewModel : ViewModelBase, IDataErrorInfo
    {
        public delegate void RoomsHandler(object sender, RoomsEventArgs e);
        public event RoomsHandler Notify;

        public static readonly DependencyProperty CloseCommandProperty =
            DependencyProperty.Register("CloseCommand", typeof(ICommand), typeof(AddRoomsViewModel), new PropertyMetadata(null));

        public static readonly DependencyProperty NameButtonProperty =
            DependencyProperty.Register("NameButton", typeof(string), typeof(AddRoomsViewModel), new PropertyMetadata(null));

        private int InitValue;
        private Dictionary<string, bool> validProperties;
        private readonly Room currentRoom;
        private bool allPropertiesValid = false;

        public ObservableCollection<Room> Rooms { get; set; }
        public bool IsAdd { get; set; }

        public int Id
        {
            get
            {
                return currentRoom.Id;
            }

            set
            {
                if (currentRoom.Id != value)
                {
                    currentRoom.Id = value;
                    OnPropertyChanged("Id");
                }
            }
        }

        public int Number
        {
            get
            {
                return currentRoom.Number;
            }

            set
            {

                if (currentRoom.Number != value)
                {
                    currentRoom.Number = value;
                    OnPropertyChanged("Number");
                }
            }
        }

        public TypeRoom TypeRoom
        {
            get
            {
                return currentRoom.TypeRoom;
            }

            set
            {
                if (currentRoom.TypeRoom != value)
                {
                    currentRoom.TypeRoom = value;
                    OnPropertyChanged("TypeRoom");
                }
            }
        }

        public PlaceRoom PlaceRoom
        {
            get
            {
                return currentRoom.PlaceRoom;
            }

            set
            {
                if (currentRoom.PlaceRoom != value)
                {
                    currentRoom.PlaceRoom = value;
                    OnPropertyChanged("PlaceRoom");
                }
            }
        }

        public FeatureRoom FeatureRoom
        {
            get
            {
                return currentRoom.FeatureRoom;
            }

            set
            {
                if (currentRoom.FeatureRoom != value)
                {
                    currentRoom.FeatureRoom = value;
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
                if(allPropertiesValid != value)
                {
                    allPropertiesValid = value;
                    OnPropertyChanged("AllPropertiesValid");
                }
            }
        }

        public string Error
        {
            get
            {
                return (currentRoom as IDataErrorInfo).Error;
            }
        }

        public string this[string propertyName]
        {
            get
            {
                string error = (currentRoom as IDataErrorInfo)[propertyName];
                validProperties[propertyName] = String.IsNullOrEmpty(error) ? true : false;
                ValidateProperties();
                CommandManager.InvalidateRequerySuggested();
                return error;
            }
        }

        private void ValidateProperties()
        {
            foreach(bool isValid in validProperties.Values)
            {
                if(!isValid)
                {
                    AllPropertiesValid = false;
                    return;
                }
            }
            AllPropertiesValid = true;
        }

        public string NameButton
        {
            get
            {
                return (string)GetValue(NameButtonProperty);
            }

            set
            {
                SetValue(NameButtonProperty, value);
            }
        }

        public ICommand CloseCommand
        {
            get
            {
                return (ICommand)GetValue(CloseCommandProperty);
            }
            set
            {
                SetValue(CloseCommandProperty, value);
            }
        }

        private RelayCommand addNewRooms;
        public RelayCommand AddNewRooms
        {
            get
            {
                return addNewRooms ?? (addNewRooms = new RelayCommand(
                    obj =>
                    {
                        Notify?.Invoke(this, new RoomsEventArgs(currentRoom));
                        Close();
                    },
                    obj => NotBusy(currentRoom) == true));
            }
        }

        private bool NotBusy(Room room)
        {
            Room obj = null;
            if (IsAdd == false && InitValue == room.Number) return true;

            obj = Rooms.Where(it => it.Number == room.Number).FirstOrDefault();

            return obj == null; 
        }

        public AddRoomsViewModel()
        {
            CloseCommand = new RelayCommand(obj => Close());

            currentRoom = new Room();
            InitConstr();
        }

        public AddRoomsViewModel(int number)
        {
            CloseCommand = new RelayCommand(obj => Close());
            InitValue = number;

            currentRoom = new Room();
            InitConstr();
        }

        private void InitConstr()
        {
            validProperties = new Dictionary<string, bool>();
            validProperties.Add("Number", false);
        }
    }
}
