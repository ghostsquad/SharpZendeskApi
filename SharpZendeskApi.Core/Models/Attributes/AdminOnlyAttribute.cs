namespace SharpZendeskApi.Core.Models.Attributes
{
    using System;

    /// <summary>
    ///     The admin only attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class AdminOnlyAttribute : Attribute, IZendeskSpecialAttribute
    {        
    }
}