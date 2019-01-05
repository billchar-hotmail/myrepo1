using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SecureNotesWebClient.Pages
{
    //[Authorize]
    public class TestModel : PageModel
    {
        public string Message { get; set; }

        public void OnGet()
        {
            Message = "This is a test page.";
            var s = User.Identity.Name;
            var s2 = s + "1";
        }
    }
}
