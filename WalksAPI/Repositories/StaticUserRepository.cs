using WalksAPI.Models.Domain;

namespace WalksAPI.Repositories
{
    public class StaticUserRepository : IUserRepository
    {
        private List<User> users = new List<User>()
        {
        //    new User()
        //    {
        //        FirstName="Read Only",
        //        LastName="User",
        //        EmailAddress="readonlyuser@gmail.com",
        //        Id=Guid.NewGuid(),
        //        UserName="readonly@user.com",
        //        Password="Readonly@user",
        //        Roles=new List<string>{"reader"}
        //    },
        //    new User()
        //    {
        //        FirstName="Read Write",
        //        LastName="User",
        //        EmailAddress="readwriteuser@gmail.com",
        //        Id=Guid.NewGuid(),
        //        UserName="readwrite@user.com",
        //        Password="Readwrite@user",
        //        Roles=new List<string>{"reader","writer"}
        //    }
        };
        public async Task<User> Authenticate(string username, string password)
        {
            var user= users.Find(x=>x.UserName.Equals(username,StringComparison.InvariantCultureIgnoreCase) 
            && x.Password==password);

            return user;

        }
    }
}
