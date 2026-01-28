using Microsoft.EntityFrameworkCore;
using PhoneNumbers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using VladimirVAZ3.Data;

namespace VladimirVAZ3.Scripts
{
    class CheckData
    {
        #region Singleton Implementation
        private static readonly Lazy<CheckData> lazy = new(() => new CheckData());

        public static CheckData Instance => lazy.Value;
        #endregion

        #region Fields and Properties
        private Registration _mainWindow;
        private readonly List<ContentControl> _queueMessage = new List<ContentControl>();
        #endregion

        private ConnectTablesClient? _client;
        private ConnectTablesUsers? _users;

        private char[] _category;
        private CheckData()
        {
            // Ничего не делаем, так как экземпляр должен быть уникальным
        }

        public void AttachMainWindow(Registration mainWindow)
        {
            _mainWindow = mainWindow ?? throw new ArgumentNullException(nameof(mainWindow));
        }

        public async Task<bool> CheckDataAndAdd()
        {
            _client = new();
            _users = new();

            string[]? Fio = _mainWindow.FIO.Text.Split(' ');

            if (string.IsNullOrWhiteSpace(_mainWindow.FIO.Text) || string.IsNullOrWhiteSpace(_mainWindow.DataNumberPassport.Text) ||
                string.IsNullOrWhiteSpace(_mainWindow.DataSeriesPassport.Text) || string.IsNullOrWhiteSpace(_mainWindow.NumberDriversRool.Text) || 
                string.IsNullOrEmpty(_mainWindow.NumberPhone.Text))
            {
                ShowMessageService.Instance.Show(Colors.LightPink, StaticValues.Instance.MessageIcon.ElementAt(0), "Даные должны быть заполнены");
                return false;
            }

            else if (Fio.Length < 3 || Fio.Length > 3)
            {
                ShowMessageService.Instance.Show(Colors.LightPink, StaticValues.Instance.MessageIcon.ElementAt(0), "Не верно введено ФИО");
                return false;
            }

            else if (
                _client.Client.Any(
                    x => (x.Name == Fio[0]) &&
                         (x.Surname == Fio[1]) &&
                         (x.Fathername == Fio[2])))
            {
                ShowMessageService.Instance.Show(Colors.LightPink, StaticValues.Instance.MessageIcon.ElementAt(0), "ФИО Зарегистрировано в бд");
                return false;
            }

            else if (_mainWindow.DataSeriesPassport.Text.Length != 4)
            {
                ShowMessageService.Instance.Show(Colors.LightPink, StaticValues.Instance.MessageIcon.ElementAt(0), "Серия паспорта должена состоять из 4 символов");
                return false;
            }

            else if (_mainWindow.DataNumberPassport.Text.Length != 8)
            {
                ShowMessageService.Instance.Show(Colors.LightPink, StaticValues.Instance.MessageIcon.ElementAt(0), "Номер паспорта должен состоять из 8 символов");
                return false;
            }

            else if (!CheckCategory(out string Message))
            {
                ShowMessageService.Instance.Show(Colors.LightPink, StaticValues.Instance.MessageIcon.ElementAt(0), Message);
                return false;
            }

            else if (_mainWindow.DateRool.SelectedDate is null || _mainWindow.DateBirthday.SelectedDate is null)
            {
                ShowMessageService.Instance.Show(Colors.LightPink, StaticValues.Instance.MessageIcon.ElementAt(0), "Дата не должна быть пустой");
                return false;
            }

            else if (_mainWindow.NumberDriversRool.Text.Length == 12)
            {
                ShowMessageService.Instance.Show(Colors.LightPink, StaticValues.Instance.MessageIcon.ElementAt(0), "Водительские данные должны содержать 12 символов");
                return false;
            }

            if (!_mainWindow.NumberDriversRool.Text.All(char.IsDigit) ||
                !_mainWindow.DataNumberPassport.Text.All(char.IsDigit) ||
                !_mainWindow.DataSeriesPassport.Text.All(char.IsDigit))
            {
                ShowMessageService.Instance.Show(Colors.LightPink, StaticValues.Instance.MessageIcon.ElementAt(0),
                                                 "Номер водительского удостоверения, серия и номер паспорта должны содержать только цифры без пробелов.");
                return false;
            }

            else if (!IsValidInternationalPhoneNumber(_mainWindow.NumberPhone.Text))
            {
                ShowMessageService.Instance.Show(Colors.LightPink, StaticValues.Instance.MessageIcon.ElementAt(0), "Не коректный номер телефона");
                return false;
            }

            else if (_mainWindow.Login.Text.Length < 6 || _mainWindow.Password.Text.Length < 6)
            {
                ShowMessageService.Instance.Show(Colors.LightPink, StaticValues.Instance.MessageIcon.ElementAt(0), "Пароль должен содержать больше 5 символов");
                return false;
            }

            else if (_users.Users.Any(x => x.Login == _mainWindow.Login.Text))
            {
                ShowMessageService.Instance.Show(Colors.LightPink, StaticValues.Instance.MessageIcon.ElementAt(0), "Логин уже существует");
                return false;
            }

            else if (!IsValidEmail())
            {
                ShowMessageService.Instance.Show(Colors.LightPink, StaticValues.Instance.MessageIcon.ElementAt(0), "Не корректная почта");
                return false;
            }

            else if (_users.Users.Any(x => x.Mail.Equals(_mainWindow.Email.Text)))
            {
                ShowMessageService.Instance.Show(Colors.LightPink, StaticValues.Instance.MessageIcon.ElementAt(0), "Почта уже существует");
                return false;
            }

            ShowMessageService.Instance.Show(Colors.LightGreen, StaticValues.Instance.MessageIcon.ElementAt(2), "Успешная регистрация");
            AddData(Fio);
            return true;
        }

