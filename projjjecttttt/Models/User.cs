using System.Collections;

namespace projjjecttttt.Models
{
    public class User
    {
        public string Id { get; set; }
        public string FullName { get; set; }

        public int Age { get; set; }
        public string Email { get; set; }

        public IEnumerable<string> Roles { get; set; }
    }
}
