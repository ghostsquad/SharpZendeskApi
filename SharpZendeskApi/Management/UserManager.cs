﻿namespace SharpZendeskApi.Management
{
    using System;
    using System.Collections.Generic;

    using RestSharp;

    using SharpZendeskApi.Models;

    public class UserManager : ManagerBase<User, IUser>, IUserManager
    {
        #region Constants

        private const string CreateUserEndpoint = "users.json";

        // TODO
        private const string GetManyFromGroupEndpoint = "groups/{0}/users.json";

        // TODO
        private const string GetManyFromOrgEndpoint = "organizations/{0}/users.json";

        private const string GetManyEndpoint = "users/show_many.json?ids={0}";

        private const string SingleUserEndpoint = "users/{0}.json";

        private const string SearchEndpoint = "users/search.json";

        #endregion

        #region Constructors and Destructors

        public UserManager(ZendeskClientBase client)
            : base(client)
        {
        }

        #endregion

        #region Public Methods and Operators

        public override IUser Get(int id)
        {
            var url = string.Format(SingleUserEndpoint, id);
            return this.Get(url, id);
        }

        public override IListing<IUser> GetMany(IEnumerable<int> ids)
        {
            return this.GetMany(string.Format(GetManyEndpoint, string.Join(",", ids)));
        }

        public override IUser SubmitNew(IUser obj)
        {
            return this.SubmitNew(CreateUserEndpoint, obj);
        }

        public override void SubmitUpdatesFor(IUser obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }

            var url = string.Format(SingleUserEndpoint, obj.Id);
            this.SubmitUpdatesFor(url, obj);
        }

        public IListing<IUser> Search(string query)
        {
            var request = new RestRequest(SearchEndpoint, Method.GET);
            request.AddParameter("query", query, ParameterType.QueryString);

            return this.Client.GetListing<User, IUser>(request);
        }

        #endregion
    }
}