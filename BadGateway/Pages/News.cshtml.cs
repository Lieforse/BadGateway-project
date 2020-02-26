using BadGateway.DataAccess;
using BadGateway.DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BadGateway.Pages
{
    public class NewsModel : PageModel
    {
        private readonly AppDbContext appDbContext;

        public Post Post{ get; set; }

        public NewsModel(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public IActionResult OnGet(int id)
        {
            Post = appDbContext.Posts.Find(id);
            if (Post == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}