        private async void AddData(string[] Fio)
        {
            await _client.Client.AddAsync(new()
            {
                Name = Fio[0],
                Surname = Fio[1],
                Fathername = Fio[2],
                SeriesPassport = int.Parse(_mainWindow.DataSeriesPassport.Text),
                NumberPassport = int.Parse(_mainWindow.DataNumberPassport.Text),
                Category = _category,
                DateReceivesRools = _mainWindow.DateRool.SelectedDate.Value.Date,
                Stage = _mainWindow.DateRool.SelectedDate.Value.Year - DateTime.Now.Year,
                NumberDriversRools = long.Parse(_mainWindow.NumberDriversRool.Text),
                DateBirthday = _mainWindow.DateBirthday.SelectedDate.Value.Date,
                Phone = _mainWindow.NumberPhone.Text
            });
            await _client.SaveChangesAsync();

            _users.Users.Add(new()
            {
                Login = _mainWindow.Login.Text,
                Password = _mainWindow.Password.Text,
                Mail = _mainWindow.Email.Text,
                UserID = _client.Client.OrderBy(x => x.id).LastAsync().Result.id,
            });
            _users.SaveChanges();
        }

        private bool IsValidEmail()
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(_mainWindow.Email.Text);
                return addr.Address == _mainWindow.Email.Text;
            }
            catch
            {
                return false;
            }
        }
        private bool IsValidInternationalPhoneNumber(string input)
        {
            PhoneNumberUtil phoneUtil = PhoneNumberUtil.GetInstance();
            try
            {
                PhoneNumber parsedNumber = phoneUtil.Parse($"+7{input}", ""); // Строка "" означает автоопределение страны
                return phoneUtil.IsValidNumber(parsedNumber);
            }
            catch (NumberParseException)
            {
                return false;
            }
        }
        private bool CheckCategory(out string Message)
        {
            List<ComboBox> boxes = FindVisualChildren<ComboBox>(_mainWindow.ListCategory).ToList();
            List<char> Category = [];
            HashSet<string> uniqueCategories = new HashSet<string>(StringComparer.OrdinalIgnoreCase); // Храним уникальные строки
            bool anySelected = false;

            foreach (var box in boxes)
            {
                string? selectedValue = string.Empty;

                if (box.SelectedItem is Label selectedLabel) selectedValue = selectedLabel.Content?.ToString();

                if (!string.IsNullOrEmpty(selectedValue))
                {
                    anySelected = true;

                    // Пробуем добавить категорию в уникальный набор
                    if (!uniqueCategories.Add(selectedValue))
                    {
                        Message = $"Категория указана дважды";
                        return false;
                    }

                    Category.Add(char.Parse(selectedValue));
                }
            }

            if (!anySelected)
            {
                Message = "Нужно выбрать хотя бы одну категорию";
                return false;
            }

            _category = Category.ToArray();
            Message = string.Empty;
            return true;
        }
        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child is T typedChild)
                    {
                        yield return typedChild;
                    }

                    foreach (T otherTypedChild in FindVisualChildren<T>(child))
                    {
                        yield return otherTypedChild;
                    }
                }
            }
        }
    }
}
