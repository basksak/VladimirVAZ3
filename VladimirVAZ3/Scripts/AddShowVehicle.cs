using Microsoft.EntityFrameworkCore;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using VladimirVAZ3.Data;

namespace VladimirVAZ3.Scripts
{
    public class AddShowVehicle
    {
        #region Singleton Implementation
        private static readonly Lazy<AddShowVehicle> lazy = new(() => new AddShowVehicle());

        public static AddShowVehicle Instance => lazy.Value;
        #endregion

        #region Fields and Properties
        private Menu _mainWindow;

        private AddShowVehicle()
        {
            // Ничего не делаем, так как экземпляр должен быть уникальным
        }

        public void AttachMainWindow(Menu mainWindow) => _mainWindow = mainWindow ?? throw new ArgumentNullException(nameof(mainWindow));
        #endregion

        public async Task AddVehicle(DependencyProperty Parent, Func<Vehicles, bool>? func = null)
        {
            List<Vehicles> _listVehicle = new();
            List<ImagesVehicles> _listImage = new();
            List<Prices> _listPrice = new();

            await _mainWindow.Dispatcher.InvokeAsync(() =>
            {
                _listVehicle = new ConnectTablesVehicles().Vehicles.ToList();
                _listImage = new ConnectTablesImagesVehicles().ImagesVehicles.ToList();
                _listPrice = new ConnectTablesPrice().Prices.ToList();
            });

            string resource = string.Empty;

            if (_mainWindow.Vechicle.Children.Count > 0)
                _mainWindow.Vechicle.Children.Clear();

            IEnumerable<Vehicles> filteredVehicles = func != null ? _listVehicle.Where(func) : _listVehicle;

            foreach (var vehicle in filteredVehicles)
            {
                if (vehicle.State)
                    resource = "ClosedVehicle";

                else resource = "BuyVehicle";

                ContentControl? Vehicle = new ContentControl
                {
                    ContentTemplate = (DataTemplate)_mainWindow.FindResource(resource),
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

                _mainWindow.Vechicle.Children.Add(Vehicle);

                await Animations.Instance.AnimationHight(Parent, _mainWindow.Dispatcher, Vehicle, 0f, 220f, TimeSpan.FromSeconds(2f));
                await Task.Delay(100);
            }
        }
    }
}
