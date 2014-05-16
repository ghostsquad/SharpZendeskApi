namespace SharpZendeskApi.Management
{
    using System.Collections.Generic;

    using SharpZendeskApi.Models;

    public interface IAttachmentManager
    {
        IAttachment Get(int id);

        void SubmitUpdatesFor(IAttachment obj);

        IListing<IAttachment> GetMany(IEnumerable<int> ids);

        IAttachment SubmitNew(IAttachment obj);

        ZendeskClientBase Client { get; set; }

        bool TryGet(int id, out IAttachment value);
    }
}