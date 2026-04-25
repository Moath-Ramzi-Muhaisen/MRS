using Domain.Entites;
using Domain.Enums;
using Infrastructre.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using static Domain.Entites.Role;

namespace Infrastructre.Data
{
    public static class UserSeedData
    {
        private readonly static string adminPassword = "Admin@123";
        public static void UserSeed(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            if (!context.Roles.Any())
            {
                var roles = new List<Role>
                {
                    new Role { Name = SystemRole.Admin.ToString(), Code = SystemRole.Admin },
                    new Role { Name = SystemRole.Employee.ToString(), Code = SystemRole.Employee },
                    new Role { Name = SystemRole.Technician.ToString(), Code = SystemRole.Technician }
                };
                context.Roles.AddRange(roles);
                context.SaveChanges();
            }

            if (!context.Users.Any())
            {
                var adminRoleId = context.Roles.FirstOrDefault(r => r.Code == SystemRole.Admin).Id;
                var user = new User
                {
                    Name = "Admin User",
                    Email = "admin@mrs.com",
                    PhoneNumber = "0785531213",
                    RoleId = adminRoleId,
                    Location = "Head Office"
                };

                var passwordHasher = new PasswordHasher<User>();
                user.Password = passwordHasher.HashPassword(user, adminPassword);

                context.Users.Add(user);
                context.SaveChanges();
            }
        }
    }
}