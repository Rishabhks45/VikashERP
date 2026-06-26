using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using VikashERP.Infrastructure.Data;
using VikashERP.SharedKernel.Services;

class Program
{
    static void Main()
    {
        var connectionString = "Host=localhost;Port=5432;Database=vikashironix;Username=postgres;Password=sa";
        var masterKey = "aU5FU1RIQY5NUzU3Q1JFVEtFWTk4NzY1NDMyMUFCQ0RFRkdISUdLTE1OTw==";

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseNpgsql(connectionString)
            .Options;

        using var db = new ApplicationDbContext(options);
        var encryptor = new EncryptionService();

        try
        {
            var users = db.Users.ToList();
            Console.WriteLine($"Found {users.Count} users:");
            foreach (var u in users)
            {
                string decryptedPassword = "N/A";
                try
                {
                    if (!string.IsNullOrEmpty(u.PasswordHash))
                    {
                        decryptedPassword = encryptor.Decrypt(u.PasswordHash, masterKey);
                    }
                }
                catch (Exception ex)
                {
                    decryptedPassword = $"Decryption Error: {ex.Message}";
                }

                Console.WriteLine($"- Email: {u.Email} | Role: {u.Role} | Name: {u.FirstName} {u.LastName} | Password: {decryptedPassword}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("ERROR: " + ex.ToString());
        }
    }
}
