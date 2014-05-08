namespace SharpZendeskApi.Test.Common
{
    using System;

    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Kernel;

    /// <summary>
    ///     The object specimen customization.
    /// </summary>
    public class ObjectSpecimenCustomization : ISpecimenBuilder
    {
        #region Fields

        /// <summary>
        ///     The usage count.
        /// </summary>
        private int usageCount;

        private Fixture fixture;

        public ObjectSpecimenCustomization(Fixture fixture)
        {
            this.fixture = fixture;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The create.
        /// </summary>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        public object Create(object request, ISpecimenContext context)
        {
            var requestAsType = request as Type;
            if (requestAsType != null && requestAsType == typeof(object))
            {
                this.usageCount++;

                if (this.usageCount % 2 == 0)
                {
                    return this.fixture.Create<int>();
                }

                if (this.usageCount % 3 == 0)
                {
                    return new Guid().ToString();
                }

                return new bool();
            }

            return new NoSpecimen();
        }

        #endregion
    }
}