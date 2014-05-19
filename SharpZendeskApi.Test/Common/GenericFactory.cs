namespace SharpZendeskApi.Test.Common
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The generic factory.
    /// Used some ideas from here:
    /// http://codereview.stackexchange.com/questions/8307/implementing-factory-design-pattern-with-generics
    /// </summary>
    /// <typeparam name="T">
    /// The base type to supported
    /// </typeparam>
    public abstract class GenericFactory<T>
    {
        #region Fields

        /// <summary>
        ///     The registered derived types.
        /// </summary>
        private readonly HashSet<Type> registeredTypes;

        /// <summary>
        ///     The supported base type.
        /// </summary>
        private readonly Type supportedBaseType;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="FactoryBase{T}" /> class.
        /// </summary>
        protected GenericFactory()
        {
            this.registeredTypes = new HashSet<Type>();
            this.supportedBaseType = typeof(T);
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The create.
        /// </summary>
        /// <param name="args">
        /// The args.
        /// </param>
        /// <typeparam name="derivedT">
        /// The derived type to create.
        /// </typeparam>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// If the derived type is not registered.
        /// </exception>
        protected T Create<TDerived>(params object[] args)
        {            
            if (this.registeredTypes.Contains(typeof(TDerived)))
            {
                return (T)Activator.CreateInstance(typeof(TDerived), args);
            }

            throw new ArgumentException(string.Format("{0} is not registered!", typeof(TDerived)));
        }

        /// <summary>
        /// The register.
        /// </summary>
        /// <typeparam name="TDerived">
        /// The derived type to register.
        /// </typeparam>
        /// <exception cref="ArgumentException">
        /// Abstract classes and Interfaces cannot be registered.
        /// </exception>
        public void Register<TDerived>() where TDerived : T
        {
            Type derivedType = typeof(TDerived);

            if (derivedType.IsInterface || derivedType.IsAbstract)
            {
                throw new ArgumentException("Abstract classes and Interfaces cannot be registered!");
            }

            this.registeredTypes.Add(derivedType);
        }

        #endregion
    }
}