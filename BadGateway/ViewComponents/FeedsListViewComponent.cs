using System.Threading.Tasks;
using BadGateway.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BadGateway.ViewComponents
{
    public class FeedsListViewComponent : ViewComponent
    {
        private readonly AppDbContext appDbContext;

        public FeedsListViewComponent(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var feeds = await appDbContext.Feeds.ToListAsync();
            return View(feeds);
        }
    }
}
