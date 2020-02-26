using System.Linq;
using System.Threading.Tasks;
using BadGateway.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BadGateway.ViewComponents
{
    public class LatestPostsViewComponent : ViewComponent
    {
        private readonly AppDbContext appDbContext;

        public LatestPostsViewComponent(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var posts = await appDbContext.Posts.OrderByDescending(p => p.DateCreated).Take(3).ToListAsync();
            return View(posts);
        }
    }
}
