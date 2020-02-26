using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BadGateway.DataAccess;
using BadGateway.DataAccess.Models;
using BadGateway.Services.Abstractions;
using CodeHollow.FeedReader;
using CodeHollow.FeedReader.Feeds;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BadGateway.HostedServices
{
    public class PostGrabberService : BackgroundService
    {
        private readonly ILogger<PostGrabberService> logger;
        private readonly IServiceProvider services;
        private readonly IWebHostEnvironment host;

        public PostGrabberService(
            ILogger<PostGrabberService> logger,
            IServiceProvider services,
            IWebHostEnvironment host)
        {
            this.logger = logger;
            this.services = services;
            this.host = host;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            this.logger.LogInformation($"ContentRootPath: {host.ContentRootPath}");
            this.logger.LogInformation($"WebRootPath: {host.WebRootPath}");
            this.logger.LogInformation("Grabber started...");

            while (!stoppingToken.IsCancellationRequested)
            {
                var posts = new List<Post>();

                using (var scope = this.services.CreateScope())
                {
                    var appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    var feeds = await appDbContext.Feeds.ToListAsync();

                    var addedPosts = new List<Post>();
                    var random = new Random();
                    foreach (var feed in feeds)
                    {
                        try
                        {
                            this.logger.LogInformation("Start grabbing {feedName} on {feedUrl}...", feed.Name, feed.Url);

                            var rss = await FeedReader.ReadAsync(feed.Url);
                            var feedPosts = rss.Items
                                .Where(i => !feed.DateUpdated.HasValue || (i.PublishingDate.HasValue && i.PublishingDate > feed.DateUpdated))
                                .Select(feedItem => new Post
                                {
                                    Title = feedItem.Title,
                                    Content = feedItem.Content,
                                    Description = feedItem.Description,
                                    ImageUrl = (feedItem.SpecificItem as MediaRssFeedItem)?.Media.FirstOrDefault()?.Url
                                        ?? $"https://picsum.photos/id/{random.Next(1, 1080)}/1000/500",
                                    SourceUrl = feedItem.Link
                                })
                                .ToList();

                            feedPosts.ForEach(feed.Posts.Add);
                            addedPosts.AddRange(feed.Posts);
                            this.logger.LogInformation("Grabbed {postNum} posts", feedPosts.Count);
                        }
                        catch (Exception ex)
                        {
                            this.logger.LogError(ex, $"Issue on parsing {feed.Name}");
                            throw;
                        }
                        feed.DateUpdated = DateTime.UtcNow;
                    }

                    await appDbContext.SaveChangesAsync();

                    var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
                    var subscibers = await appDbContext.Subscribers.ToListAsync();
                    NotifySubscribers(emailService, subscibers, addedPosts);
                }

                await Task.Delay(TimeSpan.FromMinutes(1));
            }
        }

        private void NotifySubscribers(
            IEmailService emailService,
            IEnumerable<Subscriber> subscribers,
            IEnumerable<Post> posts)
        {
            this.logger.LogInformation("Start notification sending...");

            foreach (var subscriber in subscribers)
            {
                foreach (var post in posts) {
                    emailService.SendNewPostNotification(subscriber, post);
                }
            }
        }
    }
}
