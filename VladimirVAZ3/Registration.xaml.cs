using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using VladimirVAZ3.Scripts;

namespace VladimirVAZ3
{
    public partial class Registration : Page
    {
        private MainWindow _logFrame;
        public Registration(MainWindow Registration)
        {
            _logFrame = Registration;
            InitializeComponent();
            CheckData.Instance.AttachMainWindow(this);
        }

        private async void RegistrationButton_Click(object sender, RoutedEventArgs e) => await CheckData.Instance.CheckDataAndAdd();

        private void FIO_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox Fio = (TextBox)sender;
            Fio.Text = Regex.Replace(Fio.Text, @"\s+", " ").Trim();
        }

        private async void Back_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e) => await Animations.Instance.AnimationHight(HeightProperty, Dispatcher, _logFrame.RegistrationFrame, _logFrame.RegistrationFrame.ActualHeight, 0f, TimeSpan.FromSeconds(1f));
    }
}
