using System;
using Microsoft.EntityFrameworkCore;
using VikashERP.Infrastructure.Data;

var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=vikashironix;Username=postgres;Password=sa");

using var context = new ApplicationDbContext(optionsBuilder.Options);
using var conn = context.Database.GetDbConnection();
conn.Open();

void PrintState(string stage)
{
    using var cmd = conn.CreateCommand();
    cmd.CommandText = "SELECT relname, relkind FROM pg_class WHERE LOWER(relname) = 'expenses';";
    using var reader = cmd.ExecuteReader();
    bool found = false;
    while (reader.Read())
    {
        found = true;
        Console.WriteLine($"[{stage}] Found: Name='{reader.GetString(0)}', Kind='{reader.GetChar(1)}'");
    }
    if (!found)
    {
        Console.WriteLine($"[{stage}] No relation matching 'expenses' was found.");
    }
}

PrintState("BEFORE DROP");

using (var cmdDrop = conn.CreateCommand())
{
    cmdDrop.CommandText = "DROP TABLE IF EXISTS \"Expenses\" CASCADE;";
    cmdDrop.ExecuteNonQuery();
    Console.WriteLine("Executed DROP TABLE command.");
}

PrintState("AFTER DROP");
