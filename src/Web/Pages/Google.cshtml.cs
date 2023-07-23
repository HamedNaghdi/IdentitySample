using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages
{
    public class GoogleModel : PageModel
    {
        public IActionResult OnGet(string? ReturnUrl)
        {
            //await HttpContext.ChallengeAsync("Google");

            if (!string.IsNullOrEmpty(ReturnUrl) && !Url.IsLocalUrl(ReturnUrl))
                throw new Exception("phishing attack");

            var props = new AuthenticationProperties
            {
                RedirectUri = Url.Page("/Callback"),

                Items =
                {
                    {"UltimateReturnUrl", ReturnUrl },
                    {"scheme", "Google" },
                }
            };

            return Challenge(props, "Google");
        }
    }
}
