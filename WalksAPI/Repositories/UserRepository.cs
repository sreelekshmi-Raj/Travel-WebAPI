using Microsoft.EntityFrameworkCore;
using WalksAPI.Data;
using WalksAPI.Models.Domain;

namespace WalksAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly WalksDbContext walkDbContext;

        public UserRepository(WalksDbContext walkDbContext)
        {
            this.walkDbContext = walkDbContext;
        }
        public async Task<User> Authenticate(string username, string password)
        {
            var user=await walkDbContext.Users.FirstOrDefaultAsync(x => x.UserName.ToLower() == username.ToLower() && x.Password == password);
            if(user==null)
            {
                return null;
            }
            var userRoles = await walkDbContext.UserRoles.Where(x => x.UserId == user.Id).ToListAsync();
            if(userRoles.Any())
            {
                user.Roles = new List<string>();
                foreach(var userRole in userRoles)
                {
                    var role= walkDbContext.Roles.FirstOrDefault(x => x.Id == userRole.RoleId);
                    if(role!=null)
                    {
                        user.Roles.Add(role.Name);
                    }
                }
            }
            user.Password = null;
            return user;
           
        }
    }
}
