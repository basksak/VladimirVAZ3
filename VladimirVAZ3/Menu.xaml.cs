using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using VladimirVAZ3.Data;
using VladimirVAZ3.Scripts;

namespace VladimirVAZ3
{
    public partial class Menu : Page
    {
        private readonly List<TypesVehicles> _types = [.. new ConnectTablesTypesVehicles().TypesVehicles];

        private readonly MyRents _myRents;

        public Menu()
        {
            InitializeComponent();

            _myRents = new(this);
            AddShowVehicle.Instance.AttachMainWindow(this);
        }

        private async void ButtonSorter_Click(object sender, RoutedEventArgs e)
        {
            Button? button = (Button)sender;
            string Name = button.Content.ToString();

            await AddShowVehicle.Instance.AddVehicle(HeightProperty, x => x.TypesID == _types.Where(x => x.Type.Equals(Name)).First().id);
        }

        private async void SortedActive_Click(object sender, RoutedEventArgs e) => await AddShowVehicle.Instance.AddVehicle(HeightProperty, x => !x.State);

        private async void SortedFull_Click(object sender, RoutedEventArgs e) => await AddShowVehicle.Instance.AddVehicle(HeightProperty, default);

        private async void Buy_Click(object sender, RoutedEventArgs e)
        {
            var Parent = VisualTreeHelper.GetParent(VisualTreeHelper.GetParent(VisualTreeHelper.GetParent(sender as Button) as Grid) as Grid) as Border;
            DataForTicketCar? data = Parent.DataContext as DataForTicketCar;

            IClientInfo.Instance.idSelectedVehicle = data.id;

            Rents.Navigate(new RentsPage(this));
            await Animations.Instance.AnimationHight(HeightProperty, Dispatcher, Rents, 0, ActualHeight, TimeSpan.FromSeconds(1f));
        }

        private async void Basket_Click(object sender, RoutedEventArgs e)
        {
            StaticValues.Instance.UpdateRents?.Invoke();
            Rents.Navigate(_myRents);
            await Animations.Instance.AnimationHight(HeightProperty, Dispatcher, Rents, 0, ActualHeight, TimeSpan.FromSeconds(1f));
        }
    }
}
