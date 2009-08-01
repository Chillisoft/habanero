//---------------------------------------------------------------------------------
// Copyright (C) 2008 Chillisoft Solutions
// 
// This file is part of the Habanero framework.
// 
//     Habanero is a free framework: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     The Habanero framework is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Xml;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;

namespace Habanero.BO.Loaders
{
    /// <summary>
    /// Facilitates the loading of class definitions from xml data, which define
    /// the terms and properties of an xml structure before it can be
    /// populated with data
    /// </summary>
    public class XmlClassDefsLoader : XmlLoader, IClassDefsLoader
    {
        private ClassDefCol _classDefList;
        //private IList _classDefList;
        private readonly string _xmlClassDefs;
        

        /// <summary>
        /// Constructor to initialise a new loader. If you create the object
        /// with this constructor, you will need to use methods
        /// that provide the actual class definitions at a later stage.
        /// </summary>
        public XmlClassDefsLoader()
        {
        }

        /// <summary>
        /// Constructor to create a new list of class definitions from the
        /// string provided, using the dtd path provided
        /// </summary>
        /// <param name="xmlClassDefs">The string containing all the
        /// class definitions. If you are loading these from 
        /// a file, you can use 
        /// <code>new StreamReader("filename.xml").ReadToEnd()</code>
        /// to create a continuous string.</param>
        /// <param name="dtdLoader">The dtd loader</param>
        public XmlClassDefsLoader(string xmlClassDefs, DtdLoader dtdLoader) : this(xmlClassDefs, dtdLoader, null)
        {
        }

        /// <summary>
        /// Constructor to create a new list of class definitions from the
        /// string provided, using the dtd path provided
        /// </summary>
        /// <param name="xmlClassDefs">The string containing all the
        /// class definitions. If you are loading these from 
        /// a file, you can use 
        /// <code>new StreamReader("filename.xml").ReadToEnd()</code>
        /// to create a continuous string.</param>
        /// <param name="dtdLoader">The dtd loader</param>
        /// <param name="defClassFactory">The factory for the definition classes</param>
        public XmlClassDefsLoader(string xmlClassDefs, DtdLoader dtdLoader, IDefClassFactory defClassFactory)
            : base(dtdLoader, defClassFactory)
        {
            _xmlClassDefs = xmlClassDefs;
        }


        /// <summary>
        /// Loads class definitions, converting them from a 
        /// string containing these definitions to an IList object.
        /// If the conversion fails, an error message will be sent to the 
        /// console.
        /// </summary>
        /// <param name="xmlClassDefs">The string containing all the
        /// class definitions. If you are loading these from 
        /// a file, you can use 
        /// <code>new StreamReader("filename.xml").ReadToEnd()</code>
        /// to create a continuous string.</param>
        /// <returns>Returns an IList object containing the definitions</returns>
        public ClassDefCol LoadClassDefs(string xmlClassDefs)
            ///// <returns>Returns an IList object containing the definitions</returns>
            //public IList LoadClassDefs(string xmlClassDefs)
        {
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.LoadXml(xmlClassDefs);
            }
            catch (XmlException ex)
            {
                //Console.Out.WriteLine(ExceptionUtil.GetExceptionString(ex, 0, true));
                throw new XmlException
                    ("The class definitions XML file has no root "
                     + "element 'classes'.  The document needs a master 'classes' element "
                     + "and individual 'class' elements for each of the classes you are " + "defining.", ex);
            }
            return LoadClassDefs(doc.DocumentElement);
        }

        /// <summary>
        /// As with LoadClassDefs(string), but uses the definition string 
        /// provided on instatiation of the object if you used the
        /// parameterised constructor.
        /// </summary>
        /// <returns>Returns a ClassDefCol containing the definitions</returns>
        public ClassDefCol LoadClassDefs() ///// <returns>Returns an IList object containing the definitions</returns>
            //public IList LoadClassDefs()
        {
            return LoadClassDefs(_xmlClassDefs);
        }


