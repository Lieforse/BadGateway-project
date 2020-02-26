using System;
using System.Linq;
using System.Threading.Tasks;
using BadGateway.DataAccess;
using BadGateway.DataAccess.Models;
using BadGateway.Dto;
using BadGateway.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BadGateway.Pages
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext appDbContext;
        private readonly IEmailService emailService;

        private int pageSize = 10;

        public PaginatedList<Post> Posts { get; set; }

        public IndexModel(AppDbContext appDbContext, IEmailService emailService)
        {
            this.appDbContext = appDbContext;
            this.emailService = emailService;
        }

        public async Task<IActionResult> OnGetAsync(int? feed, string search, int pageNum = 1)
        {
            var query = appDbContext.Posts.AsQueryable();

            if (feed.HasValue)
            {
                query = query.Where(p => p.Feed.Id == feed);
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(p => p.Title.ToLower().Contains(search.ToLower()));
            }

            Posts = await PaginatedList<Post>.CreateAsync(query, pageNum, this.pageSize);

            return Page();
        }

        public IActionResult OnPostSubscribe(string email, string userName)
        {
            this.emailService.SendWelcomeEmail(email, userName);
            SaveSubscriber(email, userName);

            return RedirectToPage(nameof(Index));
        }

        private void SaveSubscriber(string email, string userName)
        {
            var subscriber = new Subscriber
            {
                Email = email,
                Name = userName
            };

            appDbContext.Subscribers.Add(subscriber);
            appDbContext.SaveChanges();
        }
    }
}
