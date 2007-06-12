using System;
using System.Xml;
using Chillisoft.Bo.ClassDefinition.v2;
using Chillisoft.Generic.v2;
using Chillisoft.Util.v2;

namespace Chillisoft.Bo.Loaders.v2
{
    /// <summary>
    /// Uses a variety of xml loaders to load all the facets that make
    /// up a business object class definition
    /// </summary>
    public class XmlClassLoader : XmlLoader
    {
        private PropDefCol _PropDefCol;
        private PrimaryKeyDef _PrimaryKeyDef;
        private KeyDefCol _KeyDefCol;
        private RelationshipDefCol _RelationshipDefCol;
        private string _ClassName;
        private string _AssemblyName;
        private SuperClassDesc _SuperClassDesc;
        private UIDefCol _UIDefCol;
        private string _TableName;
        private bool _SupportsSynchronising;

        /// <summary>
        /// Constructor to initialise a new loader
        /// </summary>
        public XmlClassLoader()
        {
        }

        /// <summary>
        /// Constructor to initialise a new loader with a dtd path
        /// </summary>
        /// <param name="dtdPath">The dtd path</param>
        public XmlClassLoader(string dtdPath) : base(dtdPath)
        {
        }

        /// <summary>
        /// Loads the class data from the xml string provided
        /// </summary>
        /// <param name="xmlClassDef">The xml class definition string.
        /// You can use <code>new StreamReader("filename.xml").ReadToEnd()</code>
        /// to read a continuous string from a file.</param>
        /// <returns>Returns the class definition loaded</returns>
        public ClassDef LoadClass(string xmlClassDef)
        {
            if (xmlClassDef == null || xmlClassDef.Length == 0)
            {
                throw new ArgumentException("The application is unable to read the " +
                    "'classDef' element from the XML class definitions file.  " +
                    "The definitions need one 'classDefs' root element and a 'classDef' " +
                    "element for each class that you are mapping.  XML elements are " +
                    "case-sensitive.");
            }
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlClassDef);
            return LoadClass(doc.DocumentElement);
        }

        /// <summary>
        /// Loads the class data using the xml element provided
        /// </summary>
        /// <param name="classElement">The xml class element</param>
        /// <returns>Returns the class definition loaded</returns>
        public ClassDef LoadClass(XmlElement classElement)
        {
            return (ClassDef) this.Load(classElement);
        }

        /// <summary>
        /// Creates a class definition using the data loaded from the reader
        /// </summary>
        /// <returns>Returns a class definition</returns>
        protected override object Create()
        {
			ClassDef def =
				new ClassDef(_AssemblyName,_ClassName, _PrimaryKeyDef, _PropDefCol, _KeyDefCol, _RelationshipDefCol,
							 _UIDefCol);
			if (_SuperClassDesc != null)
            {
                def.SuperClassDesc = _SuperClassDesc;
            }
            if (_TableName != null && _TableName.Length > 0)
            {
                def.TableName = _TableName;
            }
            def.SupportsSynchronising = _SupportsSynchronising;
            return def;
        }

        /// <summary>
        /// Loads all relevant data from the reader
        /// </summary>
        protected override void LoadFromReader()
        {
            _SuperClassDesc = null;
            itsReader.Read();
            LoadClassInfo();
            LoadTableName();
            LoadSupportsSynchronisation();

            itsReader.Read();
            if (itsReader.Name == "superClassDesc")
            {
                LoadSuperClassDesc();
            }

            LoadPropDefs();

            _KeyDefCol = new KeyDefCol();
            if (itsReader.Name == "keyDef")
            {
                LoadKeyDefs();
            }
            _PrimaryKeyDef = new PrimaryKeyDef();
            if (itsReader.Name == "primaryKeyDef")
            {
                LoadPrimaryKeyDef();
            }
            _RelationshipDefCol = new RelationshipDefCol();
            if (itsReader.Name == "relationshipDef")
            {
                LoadRelationshipDefs();
            }
            _UIDefCol = new UIDefCol();
            if (itsReader.Name == "uiDef")
            {
                LoadUIDefs();
            }
        }

