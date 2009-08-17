namespace Habanero.Base
{
    ///<summary>
    /// Some lookup lists are associated with ClassDefs in order to add new items to them easily
    /// This interface provides a base class for those that can be associated with a ClassDef.
    ///</summary>
    public interface ILookupListWithClassDef : ILookupList
    {
        ///<summary>
        /// The ClassDef associated with this lookup list.
        ///</summary>
        IClassDef ClassDef { get; }
    }
}