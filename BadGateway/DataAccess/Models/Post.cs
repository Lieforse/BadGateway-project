using System;

namespace BadGateway.DataAccess.Models
{
    public class Post
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public DateTime DateCreated { get; private set; } = DateTime.UtcNow;

        public string ImageUrl { get; set; }

        public string SourceUrl { get; set; }

        public string Description { get; set; }

        public string Content { get; set; }

        public Category Category { get; set; }

        public Feed Feed { get; set; }
    }
}
