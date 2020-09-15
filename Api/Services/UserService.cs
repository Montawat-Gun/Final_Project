using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Data;
using Api.Dtos;
using Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MathNet.Numerics.Statistics;
using System.Linq;

namespace Api.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly DataContext _context;

        public UserService(UserManager<User> userManager, DataContext context)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<User> GetUser(string username)
        {
            return await _userManager.FindByNameAsync(username);
        }

        public async Task<ICollection<UserResponse>> Suggest(string userId)
        {
            var usersInterests = new List<UserInterest>();
            var interests = await _context.Interest.ToListAsync();
            var users = await _context.Users.ToListAsync();
            var mainUser = users.Where(u => u.Id == userId).First();
            var usersResponse = new List<UserResponse>();

            var mainUserGenresId = interests.Where(g => g.UserId == mainUser.Id).OrderBy(u => u.GenreId).Select(g => (double)g.IsInterest).ToList();
            if (mainUserGenresId == null)
                return null;

            foreach (var user in users)
            {
                if (user.Id == userId)
                    continue;
                UserInterest userInterests = new UserInterest
                {
                    Username = user.UserName,
                    GenresId = interests.Where(g => g.UserId == user.Id).OrderBy(u => u.GenreId).Select(g => (double)g.IsInterest).ToList(),
                };
                if (userInterests.GenresId.Count > 0)
                {
                    userInterests.Score = Correlation.Pearson(mainUserGenresId, userInterests.GenresId);
                    usersInterests.Add(userInterests);
                }
            }
            var usersOrdered = usersInterests.OrderByDescending(u => u.Score);
            foreach (UserInterest userInterest in usersOrdered)
            {
                var user = users.Where(u => u.UserName == userInterest.Username).First();
                usersResponse.Add(MapUser(user));
            }
            return usersResponse;
        }

        private UserResponse MapUser(User user)
        {
            return new UserResponse
            {
                Username = user.UserName,
                Email = user.Email,
                Gender = user.Gender,
                BirthDate = user.BirthDate
            };
        }

        struct UserInterest
        {
            public string Username { get; set; }
            public List<double> GenresId { get; set; }
            public double Score { get; set; }
        }
    }
}