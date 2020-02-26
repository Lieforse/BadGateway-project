using System.Linq;
using System.Threading.Tasks;
using BadGateway.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BadGateway.ViewComponents
{
    public class SliderViewComponent : ViewComponent
    {
        private readonly AppDbContext appDbContext;

        public SliderViewComponent(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var posts = await appDbContext.Feeds
                .Where(f => f.Posts.Any())
                .Include(x => x.Posts)
                .ToDictionaryAsync(x => x.Name, x => x.Posts.OrderByDescending(p => p.DateCreated).FirstOrDefault());
            return View(posts);
        }
    }
}
