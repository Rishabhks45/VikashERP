using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using VikashERP.Infrastructure.Data;
using VikashERP.Domain.Entities;

namespace CheckUsersApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseNpgsql("Host=dpg-d94ekp8js32c73eb9mo0-a.singapore-postgres.render.com;Port=5432;Database=vikashironix;Username=vikashuser;Password=dKicZXGxtVTMp0odvGzGVT5CiIJsPN0k;SSL Mode=Require;Trust Server Certificate=true");
            
            using var context = new ApplicationDbContext(optionsBuilder.Options);
            
            var users = context.Users.ToList();
            Console.WriteLine($"Total users: {users.Count}");
            foreach (var user in users)
            {
                Console.WriteLine($"Id: {user.Id}, Name: {user.FirstName} {user.LastName}, Role: {user.Role}, RoleEnum: {(int)user.Role}, IsActive: {user.IsActive}");
            }
        }
    }
}
