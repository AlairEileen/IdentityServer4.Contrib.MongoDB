


namespace IdentityServer4.MongoDB.Models
{
    public abstract class UserClaim
    {
        public int Id { get; set; }
        public string Type { get; set; }
    }
}