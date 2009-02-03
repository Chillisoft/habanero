namespace Habanero.Base
{
    ///<summary>
    /// A <see cref="IBusinessObjectCreator"/> that passes off its creation logic to a delegate passed in the constructor.
    ///</summary>
    public class DelegatedBusinessObjectCreator : DelegatedBusinessObjectCreator<IBusinessObject>
    {
        ///<summary>
        /// Initialises the <see cref="DelegatedBusinessObjectCreator"/> with the specified delegate.
        ///</summary>
        ///<param name="createBusinessObjectDelegate">The delegate to be executed when CreateBusinessObject is called.</param>
        public DelegatedBusinessObjectCreator(CreateBusinessObjectDelegate createBusinessObjectDelegate)
            : base(createBusinessObjectDelegate)
        {
        }
    }

    ///<summary>
    /// A <see cref="IBusinessObjectCreator"/> that passes off its creation logic to a delegate passed in the constructor.
    ///</summary>
    public class DelegatedBusinessObjectCreator<TBusinessObject> : IBusinessObjectCreator
        where TBusinessObject : class, IBusinessObject
    {
        ///<summary>
        /// The delegate for the CreateBusinessObject method.
        ///</summary>
        /// <returns>The newly created <see cref="IBusinessObject"/>.</returns>
        public delegate TBusinessObject CreateBusinessObjectDelegate();
        private readonly CreateBusinessObjectDelegate _createBusinessObjectDelegate;

        ///<summary>
        /// Initialises the <see cref="DelegatedBusinessObjectCreator{TBusinessObject}"/> with the specified delegate.
        ///</summary>
        ///<param name="createBusinessObjectDelegate">The delegate to be executed when CreateBusinessObject is called.</param>
        public DelegatedBusinessObjectCreator(CreateBusinessObjectDelegate createBusinessObjectDelegate)
        {
            _createBusinessObjectDelegate = createBusinessObjectDelegate;
        }

        #region Implementation of IBusinessObjectCreator

        /// <summary>
        /// Creates the object, without editing or saving it.
        /// </summary>
        /// <returns></returns>
        public IBusinessObject CreateBusinessObject()
        {
            if (_createBusinessObjectDelegate != null) return _createBusinessObjectDelegate();
            return null;
        }

        #endregion
    }
}