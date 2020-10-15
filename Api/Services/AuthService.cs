using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using Microsoft.Extensions.Configuration;
using Api.Dtos;
using System.Linq;

namespace Api.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration configuration, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _configuration = configuration;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<LoginResponse> Login(LoginRequest model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, true);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(model.Username);

                return new LoginResponse { Token = await GenerateToken(user) };
            }
            return null;
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<IdentityResult> Register(RegisterRequest model)
        {
            User user = new User
            {
                UserName = model.Username,
                Email = model.Email,
                BirthDate = model.BirthDate,
                Gender = model.Gender,
                TimeCreate = DateTime.UtcNow
            };
            if (user.Gender == "Male")
            {
                user.Image = new UserImage
                {
                    ImageUrl = "https://res.cloudinary.com/sgamer/image/upload/v1602169430/LogoandIcon/male2_f3uoca.jpg"
                };
            }
            else
            {
                user.Image = new UserImage
                {
                    ImageUrl = "https://res.cloudinary.com/sgamer/image/upload/v1602169429/LogoandIcon/female2_mkbuy4.jpg"
                };
            }
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");
            }
            return result;
        }

        private async Task<string> GenerateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.Id),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var roles = await _userManager.GetRolesAsync(user);

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Secret"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var expires = DateTime.UtcNow.AddDays(7);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expires,
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}