namespace SharpZendeskApi.Test.Common.JsonObjects
{
    using System;
    using System.Collections.Generic;

    using Ploeh.AutoFixture;

    using RestSharp;

    /// <summary>
    /// The user json.
    /// </summary>
    public class UserJson : JsonTestObjectBase
    {
        #region Public Methods and Operators

        /// <summary>
        /// The get json object.
        /// </summary>
        /// <param name="fixture">
        /// The fixture.
        /// </param>
        /// <returns>
        /// The <see cref="JsonObject"/>.
        /// </returns>
        public override JsonObject GetJsonObject(IFixture fixture)
        {
            return new JsonObject
                       {
                           { "id", fixture.Create<int>() }, 
                           { "url", fixture.Create<string>() }, 
                           { "name", fixture.Create<string>() }, 
                           { "alias", fixture.Create<string>() }, 
                           { "external_id", fixture.Create<string>() }, 
                           { "created_at", fixture.Create<DateTime>().ToUtcIso8601() }, 
                           { "updated_at", fixture.Create<DateTime>().ToUtcIso8601() }, 
                           { "active", fixture.Create<bool>() }, 
                           { "verified", fixture.Create<bool>() }, 
                           { "shared", fixture.Create<bool>() }, 
                           { "shared_agent", fixture.Create<bool>() }, 
                           { "locale", fixture.Create<string>() }, 
                           { "locale_id", fixture.Create<int>() }, 
                           { "time_zone", fixture.Create<string>() }, 
                           { "last_login_at", fixture.Create<DateTime>().ToUtcIso8601() }, 
                           { "email", fixture.Create<string>() }, 
                           { "phone", fixture.Create<string>() }, 
                           { "signature", fixture.Create<string>() }, 
                           { "details", fixture.Create<string>() }, 
                           { "notes", fixture.Create<string>() }, 
                           { "organization_id", fixture.Create<int>() }, 
                           { "role", fixture.Create<string>() }, 
                           { "custom_role_id", fixture.Create<int>() }, 
                           { "moderator", fixture.Create<bool>() }, 
                           { "ticket_restriction", fixture.Create<string>() }, 
                           { "only_private_comments", fixture.Create<bool>() }, 
                           { "tags", fixture.Create<List<string>>() }, 
                           { "restricted_agent", fixture.Create<bool>() }, 
                           { "suspended", fixture.Create<bool>() }, 
                           { "photo", JsonTestObjectFactory.Instance.Create<AttachmentJson>(fixture) }, 
                           { "user_fields", JsonTestObjectFactory.Instance.CreateMany<CustomFieldJson>(fixture) }
                       };
        }

        #endregion
    }
}