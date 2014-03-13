namespace SharpZendeskApi.Core.Models.Attributes
{
    using System;

    /// <summary>
    ///     The is mandatory attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public class MandatoryAttribute : Attribute, IZendeskSpecialAttribute
    {       
    }
}