using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorWeb.Pages
{
    [Authorize]
    public class SearchModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
