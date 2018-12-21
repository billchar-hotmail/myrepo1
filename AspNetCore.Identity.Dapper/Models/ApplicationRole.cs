using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace AspNetCore.Identity.Dapper
{
    public class ApplicationRole
    { 
        public ApplicationRole()
        {
            Id = RT.Comb.Provider.Sql.Create();

            Claims = new List<Claim>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        public string ConcurrencyStamp { get; set; }
        internal List<Claim> Claims { get; set; }
    }
}
