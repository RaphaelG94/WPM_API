using System.Collections.Generic;

namespace  WPM_API.Data.DataContext.Projections.Users
{
    public class AccountProjection
    {
        public AccountProjection()
        {
            Roles = new string[0];
        }

        public string Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool Active { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string SystemhouseId { get; set; }
        public string SystemhouseName { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}
