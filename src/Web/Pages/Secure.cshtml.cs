using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages;

[Authorize(Policy = "CheckUserClaims")]
public class SecureModel : PageModel
{
    public void OnGet()
    {
    }
}
