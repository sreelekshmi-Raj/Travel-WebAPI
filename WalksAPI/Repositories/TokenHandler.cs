using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WalksAPI.Models.Domain;

namespace WalksAPI.Repositories
{
    public class TokenHandler : ITokenHandler
    {
        private readonly IConfiguration configuration;

        public TokenHandler(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public Task<string> CreateToken(User user)
        {
            

            //create claims
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.GivenName, user.FirstName));
            claims.Add(new Claim(ClaimTypes.Surname, user.LastName));
            claims.Add(new Claim(ClaimTypes.Email, user.EmailAddress));

            //adding all roles in user to claim section
            // Loop into roles of users
            user.Roles.ForEach (role =>
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            });

            //create credentials
            //create token -> create signing credential(takes symmetric key) and pass this credential to JWT security token

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var credentials=new SigningCredentials(key,SecurityAlgorithms.HmacSha256);
            //create token - it takes values of issuer,audience and claims

            var token = new JwtSecurityToken(
                configuration["Jwt:Issuer"],
                configuration["Jwt:Audience"],
                claims,
                expires:DateTime.Now.AddMinutes(15),
                signingCredentials:credentials
                );

            //write the token using JWT security token handler

            return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
            //can't return as async method
        }
    }
}
