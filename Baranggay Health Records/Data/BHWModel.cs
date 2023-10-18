using System.ComponentModel.DataAnnotations;

namespace Baranggay_Health_Records.Data
{
    public class BHWModel
    {
        public int ID { get; set; }

        [Required]
        public String? UserName { get; set; }

        [Required]
        public String? Password { get; set; }

        [Required]
        public String? FirstName { get; set; }

        [Required]
        public String? LastName { get; set; }

        public int? GetID()
        {
            return this.ID;
        }

        public String? GetAccountUserName()
        {
            return this.UserName;
        }

        public String? GetAccountPassword()
        {
            return this.Password;
        }

    }
}