        /// <summary>
        /// Load the super-class data
        /// </summary>
        private void LoadSuperClassDesc()
        {
            XmlSuperClassDescLoader superClassDescLoader = new XmlSuperClassDescLoader(itsDtdPath);
            _SuperClassDesc = superClassDescLoader.LoadSuperClassDesc(itsReader.ReadOuterXml());
        }

        /// <summary>
        /// Loads the relationship data
        /// </summary>
        private void LoadRelationshipDefs()
        {
            XmlRelationshipLoader relationshipLoader = new XmlRelationshipLoader(itsDtdPath);
            do
            {
                _RelationshipDefCol.Add(relationshipLoader.LoadRelationship(itsReader.ReadOuterXml(), _PropDefCol));
            } while (itsReader.Name == "relationshipDef");
        }

        /// <summary>
        /// Loads the UIDef data
        /// </summary>
        private void LoadUIDefs()
        {
            XmlUIDefLoader loader = new XmlUIDefLoader(itsDtdPath);
            do
            {
                _UIDefCol.Add(loader.LoadUIDef(itsReader.ReadOuterXml()));
            } while (itsReader.Name == "uiDef");
        }

        /// <summary>
        /// Loads the key definition data
        /// </summary>
        private void LoadKeyDefs()
        {
            XmlKeyLoader loader = new XmlKeyLoader(itsDtdPath);
            do
            {
                _KeyDefCol.Add(loader.LoadKey(itsReader.ReadOuterXml(), _PropDefCol));
            } while (itsReader.Name == "keyDef");
        }

        /// <summary>
        /// Loads the primary key definition data
        /// </summary>
        private void LoadPrimaryKeyDef()
        {
            XmlPrimaryKeyLoader primaryKeyLoader = new XmlPrimaryKeyLoader(itsDtdPath);
            _PrimaryKeyDef = primaryKeyLoader.LoadPrimaryKey(itsReader.ReadOuterXml(), _PropDefCol);
        }

        /// <summary>
        /// Loads the property definition data
        /// </summary>
        private void LoadPropDefs()
        {
            _PropDefCol = new PropDefCol();
            XmlPropertyLoader propLoader = new XmlPropertyLoader(itsDtdPath);
            do
            {
                _PropDefCol.Add(propLoader.LoadProperty(itsReader.ReadOuterXml()));
            } while (itsReader.Name == "propertyDef");
            //			XmlNodeList xmlPropDefs = itsClassElement.GetElementsByTagName("propertyDef");
            //			XmlPropertyLoader propLoader = new XmlPropertyLoader(itsDtdPath);
            //			foreach (XmlNode xmlPropDef in xmlPropDefs) {
            //				_PropDefCol.Add(propLoader.LoadProperty(xmlPropDef.OuterXml));
            //			}
        }

        /// <summary>
        /// Loads the table name attribute
        /// </summary>
        private void LoadTableName()
        {
            _TableName = itsReader.GetAttribute("tableName");
        }

        /// <summary>
        /// Loads the class type info data
        /// </summary>
        private void LoadClassInfo()
        {
            _ClassName = itsReader.GetAttribute("name");
            _AssemblyName = itsReader.GetAttribute("assembly");

            if (_AssemblyName == null || _AssemblyName.Length == 0)
            {
                string errorMessage = "No assembly name has been specified for the " +
                                      "class definition";
                if (_ClassName != null && _ClassName.Length > 0)
                {
                    errorMessage += " of '" + _ClassName + "'";
                }
                errorMessage += ". Within each 'classDef' element you need to set " +
                                "the 'assembly' attribute, which refers to the project or assembly " +
                                "that contains the class which is being mapped to.";
                throw new XmlException(errorMessage);
            }
            if (_ClassName == null || _ClassName.Length == 0)
            {
                throw new XmlException("No 'name' attribute has been specified for a " +
                   "'classDef' element.  The 'name' attribute indicates the name of the " +
                   "class to which a database table will be mapped.");
            }
        }

        /// <summary>
        /// Loads the attribute that indicates whether synchronisation is
        /// supported
        /// </summary>
        private void LoadSupportsSynchronisation()
        {
            _SupportsSynchronising = Convert.ToBoolean(itsReader.GetAttribute("supportsSynchronising"));
        }
    }
}
