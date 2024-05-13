using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityApp.Controllers
{
    public class UserController: Controller{

        private UserManager<IdentityUser> _userManager;

        public UserController(UserManager<IdentityUser> userManager){
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index(){
            return View(_userManager.Users);
        }

    }
}