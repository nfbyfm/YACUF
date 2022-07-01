using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YACUF.Utilities
{
    /// <summary>
    /// utility-class with functions for strings
    /// </summary>
    public static class StringUtil
    {
        #region check-functions
        /// <summary>
        /// checks if given string is not null, empty, or full of whitespaces
        /// </summary>
        /// <param name="text">string to check</param>
        /// <returns>true if string isn't empty, null, or just whitespaces</returns>
        public static bool IsValidString(this string text)
        {
            return !string.IsNullOrEmpty(text) && !string.IsNullOrWhiteSpace(text);
        }

        #endregion
    }
}
