//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
// 
// This file is part of Habanero Standard.
// 
//     Habanero Standard is free software: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     Habanero Standard is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with Habanero Standard.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using System;
using System.Xml;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;
using Habanero.BO;
using Habanero.Base;
using Habanero.Util;
using BusinessObject=Habanero.BO.BusinessObject;

namespace Habanero.Test
{
    /// <summary>
    /// Summary description for MyBO.
    /// </summary>
    public class MyBO : BusinessObject
    {
        public MyBO(): base() {}

        //public MyBO(ClassDef def) : base(def)
        //{
        //}

        //public MyBO(ClassDef def, IDatabaseConnection conn) : base(def, conn)
        //{
        //}

        public MyBO(IDatabaseConnection conn) : base(conn){}

        protected override ClassDef ConstructClassDef()
        {
            return _classDef;
        }

        public string MyName
        {
            get { return "MyNameIsMyBo"; }
        }

        public Guid MyBoID
        {
            get
            {
                return (Guid)this.GetPropertyValue("MyBoID");
            }
        }

        public static ClassDef LoadDefaultClassDef()
        {
            XmlClassLoader itsLoader = new XmlClassLoader();
            ClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""MyBO"" assembly=""Habanero.Test"">
					<property  name=""MyBoID"" />
					<property  name=""TestProp"" />
					<property  name=""TestProp2"" />
					<primaryKey>
						<prop name=""MyBoID"" />
					</primaryKey>
					<ui>
						<grid>
							<column heading=""Test Prop"" property=""TestProp"" type=""DataGridViewTextBoxColumn"" />
							<column heading=""Test Prop 2"" property=""TestProp2"" type=""DataGridViewComboBoxColumn"" />
						</grid>
						<form>
							<tab name=""Tab1"">
								<columnLayout>
									<field label=""Test Prop"" property=""TestProp"" type=""TextBox"" mapperType=""TextBoxMapper"" />
									<field label=""Test Prop 2"" property=""TestProp2"" type=""TextBox"" mapperType=""TextBoxMapper"" />
								</columnLayout>
							</tab>
						</form>
					</ui>
					<ui name=""Alternate"">
						<grid>
							<column heading=""Test Prop"" property=""TestProp"" type=""DataGridViewTextBoxColumn"" />
						</grid>
						<form>
							<tab name=""Tab1"">
								<columnLayout>
									<field label=""Test Prop"" property=""TestProp"" type=""TextBox"" mapperType=""TextBoxMapper"" />
								</columnLayout>
							</tab>
						</form>
					</ui>                    
				</class>
			");
			ClassDef.ClassDefs.Add(itsClassDef);
			return itsClassDef;
        }

        public static ClassDef LoadClassDefWithNoLookup()
        {
            XmlClassLoader itsLoader = new XmlClassLoader();
            ClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""MyBO"" assembly=""Habanero.Test"">
					<property  name=""MyBoID"" />
					<property  name=""TestProp"" />
					<property  name=""TestProp2"" />
					<primaryKey>
						<prop name=""MyBoID"" />
					</primaryKey>
					<ui>
						<grid>
							<column heading=""Test Prop"" property=""TestProp"" type=""DataGridViewTextBoxColumn"" />
							<column heading=""Test Prop 2"" property=""TestProp2"" type=""DataGridViewTextBoxColumn"" />
						</grid>
						<form>
							<tab name=""Tab1"">
								<columnLayout>
									<field label=""Test Prop"" property=""TestProp"" type=""TextBox"" mapperType=""TextBoxMapper"" />
									<field label=""Test Prop 2"" property=""TestProp2"" type=""TextBox"" mapperType=""TextBoxMapper"" />
								</columnLayout>
							</tab>
						</form>
					</ui>
                    <ui name=""Success1"">
						<grid sortColumn=""TestProp"">
							<column property=""TestProp"" />
						</grid>
					</ui>
                    <ui name=""Success2"">
						<grid sortColumn=""TestProp asc"">
							<column property=""TestProp"" />
						</grid>
					</ui>
                    <ui name=""Success3"">
						<grid sortColumn=""TestProp desc"">
							<column property=""TestProp"" />
						</grid>
					</ui>
                    <ui name=""Success4"">
						<grid sortColumn=""TestProp des"">
							<column property=""TestProp"" />
						</grid>
					</ui>
                    <ui name=""Error1"">
						<grid sortColumn=""TestProps"">
							<column property=""TestProp"" />
						</grid>
					</ui>
                    <ui name=""Error2"">
						<grid sortColumn=""TestProps desc"">
							<column property=""TestProp"" />
						</grid>
					</ui>
                    <ui name=""Error3"">
						<grid sortColumn=""TestProps descs"">
							<column property=""TestProp"" />
						</grid>
					</ui>
				</class>

			");
			ClassDef.ClassDefs.Add(itsClassDef);
			return itsClassDef;
        }