        /// <summary>
        /// As with LoadClassDefs(string), but uses the root element as a
        /// starting reference point.
        /// </summary>
        /// <param name="allClassesElement">The root element</param>
        /// <returns>Returns an IList object containing the definitions</returns>
        public ClassDefCol LoadClassDefs(XmlElement allClassesElement)
            ///// <returns>Returns an IList object containing the definitions</returns>
            //public IList LoadClassDefs(XmlElement allClassesElement)
        {
            return (ClassDefCol) this.Load(allClassesElement);
        }

        /// <summary>
        /// Returns the IList object that contains the class definitions
        /// </summary>
        /// <returns>Returns an IList object</returns>
        protected override object Create()
        {
            return _classDefList;
        }

        /// <summary>
        /// Loads the class definition data from the reader
        /// </summary>
        protected override void LoadFromReader()
        {
            _classDefList = _defClassFactory.CreateClassDefCol();
            //_classDefList = new ArrayList();
            _reader.Read();
            _reader.Read();
            do
            {
                XmlClassLoader classLoader = new XmlClassLoader(DtdLoader, _defClassFactory);
                _classDefList.Add(classLoader.LoadClass(_reader.ReadOuterXml()));
            } while (_reader.Name == "class");
            DoPostLoadChecks(); //TODO: Hageshen 18/02/2009 this should be replaced need to find a different way of doing this since this is causing
                // Issues with Firestarter
        }

        private void DoPostLoadChecks()
        {
            UpdateOwningBOHasForeignKey(_classDefList);
            CheckRelationships(_classDefList);
            UpdateKeyDefinitionsWithBoProp(_classDefList);
            UpdatePrimaryKeys(_classDefList);
            //TODO Brett 02 Feb 2009: check valid business object lookup definition i.e. is property valid and is sort direction valid
            //TODO Brett 02 Feb 2009: Validation for relationships that reversed relationship and forward relationship for
            //  a bo are not both set to OwnerHasForeignKey
        }

        private static void UpdateOwningBOHasForeignKey(ClassDefCol classDefCol)
        {
            foreach (ClassDef classDef in classDefCol)
            {
                foreach (RelationshipDef relationshipDef in classDef.RelationshipDefCol)
                {
                    if (relationshipDef is MultipleRelationshipDef)
                    {
                        relationshipDef.OwningBOHasForeignKey = true;
                    }
                    else if (relationshipDef.OwningBOHasForeignKey)
                    {
                        relationshipDef.OwningBOHasForeignKey = !OwningClassHasPrimaryKey(relationshipDef, classDef, classDefCol);
                        relationshipDef.OwningBOHasPrimaryKey = OwningClassHasPrimaryKey(relationshipDef, classDef, classDefCol);
                    }
                }
            }
        }


        private static void UpdatePrimaryKeys(IEnumerable<ClassDef> col)
        {
            foreach (ClassDef classDef in col)
            {
                PrimaryKeyDef primaryKeyDef = classDef.PrimaryKeyDef;
                if (primaryKeyDef == null) continue;
                if (!primaryKeyDef.IsGuidObjectID) continue;
                IPropDef keyPropDef = primaryKeyDef[0];
                if (primaryKeyDef.IsGuidObjectID && keyPropDef.PropertyType != typeof(Guid))
                {
                    throw new InvalidXmlDefinitionException("In the class called '" + classDef.ClassNameFull + 
                        "', the primary key is set as IsObjectID but the property '" + keyPropDef.PropertyName +
                    "' defined as part of the ObjectID primary key is not a Guid.");
                }
                keyPropDef.Compulsory = true;
                keyPropDef.ReadWriteRule = PropReadWriteRule.WriteNew;
            }
        }

        private static void UpdateKeyDefinitionsWithBoProp(ClassDefCol col)
        {
            Dictionary<ClassDef, PropDefCol> loadedFullPropertyLists = new Dictionary<ClassDef, PropDefCol>();
            foreach (ClassDef classDef in col)
            {
                UpdateKeyDefinitionsWithBoProp(loadedFullPropertyLists, classDef, col);
            }
        }

