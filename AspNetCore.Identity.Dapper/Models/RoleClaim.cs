using System;

namespace AspNetCore.Identity.Dapper
{
    internal class RoleClaim
    {
        public string Id { get; set; }
        public Guid RoleId { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
    }
}
