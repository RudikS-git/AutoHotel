using System.Windows;

using AutoHotel.Interface;
using AutoHotel.ViewModel;

namespace AutoHotel.View
{
    public partial class UserIdent : Window, IClosable
    {
        public UserIdent()
        {
            InitializeComponent();
            Loaded += UserIdent_Loaded;
        }

        private void UserIdent_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = new UserIdentViewModel();
        }

        public string GetPassword()
        {
            return PB.Password;
        }
    }
}
