using MaterialDesignThemes.Wpf;
using Microsoft.IdentityModel.Tokens;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using VladimirVAZ3.Data;
using VladimirVAZ3.Scripts;

namespace VladimirVAZ3
{
    public partial class RentsPage : Page
    {
        private Prices _price = new ConnectTablesPrice().Prices.Where(x => x.id == IClientInfo.Instance.idSelectedVehicle).First();
        private Vehicles _vehicle = new ConnectTablesVehicles().Vehicles.Where(x => x.id == IClientInfo.Instance.idSelectedVehicle).First();

        private ConntectTablesRents _rents = new();

        private Menu _menu;
        private TimeSpan? _timeEnd;

        public RentsPage(Menu menu)
        {
            _menu = menu;
            AddShowRents.Instance.AttachMainWindow(this);
            InitializeComponent();

            NameVehicle.Content = _vehicle.Name;
            EndPrice.Content = Math.Round(_price.PriceHours, 2);

            _timeEnd = TimeSpan.FromHours(1);
        }

        private void Time_SelectedTimeChanged(object sender, RoutedPropertyChangedEventArgs<DateTime?> e)
        {
            TimePicker? Time = (TimePicker)sender;
           _timeEnd = Time.SelectedTime.Value.TimeOfDay;

            EndPrice.Content = $"{Math.Round((_price.PriceHours * Time.SelectedTime.Value.Hour) + ((_price.PriceHours / 60) * Time.SelectedTime.Value.Minute), 2)}";
        }

        private async void Page_Initialized(object sender, EventArgs e) => await AddShowRents.Instance.AddVehicle(HeightProperty); 

        private async void GetVehicle_Click(object sender, RoutedEventArgs e)
        {
            if (new ConntectTablesRents().Rents.Where(x => x.ClientID == IClientInfo.Instance.idCleint).IsNullOrEmpty())
            {
                await _rents.AddAsync(new Rents()
                {
                    ResultCountHours = _timeEnd,
                    ResultSummPrice = Convert.ToDouble(EndPrice.Content),
                    AdressID = 1,
                    ClientID = IClientInfo.Instance.idCleint,
                    PricesID = _price.id
                });

                _rents.SaveChanges();

                await TodoDb.Instance.UpdateRents(_price.id);

                ShowMessageService.Instance.Show(Colors.LightGreen, StaticValues.Instance.MessageIcon.ElementAt(2), "Успешно забронировано!");
            }

            else
                ShowMessageService.Instance.Show(Colors.LightPink, StaticValues.Instance.MessageIcon.ElementAt(0), "У вас уже имеется забронированя машина!");
        }

        private async void PackIcon_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e) => await Animations.Instance.AnimationHight(HeightProperty, Dispatcher, _menu.Rents, _menu.Rents.ActualHeight, 0f, TimeSpan.FromSeconds(1f));
    }
}
