using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace Web.Pages;

public class CallbackModel : PageModel
{
    public async Task<IActionResult> OnGet()
    {
        //read the response of external Auth (stored in temp scheme cookie)
        var result = await HttpContext.AuthenticateAsync("temp");

        if (!result.Succeeded)
            throw new Exception("exteranl Auth failed.");

        var externalUser = result.Principal;
        var sub = externalUser.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var issuer = result.Properties.Items["scheme"];

        //logic
        // ...

        //sign in
        var claims = new List<Claim>
        {
            //new("sub", new Guid().ToString()),
            new("sub", $"{Guid.NewGuid()}-{sub}"),
            new("name-identity", externalUser.Identity.Name),
            new("name", externalUser.FindFirst(ClaimTypes.Name)?.Value),
            new("email", externalUser.FindFirst(ClaimTypes.Email)?.Value),
            new("role", "Admin")
        };

        var ci = new ClaimsIdentity(claims, authenticationType: issuer, nameType: "name", roleType: "role");
        var cp = new ClaimsPrincipal(ci);

        await HttpContext.SignInAsync(cp);
        //get rid of temp cookie
        await HttpContext.SignOutAsync("temp");

        var ultimateReturnUrl = result.Properties.Items["UltimateReturnUrl"];
        if (string.IsNullOrEmpty(ultimateReturnUrl))
            ultimateReturnUrl = "/";
        return Redirect(ultimateReturnUrl);
    }
}
