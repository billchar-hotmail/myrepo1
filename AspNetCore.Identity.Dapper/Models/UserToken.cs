﻿using System;

namespace AspNetCore.Identity.Dapper
{
    internal class UserToken
    {
        public Guid UserId { get; set; }
        public string LoginProvider { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
