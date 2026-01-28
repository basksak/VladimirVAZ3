using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Windows;
using System.Windows.Media;
using VladimirVAZ3.Data;
using VladimirVAZ3.Scripts;

namespace VladimirVAZ3
{
    public partial class MainWindow : Window
    {
        private ConnectTablesUsers? connectTablesUsers = new();
        private Registration _reg;
        public MainWindow()
        {
            _reg = new(this);
            InitializeComponent();
            ShowMessageService.Instance.AttachMainWindow(this);
        }

        private async void RegistrationButton_Click(object sender, RoutedEventArgs e)
        {
            RegistrationFrame.Navigate(_reg);
            await Animations.Instance.AnimationHight(HeightProperty, Dispatcher, RegistrationFrame, 0f, BorderLogOn.ActualHeight, TimeSpan.FromSeconds(1f));
        }

        private async void EnterButton_Click(object sender, RoutedEventArgs e)
        {
            if (CheckText())
            {
                IClientInfo.Instance.idCleint = new ConnectTablesUsers().Users.Where(x => x.Login == Login.Text && x.Password == Password.Password).First().UserID;
                Menu.Navigate(new Menu());
            }
        }

        private bool CheckText()
        {
            if (string.IsNullOrEmpty(Login.Text) || string.IsNullOrWhiteSpace(Login.Text)
                && string.IsNullOrEmpty(Password.Password) || string.IsNullOrWhiteSpace(Password.Password))
            {
                ShowMessageService.Instance.Show(Colors.LightPink, StaticValues.Instance.MessageIcon.ElementAt(0), "Поля не должны быть пустые");
                return false;
            }

            else if (Login.Text.Length < 6 || Password.Password.Length < 6)
            {
                ShowMessageService.Instance.Show(Colors.LightPink, StaticValues.Instance.MessageIcon.ElementAt(0), "Логин/Пароль должен быть больше 6 символов");
                return false;
            }

            else if (connectTablesUsers.Users.Where(x => x.Login == Login.Text && x.Password == Password.Password).IsNullOrEmpty())
            {
                ShowMessageService.Instance.Show(Colors.LightPink, StaticValues.Instance.MessageIcon.ElementAt(0), "Не верный Пароль/Логин");
                return false;
            }

            else
            {
                ShowMessageService.Instance.Show(Colors.LightGreen, StaticValues.Instance.MessageIcon.ElementAt(2), "Добро пожаловать");
                return true;
            }
        }
    }
}