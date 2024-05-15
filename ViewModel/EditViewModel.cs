using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityApp.ViewModel
{
    public class EditViewModel{
        public string? Id { get; set; }
        
        public string? UserName { get; set;}  
        
        [EmailAddress]
        public string? Email { get; set;}  
        
        [DataType(DataType.Password)]
        public string? Password { get; set;} 
        
        [DataType(DataType.Password)]
        [Compare(nameof(Password),ErrorMessage ="Passwords doesn't match")]
        [NotMapped]
        public string? ConfirmPassword { set; get;} 

        public IList<string>? SelectedRole { get; set; }
    }
}