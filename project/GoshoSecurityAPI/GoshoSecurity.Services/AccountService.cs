namespace GoshoSecurity.Services
{
    using GoshoSecurity.Data.Interfaces;
    using GoshoSecurity.Infrastructure;
    using GoshoSecurity.Models;
    using GoshoSecurity.Services.Interfaces;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;

    public class AccountService : IAccountService
    {
        private readonly UserManager<GoshoSecurityUser> userManager;
        private readonly IRepository<GoshoSecurityUser> usersRepository;
        private readonly Jwt jwtOptions;

        public object Endocing { get; private set; }

        public AccountService(UserManager<GoshoSecurityUser> userManager,
            IRepository<GoshoSecurityUser> usersRepository,
            IOptions<Jwt> jwtOptions)
        {
            this.userManager = userManager;
            this.usersRepository = usersRepository;
            this.jwtOptions = jwtOptions.Value;
        }

        public async Task<string> GetTokenForUser(string username, string password)
        {
            var user = await userManager.FindByNameAsync(username);

            if (user == null)
            {
                return null;
            }

            var isPasswordCorrect = await userManager.CheckPasswordAsync(user, password);

            if (!isPasswordCorrect)
            {
                return null;
            }

            var roleName = (await userManager.GetRolesAsync(user)).FirstOrDefault();

            var claims = new List<Claim>();

            if (roleName != null)
            {
                claims.Add(new Claim(ClaimTypes.Role, roleName));
            }

            claims.AddRange(new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
            });

            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtOptions.SecretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(14);

            var token = new JwtSecurityToken(
                issuer: jwtOptions.Issuer,
                audience: jwtOptions.Audience,
                claims: claims,
                expires: expires,
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token).ToString();
        }

        public async Task<bool> Delete(string userId)
        {
            var user = await this.usersRepository.All()
                .Include(p => p.Photos)
                .SingleOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return false;
            }

            try
            {
                usersRepository.Remove(user);
                await usersRepository.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
