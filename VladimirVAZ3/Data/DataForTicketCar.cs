using System.Windows.Media.Imaging;

namespace VladimirVAZ3.Data
{
    class DataForTicketCar
    {
        public int id {  get; set; }
        public BitmapImage? ImageVehicle { get; set; } = new();
        public string? NameVehicle { get; set; }
        public string? YearEnter {  get; set; }
        public string? CPP {  get; set; }
        public string? PlaceSeat { get; set; }
        public string? Price { get; set; }
    }
}
