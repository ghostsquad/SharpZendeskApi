namespace SharpZendeskApi.Management
{
    using SharpZendeskApi.Models;

    public interface IUserManager : IManager<IUser>
    {
        IListing<IUser> Search(string query);
    }
}