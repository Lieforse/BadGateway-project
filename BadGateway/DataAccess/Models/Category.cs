using System.Collections.Generic;

namespace BadGateway.DataAccess.Models
{
    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<Post> Posts { get; private set; } = new List<Post>();
    }
}
