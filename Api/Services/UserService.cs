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
            var users = await _userManager.Users.Include(i => i.Image).ToListAsync();
            var userToList = _mapper.Map<IEnumerable<UserToListDto>>(users);
            return userToList;
        }

        public async Task<UserResponse> GetUserById(string id)
        {
            var user = await _context.Users.Include(i => i.Image).Where(u => u.Id == id).FirstOrDefaultAsync();
            return _mapper.Map<UserResponse>(user);
        }

        public async Task<UserResponse> GetUserByUsername(string username)
        {
            var user = await _context.Users.Include(i => i.Image).Where(u => u.UserName == username).FirstOrDefaultAsync();
            return _mapper.Map<UserResponse>(user);
        }

        // public async Task<ICollection<UserToListDto>> Suggest(string userId)
        // {
        // var usersInterests = new List<UserInterest>();
        // var interests = await _context.Interests.ToListAsync();
        // var allUsers = await _context.Users.Include(f => f.Follower).ToListAsync();
        // var userFollowing = await _context.Follows.Where(u => u.FollowerId == userId).Select(f => f.FollowingId).ToListAsync();
        // var mainUser = allUsers.Where(u => u.Id == userId).First();
        // var usersToList = new List<UserToListDto>();

        // var mainUserGamesId = interests.Where(g => g.UserId == mainUser.Id).OrderBy(u => u.GameId).Select(g => (double)g.IsInterest).ToList();
        // if (mainUserGamesId == null || mainUserGamesId.Count == 0)
        //     return null;

        // foreach (var user in allUsers)
        // {
        //     if (user.Id == userId)
        //         continue;
        //     UserInterest userInterests = new UserInterest
        //     {
        //         Username = user.UserName,
        //         GenresId = interests.Where(g => g.UserId == user.Id).OrderBy(u => u.GameId).Select(g => (double)g.IsInterest).ToList(),
        //     };
        //     if (userInterests.GenresId.Count > 0)
        //     {
        //         userInterests.Score = Correlation.Pearson(mainUserGamesId, userInterests.GenresId);
        //         usersInterests.Add(userInterests);
        //     }
        // }
        // var usersOrdered = usersInterests.OrderByDescending(u => u.Score);
        // int count = 0;
        // foreach (UserInterest userInterest in usersOrdered)
        // {
        //     if (count >= 10)
        //         break;
        //     var user = allUsers.Where(u => u.UserName == userInterest.Username).First();
        //     if (userFollowing.Contains(user.Id))
        //         continue;
        //     usersToList.Add(_mapper.Map<UserToListDto>(user));
        //     count++;
        // }
        // return usersToList;
        // }

        public async Task<ICollection<UserToListDto>> Suggest(string userId)
        {
            var usersInterests = new List<UserInterest>();
            var allUsers = await _context.Users.Include(i => i.Interests).ToListAsync();
            var gamesId = await _context.Games.Select(i => i.GameId).ToListAsync();
            var mainUser = await _context.Users.Include(f => f.Following).Include(i => i.Interests).Where(u => u.Id == userId).FirstOrDefaultAsync();
            if (mainUser == null)
                return null;
            var following = mainUser.Following.Select(f => f.FollowingId);
            var empty = new double[gamesId.Count];
            for (int i = 0; i < gamesId.Count; i++)
            {
                empty[i] = 0;
            }
            var mainUserInterest = mainUser.Interests.Select(g => (double)g.GameId).ToList();
            var mainUserGamesId = new double[gamesId.Count];
            empty.CopyTo(mainUserGamesId, 0);
            foreach (var interest in mainUserInterest)
            {
                var index = gamesId.IndexOf((int)interest);
                if (index >= 0)
                    mainUserGamesId[index] = 1;
            }
            foreach (var user in allUsers)
            {
                if (user.Id == mainUser.Id || following.Contains(user.Id))
                    continue;
                var gamesInterest = user.Interests.Select(g => g.GameId).ToList();
                var userInterest = new UserInterest
                {
                    UserId = user.Id,
                    GamesId = new double[gamesId.Count],
                    Score = -1.0
                };
                empty.CopyTo(userInterest.GamesId, 0);
                foreach (var interest in gamesInterest)
                {
                    var index = gamesId.IndexOf(interest);
                    if (index >= 0)
                        userInterest.GamesId[index] = 1;
                }
                userInterest.Score = Correlation.Pearson(mainUserGamesId, userInterest.GamesId);
                usersInterests.Add(userInterest);
            }
            var usersOrdered = usersInterests.OrderByDescending(u => u.Score);
            var usersToList = new List<UserToListDto>();
            int count = 0;
            foreach (UserInterest userInterest in usersOrdered)
            {
                if (count >= 10)
                    break;
                var user = allUsers.Where(u => u.Id == userInterest.UserId).First();
                usersToList.Add(_mapper.Map<UserToListDto>(user));
                count++;
            }
            return usersToList;
        }

        public async Task<IdentityResult> UpdateUser(string userId, UserEditRequest userToEdit)
        {
            var user = await _userManager.FindByIdAsync(userId);
            // if (userToEdit.UserName != user.UserName && await _context.Users.AnyAsync(u => u.UserName == userToEdit.UserName))
            // {
            //     return null;
            // }
            _mapper.Map(userToEdit, user);
            var response = await _userManager.UpdateAsync(user);
            return response;
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
            public string UserId { get; set; }
            public double[] GamesId { get; set; }
            public double Score { get; set; }
        }
    }
}