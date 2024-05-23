using IdentityApp.Models;
using IdentityApp.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace IdentityApp.Controllers
{
    public class UserController: Controller{

        private UserManager<AppUser> _userManager;
        private RoleManager<AppRole> _roleManager; 

        public UserController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager){
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult Index(){
            return View(_userManager.Users);
        }


        [HttpGet]
        public IActionResult Create(){
            if(!User.IsInRole("King")){
                return RedirectToAction("Login","Account");
            }
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
            if(!User.IsInRole("King")){
                return RedirectToAction("Login","Account");
            }
            if (id == null){
                return RedirectToAction("Error");
            }
            ViewBag.Roles = await _roleManager.Roles.Select(i=>i.Name).ToListAsync();
            var user = await _userManager.FindByIdAsync(id);

            if(user!=null){
                return View(new EditViewModel{
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    SelectedRole = await _userManager.GetRolesAsync(user)
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
                        await _userManager.RemoveFromRolesAsync(user, await _userManager.GetRolesAsync(user));
                        if(model.SelectedRole!=null){
                            await _userManager.AddToRolesAsync(user, model.SelectedRole);
                        }
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

        [HttpGet]
        public async Task<IActionResult> Delete(string? id){
            if(!User.IsInRole("King")){
                return RedirectToAction("Login","Account");
            }
            if (id == null){
                return View("Error");
            }
            var user = await _userManager.FindByIdAsync(id);
            if(user!=null){
                return View("Delete", user);
            }
            else{
                return View("Error");
            }
        }
        [HttpPost]
        public async Task<IActionResult> DeletePost(string id){
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return View("Error");
            }
            
            await _userManager.DeleteAsync(user);
            return RedirectToAction("Index");
        }
    }
}