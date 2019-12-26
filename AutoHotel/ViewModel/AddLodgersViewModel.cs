using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

using AutoHotel.RoomLodger;
using AutoHotel.ViewModel.Commands;
using AutoHotel.ViewModel.EventArgs;

namespace AutoHotel.ViewModel
{
    class AddLodgersViewModel :ViewModelBase, IDataErrorInfo
    {
        public delegate void LodgersHandler(object sender, LodgersEventArgs e);
        public event LodgersHandler Notify;

        public static readonly DependencyProperty CloseCommandProperty =
            DependencyProperty.Register("CloseCommand", typeof(ICommand), typeof(AddLodgersViewModel), new PropertyMetadata(null));

        public static readonly DependencyProperty NameButtonProperty =
            DependencyProperty.Register("NameButton", typeof(string), typeof(AddLodgersViewModel), new PropertyMetadata(null));

        public int NumPeople { get; set; }
        public ObservableCollection<Administration> Administrations { get; set; }

        private Dictionary<string, bool> validProperties;
        private readonly Administration currentAdmin;
        private readonly Administration newAdmin;
        private bool allPropertiesValid = false;

        public int IdRoom { get; set; }

        public int Id
        {
            get
            {
                return currentAdmin.Id;
            }
            set
            {
                if (currentAdmin.Id != value)
                {
                    currentAdmin.Id = value;
                    OnPropertyChanged("Id");
                }
            }
        }

        public int IdEmession
        {
            get
            {
                return currentAdmin.IdEmession;
            }
            set
            {
                if (currentAdmin.IdEmession != value)
                {
                    currentAdmin.IdEmession = value;
                    OnPropertyChanged("IdEmession");
                }
            }
        }