        public static ClassDef LoadClassDefWithBoolean()
        {
            XmlClassLoader itsLoader = new XmlClassLoader();
            ClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""MyBO"" assembly=""Habanero.Test"">
					<property  name=""MyBoID"" />
					<property  name=""TestProp"" />
					<property  name=""TestProp2"" />
					<property  name=""TestBoolean"" type=""Boolean"" />
					<primaryKey>
						<prop name=""MyBoID"" />
					</primaryKey>
					<ui>
						<grid>
							<column heading=""Test Prop"" property=""TestProp"" type=""DataGridViewTextBoxColumn"" />
							<column heading=""Test Prop 2"" property=""TestProp2"" type=""DataGridViewTextBoxColumn"" />
							<column heading=""Test Boolean"" property=""TestBoolean"" type=""DataGridViewCheckBoxColumn"" />
						</grid>
						<form>
							<tab name=""Tab1"">
								<columnLayout>
									<field label=""Test Prop"" property=""TestProp"" type=""TextBox"" mapperType=""TextBoxMapper"" />
									<field label=""Test Prop 2"" property=""TestProp2"" type=""TextBox"" mapperType=""TextBoxMapper"" />
									<field label=""Test Boolean"" property=""TestBoolean"" type=""CheckBox"" mapperType=""CheckBoxMapper"" />
								</columnLayout>
							</tab>
						</form>
					</ui>
				</class>
				
			");
			ClassDef.ClassDefs.Add(itsClassDef);
			return itsClassDef;
        }

        public static ClassDef LoadClassDefWithDateTime()
        {
            XmlClassLoader itsLoader = new XmlClassLoader();
            ClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""MyBO"" assembly=""Habanero.Test"">
					<property  name=""MyBoID"" />
					<property  name=""TestProp"" />
					<property  name=""TestProp2"" />
					<property  name=""TestDateTime"" type=""DateTime"" />
					<primaryKey>
						<prop name=""MyBoID"" />
					</primaryKey>
					<ui>
						<grid>
							<column heading=""Test Prop"" property=""TestProp"" type=""DataGridViewTextBoxColumn"" />
							<column heading=""Test Prop 2"" property=""TestProp2"" type=""DataGridViewTextBoxColumn"" />
							<column heading=""Test Boolean"" property=""TestBoolean"" type=""DataGridViewCheckBoxColumn"" />
						</grid>
						<form>
							<tab name=""Tab1"">
								<columnLayout>
									<field label=""Test Prop"" property=""TestProp"" type=""TextBox"" mapperType=""TextBoxMapper"" />
									<field label=""Test Prop 2"" property=""TestProp2"" type=""TextBox"" mapperType=""TextBoxMapper"" />
									<field label=""Test Boolean"" property=""TestBoolean"" type=""CheckBox"" mapperType=""CheckBoxMapper"" />
								</columnLayout>
							</tab>
						</form>
					</ui>
				</class>
				
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }

