﻿using System;

namespace AspNetCore.Identity.Dapper
{
    internal class UserClaim
    {
        public string Id { get; set; }
        public Guid UserId { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
    }
}
