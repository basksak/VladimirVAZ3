using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using VladimirVAZ3.Data;

namespace VladimirVAZ3.Scripts
{
    public class AddShowRents : IClientInfo
    {
        #region Singleton Implementation
        private static readonly Lazy<AddShowRents> lazy = new(() => new AddShowRents());

        public static AddShowRents Instance => lazy.Value;
        #endregion

        #region Fields and Properties
        private RentsPage _mainWindow;

        private AddShowRents()
        {
            // Ничего не делаем, так как экземпляр должен быть уникальным
        }

        public void AttachMainWindow(RentsPage mainWindow) => _mainWindow = mainWindow ?? throw new ArgumentNullException(nameof(mainWindow));
        #endregion

        private readonly List<Vehicles> _listVehicle = new ConnectTablesVehicles().Vehicles.ToList();
        private readonly List<ImagesVehicles> _listImage = new ConnectTablesImagesVehicles().ImagesVehicles.ToList();
        private readonly List<Prices> _listPrice = new ConnectTablesPrice().Prices.ToList();

        public async Task AddVehicle(DependencyProperty Parent)
        {
            if (_mainWindow.Rents.Children.Count > 0)
                _mainWindow.Rents.Children.Clear();

            foreach (var vehicle in _listVehicle.Where(x => x.id == IClientInfo.Instance.idSelectedVehicle))
            {
                ContentControl? Vehicle = new ContentControl
                {
                    ContentTemplate = (DataTemplate)_mainWindow.FindResource("RentsVehicle"),
                    Content = new DataForTicketCar
                    {
                        id = vehicle.id,
                        ImageVehicle = new BitmapImage(new Uri(_listImage.Where(x => x.id == vehicle.id).First().Url)),
                        NameVehicle = vehicle.Name,
                        YearEnter = "Выпуск " + vehicle.YearRelease.Value.ToShortDateString(),
                        CPP = "КПП " + vehicle.CPP.ToString(),
                        PlaceSeat = "Мест " + vehicle.PassadPlace.ToString(),
                        Price = "Цена.ч " + Math.Round(_listPrice.Where(x => x.id == vehicle.id).First().PriceHours, 2).ToString() + "p"
                    }
                };

                _mainWindow.Rents.Children.Add(Vehicle);
            }
        }
    }
}
