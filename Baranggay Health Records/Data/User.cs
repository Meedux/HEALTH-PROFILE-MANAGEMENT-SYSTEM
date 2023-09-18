using System.ComponentModel.DataAnnotations;

namespace Baranggay_Health_Records.Data
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public String? UserName { get; set; }

        [Required]

        public String? Password { get; set; }

        [Required]
        public String? FirstName {  get; set; }

        [Required]
        public String? LastName { get; set; }

        [Required]
        public String? AccountType { get; set; }
    }
}
