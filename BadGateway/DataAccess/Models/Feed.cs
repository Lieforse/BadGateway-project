using System;
using System.Collections.Generic;

namespace BadGateway.DataAccess.Models
{
    public class Feed
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        public DateTime? DateUpdated { get; set; }

        public IList<Post> Posts { get; private set; } = new List<Post>();
    }
}
