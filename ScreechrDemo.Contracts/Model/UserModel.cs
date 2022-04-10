namespace ScreechrDemo.Contracts.Model
{
    public class UserModel
    {
        public ulong Id { get; init; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SecretToken { get; set; }
        public string ProfileImageUrl { get; set; }
    }
}
