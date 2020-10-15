using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Data;
using Api.Models;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace Api.Helpers
{
    public static class Seed
    {
        public static async Task SeedData(DataContext context, UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            SeedTags(context);
            SeedGames(context);
            await SeedUser(userManager, roleManager);
        }

        static void SeedTags(DataContext _context)
        {
            if (!_context.Tags.Any())
            {
                var data = System.IO.File.ReadAllText("Datas/TagsData.json");
                var tags = JsonConvert.DeserializeObject<List<Tag>>(data);
                foreach (Tag tag in tags)
                {
                    _context.Tags.Add(tag);
                }
                _context.SaveChanges();
            }
        }

        static void SeedGames(DataContext _context)
        {
            if (!_context.Games.Any())
            {
                var data = System.IO.File.ReadAllText("Datas/GamesData.json");
                var games = JsonConvert.DeserializeObject<List<Game>>(data);
                foreach (Game game in games)
                {
                    _context.Games.Add(game);
                }
                _context.SaveChanges();
            }
        }
        static async Task SeedUser(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            var roles = new List<Role>
            {
                new Role{Name="Administrator"},
                new Role{Name="User"}
            };
            foreach (var role in roles)
            {
                await roleManager.CreateAsync(role);
            }
            var admins = new List<User>
            {
                new User{UserName = "Gun",Email = "Admin@gmail.com",BirthDate = DateTime.UtcNow,Gender = "Male"},
                new User{UserName = "Froung",Email = "Admin@gmail.com",BirthDate = DateTime.UtcNow,Gender = "Male"},
                new User{UserName = "Na",Email = "Admin@gmail.com",BirthDate = DateTime.UtcNow,Gender = "Male"}
            };

            foreach (var admin in admins)
            {
                admin.Image = new UserImage
                {
                    ImageUrl = "https://res.cloudinary.com/sgamer/image/upload/v1602169430/LogoandIcon/male2_f3uoca.jpg"
                };
                await userManager.CreateAsync(admin, "Pa$$W0rd12345678");
                await userManager.AddToRolesAsync(admin, new[] { "User", "Administrator" });
            }
        }
    }
}