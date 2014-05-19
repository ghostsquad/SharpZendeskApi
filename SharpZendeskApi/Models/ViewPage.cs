namespace SharpZendeskApi.Models
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    internal class ViewPage : IPage<View>
    {
        public IList<View> Collection
        {
            get
            {
                return this.Views;
            }

            // this set is used exclusively by RestSharp when populating this class
            [ExcludeFromCodeCoverage]
            set
            {
                this.Views = value;
            }
        }

        public IList<View> Views { get; set; }

        public string NextPage { get; set; }

        public int Count { get; set; }

        public string PreviousPage { get; set; }
    }
}
