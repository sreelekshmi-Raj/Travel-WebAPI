namespace WalksAPI.Models.Domain
{
    public class User_Role
    {
        //store all info of how user has role and how many roles for1 user
        public Guid Id { get; set; }

        //Add navigation property to user table
        public Guid UserId { get; set; }//auomatically map to userid 
        public User User { get; set; }//user table itself
        //connect to Role table
        public Guid RoleId { get; set; }
        public Role Role { get; set; }

    }
}
