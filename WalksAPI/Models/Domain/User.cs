using System.ComponentModel.DataAnnotations.Schema;

namespace WalksAPI.Models.Domain
{
    public class User
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [NotMapped]
        public List<string> Roles { get; set; }
        //list of Roles- many to many relnship - create class User_Role
        //Navigation prop
        public List<User_Role> UserRoles { get; set; }


    }
}