        private static void UpdateKeyDefinitionsWithBoProp
            (IDictionary<ClassDef, PropDefCol> loadedFullPropertyLists, ClassDef classDef, ClassDefCol col)
        {
            //This method fixes all the references for a particulare class definitions key definition
            // the issue is that the key definition at the beginiing has a reference to a PropDef that is not
            // valid i.e. does not reference the Prop Def for a particular property.
            // This method attempts to find the actual prop def from the class def and associated it with the keydef.
            if (classDef == null) return;
            PropDefCol allPropsForAClass = GetAllClassDefProps(loadedFullPropertyLists, classDef, col);
            foreach (KeyDef keyDef in classDef.KeysCol)
            {
                PropDefCol propDefCol = new PropDefCol();
                foreach (PropDef propDef in keyDef)
                {
                    propDefCol.Add(propDef);
                }
                keyDef.Clear();
                //Check Key Properties
                foreach (PropDef propDef in propDefCol)
                {
                    string propertyName = propDef.PropertyName;
                    if (!allPropsForAClass.Contains(propertyName))
                    {
                        throw new InvalidXmlDefinitionException
                            (String.Format
                                 ("In a 'prop' element for the '{0}' key of "
                                  + "the '{1}' class, the propery '{2}' given in the "
                                  + "'name' attribute does not exist for the class or for any of it's superclasses. "
                                  + "Either add the property definition or check the spelling and "
                                  + "capitalisation of the specified property.", keyDef.KeyName,
                                  classDef.ClassName, propertyName));
                    }
                    IPropDef keyPropDef = allPropsForAClass[propertyName];
                    keyDef.Add(keyPropDef);
                }
            }
        }

        internal static PropDefCol GetAllClassDefProps
            (IDictionary<ClassDef, PropDefCol> loadedFullPropertyLists, ClassDef classDef, ClassDefCol col)
        {
            PropDefCol allProps;
            if (loadedFullPropertyLists.ContainsKey(classDef))
            {
                allProps = loadedFullPropertyLists[classDef];
            }
            else
            {
                allProps = new PropDefCol();
                ClassDef currentClassDef = classDef;
                while (currentClassDef != null)
                {
                    foreach (PropDef propDef in currentClassDef.PropDefcol)
                    {
                        if (allProps.Contains(propDef.PropertyName)) continue;
                        allProps.Add(propDef);
                    }
                    currentClassDef = GetSuperClassClassDef(currentClassDef, col);
                }
                loadedFullPropertyLists.Add(classDef, allProps);
            }
            return allProps;
        }

        private static ClassDef GetSuperClassClassDef(ClassDef currentClassDef, ClassDefCol col)
        {
            SuperClassDef superClassDef = currentClassDef.SuperClassDef;
            return superClassDef == null ? null : col[superClassDef.AssemblyName, superClassDef.ClassName];
        }


        private static void CheckRelationships(ClassDefCol classDefs)
        {
            Dictionary<ClassDef, PropDefCol> loadedFullPropertyLists = new Dictionary<ClassDef, PropDefCol>();
            foreach (ClassDef classDef in classDefs)
            {
                CheckRelationshipsForAClassDef(loadedFullPropertyLists, classDef, classDefs);
            }
        }

        private static void CheckRelationshipsForAClassDef
            (IDictionary<ClassDef, PropDefCol> loadedFullPropertyLists, ClassDef classDef, ClassDefCol classDefs)
        {
            if (classDef == null) return;
            
            foreach (RelationshipDef relationshipDef in classDef.RelationshipDefCol)
            {
                ClassDef relatedObjectClassDef = GetRelatedObjectClassDef(classDefs, relationshipDef);
                ValidateReverseRelationship(classDef, relationshipDef, relatedObjectClassDef);
                ValidateRelKeyDef(classDef, classDefs, relationshipDef, relatedObjectClassDef, loadedFullPropertyLists);
            }
        }

