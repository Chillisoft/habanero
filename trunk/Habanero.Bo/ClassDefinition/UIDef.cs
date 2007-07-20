namespace Habanero.Bo.ClassDefinition
{
    /// <summary>
    /// Defines how the properties of a class are displayed in a user
    /// interface, as specified in the class definitions xml file.
    /// This consists of definitions for a grid display and an editing form.
    /// </summary>
    public class UIDef 
    {
        private string _name;
        private UIForm _uiForm;
        private UIGrid _uiGrid;

        /// <summary>
        /// Constructor to initialise a new definition with the name, form
        /// and grid definitions provided
        /// </summary>
        /// <param name="name">The name</param>
        /// <param name="uiForm">The form definition</param>
        /// <param name="uiGrid">The grid definition</param>
        public UIDef(string name, UIForm uiForm, UIGrid uiGrid)
        {
            _name = name;
            _uiForm = uiForm;
            _uiGrid = uiGrid;
        }

        /// <summary>
        /// Returns the form definition
        /// </summary>
        public UIForm UIForm
        {
            get { return _uiForm; }
            protected set { _uiForm = value; }
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
        public UIGrid UIGrid
        {
            get { return _uiGrid; }
            protected set { _uiGrid = value; }
        }

        /// <summary>
        /// Returns the form property definitions
        /// </summary>
        /// <returns>Returns a UIForm object</returns>
        public UIForm GetUIFormProperties()
        {
            return this.UIForm;
        }

        /// <summary>
        /// Returns the grid property definitions
        /// </summary>
        /// <returns>Returns a UIGridDef object</returns>
        public UIGrid GetUIGridProperties()
        {
            return this.UIGrid;
        }
    }
}