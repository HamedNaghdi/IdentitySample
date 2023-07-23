using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace Web.Pages.Account;

public class LoginModel : PageModel
{
    [BindProperty]
    public string? Username { get; set; }

    [BindProperty]
    public string? Password { get; set; }

    [BindProperty(SupportsGet =true)]
    public string? ReturnUrl { get; set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPost()
    {
        if (!string.IsNullOrEmpty(Username) && Password == "123")
        {
            //ok
            var claims = new List<Claim>
            {
                //new("sub", new Guid().ToString()),
                new("sub", Username),
                new("name", "Hamed"),
                new("role", "Admin")
            };

            var ci = new ClaimsIdentity(claims, authenticationType: "pwd", nameType: "name", roleType: "role");

            var prop = new AuthenticationProperties
            {
                Items =
                {
                    {"key1", "value1"},
                    {"key2", "value2" }
                }
            };
            
            var cp = new ClaimsPrincipal(ci);

            await HttpContext.SignInAsync(cp, prop);

            return LocalRedirect(ReturnUrl ?? "/");
        }

        return Page();
    }
}
