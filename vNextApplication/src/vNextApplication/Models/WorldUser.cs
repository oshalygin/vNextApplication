using System;
using Microsoft.AspNet.Identity.EntityFramework;

namespace vNextApplication.Models
{
    public class WorldUser : IdentityUser
    {
        public DateTime FirstTrip { get; set; }
    }
}