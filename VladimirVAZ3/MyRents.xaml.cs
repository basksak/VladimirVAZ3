using Microsoft.IdentityModel.Tokens;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using VladimirVAZ3.Data;
using VladimirVAZ3.Scripts;
using System.Windows.Input;

namespace VladimirVAZ3
{
    public partial class MyRents : Page
    {
        public CancellationTokenSource token = new();
        private DispatcherTimer _updateIncome;
        private Menu _backPage;

        public MyRents(Menu backPage)
        {
            StaticValues.Instance.UpdateRents += UpdatePageRents;
            _backPage = backPage;
            InitializeComponent();

            _updateIncome = new()
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _updateIncome.Tick += Incomes;
            _updateIncome.Start();
        }

        private async void Incomes(object sender, EventArgs e)
        {
            // Получение первой записи из Rent, связанной с данным клиентом
            var rentRecord = new ConntectTablesRents().Rents
                                        .FirstOrDefault(x => x.ClientID == IClientInfo.Instance.idCleint);

            if (rentRecord != null && rentRecord.ResultCountHours.HasValue)
            {
                if (TimeSpan.Parse(Time.Content.ToString()) != TimeSpan.Zero)
                {
                    TodoDb.Instance.UpdateTimeRents();
                    Time.Content = rentRecord.ResultCountHours.Value;
                }
                else
                {
                    await TodoDb.Instance.UpdateRents(IClientInfo.Instance.idSelectedVehicle);
                    _updateIncome.Stop();

                    if (new ConntectTablesRents().Rents.Any(x => x.ClientID == IClientInfo.Instance.idCleint) == false)
                    {
                        NoneRents.Visibility = Visibility.Visible;
                        NameVehicle.Content = string.Empty;
                        Time.Visibility = Visibility.Hidden;
                    }
                }
            }
            else
            {
                NoneRents.Visibility = Visibility.Visible;
                NameVehicle.Content = string.Empty;
                Time.Visibility = Visibility.Hidden;
                MyRent.Children.Clear();
            }
        }

        private async void PackIcon_PreviewMouseDown(object sender, MouseButtonEventArgs e) 
        {
            await Animations.Instance.AnimationHight(HeightProperty, Dispatcher, _backPage.Rents, _backPage.Rents.ActualHeight, 0f, TimeSpan.FromSeconds(1f));

            await AddShowVehicle.Instance.AddVehicle(HeightProperty, default);
        }

        public async void UpdatePageRents()
        {
            if (new ConntectTablesRents().Rents.Where(x => x.ClientID == IClientInfo.Instance.idCleint).IsNullOrEmpty())
            {
                NoneRents.Visibility = Visibility.Visible;
                Time.Visibility = Visibility.Hidden;
            }

            else
            {
                AddMyRents.Instance.AttachMainWindow(this);
                await AddMyRents.Instance.AddVehicle(HeightProperty);
                NameVehicle.Content = AddMyRents.Instance._vehicle.Name;

                NoneRents.Visibility = Visibility.Hidden;
                Time.Visibility = Visibility.Visible;
            }
        }
    }
}