        public string Name
        {
            get
            {
                return currentAdmin.Name;
            }
            set
            {
                if (currentAdmin.Name != value)
                {
                    currentAdmin.Name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        public string NumberPhone
        {
            get
            {
                return currentAdmin.NumberPhone;
            }
            set
            {
                if (currentAdmin.NumberPhone != value)
                {
                    currentAdmin.NumberPhone = value;
                    OnPropertyChanged("NumberPhone");
                }
            }
        }
        public string Email
        {
            get
            {
                return currentAdmin.Email;
            }
            set
            {
                if (currentAdmin.Email != value)
                {
                    currentAdmin.Email = value;
                    OnPropertyChanged("Email");
                }
            }
        }
        public string Remark
        {
            get
            {
                return currentAdmin.Remark;
            }
            set
            {
                if (currentAdmin.Remark != value)
                {
                    currentAdmin.Remark = value;
                    OnPropertyChanged("Remark");
                }
            }
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

        public DateTime DateStart
        {
            get 
            {
                return currentAdmin.DateCheck;
            }
            set 
            { 
                if (currentAdmin.DateCheck != value)
                {
                    currentAdmin.DateCheck = value;
                    OnPropertyChanged("DateStart");
                }

                if (DateEnd != null && (DateStart > DateEnd))
                {
                    DateEnd = new DateTime(DateStart.Year, DateStart.Month, DateStart.AddDays(1).Day);
                }
            }
        }

        public DateTime DateEnd
        {
            get
            {
                return currentAdmin.DateEviction;
            }
            set
            {
                if (currentAdmin.DateEviction != value)
                {
                    currentAdmin.DateEviction = value;
                    OnPropertyChanged("DateEnd");
                }
            }
        }

        private string[] FullAdultsList;
        private string [] adultsList;
        public string[] AdultsList
        {
            get
            {
                if (adultsList == null)
                {

                    adultsList = new string[NumPeople];


                    for (int i = 0; i < NumPeople; i++)
                    {
                        adultsList[i] = (i + 1).ToString();
                    }

                    FullAdultsList = (string[])adultsList.Clone();

                }
                return adultsList;
            }
            set
            {
                adultsList = value;
                OnPropertyChanged("AdultsList");
            }
        }

        private string[] FullChildsList;
        private string[] childsList;
        public string[] ChildsList
        {
            get
            {
                if (childsList == null)
                {
                    childsList = new string[NumPeople];

                    for (int i = 0; i < NumPeople; i++)
                    {
                        childsList[i] = i.ToString();
                    }

                    FullChildsList = (string[])childsList.Clone();

                }
                return childsList;
            }

            set
            {
                childsList = value;
                OnPropertyChanged("ChildsList");
            }
        }

        private string selectedAdults;
        public string SelectedAdults
        {
            get
            {
                return selectedAdults;
            }

            set
            {
                currentAdmin.NumAdults = int.Parse(value);
                selectedAdults = value;

                if (ChildsList != null && (ChildsList.Count() > 1 || AdultsList.Count() > 1))
                {
                    ChildsList = FullChildsList.Where(it => int.Parse(it) <= (NumPeople - currentAdmin.NumAdults)).ToArray();
                }

                OnPropertyChanged("SelectedAdults");
            }
        }

        private string selectedChilds;
        public string SelectedChilds
        {
            get
            {
                return selectedChilds;
            }

            set
            {
                currentAdmin.NumChilds = int.Parse(value);
                selectedChilds = value;

                if (AdultsList != null && (ChildsList.Count() > 1 || AdultsList.Count() > 1))
                {
                    AdultsList = FullAdultsList.Where(it => int.Parse(it) <= (NumPeople - currentAdmin.NumChilds)).ToArray();
                }

                OnPropertyChanged("SelectedChilds");
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

        private RelayCommand addNewLodgers;
        public RelayCommand AddNewLodgers
        {
            get
            {
                return addNewLodgers ?? (addNewLodgers = new RelayCommand(
                    obj =>
                    {
                        Notify?.Invoke(this, new LodgersEventArgs(currentAdmin));
                        Close();
                    },
                    obj => AllPropertiesValid == true && Name != null && NumberPhone != null && CheckFree() == true));
            }
        }

        private bool CheckFree()
        {
            var _adm = Administrations.Where(it => IdRoom == it.IdEmession &&
                                            (((DateStart.Date <= it.DateCheck.Date &&
                                            DateEnd.Date >= it.DateCheck.Date) &&

                                            (DateStart.Date <= it.DateEviction.Date &&
                                            DateEnd.Date >= it.DateEviction.Date)) ||
        
                                            ((DateStart.Date >= it.DateCheck.Date &&
                                            DateStart.Date <= it.DateEviction.Date) ||

                                            (DateEnd.Date >= it.DateCheck.Date &&
                                            DateEnd.Date <= it.DateEviction.Date)))).FirstOrDefault();

            if (_adm == null) return true;
            else return false;
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

        public string Error
        {
            get
            {
                return (currentAdmin as IDataErrorInfo).Error;
            }
        }

        public string this[string propertyName]
        {
            get
            {
                string error = (currentAdmin as IDataErrorInfo)[propertyName];
                validProperties[propertyName] = String.IsNullOrEmpty(error) ? true : false;
                ValidateProperties();
                CommandManager.InvalidateRequerySuggested(); // обновление биндингов на команды
                return error;

            }
        }

        private void ValidateProperties()
        {
            foreach (bool isValid in validProperties.Values)
            {
                if (!isValid)
                {
                    AllPropertiesValid = false;
                    return;
                }
            }
            AllPropertiesValid = true;
        }

        public AddLodgersViewModel(int _numPeople, Administration admin)
        {
            NumPeople = _numPeople;
            CloseCommand = new RelayCommand(obj => Close());

            newAdmin = admin;
            currentAdmin = new Administration();
            InitConstr();

            Remark = "";
        }

        public AddLodgersViewModel(int _numPeople)
        {
            NumPeople = _numPeople;
            CloseCommand = new RelayCommand(obj => Close());

            currentAdmin = new Administration();
            InitConstr();

            Remark = "";
        }

        private void InitConstr()
        {
            validProperties = new Dictionary<string, bool>();
            validProperties.Add("Name", false);
            validProperties.Add("NumberPhone", false);
            validProperties.Add("Email", false);
        }
    }
}
