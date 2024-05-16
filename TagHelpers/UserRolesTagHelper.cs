using IdentityApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace IdentityApp.TagHelpers
{
    [HtmlTargetElement("td", Attributes ="asp-user-roles")]
    public class UserRolesTagHelper:TagHelper{
        private readonly RoleManager<AppRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public UserRolesTagHelper(RoleManager<AppRole> roleManager, UserManager<AppUser> userManager){
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HtmlAttributeName("asp-user-roles")]
        public string UserId { get; set; }=null!;

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            
            var user = await _userManager.FindByIdAsync(UserId);

            if(user!=null){
                var roles = await _userManager.GetRolesAsync(user);
                var rolesToList = roles.ToList();
                output.Content.SetHtmlContent(rolesToList.Count()==0 ? "No User" : setHtml(rolesToList) );
            }
        }

        private string setHtml(List<string> roleNames)
        {
            var html ="<ul>";

            foreach (var item in roleNames)
            {
                html+="<li>"+ item +"</li>";
            }

            html+="</ul>";
            return html;
        }
    }
}