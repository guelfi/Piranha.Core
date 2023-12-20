using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Piranha.Manager;
using Piranha.Manager.LocalAuth;
using System.ComponentModel.DataAnnotations;

namespace RazorWeb.Pages
{
    public class LoginModel : PageModel
    {
        private readonly ISecurity _service;
        private readonly ManagerLocalizer _localizer;

        [BindProperty]
        [Required]
        public string Username { get; set; }

        [BindProperty]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public LoginModel(ISecurity service, ManagerLocalizer localizer)
        {
            _service = service;
            _localizer = localizer;
        }

        public void OnGet(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            await _service.SignOut(HttpContext);

            if (!ModelState.IsValid || (await _service.SignIn(HttpContext, Username, Password)) != LoginResult.Succeeded)
            {
                ModelState.Clear();
                ModelState.AddModelError(string.Empty, _localizer.General["Username and/or password are incorrect."].Value);
                return Page();
            }


            if (!string.IsNullOrEmpty(returnUrl))
            {
                return LocalRedirect($"~/login/auth?returnUrl={returnUrl}");
            }
            return LocalRedirect("~/login/auth");
        }
    }
}
