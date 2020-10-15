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
        private readonly IImageService _imageService;

        public UserService(UserManager<User> userManager, IImageService imageService, DataContext context, IMapper mapper)
        {
            _imageService = imageService;
            _mapper = mapper;
            _context = context;
            _userManager = userManager;
        }

        public async Task<IEnumerable<UserToList>> GetUsers()
        {
            var users = await _userManager.Users.Include(i => i.Image).ToListAsync();
            var userToList = _mapper.Map<IEnumerable<UserToList>>(users);
            return userToList;
        }

        public async Task<UserDetail> GetUserById(string id)
        {
            var user = await _context.Users.Where(u => u.Id == id).Include(i => i.Image).FirstOrDefaultAsync();
            return _mapper.Map<UserDetail>(user);
        }

        public async Task<UserDetail> GetUserByUsername(string username)
        {
            var user = await _context.Users.Where(u => u.NormalizedUserName == username.ToUpper()).Include(i => i.Image)
            .Include(i => i.Interests).ThenInclude(g => g.Game)
            .FirstOrDefaultAsync();
            var userDetail = _mapper.Map<UserDetail>(user);

            userDetail.FollowingCount = await _context.Follows.Where(u => u.FollowerId == user.Id).CountAsync();
            userDetail.FollowerCount = await _context.Follows.Where(u => u.FollowingId == user.Id).CountAsync();
            return userDetail;
        }

        public async Task<ICollection<UserToList>> Suggest(string userId)
        {
            var usersInterests = new List<UserInterest>();
            var allUsers = await _context.Users.Include(i => i.Image).Include(i => i.Interests).ToListAsync();
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
            var usersToList = new List<UserToList>();
            int count = 0;
            foreach (UserInterest userInterest in usersOrdered)
            {
                if (count >= 5)
                    break;
                var user = allUsers.Where(u => u.Id == userInterest.UserId).First();
                usersToList.Add(_mapper.Map<UserToList>(user));
                count++;
            }
            return usersToList;
        }

        public async Task<IdentityResult> UpdateUser(string userId, UserEditRequest userToEdit)
        {
            var user = await _userManager.FindByIdAsync(userId);
            _mapper.Map(userToEdit, user);
            var result = await _userManager.UpdateAsync(user);
            return result;
        }

        public async Task<IdentityResult> UpdateUserPassword(string userId, UserEditPasswordRequest passwordToEdit)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return null;
            var result = await _userManager.ChangePasswordAsync(user, passwordToEdit.CurrentPassword, passwordToEdit.NewPassword);
            return result;
        }

        public async Task<UserDetail> DeleteUser(string id)
        {
            var user = await _context.Users.Where(x => x.Id == id).Include(i => i.Image)
            .Include(i => i.Interests).Include(r => r.UserRole).FirstOrDefaultAsync();
            var follows = await _context.Follows.Where(u => u.FollowerId == id || u.FollowingId == id).ToListAsync();
            var messages = await _context.Messages.Where(x => x.RecipientId == id || x.SenderId == id).ToListAsync();
            var notifications = await _context.Notifications.Where(x => x.RecipientId == id).ToListAsync();
            var posts = await _context.Posts.Where(x => x.UserId == id).Include(i => i.Image).ToListAsync();
            if (user == null)
                return null;
            foreach (var follow in follows)
            {
                _context.Follows.Remove(follow);
            }
            foreach (var interest in user.Interests)
            {
                _context.Interests.Remove(interest);
            }
            foreach (var message in messages)
            {
                _context.Messages.Remove(message);
            }
            foreach (var notification in notifications)
            {
                _context.Notifications.Remove(notification);
            }
            foreach (var post in posts)
            {
                await _imageService.DeletePostImage(post.PostId);
                foreach (var comment in post.Comments)
                {
                    _context.Comments.Remove(comment);
                }
                foreach (var like in post.Likes)
                {
                    _context.Likes.Remove(like);
                }
                _context.Posts.Remove(post);
            }
            foreach (var role in user.UserRole)
            {
                _context.UserRoles.Remove(role);
            }
            _context.UserImages.Remove(user.Image);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return _mapper.Map<UserDetail>(user);
        }

        public async Task<IEnumerable<UserToList>> SearchUser(string userId, string searchString)
        {
            var users = await _context.Users.Where(u => u.NormalizedUserName.Contains(searchString.ToUpper()) && u.Id != userId)
            .Include(i => i.Image).ToListAsync();
            return _mapper.Map<IEnumerable<UserToList>>(users);
        }

        struct UserInterest
        {
            public string UserId { get; set; }
            public double[] GamesId { get; set; }
            public double Score { get; set; }
        }
    }
}