namespace SharpZendeskApi
{
    #region

    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;

    #endregion

    /// <summary>
    ///     The date time extensions.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class DateTimeExtensions
    {        
        #region Constants

        /// <summary>
        /// The iso 8601 date format.
        /// http://www.w3.org/TR/NOTE-datetime
        /// </summary>
        public const string Iso8601DateFormat = "yyyy'-'MM'-'dd";

        /// <summary>
        /// The iso 8601 date time format.
        /// http://www.w3.org/TR/NOTE-datetime
        /// </summary>
        public const string Iso8601DateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'";

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     The parse exact iso 8601 date time.
        /// </summary>
        /// <param name="value">
        ///     The value.
        /// </param>
        /// <returns>
        ///     The <see cref="DateTime" />.
        /// </returns>
        public static DateTime ParseExactIso8601DateTime(string value)
        {
            return DateTime.ParseExact(value, Iso8601DateTimeFormat, CultureInfo.InvariantCulture);
        }

        /// <summary>
        ///     The to iso 8601.
        /// </summary>
        /// <param name="dateTime">
        ///     The date time.
        /// </param>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public static string ToUtcIso8601(this DateTime dateTime)
        {
            if (dateTime.Kind == DateTimeKind.Unspecified)
            {
                // If we don't know what the DateTime kind is, pretend it's UTC.
                dateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
            }

            return dateTime.ToUniversalTime().ToString(Iso8601DateTimeFormat, CultureInfo.InvariantCulture);
        }

        /// <summary>
        ///     The try parse exact iso 8601 date time.
        /// </summary>
        /// <param name="value">
        ///     The value.
        /// </param>
        /// <param name="dateTime">
        ///     The date time.
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public static bool TryParseExactIso8601DateTime(string value, out DateTime dateTime)
        {
            return TryParseExactIso8601DateTime(value, CultureInfo.InvariantCulture, out dateTime);
        }

        public static bool TryParseExactIso8601DateTime(string value, CultureInfo cultureInfo, out DateTime dateTime)
        {
            return DateTime.TryParseExact(
                value,
                Iso8601DateTimeFormat,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out dateTime);
        }

        #endregion
    }
}