using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BooksApp_Spring2024
{
    public class ApplicationUser : Microsoft.AspNetCore.Identity.IdentityUser
    {
        [Required]
        public string FullName { get; set; }

        public string? StreetAddress { get; set; }

        public string? City { get; set; }

        public string? PostalCode { get; set; }

        public string? State { get; set; }

        [NotMapped]
        public string? RoleName { get; set; }
    }
}
