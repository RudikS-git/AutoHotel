using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Windows;

using AutoHotel.Models;
using AutoHotel.ViewModel.Commands;
using AutoHotel.Interface;

namespace AutoHotel.ViewModel
{
    class UserIdentViewModel : ViewModelBase
    {
        private ObservableCollection<Worker> workers;
        private string connection;
        private string login;

        public UserIdentViewModel()
        {
            connection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            InitWorkers();

        }
        private void InitWorkers()
        {
            Worker.Select(out workers);
        }

        public string Login
        {
            get
            {
                return login;
            }

            set
            {
                login = value;
                OnPropertyChanged("Login");

            }
        }

        private RelayCommand input;
        public RelayCommand Input
        {
            get
            {
                // нарушение mvvm
                return input ?? (input = new RelayCommand(obj =>
                    {
                        var pb = obj as IClosable;
                        Worker currentUser;

                        try
                        {
                            string pass = pb.GetPassword();
                            currentUser = workers.Where(it => it.Login == Login && it.Pass == pass).SingleOrDefault();
                        }
                        catch
                        {
                            currentUser = null;
                            MessageBox.Show("В базе более одного работника с одинаковым логином", "Error");
                            return;
                        }

                        if (currentUser != null)
                        {

                            var add = new MainWindowViewModel()
                            {
                                Title = $"Автоматический отель : Авторизованный логин - {Login}",
                                Resize = ResizeMode.CanResize,
                                ButtonVisible = currentUser.Post == Enums.PostOrganization.Admin ? Visibility.Visible : Visibility.Collapsed,
                                Heigth = 500,
                                Width = 1000
                            };
                            
                            Show(add);

                            pb.Close();
                            MessageBox.Show(string.Format("{0}, вы успешно вошли", currentUser.Login));

                        }
                        else MessageBox.Show("Неверный логин или пароль");
                    },
                    obj =>
                        Login != null
                    ));
            }
        }
    }
}
