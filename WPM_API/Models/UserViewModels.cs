using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace WPM_API.Models
{
    public class UserViewModel
    {
        public UserViewModel() { }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public Boolean Active { get; set; }
        public bool Admin { get; set; }
        public string Roles { get; set; }
        public SystemhouseRefViewModel Systemhouse { get; set; }
        public CustomerViewModel Customer { get; set; }
        
        // Wird erst beöntigt, wenn Membern subscriptions zugewiesen werden
         // public SubscriptionViewModel Subscriptions { get; set; }

    }

    public class AddUserViewModel
    {
        public AddUserViewModel() { }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Boolean Active { get; set; }
        public Boolean Admin { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public Role Role { get; set; }
        /// <summary>
        /// Id of Systemhouse; set with id if role=customer
        /// </summary>
        public string Systemhouse { get; set; }
        /// <summary>
        /// Id of Customer; set with id if role=systemhouse
        /// </summary>
        public string Customer { get; set; }
    }

    public class UserEditViewModel 
    {
        public UserEditViewModel() { }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public Boolean Active { get; set; }
        public Boolean Admin { get; set; }
        /// <summary>
        /// Id of Systemhouse; set with id if role=customer
        /// </summary>
        public string Systemhouse { get; set; }
        /// <summary>
        /// Id of Customer; set with id if role=systemhouse
        /// </summary>
        public string Customer { get; set; }
        public string Role { get; set; }
    }

    public class BugReportViewModel {
        public string UserId { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
    }

    public enum Role
    {
        none = 0,
        systemhouse = 1,
        customer = 2
    }
}
