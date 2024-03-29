﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using YACUF.Models;

namespace YACUF.Extensions
{
    /// <summary>
    /// utility-class with functions for strings
    /// </summary>
    public static class StringExtension
    {
        #region check-functions
        /// <summary>
        /// checks if given string is not null, empty, or full of whitespaces
        /// </summary>
        /// <param name="value">string to check</param>
        /// <returns>true if string isn't empty, null, or just whitespaces</returns>
        public static bool IsValidString([NotNullWhen(true)] this string? value)
        {
            if (value != null)
                return !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value);
            else
                return false;
        }

        /// <summary>
        /// check if the current string might be an URL
        /// </summary>
        /// <param name="value">the text to check</param>
        /// <returns>true if the given text contains 'www' or 'http'</returns>
        public static bool IsURL(this string? value)
        {
            if (value.IsValidString())
                return value.ToLower().Contains("www") || value.ToLower().Contains("http");
            else
                return false;
        }
        #endregion

        #region getter functions
        /// <summary>
        /// checks if the current string ends with a number (integer) and if so, returns the parsed value
        /// </summary>
        /// <param name="text">the string to check</param>
        /// <param name="number">the found number at the end of the text</param>
        /// <param name="lastDigitIndex">the index of the last character that is a digit</param>
        /// <returns>true if </returns>
        public static bool GetTrailingNumber(this string? text, out int number, out int lastDigitIndex)
        {
            bool hasTrailingNumberEnd = false;
            number = 0;
            lastDigitIndex = 0;

            if (text.IsValidString())
            {
                //go backwards through the text and check if char is a digit
                string numberString = "";

                for (int i = text.Length - 1; i >= 0; i--)
                {
                    char tmpLetter = text.ElementAt(i);

                    if (Char.IsDigit(tmpLetter))
                    {
                        //char is a digit -> add to help-string; set the index of the last digit char
                        numberString = tmpLetter + numberString;
                        lastDigitIndex = i;
                    }
                    else
                    {
                        //char found, that isn't a digit -> exit search loop
                        break;
                    }
                }

                //check if a list of char could be found at the end of the string -> if so, parse the integer value and set the return value
                if (numberString.Length > 0)
                {
                    number = int.Parse(numberString);
                    hasTrailingNumberEnd = true;
                }
            }

            return hasTrailingNumberEnd;
        }

        /// <summary>
        /// tries to get the basic file type of the current string (idealy a file path), using default video extensions
        /// </summary>
        /// <param name="filePath">the string to check (idealy a file path)</param>
        /// <returns>the basic file type</returns>
        public static BasicFileType GetBasicFileType(this string? filePath)
        {
            return filePath.GetBasicFileType(new() { ".mp4", ".mpeg", ".mpg", ".webm", ".avi" });
        }

        /// <summary>
        /// tries to get the basic file type of the current string (idealy a file path)
        /// </summary>
        /// <param name="filePath">the string to check (idealy a file path)</param>
        /// <param name="videoExtensions">list of accepted video extensions</param>
        /// <returns>the basic file type</returns>
        public static BasicFileType GetBasicFileType(this string? filePath, List<string> videoExtensions)
        {
            BasicFileType result = BasicFileType.Other;

            if(filePath.IsValidString())
            {
                filePath = filePath.ToLower();

                if (filePath.EndsWith(".txt"))
                    result = BasicFileType.Text;
                else if (filePath.EndsWith(".xml"))
                    result = BasicFileType.XML;
                else if (filePath.EndsWith(".mp3"))
                    result = BasicFileType.MP3;
                else
                {
                    foreach(string vidEx in videoExtensions)
                    {
                        if(filePath.EndsWith(vidEx))
                        {
                            result = BasicFileType.Video;
                            break;
                        }
                    }
                }
            }

            return result;
        }
        #endregion
    }
}
