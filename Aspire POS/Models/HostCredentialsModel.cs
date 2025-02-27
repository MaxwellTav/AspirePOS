using Microsoft.AspNetCore.Identity;

namespace Aspire_POS.Models
{
    public class HostCredentialsModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string ClientKey { get; set; }
        public string ClientSecret { get; set; }
        public string ApiUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public virtual AspNetUserModel User { get; set; }
    }

}
