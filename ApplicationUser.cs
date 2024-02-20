using System.ComponentModel.DataAnnotations;

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
    }
}
