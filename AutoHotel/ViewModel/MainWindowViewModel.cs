using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Windows;
using System.Windows.Data;

using AutoHotel.Enums;
using AutoHotel.RoomLodger;
using AutoHotel.ViewModel.Commands;
using AutoHotel.ViewModel.EventArgs;
using AutoHotel.ViewModel.FiltersData;

namespace AutoHotel.ViewModel
{
    class MainWindowViewModel : ViewModelBase
    {
        private ICollectionView _customViewAdmin;
        private ICollectionView _customViewRoom;

        private FiltreData filtreData;

        private string connection;

        public MainWindowViewModel()
        {
            connection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            InitializeTable();
            InitializeFiltre();

            _customViewRoom.SortDescriptions.Add(new SortDescription("Number", ListSortDirection.Ascending));
            _customViewAdmin.SortDescriptions.Add(new SortDescription("DateCheck", ListSortDirection.Ascending));
            _customViewAdmin.SortDescriptions.Add(new SortDescription("DateEviction", ListSortDirection.Ascending));
            filtreData = new FiltreData();
        }

        #region Init
        private void InitializeFiltre()
        {
            _customViewRoom = CollectionViewSource.GetDefaultView(Rooms);

            _customViewAdmin = CollectionViewSource.GetDefaultView(Administrations);
            _customViewAdmin.Filter = (obj) => obj == null;
        }

        private void InitializeTable()
        {
            Room.Select(out rooms);
            Administration.Select(out administrations);
        }

        #endregion

        #region WindowsCreating
        private void WindowCreateAddRooms()
        {
            var add = new AddRoomsViewModel()
            {
                Title = "Новая комната",
                Number = rooms.Count == 0 ? 0 : rooms[rooms.Count - 1].Number + 1,
                PlaceRoom = PlaceRoom.SINGLE,
                TypeRoom = TypeRoom.STANDART,
                FeatureRoom = FeatureRoom.none,
                NameButton = "Добавить",
                Rooms = rooms,
                IsAdd = true,
                Resize = ResizeMode.NoResize
            };

            add.Notify += SuccessfulClosed;
            ShowDialog(add);
        }

        private void WindowCreateEditRooms()
        {
            var add = new AddRoomsViewModel(selectedRoom.Number)
            {
                Title = "Редактирование комнаты",
                Number = selectedRoom.Number,
                TypeRoom = selectedRoom.TypeRoom,
                PlaceRoom = selectedRoom.PlaceRoom,
                FeatureRoom = selectedRoom.FeatureRoom,
                NameButton = "Изменить",
                Rooms = rooms,
                IsAdd = false,
                Resize = ResizeMode.NoResize
            };

            add.Notify += SuccessfulClosedEdit;
            ShowDialog(add);
        }

        private void WindowCreateAddLodgers()
        {
            var add = new AddLodgersViewModel((int)selectedRoom.PlaceRoom + 1)
            {
                Title = "Регистрация постояльца",
                DateStart = DateTime.Now,
                DateEnd = DateTime.Now.AddDays(1),
                Email = "",
                SelectedAdults = "1",
                SelectedChilds = "0",
                Administrations = administrations,
                NameButton = "Зарегистрировать",
                IdRoom = selectedRoom.Id,
                Resize = ResizeMode.NoResize
            };
            add.Notify += SuccessfulClosed;
            ShowDialog(add);
            _customViewAdmin.Filter = FiltreAdmins;
        }

        private void WindowCreateEditLodgers()
        {
            var add = new AddLodgersViewModel((int)selectedRoom.PlaceRoom + 1, selectedAdmin)
            {
                Title = "Редактирование информации о постояльце",
                Name = selectedAdmin.Name,
                NumberPhone = selectedAdmin.NumberPhone,
                Email = selectedAdmin.Email,
                SelectedAdults = selectedAdmin.NumAdults.ToString(),
                SelectedChilds = selectedAdmin.NumChilds.ToString(),
                Remark = selectedAdmin.Remark,
                DateStart = selectedAdmin.DateCheck,
                DateEnd = selectedAdmin.DateEviction,
                Administrations = administrations,
                NameButton = "Изменить",
                IdRoom = selectedRoom.Id,
                Resize = ResizeMode.NoResize
            };
            add.Notify += SuccessfulClosedEdit;
            ShowDialog(add);
            _customViewAdmin.Filter = FiltreAdmins;
        }

