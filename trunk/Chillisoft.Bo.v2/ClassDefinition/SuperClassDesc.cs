namespace Chillisoft.Bo.ClassDefinition.v2
{
    /// <summary>
    /// Manages a super-class in the case where inheritance is being used.
    /// </summary>
    /// TODO ERIC - what is desc? description?  maybe this could be renamed
    public class SuperClassDesc
    {
        private ORMapping mORMapping;
        private ClassDef mSuperClassDef;

        /// <summary>
        /// Constructor to create a new super-class
        /// </summary>
        /// <param name="superClassDef">The class definition</param>
        /// <param name="mapping">The type of OR-Mapping to use. See
        /// the ORMapping enumeration for more detail.</param>
        public SuperClassDesc(ClassDef superClassDef, ORMapping mapping)
        {
            mORMapping = mapping;
            this.mSuperClassDef = superClassDef;
        }

        /// <summary>
        /// Returns the type of ORMapping used.  See the ORMapping
        /// enumeration for more detail.
        /// </summary>
        public ORMapping ORMapping
        {
            get { return mORMapping; }
        }

        /// <summary>
        /// Returns the class definition for this super-class
        /// </summary>
        public ClassDef SuperClassDef
        {
            get { return mSuperClassDef; }
        }
    }
}