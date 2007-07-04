namespace Habanero.Base
{
    /// <summary>
    /// Defines how the properties of a class are displayed in a user
    /// interface, as specified in the class definitions xml file.
    /// This consists of definitions for a grid display and an editing form.
    /// </summary>
    public class UIDef : IUserInterfaceMapper
    {
        private string _name;
        private UIFormDef _uiFormDef;
        private UIGridDef _uiGridDef;

        /// <summary>
        /// Constructor to initialise a new definition with the name, form
        /// and grid definitions provided
        /// </summary>
        /// <param name="name">The name</param>
        /// <param name="uiFormDef">The form definition</param>
        /// <param name="uiGridDef">The grid definition</param>
        public UIDef(string name, UIFormDef uiFormDef, UIGridDef uiGridDef)
        {
            _name = name;
            _uiFormDef = uiFormDef;
            _uiGridDef = uiGridDef;
        }

        //		private static Hashtable _uiDefs;
        //
        //		public static Hashtable GetUIDefs() {
        //			if (_uiDefs == null) {
        //				_uiDefs = new Hashtable();
        //			}
        //			return _uiDefs;
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
			get { return _uiFormDef; }
			protected set { _uiFormDef = value; }
        }

        /// <summary>
        /// Returns the name
        /// </summary>
        public string Name
        {
            get { return _name; }
			protected set { _name = value; }
        }

        /// <summary>
        /// Returns the grid definition
        /// </summary>
        public UIGridDef UIGridDef
        {
            get { return _uiGridDef; }
			protected set { _uiGridDef = value; }
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