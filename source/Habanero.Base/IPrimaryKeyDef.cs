namespace Habanero.Base
{
    public interface IPrimaryKeyDef
    {
        /// <summary>
        /// Returns true if the primary key is a propery the object's ID, that is,
        /// the primary key is a single discrete property that is an immutable Guid and serves as the ID.
        /// </summary>
        bool IsGuidObjectID { get; set; }

        void Add(IPropDef propDef);
    }
}