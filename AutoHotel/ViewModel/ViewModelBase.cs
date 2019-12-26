using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace AutoHotel.ViewModel
{
    class ViewModelBase : DependencyObject, INotifyPropertyChanged
    {
        private
            ChildWindow _wnd = null;

        public string Title
        {
            get 
            { 
                return (string)GetValue(TitleProperty); 
            }
            set 
            { 
                SetValue(TitleProperty, value); 
            }
        }

        public ResizeMode Resize
        {
            get
            {
                return (ResizeMode)GetValue(ResizeProperty);
            }
            set
            {
                SetValue(ResizeProperty, value);
            }
        }

        public int Heigth
        {
            get
            {
                return (int)GetValue(HeighthProperty);
            }
            set
            {
                SetValue(HeighthProperty, value);
            }
        }

        public int Width
        {
            get
            {
                return (int)GetValue(WidthProperty);
            }
            set
            {
                SetValue(WidthProperty, value);
            }
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(ViewModelBase), new PropertyMetadata(""));
        public static readonly DependencyProperty ResizeProperty =
            DependencyProperty.Register("Resize", typeof(ResizeMode), typeof(ViewModelBase), new PropertyMetadata(null));

        public static readonly DependencyProperty HeighthProperty =
            DependencyProperty.Register("Heigth", typeof(int), typeof(ViewModelBase), new PropertyMetadata(null));

        public static readonly DependencyProperty WidthProperty =
            DependencyProperty.Register("Width", typeof(int), typeof(ViewModelBase), new PropertyMetadata(null));

        /// Метод вызываемый окном при закрытии
        protected virtual void Closed()
        {
            
        }

        /// Методы вызываемый для закрытия окна связанного с ViewModel
        public bool Close()
        {
            var result = false;
            if (_wnd != null)
            {
                _wnd.Close();
                _wnd = null;
                result = true;
            }
            return result;
        }

        /// Метод показа ViewModel в окне
        protected void Show(ViewModelBase viewModel)
        {
            viewModel._wnd = new ChildWindow();
            viewModel._wnd.DataContext = viewModel;
            viewModel._wnd.Closed += (sender, e) => Closed();
            viewModel._wnd.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            viewModel._wnd.Show();
        }

        protected void ShowDialog(ViewModelBase viewModel)
        {
            viewModel._wnd = new ChildWindow();
            viewModel._wnd.DataContext = viewModel;
            viewModel._wnd.Closed += (sender, e) => Closed();
            viewModel._wnd.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            viewModel._wnd.ShowDialog();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
