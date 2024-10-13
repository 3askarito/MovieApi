using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MoviesAPI.AuthorizationModels;
using MoviesAPI.Helpers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MoviesAPI.Services
{
    public class AuthServices(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IOptions<JWT> option) : IAuthServices
    {
        private readonly JWT jwt = option.Value;
        public async Task<AuthModel> RegisterAsync(RegisterModel model)
        {
            if (await userManager.FindByNameAsync(model.UserName) is not null)
                return new AuthModel { Message = "UserName Is Already Registered" };
            if (await userManager.FindByEmailAsync(model.Email) is not null)
                return new AuthModel { Message = "Email Is Already Registered" };
            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName
            };
            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                var errors = string.Empty;
                foreach (var error in result.Errors)
                    errors += $"{error.Description}\n";
                return new AuthModel { Message = errors };
            }
            await userManager.AddToRoleAsync(user, "User");
            var JwtSecurityToken = await CreateToken(user);
            return new AuthModel
            {
                UserName = user.UserName,
                Email = user.Email,
                IsAuthenticated = true,
                Roles = new List<string> { "User" },
                ExpiresOn = JwtSecurityToken.ValidTo,
                Token = new JwtSecurityTokenHandler().WriteToken(JwtSecurityToken)
            };
        }
        public async Task<AuthModel> GetTokenAsync(TokenRequestModel model)
        {
            var authModel = new AuthModel();

            var user = await userManager.FindByEmailAsync(model.Email);

            if (user is null || !await userManager.CheckPasswordAsync(user, model.Password))
            {
                authModel.Message = "Email or Password is incorrect!";
                return authModel;
            }

            var jwtSecurityToken = await CreateToken(user);
            var rolesList = await userManager.GetRolesAsync(user);

            authModel.IsAuthenticated = true;
            authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            authModel.Email = user.Email;
            authModel.UserName = user.UserName;
            authModel.ExpiresOn = jwtSecurityToken.ValidTo;
            authModel.Roles = rolesList.ToList();

            return authModel;
        }
        public async Task<string> AddRoleAsync(AddRoleModel model)
        {
            var user = await userManager.FindByIdAsync(model.UserId);
            if (user is null || !await roleManager.RoleExistsAsync(model.RoleName))
                return "Invalid User id or Role !!!";
            if (await userManager.IsInRoleAsync(user, model.RoleName))
                return "User already Assigned to this Role!!!"; ;
            var result = await userManager.AddToRoleAsync(user, model.RoleName);
            return result.Succeeded? string.Empty : "Something Went Wrong";
        }
        private async Task<JwtSecurityToken> CreateToken(ApplicationUser user)
        {
            var userCLaims = await userManager.GetClaimsAsync(user);
            var roles = await userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();
            foreach(var role in roles)
                roleClaims.Add(new Claim("roles", role));
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }.Union(roleClaims).Union(userCLaims);
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var JwtSecurityToken = new JwtSecurityToken(
                issuer: jwt.Issuer,
                signingCredentials: signingCredentials,
                audience: jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(jwt.DurationInDays)
                );
            return JwtSecurityToken;
        }
    }
}
