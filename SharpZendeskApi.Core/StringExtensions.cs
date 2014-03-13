namespace SharpZendeskApi.Core
{
    using System;
    using System.Globalization;
    using System.Text.RegularExpressions;
    using System.Threading;

    /// <summary>
    /// The string extensions.
    /// </summary>
    public static class StringExtensions
    {
        #region Public Methods and Operators

        /// <summary>
        /// Use the current thread's culture info for conversion
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string ToTitleCase(this string value)
        {
            var cultureInfo = Thread.CurrentThread.CurrentCulture;
            return ToTitleCase(value.ToLower(), cultureInfo);
        }

        /// <summary>
        /// Overload which uses the culture info with the specified name
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="cultureInfoName">
        /// The culture Info Name.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string ToTitleCase(this string value, string cultureInfoName)
        {
            var cultureInfo = new CultureInfo(cultureInfoName);
            return ToTitleCase(value.ToLower(), cultureInfo);
        }

        /// <summary>
        ///     The regex replace.
        /// </summary>
        private const string CamelCaseUnderscoreRegexReplace = @"$1$3_$2$4";        

        /// <summary>
        ///     The regex.
        /// </summary>
        private static readonly Regex CamelCaseRegex = new Regex(
            "([A-Z])([A-Z][a-z])|([a-z0-9])([A-Z])",
            RegexOptions.Compiled);

        /// <summary>
        /// Overload which uses the specified culture info
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="cultureInfo">
        /// The culture Info.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string ToTitleCase(this string value, CultureInfo cultureInfo)
        {
            var titleCaseString = cultureInfo.TextInfo.ToTitleCase(value.ToLower());
            return titleCaseString;
        }

        public static string ToCPlusPlusNamingStyle(this string value)
        {
            var underscoredNameVersion = CamelCaseRegex.Replace(value, CamelCaseUnderscoreRegexReplace);
            return underscoredNameVersion.ToLower();
        }

        #endregion

        public static string ToTitleCaseSansUnderscores(this string value)
        {
            // http://theburningmonk.com/2010/08/dotnet-tips-string-totitlecase-extension-methods/
            // http://stackoverflow.com/questions/3386609/function-to-make-pascal-case-c
            return value.ToTitleCase().Replace("_", String.Empty);
        }

        public static string WrapSerializedStringInTypeRoot(this string serializedJson, Type typeForRoot)
        {
            return string.Format("{{\"{0}\":{1}}}", typeForRoot.GetTypeNameAsCPlusPlusStyle(), serializedJson);
        }

        public static string Pluralize(this string value)
        {
            return value + "s";
        }
    }
}