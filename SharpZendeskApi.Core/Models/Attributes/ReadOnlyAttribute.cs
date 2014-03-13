namespace SharpZendeskApi.Core.Models.Attributes
{
    using System;

    /// <summary>
    ///     The is read only attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public class ReadOnlyAttribute : Attribute, IZendeskSpecialAttribute
    {
    }
}