        private static ClassDef GetRelatedObjectClassDef(ClassDefCol classDefs, RelationshipDef relationshipDef)
        {
            ClassDef relatedObjectClassDef;
            try
            {
                relatedObjectClassDef =
                    classDefs[relationshipDef.RelatedObjectAssemblyName, relationshipDef.RelatedObjectClassNameWithTypeParameter];
            }
            catch (HabaneroDeveloperException)
            {
                try
                {
                    relatedObjectClassDef =
                        ClassDef.ClassDefs[relationshipDef.RelatedObjectAssemblyName, relationshipDef.RelatedObjectClassNameWithTypeParameter];
                }
                catch (HabaneroDeveloperException ex)
                {
                    throw new InvalidXmlDefinitionException
                        (string.Format
                             ("The relationship '{0}' could not be loaded because when trying to retrieve its related class the folllowing error was thrown '{1}'",
                              relationshipDef.RelationshipName, ex.Message), ex);
                }
            }
            return relatedObjectClassDef;
        }

        private static void ValidateRelKeyDef
            (ClassDef classDef, ClassDefCol classDefs, IRelationshipDef relationshipDef, ClassDef relatedObjectClassDef,
             IDictionary<ClassDef, PropDefCol> loadedFullPropertyLists)
        {
            PropDefCol allPropsForClassDef = GetAllClassDefProps(loadedFullPropertyLists, classDef, classDefs);
            PropDefCol allPropsForRelatedClassDef = GetAllClassDefProps
                (loadedFullPropertyLists, relatedObjectClassDef, classDefs);
            // Check Relationship Properties
            foreach (RelPropDef relPropDef in relationshipDef.RelKeyDef)
            {
                string ownerPropertyName = relPropDef.OwnerPropertyName;
                if (!allPropsForClassDef.Contains(ownerPropertyName))
                {
                    throw new InvalidXmlDefinitionException
                        (String.Format
                             ("In a 'relatedProperty' element for the '{0}' relationship of "
                              + "the '{1}' class, the property '{2}' given in the "
                              + "'property' attribute does not exist for the class or for any of it's superclasses. "
                              + "Either add the property definition or check the spelling and "
                              +
                              "capitalisation of the specified property. Check in the ClassDefs.xml file or fix in Firestarter",
                              relationshipDef.RelationshipName, classDef.ClassName, ownerPropertyName));
                }
                string relatedClassPropName = relPropDef.RelatedClassPropName;
                if (!allPropsForRelatedClassDef.Contains(relatedClassPropName))
                {
                    throw new InvalidXmlDefinitionException
                        (String.Format
                             ("In a 'relatedProperty' element for the '{0}' relationship of "
                              + "the '{1}' class, the property '{2}' given in the "
                              +
                              "'relatedProperty' attribute does not exist for the Related class '{3}' or for any of it's superclasses. "
                              + "Either add the property definition or check the spelling and "
                              +
                              "capitalisation of the specified property. Check in the ClassDefs.xml file or fix in Firestarter",
                              relationshipDef.RelationshipName, classDef.ClassName, relatedClassPropName,
                              relatedObjectClassDef.ClassNameFull));
                }
            }
        }

