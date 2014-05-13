namespace SharpZendeskApi.Models
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    public class UserPage : IPage<IUser>
    {
        public IList<IUser> Collection
        {
            get
            {
                return this.Users;
            }

            // this set is used exclusively by RestSharp when populating this class
            [ExcludeFromCodeCoverage]
            set
            {
                this.Users = value;
            }
        }

        public IList<IUser> Users { get; set; }

        public string NextPage { get; set; }

        public int Count { get; set; }

        public string PreviousPage { get; set; }
    }
}
