using IdentityApp.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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

        [HttpGet]
        public IActionResult Create(){
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateViewModel model){
            if (ModelState.IsValid){
                var user = new IdentityUser{
                    UserName = model.UserName,
                    Email = model.Email,
                };

                IdentityResult result = await _userManager.CreateAsync(user,model.Password);

                if(result.Succeeded){
                    return RedirectToAction("Index");
                }

                foreach (IdentityError err in result.Errors)
                {
                    ModelState.AddModelError("",err.Description);
                }
            }
                return View(model); 
        }
    }
}