using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using VladimirVAZ3.Data;

namespace VladimirVAZ3.Scripts
{
    public class AddMyRents
    {
        #region Singleton Implementation
        private static readonly Lazy<AddMyRents> lazy = new(() => new AddMyRents());

        public static AddMyRents Instance => lazy.Value;
        #endregion

        #region Fields and Properties
        private MyRents _mainWindow;

        private AddMyRents()
        {

        }

        public void AttachMainWindow(MyRents mainWindow) => _mainWindow = mainWindow ?? throw new ArgumentNullException(nameof(mainWindow));
        #endregion

        private Rents? _rents;
        private List<ImagesVehicles> _listImage;
        private List<Prices> _listPrice;
        public Vehicles? _vehicle { get; private set; }

        public async Task AddVehicle(DependencyProperty Parent)
        {
            await UpdateData();

            if (_mainWindow.MyRent.Children.Count > 0)
                _mainWindow.MyRent.Children.Clear();

            ContentControl? Vehicle = new ContentControl
            {
                ContentTemplate = (DataTemplate)_mainWindow.FindResource("RentsVehicle"),
                Content = new DataForTicketCar
                {
                    id = _vehicle.id,
                    ImageVehicle = new BitmapImage(new Uri(_listImage.Where(x => x.id == _vehicle.id).First().Url)),
                    NameVehicle = _vehicle.Name,
                    YearEnter = "Выпуск " + _vehicle.YearRelease.Value.ToShortDateString(),
                    CPP = "КПП " + _vehicle.CPP.ToString(),
                    PlaceSeat = "Мест " + _vehicle.PassadPlace.ToString(),
                    Price = "Цена.ч " + Math.Round(_listPrice.Where(x => x.id == _vehicle.id).First().PriceHours, 2).ToString() + "p"
                }
            };

            _mainWindow.MyRent.Children.Add(Vehicle);
        }

        private async Task UpdateData()
        {
            await _mainWindow.Dispatcher.InvokeAsync(() =>
            {
                _rents = new ConntectTablesRents().Rents.Where(x => x.ClientID == IClientInfo.Instance.idCleint).First();
                _listImage = new ConnectTablesImagesVehicles().ImagesVehicles.ToList();
                _listPrice = new ConnectTablesPrice().Prices.ToList();
                _vehicle = new ConnectTablesVehicles().Vehicles.Where(x => x.id == _rents.PricesID).First();
            });
        }
    }
}
