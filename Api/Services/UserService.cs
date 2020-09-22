using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Data;
using Api.Dtos;
using Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MathNet.Numerics.Statistics;
using System.Linq;
using AutoMapper;

namespace Api.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public UserService(UserManager<User> userManager, DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
            _userManager = userManager;
        }

        public async Task<IEnumerable<UserToListDto>> GetUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            var userToList = _mapper.Map<IEnumerable<UserToListDto>>(users);
            return userToList;
        }

        public async Task<UserResponse> GetUser(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            return _mapper.Map<UserResponse>(user);
        }

        public async Task<ICollection<UserResponse>> Suggest(string userId)
        {
            var usersInterests = new List<UserInterest>();
            var interests = await _context.Interests.ToListAsync();
            var allUsers = await _context.Users.Include(f => f.Follower).ToListAsync();
            var userFollowing = await _context.Follows.Where(u => u.FollowerId == userId).Select(f => f.FollowingId).ToListAsync();
            var mainUser = allUsers.Where(u => u.Id == userId).First();
            var usersResponse = new List<UserResponse>();

            var mainUserGenresId = interests.Where(g => g.UserId == mainUser.Id).OrderBy(u => u.GenreId).Select(g => (double)g.IsInterest).ToList();
            if (mainUserGenresId == null || mainUserGenresId.Count == 0)
                return null;

            foreach (var user in allUsers)
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
            int count = 0;
            foreach (UserInterest userInterest in usersOrdered)
            {
                if (count >= 10)
                    break;
                var user = allUsers.Where(u => u.UserName == userInterest.Username).First();
                if (userFollowing.Contains(user.Id))
                    continue;
                usersResponse.Add(_mapper.Map<UserResponse>(user));
                count++;
            }
            return usersResponse;
        }

        public async Task<UserResponse> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var follows = await _context.Follows.Where(u => u.FollowerId == id || u.FollowingId == id).ToListAsync();
            if (user == null)
                return null;
            foreach (var follow in follows)
            {
                _context.Follows.Remove(follow);
            }
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return _mapper.Map<UserResponse>(user);
        }

        struct UserInterest
        {
            public string Username { get; set; }
            public List<double> GenresId { get; set; }
            public double Score { get; set; }
        }
    }
}