// http://developer.zendesk.com/documentation/rest_api/views.html#conditions
// Conditions check the value of ticket fields and select the ticket if the conditions are met. 
// Conditions are represented as a JSON object with two arrays of one or more conditions.
// 
// The first array lists all the conditions that must be met. The second array lists any condition that must be met.
// 
// Name Type  Description
// ---- ----  -----------
// all  array Logical AND. Tickets must fulfill all of the conditions to be considered matching
// any  array Logical OR. Tickets may satisfy any of the conditions to be considered matching
namespace SharpZendeskApi.Models
{
    using System.Collections.Generic;

    public class Conditions : IZendeskThing
    {

        public List<Condition> All { get; set; }

        public List<Condition> Any { get; set; }
    }
}
