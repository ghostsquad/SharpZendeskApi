namespace SharpZendeskApi.Management
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using SharpZendeskApi.Models;

    public class AttachmentManager : ManagerBase<Attachment, IAttachment>
    {
        private const string SingleEndpoint = "attachments/{0}.json";


        public AttachmentManager(IZendeskClient client)
            : base(client)
        {
        }

        public override IAttachment Get(int id)
        {
            throw new NotImplementedException();
        }

        public override void SubmitUpdatesFor(IAttachment obj)
        {
            throw new NotImplementedException();
        }

        public override IListing<IAttachment> GetMany(IEnumerable<int> ids)
        {
            throw new NotImplementedException();
        }

        public override IAttachment SubmitNew(IAttachment obj)
        {
            throw new NotImplementedException();
        }
    }
}
