using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aspire_POS.Models
{
    [Table("AspNetUsers")]
    public class AspNetUserModel : IdentityUser
    {
        [Key]
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }

        public virtual HostCredentialsModel HostCredentials { get; set; }
    }
}
