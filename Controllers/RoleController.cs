using IdentityApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityApp.Controllers
{
    public class RoleController:Controller{
        private readonly RoleManager<AppRole> _roleManeger;
        private readonly UserManager<AppUser> _userManager;
        public RoleController(RoleManager<AppRole> roleManager, UserManager<AppUser> userManager){
            _roleManeger = roleManager;
            _userManager = userManager;
        }

        public ActionResult Index(){
            return View(_roleManeger.Roles);
        }

        public IActionResult Create(){
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(AppRole model){
            if(model==null){
                return RedirectToAction("Error");
            }
            if(ModelState.IsValid){
               var result = await _roleManeger.CreateAsync(model);
               if(result.Succeeded){
                return RedirectToAction("Index");
               }
               foreach (var err in result.Errors)
               {
                    ModelState.AddModelError("",err.Description);
               }
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(string? id){
            if (id == null){
                return View("Error");
            }
            var role = await _roleManeger.FindByIdAsync(id);
            if(role!=null){
                return View("Delete", role);
            }
            else{
                return View("Error");
            }
        }
        [HttpPost]
        public async Task<IActionResult> DeletePost(string id){
            var role = await _roleManeger.FindByIdAsync(id);
            if (role == null)
            {
                return View("Error");
            }
            
            await _roleManeger.DeleteAsync(role);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Edit (string? id){
            if(id==null){return View("Error");}
            var role = await _roleManeger.FindByIdAsync(id);
            if(role==null){return View("Error");}
            if(role.Name!=null){
                var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name);
                ViewBag.Users = usersInRole;
            }
            return View("Edit", role);
        }
        [HttpPost]
        public async Task<IActionResult> Edit (string id, AppRole role){
            if(role==null){return View("Error");}
            if(id!=role.Id){return View("Error");}
            var changeRole = await _roleManeger.FindByIdAsync(id);
            if(changeRole!=null){
                changeRole.Name=role.Name;
                await _roleManeger.UpdateAsync(changeRole);
                return RedirectToAction("Index");
            }
            if(role.Name!=null){
                var users = await _userManager.Users
                .Where(x => _userManager.IsInRoleAsync(x, role.Name).Result)
                .ToListAsync();
                ViewBag.Users = users;
            }
            return View(role);
        }
    }
}