        public static ClassDef LoadClassDefWithLookup()
        {
            XmlClassLoader itsLoader = new XmlClassLoader();
            ClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""MyBO"" assembly=""Habanero.Test"">
					<property  name=""MyBoID"" />
					<property  name=""TestProp"" />
					<property  name=""TestProp2"" type=""Guid"" >
						<simpleLookupList>
							<item display=""s1"" value=""{E6E8DC44-59EA-4e24-8D53-4A43DC2F25E7}"" />
							<item display=""s2"" value=""{F428FADC-3740-412c-91A7-ECEB4D414414}"" />
						</simpleLookupList>
					</property>
					<primaryKey>
						<prop name=""MyBoID"" />
					</primaryKey>
					<ui>
						<grid>
							<column heading=""Test Prop"" property=""TestProp"" type=""DataGridViewTextBoxColumn"" />
							<column heading=""Test Prop 2"" property=""TestProp2"" type=""DataGridViewComboBoxColumn"" />
						</grid>
						<form>
							<tab name=""Tab1"">
								<columnLayout>
									<field label=""Test Prop"" property=""TestProp"" type=""TextBox"" mapperType=""TextBoxMapper"" />
									<field label=""Test Prop 2"" property=""TestProp2"" type=""TextBox"" mapperType=""TextBoxMapper"" />
								</columnLayout>
							</tab>
						</form>
					</ui>
					<ui name=""duplicateColumns"">
						<grid>
							<column heading=""Test Prop"" property=""TestProp"" type=""DataGridViewTextBoxColumn"" />
							<column heading=""Test Prop"" property=""TestProp"" type=""DataGridViewTextBoxColumn"" />
							<column heading=""Test Prop 2"" property=""TestProp2"" type=""DataGridViewComboBoxColumn"" />
						</grid>
					</ui>
				</class>


			");
			ClassDef.ClassDefs.Add(itsClassDef);
			return itsClassDef;
        }

        public static ClassDef LoadClassDefWithSimpleIntegerLookup()
        {
            XmlClassLoader itsLoader = new XmlClassLoader();
            ClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""MyBO"" assembly=""Habanero.Test"">
					<property  name=""MyBoID"" />
					<property  name=""TestProp"" />
                    <property name=""TestProp2"" type=""Int32"" default=""1"" compulsory=""true"">
                      <simpleLookupList>
                        <item display=""Integer"" value=""2"" />
                        <item display=""Selection"" value=""3"" />
                        <item display=""Text"" value=""1"" />
                      </simpleLookupList>
                    </property>
					<primaryKey>
						<prop name=""MyBoID"" />
					</primaryKey>
					<ui>
						<grid>
							<column heading=""Test Prop"" property=""TestProp"" type=""DataGridViewTextBoxColumn"" />
							<column heading=""Test Prop 2"" property=""TestProp2"" type=""DataGridViewComboBoxColumn"" />
						</grid>
						<form>
							<tab name=""Tab1"">
								<columnLayout>
									<field label=""Test Prop"" property=""TestProp"" type=""TextBox"" mapperType=""TextBoxMapper"" />
									<field label=""Test Prop 2"" property=""TestProp2"" type=""TextBox"" mapperType=""TextBoxMapper"" />
								</columnLayout>
							</tab>
						</form>
					</ui>
				</class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }
        public static ClassDef LoadClassDefWithStringLookup()
        {
            XmlClassLoader itsLoader = new XmlClassLoader();
            ClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""MyBO"" assembly=""Habanero.Test"">
					<property  name=""MyBoID"" />
					<property  name=""TestProp"" />
					<property  name=""TestProp2"" >
						<simpleLookupList>
							<item display=""Started"" value=""S"" />
							<item display=""Complete"" value=""C"" />
						</simpleLookupList>
					</property>
					<primaryKey>
						<prop name=""MyBoID"" />
					</primaryKey>
					<ui>
						<grid>
							<column heading=""Test Prop"" property=""TestProp"" type=""DataGridViewTextBoxColumn"" />
							<column heading=""Test Prop 2"" property=""TestProp2"" type=""DataGridViewComboBoxColumn"" />
						</grid>
						<form>
							<tab name=""Tab1"">
								<columnLayout>
									<field label=""Test Prop"" property=""TestProp"" type=""TextBox"" mapperType=""TextBoxMapper"" />
									<field label=""Test Prop 2"" property=""TestProp2"" type=""TextBox"" mapperType=""TextBoxMapper"" />
								</columnLayout>
							</tab>
						</form>
					</ui>
				</class>


			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }

