using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SecureNotesWpfClient.Helpers
{
    public static class BoolHelper
    {
        public static bool ParseOrDefault(string value, bool defaultValue = false)
        {
            bool x;
            if (bool.TryParse(value, out x))
                return x;
            else
                return defaultValue;
        }
    }
}