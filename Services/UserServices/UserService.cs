using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MyInstagramApi.Model;
using MyInstagramApi.ViewModels;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MyInstagramApi.Services.UserServices
{
    public class UserService : IUserService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JWT _jwt;

        public List<string> ErrorMsg;

        public UserService(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<JWT> jwt)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwt = jwt.Value;

            ErrorMsg = new List<string>();
        }

        public async Task<List<string>> RegisterAsync(RegisterModel model)
        {
            string errormsg = null;
            var EmailExist = await _userManager.FindByEmailAsync(model.Email);
            var UsernameExist = await _userManager.FindByNameAsync(model.Username);

            if(EmailExist != null || UsernameExist != null)
            {
                errormsg =$"Account with {((EmailExist != null) ? $"Email {model.Email}" : $"Username {model.Username}" )}  aready exist";
               
                ErrorMsg.Add(errormsg);
                return ErrorMsg;
            }

            User user = new User()
            {
                Email = model.Email,
                DateOfBirth = model.DateOfBirth,
                CreatedAt = DateTime.Now,
                CountryName = model.CountryName,
                PhoneNumber = model.PhoneNumber,
                UserName = model.Username
            };

            IdentityResult result = await _userManager.CreateAsync(user, model.Password);

            if(result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");
                return null;
            }

            if(result.Errors != null)
            {
               foreach(var Error in result.Errors)
                {
                    ErrorMsg.Add(Error.ToString());
                }
            }

            return ErrorMsg;
        }

        public async Task<AuthenticationModel> GetTokenAsync(TokenRequestModel model)
        {
            AuthenticationModel authenticationModel = new AuthenticationModel();
            var user = await _userManager.FindByEmailAsync(model.Email);

            if(user == null)
            {
                authenticationModel.IsAuthenticated = false;
                authenticationModel.Message = $"No user registered with Email {model.Email}";
                return authenticationModel;
            }

            if(await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var roles = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
                JwtSecurityToken jwtSecurityToken = await CreateJwtToken(user);

                authenticationModel.Email = user.Email;
                authenticationModel.ID = user.Id;
                authenticationModel.IsAuthenticated = true;
                authenticationModel.Roles = roles.ToList();
                authenticationModel.UserName = user.UserName;
                authenticationModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
                return authenticationModel;
            }
            authenticationModel.IsAuthenticated = false;
            authenticationModel.Message = "User cresidential is incorrect";
            return authenticationModel;
        }

        private async Task<JwtSecurityToken> CreateJwtToken(IdentityUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            List<Claim> Roles = new List<Claim>();

            foreach(string role in roles)
            {
                Roles.Add(new Claim("role", role));
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(Roles);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha512);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(_jwt.DurationInDays),
                signingCredentials: signingCredentials);
            return jwtSecurityToken;

        }
    }
}
