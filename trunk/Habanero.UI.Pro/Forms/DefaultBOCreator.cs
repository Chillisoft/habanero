using System;
using Habanero.Bo.ClassDefinition;
using Habanero.Bo;
using Habanero.Base;
using BusinessObject=Habanero.Bo.BusinessObject;

namespace Habanero.Ui.Forms
{
    /// <summary>
    /// Creates business objects.  The default creator is used by facilities
    /// like ReadOnlyGridWithButtons to create new business objects.  Inherit
    /// from this class if you need to carry out additional steps at the time
    /// of creating a new business object.
    /// </summary>
    public class DefaultBOCreator : IObjectCreator
    {
        private readonly ClassDef _classDef;

        /// <summary>
        /// Constructor to initialise a new object creator
        /// </summary>
        /// <param name="classDef">The class definition</param>
        public DefaultBOCreator(ClassDef classDef)
        {
            _classDef = classDef;
        }

        /// <summary>
        /// Creates a business object
        /// </summary>
        /// <param name="editor">An object editor</param>
        /// <returns>Returns the business object created</returns>
        public Object CreateObject(IObjectEditor editor)
        {
            return this.CreateObject(editor, null);
        }

        /// <summary>
        /// Creates a business object
        /// </summary>
        /// <param name="editor">An object editor</param>
        /// <param name="initialiser">An object initialiser</param>
        /// <returns>Returns the business object created</returns>
        public Object CreateObject(IObjectEditor editor, IObjectInitialiser initialiser)
        {
            BusinessObject newBo = _classDef.CreateNewBusinessObject();
            if (initialiser != null)
            {
                initialiser.InitialiseObject(newBo);
            }
            if (editor.EditObject(newBo))
            {
                return newBo;
            }
            else
            {
                return null;
            }
        }
    }
}