        #endregion

        #region EventClosedWindows

        public void SuccessfulClosed(object obj, RoomsEventArgs e)
        {
            rooms.Add(e._room);
            SelectedRoom = e._room;

            e._room.Insert();
        }

        public void SuccessfulClosedEdit(object obj, RoomsEventArgs e)
        {
            selectedRoom.Number = e._room.Number;
            selectedRoom.TypeRoom = e._room.TypeRoom;
            selectedRoom.PlaceRoom = e._room.PlaceRoom;
            selectedRoom.FeatureRoom = e._room.FeatureRoom;

            e._room.Update();
        }

        public void SuccessfulClosed(object obj, LodgersEventArgs e)
        {
            administrations.Add(e._admin);

            e._admin.IdEmession = SelectedRoom.Id;
            SelectedAdmin = null;

            e._admin.Insert();
        }

        public void SuccessfulClosedEdit(object obj, LodgersEventArgs e)
        {
            selectedAdmin.Name = e._admin.Name;
            selectedAdmin.Email = e._admin.Email;
            selectedAdmin.NumberPhone = e._admin.NumberPhone;
            selectedAdmin.Remark = e._admin.Remark;
            selectedAdmin.DateCheck = e._admin.DateCheck;
            selectedAdmin.DateEviction = e._admin.DateEviction;
            selectedAdmin.NumAdults = e._admin.NumAdults;
            selectedAdmin.NumChilds = e._admin.NumChilds;

            selectedAdmin.Update();
        }

        #endregion

        private ObservableCollection<Administration> administrations;
        public ObservableCollection<Administration> Administrations
        {
            get
            {
                return administrations;
            }

            set
            {
                administrations = value;
                OnPropertyChanged("Administrations");
            }
        }

        private ObservableCollection<Room> rooms;
        public ObservableCollection<Room> Rooms
        {
            get
            {
                return rooms;
            }

            set
            {
                rooms = value;
                OnPropertyChanged("Rooms");
            }
        }

        private Visibility buttonVisible;
        public Visibility ButtonVisible
        {
            get
            {
                return buttonVisible;
            }

            set
            {
                buttonVisible = value;
                OnPropertyChanged("ButtonVisible");
            }
        }

        private Administration selectedAdmin;
        public Administration SelectedAdmin
        {
            get
            {
                return selectedAdmin;
            }

            set
            {
                selectedAdmin = value;
                OnPropertyChanged("SelectedAdmin");
            }
        }

        private Room selectedRoom;
        public Room SelectedRoom
        {
            get
            {
                return selectedRoom;
            }

            set
            {
                selectedRoom = value;
                OnPropertyChanged("SelectedRoom");
                // selectedLodgers = (CollectionViewSource)CollectionViewSource.GetDefaultView(Administrations.Item);
                //  selectedLodgers.Source = Administrations;
                //  selectedLodgrs.SortDescriptions.Add(new SortDescription("Number", (ListSortDirection)selectedRoom.Number));
                if (selectedRoom != null)
                {
                    _customViewAdmin.Filter = FiltreAdmins;
                }
            }
        }

        private bool FiltreAdmins(object item)
        {
            Administration admin = item as Administration;
            return (admin.IdEmession == selectedRoom.Id ? true : false);
        }

        private bool isChecked;
        public bool IsChecked
        {
            get
            {
                return isChecked;
            }

            set
            {
                isChecked = value;
                OnPropertyChanged("IsChecked");
            }
        }

