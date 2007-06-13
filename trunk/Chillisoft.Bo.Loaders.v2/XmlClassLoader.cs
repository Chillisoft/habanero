using System;
using System.Collections;
using System.Collections.Generic;
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
        private PropDefCol itsPropDefCol;
        private PrimaryKeyDef itsPrimaryKeyDef;
        private KeyDefCol itsKeyDefCol;
        private RelationshipDefCol itsRelationshipDefCol;
        private string itsClassName;
        private string itsAssemblyName;
        private Type itsClassType;
        private SuperClassDesc itsSuperClassDesc;
        private UIDefCol itsUIDefCol;
        private string itsTableName;
        private bool itsSupportsSynchronising;

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
            ClassDef def = new ClassDef(itsClassType, itsPrimaryKeyDef,
                                        itsPropDefCol, itsKeyDefCol, 
                                        itsRelationshipDefCol, itsUIDefCol);
            if (itsSuperClassDesc != null)
            {
                def.SuperClassDesc = itsSuperClassDesc;
            }
            if (itsTableName != null && itsTableName != "")
            {
                def.TableName = itsTableName;
            }
            def.SupportsSynchronising = itsSupportsSynchronising;
            return def;
        }

        /// <summary>
        /// Loads all relevant data from the reader
        /// </summary>
        protected override void LoadFromReader()
        {
            itsClassType = null;
            itsSuperClassDesc = null;
            itsReader.Read();
            LoadClassType();
            LoadTableName();
            LoadSupportsSynchronisation();

            itsReader.Read();

            List<string> keyDefXmls = new List<string>();
            List<string> propDefXmls = new List<string>();
            List<string> relationshipDefXmls = new List<string>();
            List<string> uiDefXmls = new List<string>();
            string superclassDescXML = null;
            string primaryKeDefXML = null;
            while (itsReader.Name != "classDef")
            {
                switch (itsReader.Name)
                {
                    case "superClassDesc":
                        superclassDescXML = itsReader.ReadOuterXml();
                        break;
                    case "propertyDef":
                        propDefXmls.Add(itsReader.ReadOuterXml());
                        break;
                    case "keyDef":
                        keyDefXmls.Add(itsReader.ReadOuterXml());
                        break;
                    case "primaryKeyDef":
                        primaryKeDefXML = itsReader.ReadOuterXml();
                        break;
                    case "relationshipDef":
                        relationshipDefXmls.Add(itsReader.ReadOuterXml());
                        break;
                    case "uiDef":
                        uiDefXmls.Add(itsReader.ReadOuterXml());
                        break;
                    default:
                        throw new InvalidXmlDefinitionException("The element '" +
                            itsReader.Name + "' is not a recognised class " +
                            "definition element.  Ensure that you have the correct " +
                            "spelling and capitalisation, or see the documentation " +
                            "for available options.");
                        break;
                }
            }

            LoadSuperClassDesc(superclassDescXML);
            LoadPropDefs(propDefXmls);
            LoadKeyDefs(keyDefXmls);
            LoadPrimaryKeyDef(primaryKeDefXML);
            LoadRelationshipDefs(relationshipDefXmls);
            LoadUIDefs(uiDefXmls);
        }

        /// <summary>
        /// Load the super-class data
        /// </summary>
        private void LoadSuperClassDesc(string xmlDef)
        {
            if (xmlDef != null)
            {
                XmlSuperClassDescLoader superClassDescLoader = new XmlSuperClassDescLoader(itsDtdPath);
                itsSuperClassDesc = superClassDescLoader.LoadSuperClassDesc(xmlDef);
            }
        }

        /// <summary>
        /// Loads the relationship data
        /// </summary>
        private void LoadRelationshipDefs(List<string> xmlDefs)
        {
            itsRelationshipDefCol = new RelationshipDefCol();
            XmlRelationshipLoader relationshipLoader = new XmlRelationshipLoader(itsDtdPath);
            foreach (string relDefXml in xmlDefs)
            {
                itsRelationshipDefCol.Add(relationshipLoader.LoadRelationship(relDefXml, itsPropDefCol));
            }
        }

        /// <summary>
        /// Loads the UIDef data
        /// </summary>
        private void LoadUIDefs(List<string> xmlDefs)
        {
            itsUIDefCol = new UIDefCol();
            XmlUIDefLoader loader = new XmlUIDefLoader(itsDtdPath);
            foreach (string uiDefXml in xmlDefs)
            {
                itsUIDefCol.Add(loader.LoadUIDef(uiDefXml));
            }
        }

        /// <summary>
        /// Loads the key definition data
        /// </summary>
        private void LoadKeyDefs(List<string> xmlDefs)
        {
            itsKeyDefCol = new KeyDefCol();
            XmlKeyLoader loader = new XmlKeyLoader(itsDtdPath);
            foreach (string keyDefXml in xmlDefs)
            {
                itsKeyDefCol.Add(loader.LoadKey(keyDefXml, itsPropDefCol));
            }
        }

        /// <summary>
        /// Loads the primary key definition data
        /// </summary>
        private void LoadPrimaryKeyDef(string xmlDef)
        {
            if (xmlDef == null)
            {
                throw new InvalidXmlDefinitionException("Could not find a " +
                    "'primaryKeyDef' element in the class definition for the class '" +
                    itsClassType + "'.  Each class definition requires a primary key " +
                    "definition, which is composed of one or more property definitions, " +
                    "implying that you will need at least one 'propertyDef' element as " +
                    "well.");
            }
            itsPrimaryKeyDef = new PrimaryKeyDef();
            XmlPrimaryKeyLoader primaryKeyLoader = new XmlPrimaryKeyLoader(itsDtdPath);
            itsPrimaryKeyDef = primaryKeyLoader.LoadPrimaryKey(xmlDef, itsPropDefCol);
            if (itsPrimaryKeyDef == null)
            {
                throw new InvalidXmlDefinitionException("There was an error loading " +
                    "the 'primaryKeyDef' element in the class definition for the class '" +
                    itsClassType + "'.  Each class definition requires a primary key " +
                    "definition, which is composed of one or more property definitions, " +
                    "implying that you will need at least one 'propertyDef' element as " +
                    "well.");
            }
        }

        /// <summary>
        /// Loads the property definition data
        /// </summary>
        private void LoadPropDefs(List<string> xmlDefs)
        {
            itsPropDefCol = new PropDefCol();
            XmlPropertyLoader propLoader = new XmlPropertyLoader(itsDtdPath);
            foreach (string propDefXml in xmlDefs)
            {
                itsPropDefCol.Add(propLoader.LoadProperty(propDefXml));
            }
            //			XmlNodeList xmlPropDefs = itsClassElement.GetElementsByTagName("propertyDef");
            //			XmlPropertyLoader propLoader = new XmlPropertyLoader(itsDtdPath);
            //			foreach (XmlNode xmlPropDef in xmlPropDefs) {
            //				itsPropDefCol.Add(propLoader.LoadProperty(xmlPropDef.OuterXml));
            //			}
        }

        /// <summary>
        /// Loads the table name attribute
        /// </summary>
        private void LoadTableName()
        {
            itsTableName = itsReader.GetAttribute("tableName");
        }

        /// <summary>
        /// Loads the class type data
        /// </summary>
        private void LoadClassType()
        {
            itsClassName = itsReader.GetAttribute("name");
            itsAssemblyName = itsReader.GetAttribute("assembly");

            if (itsAssemblyName == null || itsAssemblyName.Length == 0)
            {
                string errorMessage = "No assembly name has been specified for the " +
                                      "class definition";
                if (itsClassName != null && itsClassName.Length > 0)
                {
                    errorMessage += " of '" + itsClassName + "'";
                }
                errorMessage += ". Within each 'classDef' element you need to set " +
                                "the 'assembly' attribute, which refers to the project or assembly " +
                                "that contains the class which is being mapped to.";
                throw new XmlException(errorMessage);
            }
            if (itsClassName == null || itsClassName.Length == 0)
            {
                throw new XmlException("No 'name' attribute has been specified for a " +
                   "'classDef' element.  The 'name' attribute indicates the name of the " +
                   "class to which a database table will be mapped.");
            }

            try
            {
                itsClassType = TypeLoader.LoadType(itsAssemblyName, itsClassName);
            }
            catch (UnknownTypeNameException ex)
            {
                throw new UnknownTypeNameException("Unable to load the class type while " +
                       "attempting to load a class definition, given the 'assembly' as: '" +
                       itsAssemblyName + "', and the 'class' as: '" + itsClassName +
                       "'. Check that the class exists in the given assembly name and " +
                       "that spelling and capitalisation are correct.", ex);
                //itsClassType = null;
            }
        }

        /// <summary>
        /// Loads the attribute that indicates whether synchronisation is
        /// supported
        /// </summary>
        private void LoadSupportsSynchronisation()
        {
            itsSupportsSynchronising = Convert.ToBoolean(itsReader.GetAttribute("supportsSynchronising"));
        }
    }
}