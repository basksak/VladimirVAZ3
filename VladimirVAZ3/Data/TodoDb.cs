using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using VladimirVAZ3.Scripts;

namespace VladimirVAZ3.Data
{
    internal class TodoDb : DbContext
    {
        private static readonly Lazy<TodoDb> lazy = new(() => new TodoDb());

        private TodoDb() { }

        public static TodoDb Instance => lazy.Value;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlServer(StaticValues.Instance.Path);

        //public bool CheckLoginAndPasswordUsers(string function, params string[] values) => Database.ExecuteSqlRaw("EXEC UpdateTimeVehicles");
        public void UpdateTimeRents() => Database.ExecuteSqlRaw("EXEC UpdateTimeVehicles");
        public async Task UpdateRents(int IDPrice) => await Database.ExecuteSqlRawAsync("EXEC UpdateStateVehicles @id", new SqlParameter("id", IDPrice)); 
    }
}