        private int filtreCbIndex;
        public int FiltreCbIndex
        {
            get
            {
                return filtreCbIndex;
            }

            set
            {
                filtreCbIndex = value;
                OnPropertyChanged("FiltreCbIndex");
            }
        }

        #region Commands

        private RelayCommand createWindowAddRooms;
        public RelayCommand CreateWindowAddRooms
        {
            get
            {
                return createWindowAddRooms ??
                    (createWindowAddRooms = new RelayCommand(obj =>
                    {
                        WindowCreateAddRooms();
                    }));
            }
        }

        private RelayCommand createWindowEditRooms;
        public RelayCommand CreateWindowEditRooms
        {
            get
            {
                return createWindowEditRooms ??
                    (createWindowEditRooms = new RelayCommand(obj =>
                    {
                        WindowCreateEditRooms();
                    },
                    obj => SelectedRoom != null));
            }

        }

        private RelayCommand createWindowAddLodgers;
        public RelayCommand CreateWindowAddLodgers
        {
            get
            {
                return createWindowAddLodgers ??
                    (createWindowAddLodgers = new RelayCommand(obj =>
                    {
                        WindowCreateAddLodgers();
                    },
                    obj => SelectedRoom != null));
            }

        }

        private RelayCommand createWindowEditLodgers;
        public RelayCommand CreateWindowEditLodgers
        {
            get
            {
                return createWindowEditLodgers ??
                    (createWindowEditLodgers = new RelayCommand(obj =>
                    {
                        WindowCreateEditLodgers();
                    },
                    obj => SelectedAdmin != null));
            }

        }

        private RelayCommand deleteRoom;
        public RelayCommand DeleteRoom
        {
            get
            {
                return deleteRoom ??
                (deleteRoom = new RelayCommand(obj =>
                {
                    Room room = obj as Room;

                    if (room != null)
                    {
                        int id = room.Id;
                        Rooms.Remove(room);
                        room.Delete();

                        for (int i = 0; i < administrations.Count; i++)
                        {
                            if (administrations[i].IdEmession == id)
                            {
                                Administrations.Remove(administrations[i]);
                            }
                        }

                        MessageBox.Show(string.Format("Комната №{0} удалена", room.Id));
                    }
                },
                (obj) => Rooms.Count > 0));
            }
        }

        private RelayCommand deleteLodger;
        public RelayCommand DeleteLodger
        {
            get
            {
                return deleteLodger ??
                (deleteLodger = new RelayCommand(obj =>
                {
                    Administration admin = obj as Administration;

                    if (admin != null)
                    {
                        Administrations.Remove(admin);
                        admin.Delete();

                        MessageBox.Show(string.Format("Постоялец {0} удален", admin.Name));
                    }
                },
                (obj) => Administrations.Count > 0));
            }
        }

        private RelayCommand exitCommand;
        public RelayCommand ExitCommand
        {
            get
            {
                if (exitCommand == null)
                {
                    exitCommand = new RelayCommand(obj =>
                    {
                        Application.Current.Shutdown();
                    });
                }

                return exitCommand;
            }
        }

        private RelayCommand filterBlankNumber;
        public RelayCommand FilterBlankNumber
        {
            get
            {
                return filterBlankNumber ??
                (filterBlankNumber = new RelayCommand(obj =>
                {
                    if (IsChecked == true)
                    {
                        _customViewRoom.Filter = (it =>
                        {
                            var _room = it as Room;
                            Administration _admin = null;

                            if (_room != null)
                            {
                                _admin = administrations.Where(lodger => lodger.IdEmession == _room.Id &&
                                                                    lodger.DateEviction.Date >= DateTime.Now.Date
                                                                  ).FirstOrDefault();
                            }

                            return _admin == null ? true : false;

                        });
                        _customViewAdmin.Filter = (admObj) => admObj == null;
                    }
                    else
                    {
                        _customViewRoom.Filter = null;
                        _customViewAdmin.Filter = admObj => admObj == null;
                    }

                }));
            }
        }

