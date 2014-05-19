namespace SharpZendeskApi.Models
{
    using System.Collections.Generic;

    public interface IUpload
    {
        string Token { get; }

        IList<IAttachment> Attachments { get; }
    }
}
