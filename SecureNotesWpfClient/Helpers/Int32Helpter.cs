using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SecureNotesWpfClient.Helpers
{
    public static class Int32Helper
    {
        public static Int32 ParseOrDefault(string value, Int32 defaultValue = 0)
        {
            Int32 x;
            if (Int32.TryParse(value, out x))
                return x;
            else
                return defaultValue;
        }
    }
}