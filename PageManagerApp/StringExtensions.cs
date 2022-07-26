using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace PageManagerApp
{
    public static class StringExtensions
    {
        public const string AlphaNumeric = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";


        /// <summary>
        ///     Builds a string that frames text with the specified character.
        /// </summary>
        /// <param name="header">Text to be framed.</param>
        /// <param name="borderWidth">Length of border</param>
        /// <param name="frameCharacter">Character to use to create framed border</param>
        /// <returns></returns>
        public static string BuildFramedHeader(this string header, int borderWidth = 80, string frameCharacter = "=")
        {
            var headerlength = header.Length;

            if (header.Length > borderWidth)
                header = header.Substring(0, borderWidth - 5) + "...";
            var headerStartPosition = (borderWidth - headerlength) / 2;
            var border = frameCharacter.Repeat(borderWidth);
            var sb = new StringBuilder();
            sb.Append(border).AppendLine();
            sb.Append(header.PadLeft(headerStartPosition + header.Length)).AppendLine();
            sb.Append(border).AppendLine();
            return sb.ToString();
        }

        public static IEnumerable<char> CharsToTitleCase(this string s)
        {
            var newWord = true;
            foreach (var c in s)
            {
                if (newWord)
                {
                    yield return char.ToUpper(c);
                    newWord = false;
                }
                else yield return char.ToLower(c);
                if (c == ' ') newWord = true;
            }
        }

        /// <summary>
        ///     Generates a random alphanumeric string of a specified size
        /// </summary>
        /// <param name="length">Desired length of output string.</param>
        /// <returns>Randomly generated string</returns>
        public static string GetRandomString(this int length)
        {
            var s = "";
            using (var provider = new RNGCryptoServiceProvider())
            {
                while (s.Length != length)
                {
                    var oneByte = new byte[1];
                    provider.GetBytes(oneByte);
                    var character = (char)oneByte[0];
                    if (AlphaNumeric.Contains(character))
                        s += character;
                }
            }
            return s;
        }

        /// <summary>
        ///     Inserts a character at the specified interval
        /// </summary>
        /// <param name="input">String to format</param>
        /// <param name="interval">The interval position of the inserted character</param>
        /// <param name="characterToInsert">Character to insert</param>
        /// <returns>String formatted with inserted character.</returns>
        public static string InsertCharAtInterval(this string input, int interval, char characterToInsert = '-')
        {
            if (input.Length <= interval)
                return input;

            var list = Enumerable
                .Range(0, input.Length / interval)
                .Select(i => input.Substring(i * interval, interval));

            var output = string.Join(characterToInsert.ToString(), list);
            return output;
        }

        /// <summary>
        ///     Determines if submitted string consists only of numbers.
        /// </summary>
        /// <param name="value">String to be evaluated.</param>
        /// <returns>True, if string contains only digits.</returns>
        public static bool IsDigitsOnly(this string value)
        {
            return value.All(c => (c >= '0') && (c <= '9'));
        }

        /// <summary>
        ///     Determines if string is null or empty or consists of only whitespace.
        /// </summary>
        /// <param name="s">string to evaluate.</param>
        /// <returns>True if null, empty or whitespace.</returns>
        public static bool IsNullOrEmptyOrWhiteSpace(this string s)
        {
            return string.IsNullOrEmpty(s) || (s.Trim() == string.Empty);
        }

        /// <summary>
        ///     Determines if string is a valid supported integer.
        /// </summary>
        /// <param name="value">The string to be evaluated.</param>
        /// <returns>True, if string is a valid integer.</returns>
        public static bool IsNumeric(this string value)
        {
            long retNum;
            return long.TryParse(value, NumberStyles.Integer,
                NumberFormatInfo.InvariantInfo, out retNum);
        }

        /// <summary>
        ///     Verifies that a date string is a valid date.
        /// </summary>
        /// <param name="date">String representation of a date</param>
        /// <returns>True, if string is a valid date.</returns>
        public static bool IsValidDate(this string date)
        {
            string[] formats = { "MM/dd/yyyy" };
            DateTime parsedDate;
            return DateTime.TryParseExact(date, formats, new CultureInfo("en-US"),
                DateTimeStyles.None, out parsedDate);
        }

        /// <summary>
        ///     Returns a string to the left of the specified character.
        /// </summary>
        /// <param name="s">String to truncate</param>
        /// <param name="c">Character to stop at.</param>
        /// <returns>Truncated string or empty string if null or character not found.</returns>
        public static string LeftOf(this string s, char c)
        {
            var result = string.Empty;
            if (string.IsNullOrWhiteSpace(s)) return result;
            var index = s.IndexOf(c);
            if (index >= 0)
                result = s.Substring(0, index);
            return result;
        }

        /// <summary>
        ///     Returns a string to the left of the specified phrase.
        /// </summary>
        /// <param name="s">String to truncate</param>
        /// <param name="phrase">Phrase to stop at.</param>
        /// <returns>Truncated string or empty string if null or character not found.</returns>
        public static string LeftOf(this string s, string phrase)
        {
            var result = string.Empty;
            if (string.IsNullOrWhiteSpace(s)) return result;
            var index = s.IndexOf(phrase, StringComparison.Ordinal);
            if (index >= 0)
                result = s.Substring(0, index);
            return result;
        }

        /// <summary>
        ///     Repeats a string a specified number of times.
        /// </summary>
        /// <param name="input">String to repeat.</param>
        /// <param name="count"># of times to repeat string.</param>
        /// <returns></returns>
        public static string Repeat(this string input, int count)
        {
            if (input == null)
                return null;

            var sb = new StringBuilder();

            for (var repeat = 0; repeat < count; repeat++)
                sb.Append(input);

            return sb.ToString();
        }

        /// <summary>
        ///     Replaces a string within a string.
        /// </summary>
        /// <param name="originalString">The string before conversion.</param>
        /// <param name="oldValue">The string to replace.</param>
        /// <param name="newValue">The replacement string</param>
        /// <param name="comparisonType">The constraint on comparison.</param>
        /// <returns>String after replacement.</returns>
        public static string Replace(this string originalString, string oldValue, string newValue,
            StringComparison comparisonType)
        {
            var startIndex = 0;

            while (true)
            {
                startIndex = originalString.IndexOf(oldValue, startIndex, comparisonType);

                if (startIndex < 0)
                    break;

                originalString = string.Concat(originalString.Substring(0, startIndex), newValue,
                    originalString.Substring(startIndex + oldValue.Length));

                startIndex += newValue.Length;
            }

            return originalString;
        }

        /// <summary>
        ///     Returns a string to the right of the specified character.
        /// </summary>
        /// <param name="s">String to truncate</param>
        /// <param name="c">Character to start at.</param>
        /// <returns>Truncated string or empty string if null or character not found.</returns>
        public static string RightOf(this string s, char c)
        {
            var result = string.Empty;
            if (string.IsNullOrWhiteSpace(s)) return result;
            var index = s.IndexOf(c);
            if (index >= 0)
                result = s.Substring(++index, s.Length - index);
            return result;
        }

        /// <summary>
        ///     Parses a string into a list of values. If it cannot
        ///     be parsed, returns an empty string list.
        /// </summary>
        /// <param name="s">String to be parsed.</param>
        /// <param name="c">Delimiter</param>
        /// <returns>String array of parsed values</returns>
        public static string[] SplitOrEmpty(this string s, char c)
        {
            string[] result = { "" };
            if (!s.IsNullOrEmptyOrWhiteSpace() && s.Contains(c))
                result = s.Split(c);
            return result;
        }


        /// <summary>
        ///     Strip a string of the specified character.
        /// </summary>
        /// <param name="s">the string to process</param>
        /// <param name="character">character to remove from the string</param>
        /// <example>
        ///     string s = "abcde";
        ///     s = s.Strip('b');  //s becomes 'acde;
        /// </example>
        /// <returns></returns>
        public static string Strip(this string s, char character)
        {
            s = s.Replace(character.ToString(), "");

            return s;
        }

        /// <summary>
        ///     Strip a string of the specified characters.
        /// </summary>
        /// <param name="s">the string to process</param>
        /// <param name="chars">list of characters to remove from the string</param>
        /// <example>
        ///     string s = "abcde";
        ///     s = s.Strip('a', 'd');  //s becomes 'bce;
        /// </example>
        /// <returns></returns>
        public static string Strip(this string s, params char[] chars)
        {
            return chars.Aggregate(s, (current, c) => current.Replace(c.ToString(), ""));
        }

        /// <summary>
        ///     Strip a string of the specified substring.
        /// </summary>
        /// <param name="s">the string to process</param>
        /// <param name="subString">substring to remove</param>
        /// <example>
        ///     string s = "abcde";
        ///     s = s.Strip("bcd");  //s becomes 'ae;
        /// </example>
        /// <returns></returns>
        public static string Strip(this string s, string subString)
        {
            s = s.Replace(subString, "");

            return s;
        }

        /// <summary>
        ///     Removes all non-numeric characters.
        /// </summary>
        /// <param name="s">String to validate.</param>
        /// <returns>Numeric string representation or an empty string if null.</returns>
        public static string StripNonNumeric(this string s)
        {
            return s.IsNullOrEmptyOrWhiteSpace() ? string.Empty : Regex.Replace(s, "[^0-9]", "");
        }

        /// <summary>
        ///     Converts string to enum object
        /// </summary>
        /// <typeparam name="T">Type of enum</typeparam>
        /// <param name="value">String dateString to convert</param>
        /// <returns>Returns enum object</returns>
        public static T ToEnum<T>(this string value)
            where T : struct
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        /// <summary>
        ///     Converts string to integer. If string is not numeric
        ///     return 0.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static int ToInt32(this string s)
        {
            if (s.IsNullOrEmptyOrWhiteSpace())
                return 0;
            return !s.IsNumeric() ? 0 : Convert.ToInt32(s);
        }

        /// <summary>
        ///     Converts string to proper case. Handles null input.
        /// </summary>
        /// <param name="s">The string to convert.</param>
        /// <returns>String in proper case.</returns>
        public static string ToProperCase(this string s)
        {
            return s.IsNullOrEmptyOrWhiteSpace() ? s : new string(s.CharsToTitleCase().ToArray());
        }
    }
}