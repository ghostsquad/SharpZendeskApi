namespace SharpZendeskApi.Core.Test.Common.JsonObjects
{
    using System;

    using Ploeh.AutoFixture;

    using RestSharp;

    /// <summary>
    /// The ticket field json.
    /// </summary>
    public class TicketFieldJson : JsonTestObjectBase
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
                           { "active", fixture.Create<bool>() }, 
                           { "collapsed_for_agents", fixture.Create<string>() }, 
                           { "created_at", fixture.Create<DateTime>().ToUtcIso8601() },
                           /* for now, I'm commenting this out, as this object is not described in the documentation
                            * http://developer.zendesk.com/documentation/rest_api/ticket_fields.html
                            * { "custom_field_options", fixture.CreateMany<object>() }, 
                            */
                           { "description", fixture.Create<string>() }, 
                           { "editable_in_portal", fixture.Create<bool>() }, 
                           { "id", fixture.Create<int>() }, 
                           { "position", fixture.Create<int>() }, 
                           { "regexp_for_validation", fixture.Create<string>() }, 
                           { "removable", fixture.Create<bool>() }, 
                           { "required", fixture.Create<bool>() }, 
                           { "required_in_portal", fixture.Create<bool>() }, 
                           /* for now, I'm commenting this out, as this object is not described in the documentation
                            * http://developer.zendesk.com/documentation/rest_api/ticket_fields.html
                            * { "system_field_options", fixture.CreateMany<object>() }, 
                            */
                           { "tag", fixture.Create<string>() }, 
                           { "title", fixture.Create<string>() }, 
                           { "title_in_portal", fixture.Create<string>() }, 
                           { "type", fixture.Create<string>() }, 
                           { "updated_at", fixture.Create<DateTime>().ToUtcIso8601() }, 
                           { "url", fixture.Create<string>() }, 
                           { "visible_in_portal", fixture.Create<bool>() }
                       };
        }

        #endregion
    }
}