﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SecureNotesWebClient.Data
{
    public class ApplicationUser2 : IdentityUser
    {

        //[Required]
        public string FullName { get; set; }

        public ApplicationUser2()
        {

        }
    }
}
