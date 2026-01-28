namespace VladimirVAZ3.Scripts
{
    public class IClientInfo
    {
        #region Singleton Implementation
        private static readonly Lazy<IClientInfo> lazy = new(() => new IClientInfo());

        public static IClientInfo Instance => lazy.Value;
        #endregion

        public int idCleint;
        public int idSelectedVehicle;
    }
}
