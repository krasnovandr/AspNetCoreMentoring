using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCoreMentoring.Core
{
    public static class Guard
    {
        public static void ArgumentNotNull(string argumentName, object value)
        {
            if (value is null)
            {
                throw new ArgumentNullException(argumentName);
            }
        }

        public static void ArgumentNotNullOrWhiteSpace(string argumentName, string value)
        {
            ArgumentNotNull(argumentName, value);

            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Value cannot be an empty string.", argumentName);
            }
        }
    }
}
