namespace SharpZendeskApi.Test.Common.JsonObjects
{
    using System;
    using System.Collections.Generic;

    using Ploeh.AutoFixture;

    using RestSharp;

    /// <summary>
    /// The ticket json.
    /// </summary>
    public class TicketJson : JsonTestObjectBase
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
                           { "assignee_id", fixture.Create<int>() },
                           { "brand_id", fixture.Create<int>() },
                           { "collaborator_ids", fixture.Create<List<int>>() },

                           // note "comment" is added to be compatible with ticket POST
                           { "comment", fixture.Create<string>() },
                           { "created_at", fixture.Create<DateTime>().ToUtcIso8601() },
                           {
                               "custom_fields",
                               JsonTestObjectFactory.Instance.CreateMany<CustomFieldJson>(fixture)
                           }, 
                           { "description", fixture.Create<string>() },
                           { "due_at", fixture.Create<DateTime>().ToUtcIso8601() },
                           { "external_id", fixture.Create<string>() },
                           { "followup_ids", fixture.Create<List<int>>() },
                           { "forum_topic_id", fixture.Create<int>() },
                           { "group_id", fixture.Create<int>() },
                           { "has_incidents", fixture.Create<bool>() },
                           { "id", fixture.Create<int>() },
                           { "organization_id", fixture.Create<int>() },
                           { "priority", fixture.Create<string>() },
                           { "problem_id", fixture.Create<int>() },
                           { "recipient", fixture.Create<string>() },
                           { "requester_id", fixture.Create<int>() },
                           {
                               "satisfaction_rating",
                               JsonTestObjectFactory.Instance.Create<SatisfactionRatingJson>(fixture)
                           },
                           { "sharing_agreement_ids", fixture.Create<List<int>>() },
                           { "status", fixture.Create<string>() },
                           { "subject", fixture.Create<string>() },
                           { "submitter_id", fixture.Create<int>() },
                           { "tags", fixture.Create<List<string>>() },
                           { "ticket_form_id", fixture.Create<int>() },
                           { "type", fixture.Create<string>() },
                           { "updated_at", fixture.Create<DateTime>().ToUtcIso8601() },
                           { "url", fixture.Create<string>() },
                           { "via", JsonTestObjectFactory.Instance.Create<ViaJson>(fixture) }
                       };
        }

        #endregion
    }
}