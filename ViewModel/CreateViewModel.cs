using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityApp.ViewModel
{
    public class CreateViewModel{
        [Required]
        public string UserName { get; set;} = string.Empty;
        [Required]
        [EmailAddress]
        public string Email { get; set;} = string.Empty;
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set;} = string.Empty;
        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password),ErrorMessage ="Passwords doesn't match")]
        [NotMapped]
        public string ConfirmPassword { set; get;} = string.Empty;
    }
}