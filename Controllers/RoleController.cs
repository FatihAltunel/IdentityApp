using IdentityApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityApp.Controllers
{
    public class RoleController:Controller{
        private readonly RoleManager<AppRole> _roleManeger;
        public RoleController(RoleManager<AppRole> roleManager){
            _roleManeger = roleManager;
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
    }
}