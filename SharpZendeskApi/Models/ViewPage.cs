namespace SharpZendeskApi.Models
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    public class ViewPage : IPage<IView>
    {
        public IList<IView> Collection
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

        public IList<IView> Views { get; set; }

        public string NextPage { get; set; }

        public int Count { get; set; }

        public string PreviousPage { get; set; }
    }
}
