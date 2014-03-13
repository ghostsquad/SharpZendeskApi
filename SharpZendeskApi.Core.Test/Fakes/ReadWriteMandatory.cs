﻿namespace SharpZendeskApi.Core.Test.Fakes
{
    using SharpZendeskApi.Core.Models.Attributes;

    public class ReadWriteMandatory
    {
        #region Public Properties

        [Mandatory]
        public int Mandatory { get; set; }

        [ReadOnly]
        public int Read { get; set; }

        [ReadOnly]
        [Mandatory]
        public int ReadMandatory { get; set; }

        public int ReadWrite { get; set; }

        #endregion
    }
}