        public static ClassDef LoadClassDefWithBOLookup()
        {
            return LoadClassDefWithBOLookup("");
        }
        public static ClassDef LoadClassDefWithBOLookup(string boLookupCriteria)
        {
            XmlClassLoader itsLoader = new XmlClassLoader();
            ClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""MyBO"" assembly=""Habanero.Test"">
					<property  name=""MyBoID"" />
					<property  name=""TestProp"" />
					<property  name=""TestProp2"" type=""Guid"" >
						<businessObjectLookupList class=""ContactPerson"" assembly=""Habanero.Test.BO"" "
                    + (String.IsNullOrEmpty(boLookupCriteria) ? "" : String.Format(@"criteria=""{0}"" ", ConvertToXmlString(boLookupCriteria, XmlNodeType.Attribute)))
                    + @"/>
					</property>
					<primaryKey>
						<prop name=""MyBoID"" />
					</primaryKey>
					<ui>
						<grid>
							<column heading=""Test Prop"" property=""TestProp"" type=""DataGridViewTextBoxColumn"" />
							<column heading=""Test Prop 2"" property=""TestProp2"" type=""DataGridViewComboBoxColumn"" />
						</grid>
						<form>
							<tab name=""Tab1"">
								<columnLayout>
									<field label=""Test Prop"" property=""TestProp"" type=""TextBox"" mapperType=""TextBoxMapper"" />
									<field label=""Test Prop 2"" property=""TestProp2"" type=""TextBox"" mapperType=""TextBoxMapper"" />
								</columnLayout>
							</tab>
						</form>
					</ui>
				</class>


			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }

        private static string ConvertToXmlString(string input, XmlNodeType xmlNodeType)
        {
            XmlDocument doc;
            doc = new XmlDocument();
            doc.LoadXml("<foo/>");
            string result = input;
            switch (xmlNodeType)
            {
                case XmlNodeType.None:
                    break;
                case XmlNodeType.Element:
                    break;
                case XmlNodeType.Attribute:
                    XmlAttribute newAttribute = doc.CreateAttribute("genre");
                    newAttribute.Value = input;
                    doc.DocumentElement.Attributes.Append(newAttribute);
                    result = newAttribute.InnerXml;
                    break;
                case XmlNodeType.Text:
                    doc.DocumentElement.InnerText = input;
                    result = doc.DocumentElement.InnerXml;
                    break;
                case XmlNodeType.CDATA:
                    break;
                case XmlNodeType.EntityReference:
                    break;
                case XmlNodeType.Entity:
                    break;
                case XmlNodeType.ProcessingInstruction:
                    break;
                case XmlNodeType.Comment:
                    break;
                case XmlNodeType.Document:
                    break;
                case XmlNodeType.DocumentType:
                    break;
                case XmlNodeType.DocumentFragment:
                    break;
                case XmlNodeType.Notation:
                    break;
                case XmlNodeType.Whitespace:
                    break;
                case XmlNodeType.SignificantWhitespace:
                    break;
                case XmlNodeType.EndElement:
                    break;
                case XmlNodeType.EndEntity:
                    break;
                case XmlNodeType.XmlDeclaration:
                    break;
                default:
                    throw new ArgumentOutOfRangeException("xmlNodeType");
            }
            return result;
        }


        public static ClassDef LoadClassDefWithBOStringLookup()
        {
            XmlClassLoader itsLoader = new XmlClassLoader();
            ClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""MyBO"" assembly=""Habanero.Test"">
					<property  name=""MyBoID"" />
					<property  name=""TestProp"" />
					<property  name=""TestProp2"" >
						<businessObjectLookupList class=""ContactPerson"" assembly=""Habanero.Test.BO"" />
					</property>
					<primaryKey>
						<prop name=""MyBoID"" />
					</primaryKey>
					<ui>
						<grid>
							<column heading=""Test Prop"" property=""TestProp"" type=""DataGridViewTextBoxColumn"" />
							<column heading=""Test Prop 2"" property=""TestProp2"" type=""DataGridViewComboBoxColumn"" />
						</grid>
						<form>
							<tab name=""Tab1"">
								<columnLayout>
									<field label=""Test Prop"" property=""TestProp"" type=""TextBox"" mapperType=""TextBoxMapper"" />
									<field label=""Test Prop 2"" property=""TestProp2"" type=""TextBox"" mapperType=""TextBoxMapper"" />
								</columnLayout>
							</tab>
						</form>
					</ui>
				</class>


			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }

