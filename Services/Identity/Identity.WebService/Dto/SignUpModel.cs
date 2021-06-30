using System.ComponentModel.DataAnnotations;

namespace Identity.WebService.Dto
{
    public class SignUpModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}