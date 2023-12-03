using System.ComponentModel.DataAnnotations;

namespace Baranggay_Health_Records.Data
{
    public class SecretaryModel
    {
        public int ID { get; set; }

        [Required]
        public string? UserName { get; set; }

        [Required]

        public string? Password { get; set; }

        [Required]
        public string? FirstName { get; set; }

        [Required]
        public string? LastName { get; set; }

        public int? GetID()
        {
            return ID;
        }

        public string? GetAccountUserName()
        {
            return UserName;
        }

        public string? GetAccountPassword()
        {
            return Password;
        }
    }
}
