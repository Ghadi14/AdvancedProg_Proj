using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HouseMates.Models;
using Microsoft.AspNetCore.Identity;

namespace HouseMates.Areas.Identity.Data;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
    public string firstname { get; set; }
    public string lastname { get; set; }
    public ICollection<House> Houses { get; set; } = new List<House>();
    public bool IsAdmin { get; set; }

}

