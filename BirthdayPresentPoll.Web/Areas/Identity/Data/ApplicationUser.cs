using Microsoft.AspNetCore.Identity;
using System;

namespace BirthdayPresentPoll.Web.Areas.Identity.Data
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime Birthday { get; set; }
    }
}
