namespace SharpZendeskApi.Models
{
    using System.Collections.Generic;

    internal class Upload : IUpload, IZendeskThing
    {
        public string Token { get; set; }

        public IList<IAttachment> Attachments { get; set; }
    }
}
