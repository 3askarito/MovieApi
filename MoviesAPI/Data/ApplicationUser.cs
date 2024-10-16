﻿using Microsoft.AspNetCore.Identity;
using MoviesAPI.Intefaces;

namespace MoviesAPI.Data
{
    public class ApplicationUser : IdentityUser
    {
        [Required, MaxLength(50)]
        public string FirstName { get; set; }

        [Required, MaxLength(50)]
        public string LastName { get; set; }
    }
}
