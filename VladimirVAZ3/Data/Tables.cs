using System.ComponentModel.DataAnnotations;

namespace VladimirVAZ3.Data
{
    public class Client
    {
        [Key]
        public int id { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Fathername { get; set; }
        public int SeriesPassport { get; set; }
        public int NumberPassport { get; set; }
        public char[]? Category { get; set; }
        public DateTime DateReceivesRools { get; set; }
        public int Stage {  get; set; }
        public long NumberDriversRools { get; set; }
        public DateTime DateBirthday { get; set; }
        public string? Phone { get; set; }
    }

    public class Users
    {
        [Key]
        public int id { get; set; }
        public string? Login { get; set; }
        public string? Password { get; set; }
        public string? Mail { get; set; }
        public int UserID { get; set; }
    }

    public class TypesVehicles
    {
        [Key] 
        public int id { get; set; }
        public string? Type { get; set; }
    }

    public class Prices
    {
        [Key]
        public int id { get; set; }
        public decimal PriceHours { get; set; }
        public int DiscountForBirthday { get; set; }
    }

    public class Rents
    {
        [Key]
        public int id { get; set; }
        public TimeSpan? ResultCountHours { get; set; }
        public double? ResultSummPrice { get; set; }
        public int AdressID { get; set; }
        public int ClientID { get; set; }
        public int PricesID { get; set; }
    }

    public class ImagesVehicles
    {
        [Key]
        public int id { get; set; }
        public string? Url { get; set; }
    }

    public class Vehicles
    {
        [Key]
        public int id { get; set; }
        public string? Name { get; set; }
        public DateTime? YearRelease { get; set; }
        public int CountHourDrives { get; set; }
        public int PassadPlace { get; set; }
        public char? CPP { get; set; }
        public bool State { get; set; }
        public int TypesID { get; set; }
    }
}