        private RelayCommand filtre;
        public RelayCommand Filtre
        {
            get
            {
                return filtre ??
                (filtre = new RelayCommand(obj =>
                {
                    var str = obj as string;
                    if (str != null)
                    {
                        if (str == "")
                        {
                            _customViewAdmin.Filter = null;
                            return;
                        }

                        _customViewAdmin.Filter = it =>
                        {
                            var _admin = it as Administration;

                            if (_admin != null)
                            {
                                switch (FiltreCbIndex)
                                {
                                    case 0:
                                        int resId;

                                        if (!int.TryParse(str, out resId))
                                        {
                                            return false;
                                        }

                                        return _admin.Id == resId ? true : false;
                                    case 1:
                                        DateTime dateTimeStart;

                                        if (!DateTime.TryParse(str, out dateTimeStart))
                                        {
                                            return false;
                                        }

                                        return _admin.DateCheck == dateTimeStart ? true : false;
                                    case 2:
                                        DateTime dateTimeEnd;

                                        if (!DateTime.TryParse(str, out dateTimeEnd))
                                        {
                                            return false;
                                        }
                                        return _admin.DateEviction == dateTimeEnd ? true : false;
                                    case 3:


                                        return _admin.Name == str ? true : false;
                                    case 4:
                                        return _admin.NumberPhone == str ? true : false;
                                    case 5:
                                        return _admin.Email == str ? true : false;
                                    case 6:
                                        int resAdults;

                                        if (!int.TryParse(str, out resAdults))
                                        {
                                            return false;
                                        }

                                        return _admin.NumAdults == resAdults ? true : false;
                                    case 7:
                                        int resChilds;

                                        if (!int.TryParse(str, out resChilds))
                                        {
                                            return false;
                                        }

                                        return _admin.NumChilds == resChilds ? true : false;
                                    case 8:
                                        return _admin.Remark == str ? true : false;
                                }
                            }
                            return false;

                        };
                    }
                    else
                    {
                        _customViewAdmin.Filter = null;
                    }

                }));
            }
        }

        private RelayCommand aboutTheProgram;
        public RelayCommand AboutTheProgram
        {
            get
            {
                return aboutTheProgram ??
                    (aboutTheProgram = new RelayCommand(obj =>
                    {
                        var add = new AboutTheProgramViewModel()
                        {
                            Title = "О программе",
                            Resize = ResizeMode.NoResize
                        };

                        ShowDialog(add);
                    }));
            }
        }

        private RelayCommand watchInformation;
        public RelayCommand WatchInformation
        {
            get
            {
                return watchInformation ??
                    (watchInformation = new RelayCommand(obj =>
                    {
                        var add = new WatchInfoRoomViewModel()
                        {
                            Title = "Дополнительная информация о комнате",
                            Resize = ResizeMode.NoResize,
                            featureRooom = selectedRoom.FeatureRoom,
                            countLodgers = Administrations.Where(it => it.IdEmession == selectedRoom.Id).Count()
                        };

                        ShowDialog(add);
                    }));
            }
        }

        private RelayCommand advFiltre;
        public RelayCommand AdvFiltre
        {
            get
            {
                return advFiltre ??
                    (advFiltre = new RelayCommand(obj =>
                    {
                        var add = new AdvFiltreRoomsViewModel(filtreData)
                        {
                            Title = "Расширенный фильтр",
                            Resize = ResizeMode.NoResize,
                            customViewRoom = _customViewRoom,
                            Rooms = rooms,
                            CheckedNumber = filtreData.checkedRooms.CheckedNumber,
                            CheckedType = filtreData.checkedRooms.CheckedType,
                            CheckedPlace = filtreData.checkedRooms.CheckedPlace,
                            CheckedFeature = filtreData.checkedRooms.CheckedFeature
                        };

                        ShowDialog(add);
                    }));
            }
        }


        #endregion
    }
}
