using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using VikashERP.Infrastructure.Data;

class Program
{
    static void Main()
    {
        var connectionString = "Host=localhost;Port=5432;Database=vikashironix;Username=postgres;Password=sa";

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseNpgsql(connectionString)
            .Options;

        using var db = new ApplicationDbContext(options);

        try
        {
            var conn = db.Database.GetDbConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT table_name FROM information_schema.tables WHERE table_schema = 'public' AND table_type = 'BASE TABLE';";
            using var reader = cmd.ExecuteReader();
            Console.WriteLine("Tables:");
            while (reader.Read())
            {
                Console.WriteLine(reader["table_name"]);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("ERROR: " + ex.ToString());
        }
    }
}
