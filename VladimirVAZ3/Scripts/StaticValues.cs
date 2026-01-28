using MaterialDesignThemes.Wpf;
using System.Windows.Media;

namespace VladimirVAZ3.Scripts
{
    public class StaticValues
    {
        #region Instance
        private static readonly Lazy<StaticValues> lazy = new(() => new StaticValues());

        private StaticValues() { }

        public static StaticValues Instance => lazy.Value;

        #endregion

        public string Path { get; private set; } = "Data Source=LABANDAAA2\\SQLEXPRESS;Initial Catalog=VladimirVAZ;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False;Command Timeout=30";
        public Action UpdateRents;
        public Dictionary<PackIconKind, Color> MessageIcon { get; private set; } = new()
        {
            {PackIconKind.FaceAngryOutline, Colors.Red},
            {PackIconKind.FaceConfusedOutline, Colors.Orange},
            {PackIconKind.FaceOutline, Colors.Green},
        };

    }
}
