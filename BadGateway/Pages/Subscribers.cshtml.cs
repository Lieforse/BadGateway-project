using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BadGateway.DataAccess;
using BadGateway.DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BadGateway.Pages
{
    public class SubscribersModel : PageModel
{
        private readonly AppDbContext appDbContext;
        public List<Subscriber> Subscribers { get; set; }
        public SubscribersModel(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }
        public void OnGet()
        {
            Subscribers = appDbContext.Subscribers.ToList();
        }
    }
}