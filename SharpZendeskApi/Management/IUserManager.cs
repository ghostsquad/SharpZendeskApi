namespace SharpZendeskApi.Management
{
    using System.Collections.Generic;

    using SharpZendeskApi.Models;

    public interface IUserManager
    {
        IUser Get(int id);

        IListing<IUser> GetMany(IEnumerable<int> ids);

        IUser SubmitNew(IUser obj);

        void SubmitUpdatesFor(IUser obj);

        ZendeskClientBase Client { get; set; }

        bool TryGet(int id, out IUser value);
    }
}