        private static void ValidateReverseRelationship
            (IClassDef classDef, RelationshipDef relationshipDef, ClassDef relatedClassDef)
        {

            if (!HasReverseRelationship(relationshipDef)) return;

            string reverseRelationshipName = relationshipDef.ReverseRelationshipName;
            if (!relatedClassDef.RelationshipDefCol.Contains(reverseRelationshipName))
            {
                throw new InvalidXmlDefinitionException
                    (string.Format
                         ("The relationship '{0}' could not be loaded for because the reverse relationship '{1}' defined for class '{2}' is not defined as a relationship for class '{2}'. Please check your ClassDefs.xml or fix in Firestarter.",
                          relationshipDef.RelationshipName, reverseRelationshipName, relatedClassDef.ClassNameFull));
            }

            RelationshipDef reverseRelationshipDef = (RelationshipDef) relatedClassDef.RelationshipDefCol[reverseRelationshipName];
            CheckReverseRelationshipRelKeyDefProps(relationshipDef, relatedClassDef, reverseRelationshipName, reverseRelationshipDef, classDef);
//            if (!reverseRelationshipDef.OwningBOHasForeignKey) return;
//
//            if (OwningClassHasPrimaryKey(reverseRelationshipDef, relatedClassDef))
//            {
//                reverseRelationshipDef.OwningBOHasForeignKey = false;
//                return;
//            }
            if (relationshipDef.OwningBOHasForeignKey && reverseRelationshipDef.OwningBOHasForeignKey)
            {
                string errorMessage = string.Format
                    ("The relationship '{0}' could not be loaded because the reverse relationship '{1}' defined for the related class '{2}' and the relationship '{3}' defined for the class '{4}' are both set up as owningBOHasForeignKey = true. Please check your ClassDefs.xml or fix in Firestarter.",
                     relationshipDef.RelationshipName, reverseRelationshipName, relatedClassDef.ClassNameFull,
                     relationshipDef.RelationshipName, classDef.ClassNameFull);
                throw new InvalidXmlDefinitionException(errorMessage);
            }
        }
        /// <summary>
        /// Checks to see if the relationship and reverse relationship are defined for the same relationship.
        /// </summary>
        /// <param name="relationshipDef"></param>
        /// <param name="relatedClassDef"></param>
        /// <param name="reverseRelationshipName"></param>
        /// <param name="reverseRelationshipDef"></param>
        /// <param name="classDef"></param>
        private static void CheckReverseRelationshipRelKeyDefProps(IRelationshipDef relationshipDef, IClassDef relatedClassDef, string reverseRelationshipName, IRelationshipDef reverseRelationshipDef, IClassDef classDef)
        {
            if (!ReverseRelationshipHasSameProps(relationshipDef, reverseRelationshipDef))
            {
                string message = string.Format
                    ("The relationship '{0}' could not be loaded because the reverse relationship '{1}' " 
                     + "defined for the related class '{2}' and the relationship '{3}' defined for the class '{4}' do not have the same properties defined as the relationship keys"
                     ,relationshipDef.RelationshipName, reverseRelationshipName, relatedClassDef.ClassNameFull,
                     relationshipDef.RelationshipName, classDef.ClassNameFull);
                throw new InvalidXmlDefinitionException(message);
            }
        }

        private static bool ReverseRelationshipHasSameProps(IRelationshipDef relationshipDef, IRelationshipDef reverseRelationshipDef)
        {
            if (relationshipDef.RelKeyDef.Count != reverseRelationshipDef.RelKeyDef.Count) return false;
            foreach (IRelPropDef relPropDef in relationshipDef.RelKeyDef)
            {
                bool foundMatch = false;
                foreach (IRelPropDef reverseRelPropDef in reverseRelationshipDef.RelKeyDef)
                {
                    if (relPropDef.OwnerPropertyName == reverseRelPropDef.RelatedClassPropName 
                        && relPropDef.RelatedClassPropName == reverseRelPropDef.OwnerPropertyName)
                    {
                        foundMatch = true;
                        break;
                    }
                }
                if (!foundMatch) return false;
            }
            return true;
        }

        private static bool OwningClassHasPrimaryKey(IRelationshipDef relationshipDef, ClassDef classDef, ClassDefCol classDefCol)
        {
            //For each Property in the Relationship Key check if it is defined as the primary key for the
            //class if it is then check the other properties else this is not a primaryKey
            PrimaryKeyDef primaryKeyDef = ClassDefHelper.GetPrimaryKeyDef(classDef, classDefCol);
            foreach (IRelPropDef relPropDef in relationshipDef.RelKeyDef)
            {
                bool isInKeyDef = false;
                foreach (IPropDef propDef in primaryKeyDef)
                {
                    if (propDef.PropertyName != relPropDef.OwnerPropertyName)
                    {
                        isInKeyDef = false;
                        break;
                    }
                    isInKeyDef = true;
                }
                if (!isInKeyDef) return false;
            }
            return true;
        }

        private static bool HasReverseRelationship(IRelationshipDef relationshipDef)
        {
            return !string.IsNullOrEmpty(relationshipDef.ReverseRelationshipName);
        }
    }
}