        public static ClassDef LoadClassDefWithStringRule()
        {
            XmlClassLoader itsLoader = new XmlClassLoader();
            ClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""MyBO"" assembly=""Habanero.Test"">
					<property  name=""MyBoID"" />
					<property  name=""TestProp"">
                        <rule name=""TestProp"">
                            <add key=""maxLength"" value=""5"" />
                        </rule>
                    </property>
					<property  name=""TestProp2"" />
					<primaryKey>
						<prop name=""MyBoID"" />
					</primaryKey>
					<ui />                    
				</class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }

        public static ClassDef LoadClassDefWithRelationship()
        {
            XmlClassLoader itsLoader = new XmlClassLoader();
            ClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""MyBO"" assembly=""Habanero.Test"">
					<property  name=""MyBoID"" />
					<property  name=""TestProp"" />
					<property  name=""TestProp2"" type=""Guid"" >
						<simpleLookupList>
							<item display=""s1"" value=""{E6E8DC44-59EA-4e24-8D53-4A43DC2F25E7}"" />
							<item display=""s2"" value=""{F428FADC-3740-412c-91A7-ECEB4D414414}"" />
						</simpleLookupList>
					</property>
					<property  name=""RelatedID"" type=""Guid"" />
					<primaryKey>
						<prop name=""MyBoID"" />
					</primaryKey>
					<relationship name=""MyRelationship"" type=""single"" relatedClass=""MyRelatedBo"" relatedAssembly=""Habanero.Test"">
						<relatedProperty property=""RelatedID"" relatedProperty=""MyRelatedBoID"" />
					</relationship>
					<relationship name=""MyMultipleRelationship"" type=""multiple"" relatedClass=""MyRelatedBo"" relatedAssembly=""Habanero.Test"">
						<relatedProperty property=""MyBoID"" relatedProperty=""MyBoID"" />
					</relationship>
					<ui>
						<grid>
							<column heading=""Test Prop"" property=""TestProp"" type=""DataGridViewTextBoxColumn"" />
							<column heading=""Test Prop 2"" property=""TestProp2"" type=""DataGridViewComboBoxColumn"" />
						</grid>
						<form>
							<tab name=""Tab1"">
								<columnLayout>
									<field label=""Test Prop"" property=""TestProp"" type=""TextBox"" mapperType=""TextBoxMapper"" />
									<field label=""Test Prop 2"" property=""TestProp2"" type=""TextBox"" mapperType=""TextBoxMapper"" />
								</columnLayout>
							</tab>
						</form>
					</ui>
				</class>


			");
			ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }

        //public static MyBO Create()
        //{
        //    MyBO bo = new MyBO();
        //    return bo;
        //}


    }

    public class MyRelatedBo : BusinessObject
    {
        private static ClassDef itsClassDef;

        public MyRelatedBo() : base()
        {
        }

        public MyRelatedBo(ClassDef def) : base(def)
        {
        }

        public MyRelatedBo(ClassDef def, IDatabaseConnection conn) : base(def, conn)
        {
        }

        protected override ClassDef ConstructClassDef()
        {
            return itsClassDef;
        }

        public static ClassDef LoadClassDef()
        {
            XmlClassLoader itsLoader = new XmlClassLoader();
            itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""MyRelatedBo"" assembly=""Habanero.Test"">
					<property  name=""MyRelatedBoID"" />
					<property  name=""MyRelatedTestProp"" />
					<property  name=""MyBoID"" />
					<primaryKey>
						<prop name=""MyRelatedBoID"" />
					</primaryKey>
				</class>
			");
            return itsClassDef;
        }
    }
}