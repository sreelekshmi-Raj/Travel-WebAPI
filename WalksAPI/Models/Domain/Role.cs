namespace WalksAPI.Models.Domain
{
    public class Role
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        //navigation property
        public List<User_Role> UserRoles { get; set; }

    }
}
