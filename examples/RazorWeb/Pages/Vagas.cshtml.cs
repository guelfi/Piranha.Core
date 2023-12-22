using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Piranha.Manager;
using Piranha.Manager.LocalAuth;
using System.ComponentModel.DataAnnotations;

namespace RazorWeb.Pages
{
    [Authorize]
    public class VagasModel : PageModel
    {
        public void OnGet(string returnUrl = null)
        {
        }
    }
}
