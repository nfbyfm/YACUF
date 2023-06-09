using System.Diagnostics.CodeAnalysis;

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
        /// <param name="value">string to check</param>
        /// <returns>true if string isn't empty, null, or just whitespaces</returns>
        public static bool IsValidString([NotNullWhen(true)] this string? value)
        {
            if(value!=null)
                return !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value);
            else
                return false;
        }

        
        #endregion
    }
}
