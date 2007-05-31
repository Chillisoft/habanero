namespace Chillisoft.Generic.v2
{
    /// <summary>
    /// Defines how the properties of a class are displayed in a user
    /// interface, as specified in the class definitions xml file.
    /// This consists of definitions for a grid display and an editing form.
    /// </summary>
    public class UIDef : IUserInterfaceMapper
    {
        private string itsName;
        private UIFormDef itsUIFormDef;
        private UIGridDef itsUIGridDef;

        /// <summary>
        /// Constructor to initialise a new definition with the name, form
        /// and grid definitions provided
        /// </summary>
        /// <param name="name">The name</param>
        /// <param name="uiFormDef">The form definition</param>
        /// <param name="uiGridDef">The grid definition</param>
        public UIDef(string name, UIFormDef uiFormDef, UIGridDef uiGridDef)
        {
            itsName = name;
            itsUIFormDef = uiFormDef;
            itsUIGridDef = uiGridDef;
        }

        //		private static Hashtable itsUIDefs;
        //
        //		public static Hashtable GetUIDefs() {
        //			if (itsUIDefs == null) {
        //				itsUIDefs = new Hashtable();
        //			}
        //			return itsUIDefs;
        //		}
        //
        //		public static void LoadUIDefs(IUIDefsLoader loader) {
        //			foreach (UIDef uiDef in loader.LoadUIDefs()) {
        //				UIDefName name = uiDef.Name;
        //				if (!UIDef.GetUIDefs().Contains(name)) {
        //					UIDef.GetUIDefs().Add(name, uiDef);
        //				} else {
        //					Console.Out.WriteLine("Attempted to load a UI Def when it was already defined.");
        //				}
        //			}
        //		}

        /// <summary>
        /// Returns the form definition
        /// </summary>
        public UIFormDef UIFormDef
        {
            get { return itsUIFormDef; }
        }

        /// <summary>
        /// Returns the name
        /// </summary>
        public string Name
        {
            get { return itsName; }
        }

        /// <summary>
        /// Returns the grid definition
        /// </summary>
        public UIGridDef UIGridDef
        {
            get { return itsUIGridDef; }
        }

        /// <summary>
        /// Returns the form property definitions
        /// </summary>
        /// <returns>Returns a UIFormDef object</returns>
        public UIFormDef GetUIFormProperties()
        {
            return this.UIFormDef;
        }

        /// <summary>
        /// Returns the grid property definitions
        /// </summary>
        /// <returns>Returns a UIGridDef object</returns>
        public UIGridDef GetUIGridProperties()
        {
            return this.UIGridDef;
        }
    }
}