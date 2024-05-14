using IdentityApp.Models;
using IdentityApp.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace IdentityApp.Controllers
{
    public class UserController: Controller{

        private UserManager<AppUser> _userManager;

        public UserController(UserManager<AppUser> userManager){
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
                var user = new AppUser{
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

        [HttpGet]
        public async Task<IActionResult> Edit(string id){
            if (id == null){
                return RedirectToAction("Error");
            }

            var user = await _userManager.FindByIdAsync(id);

            if(user!=null){
                return View(new EditViewModel{
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                });
            }

            return RedirectToAction("Error");

        }
        [HttpPost]
        public async Task<IActionResult> Edit(string id, EditViewModel model){
            if (id == null){
                return RedirectToAction("Index");
            }
            if(id!=model.Id){
                return View(model);
            }
            if(ModelState.IsValid){
                var user = await _userManager.FindByIdAsync(model.Id);
                if(user!=null){
                    user.Email = model.Email;
                    user.UserName = model.UserName;
                    var result = await _userManager.UpdateAsync(user);

                    if(result.Succeeded && !string.IsNullOrEmpty(model.Password)){
                        await _userManager.RemovePasswordAsync(user);
                        await _userManager.AddPasswordAsync(user, model.Password);
                        TempData["SuccessMessage"] = "Password Successfully Changed!";
                        TempData["UserName"]=model.UserName;
                    }

                    if(result.Succeeded){
                        return RedirectToAction("Index");
                    }
                    foreach (IdentityError err in result.Errors)
                    {
                        ModelState.AddModelError("", err.Description);
                    }
                }
            }
            return View(model);
        }
    }
}