using System;
using System.Collections.Generic;
using System.Text;

namespace CoreIdentityDemo.Common.Util
{
    public static class ExceptionUtil
    {
        public static void ThrowIfNull(object value, string name)
        {
            if (value == null)
                throw new ArgumentNullException(name);
        }

        public static void ThrowIfNullOrWhiteSpace(string value, string name)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Value cannot be null or empty", name);
        }
    }
}
