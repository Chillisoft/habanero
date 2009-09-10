//---------------------------------------------------------------------------------
// Copyright (C) 2009 Chillisoft Solutions
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
using System.Collections;
using System.Collections.Generic;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;

namespace Habanero.Test
{
    /// <summary>
    /// Summary description for Sample.
    /// </summary>
    public class Sample : BusinessObject
    {
        private static Dictionary<string, string> itsLookupCollection;
        private static Dictionary<string, string> itsBOLookupCollection;

        public Sample()
        {
        }

        public Sample(ClassDef classDef) : this()
        {
            _classDef = classDef;
        }

        public static IClassDef GetClassDef()
        {
            if (!ClassDef.IsDefined(typeof (Sample)))
            {
                return CreateClassDefWin();
            }
            return ClassDef.ClassDefs[typeof (Sample)];
        }

        protected override IClassDef ConstructClassDef()
        {
            _classDef = (ClassDef) GetClassDef();
            return _classDef;
        }

        public static ClassDef CreateClassDefWin()
        {
            ClassDef lClassDef = CreateClassDef();
            lClassDef.UIDefCol.Add(new SampleUserInterfaceMapperWin().GetUIDef()); 
            ClassDef.ClassDefs.Add(lClassDef);

            return lClassDef;
        }

        public static IClassDef CreateClassDefWithTwoPropsOneCompulsory()
        {
            XmlClassLoader itsLoader = new XmlClassLoader(new DtdLoader(), new DefClassFactory());
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""Sample"" assembly=""Habanero.Test"">
					<property  name=""SampleID"" type=""Guid"" />
					<property  name=""SampleText"" />
					<property  name=""SampleText2"" compulsory=""true""/>
					<primaryKey>
						<prop name=""SampleID"" />
					</primaryKey>
					<ui>
						<form>
							<tab name=""Tab1"">
								<columnLayout width=""150"">
									<field label=""CompulsorySampleText:"" property=""SampleText2"" type=""TextBox"" mapperType=""TextBoxMapper"" />
									<field label=""SampleTextNotCompulsory:"" property=""SampleText"" type=""TextBox"" mapperType=""TextBoxMapper"" />
								</columnLayout>
							</tab>
						</form>
					</ui>
				</class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }

        public static IClassDef CreateClassDefWithTwoPropsOneNotEditable()
        {
            XmlClassLoader itsLoader = new XmlClassLoader(new DtdLoader(), new DefClassFactory());
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""Sample"" assembly=""Habanero.Test"">
					<property  name=""SampleID"" type=""Guid"" />
					<property  name=""SampleText"" />
					<property  name=""SampleText2"" />
					<primaryKey>
						<prop name=""SampleID"" />
					</primaryKey>
					<ui>
						<form>
							<tab name=""Tab1"">
								<columnLayout width=""150"">
									<field label=""EditableFieldSampleText:"" property=""SampleText2"" type=""TextBox"" mapperType=""TextBoxMapper"" />
									<field label=""SampleTextNotEditableField:"" property=""SampleText"" type=""TextBox"" mapperType=""TextBoxMapper"" editable=""false"" />
								</columnLayout>
							</tab>
						</form>
					</ui>
				</class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }

        public static IClassDef CreateClassDefWithTwoPropsOneWithToolTipText()
        {
            XmlClassLoader itsLoader = new XmlClassLoader(new DtdLoader(), new DefClassFactory());
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""Sample"" assembly=""Habanero.Test"">
					<property  name=""SampleID"" type=""Guid"" />
					<property  name=""SampleText"" description=""Test tooltip text""/>
					<property  name=""SampleText2"" />
					<primaryKey>
						<prop name=""SampleID"" />
					</primaryKey>
					<ui>
						<form>
							<tab name=""Tab1"">
								<columnLayout width=""150"">
									<field property=""SampleText"" type=""TextBox"" mapperType=""TextBoxMapper"" />
									<field property=""SampleText2"" type=""TextBox"" mapperType=""TextBoxMapper"" />
								</columnLayout>
							</tab>
						</form>
					</ui>
				</class>
			");
			ClassDef.ClassDefs.Add(itsClassDef);
			return itsClassDef;
        }

        public static IClassDef CreateClassDefWithTwoPropsOneInteger()
        {
            XmlClassLoader itsLoader = new XmlClassLoader(new DtdLoader(), new DefClassFactory());
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""Sample"" assembly=""Habanero.Test"">
					<property  name=""SampleID"" type=""Guid"" />
					<property  name=""SampleText"" />
					<property  name=""SampleInt"" type=""Int32"" />
					<primaryKey>
						<prop name=""SampleID"" />
					</primaryKey>
					<ui>
                        <form>
							<tab name=""Tab1"">
								<columnLayout width=""200"">
                                    <field label=""SampleText:"" property=""SampleText"" type=""TextBox"" mapperType=""TextBoxMapper"" />
                                    <field label=""SampleInt:"" property=""SampleInt"" type=""NumericUpDown"" mapperType=""NumericUpDownIntegerMapper"" editable=""false"" />
                                </columnLayout>
							</tab>
						</form>
					</ui>
					<ui name=""TwoColumns"">
                        <form>
							<columnLayout width=""200"">
                                <field label=""SampleText1:"" property=""SampleText"" type=""TextBox"" mapperType=""TextBoxMapper"" />
                                <field label=""SampleInt1:"" property=""SampleInt"" type=""NumericUpDown"" mapperType=""NumericUpDownIntegerMapper"" editable=""false"" />
                            </columnLayout>
							<columnLayout width=""200"">
                                <field label=""SampleText2:"" property=""SampleText"" type=""TextBox"" mapperType=""TextBoxMapper"" />
                                <field label=""SampleInt2:"" property=""SampleInt"" type=""NumericUpDown"" mapperType=""NumericUpDownIntegerMapper"" editable=""false"" />
                            </columnLayout>
						</form>
					</ui>
					<ui name=""ThreeColumns"">
                        <form>
							<columnLayout width=""200"">
                                <field label=""SampleText1:"" property=""SampleText"" type=""TextBox"" mapperType=""TextBoxMapper"" />
                                <field label=""SampleInt1:"" property=""SampleInt"" type=""NumericUpDown"" mapperType=""NumericUpDownIntegerMapper"" editable=""false"" />
                            </columnLayout>
							<columnLayout width=""200"">
                                <field label=""SampleText2:"" property=""SampleText"" type=""TextBox"" mapperType=""TextBoxMapper"" />
                                <field label=""SampleInt2:"" property=""SampleInt"" type=""NumericUpDown"" mapperType=""NumericUpDownIntegerMapper"" editable=""false"" />
                            </columnLayout>
							<columnLayout width=""200"">
                                <field label=""SampleText3:"" property=""SampleText"" type=""TextBox"" mapperType=""TextBoxMapper"" />
                                <field label=""SampleInt3:"" property=""SampleInt"" type=""NumericUpDown"" mapperType=""NumericUpDownIntegerMapper"" editable=""false"" />
                            </columnLayout>
						</form>
					</ui>
					<ui name=""TwoTabs"">
                        <form>
							<tab name=""Tab1"">
								<columnLayout width=""200"">
                                    <field label=""SampleText:"" property=""SampleText"" type=""TextBox"" mapperType=""TextBoxMapper"" />
                                </columnLayout>
							</tab>
						<tab name=""Tab1"">
								<columnLayout width=""200"">
                                    <field label=""SampleInt:"" property=""SampleInt"" type=""NumericUpDown"" mapperType=""NumericUpDownIntegerMapper"" editable=""false"" />
                                </columnLayout>
							</tab>
						</form>
					</ui>
				</class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }

        private static ClassDef CreateClassDef()
        {
            PropDefCol lPropDefCol = new PropDefCol();
            lPropDefCol.Add(
                new PropDef("SampleText", typeof (String), PropReadWriteRule.ReadWrite, "SampleText",  null));
            lPropDefCol.Add(
                new PropDef("SampleText2", typeof(String), PropReadWriteRule.ReadWrite, "SampleText2", null));
            lPropDefCol.Add(
                new PropDef("SampleTextPrivate", typeof(String), PropReadWriteRule.ReadWrite, "SampleTextPrivate", null,
                            false, false, int.MaxValue, null, null, true));
            lPropDefCol.Add(
                new PropDef("SampleTextDescribed", typeof(String), PropReadWriteRule.ReadWrite, "SampleTextDescribed", null,
                            false, false, int.MaxValue, null, "This is a sample text property that has a description.", false));
            lPropDefCol.Add(
                new PropDef("SampleDate", typeof(DateTime), PropReadWriteRule.ReadWrite, "SampleDate", null));
            lPropDefCol.Add(
                new PropDef("SampleDateNullable", typeof(DateTime), PropReadWriteRule.ReadWrite, "SampleDate", null));
            lPropDefCol.Add(
                new PropDef("SampleBoolean", typeof (Boolean), PropReadWriteRule.ReadWrite, "SampleBoolean", null));
            lPropDefCol.Add(
                new PropDef("SampleLookupID", typeof (Guid), PropReadWriteRule.ReadWrite, "SampleLookupID", null));
            lPropDefCol.Add(
                new PropDef("SampleInt", typeof (int), PropReadWriteRule.ReadWrite, "SampleInt", 0));
            lPropDefCol.Add(
                new PropDef("SampleMoney", typeof (Decimal), PropReadWriteRule.ReadWrite, "SampleInt", new Decimal(0)));
            PropDef propDef = new PropDef("SampleLookup2ID", typeof (Guid), PropReadWriteRule.ReadWrite, "SampleLookup2ID", null);
            itsLookupCollection = new Dictionary<string, string>();
            itsLookupCollection.Add("Test1", new Guid("{6E8B3DDB-1B13-4566-868D-57478C1F4BEE}").ToString());
            itsLookupCollection.Add("Test2",new Guid("{7209B956-96A0-4720-8E49-DE154FA0E096}").ToString());
            itsLookupCollection.Add("Test3", new Guid("{F45DE850-C693-44d8-AC39-8CEE5435B21A}").ToString());
            propDef.LookupList = new SimpleLookupList(itsLookupCollection);
            lPropDefCol.Add(propDef);
            lPropDefCol.Add(new PropDef("SampleLookup3ID", typeof (String), PropReadWriteRule.ReadWrite, "SampleLookup3ID",
                                        null));
            PropDef def = new PropDef("SampleID", typeof (Guid), PropReadWriteRule.WriteOnce, null);
            lPropDefCol.Add(def);
            PrimaryKeyDef primaryKey = new PrimaryKeyDef();
            primaryKey.IsGuidObjectID = true;
            primaryKey.Add(lPropDefCol["SampleID"]);
            KeyDefCol keysCol = new KeyDefCol();
            RelationshipDefCol relDefCol = new RelationshipDefCol();
            return new ClassDef(typeof (Sample), primaryKey, lPropDefCol, keysCol, relDefCol);
        }

        public static ClassDef CreateClassDefVWG()
        {
            ClassDef lClassDef = CreateClassDef();
            lClassDef.UIDefCol.Add(new SampleUserInterfaceMapperVWG().GetUIDef());
            ClassDef.ClassDefs.Add(lClassDef);

            return lClassDef;
        }

        public static Dictionary<string, string> LookupCollection
        {
            get { return itsLookupCollection; }
        }

        public static Dictionary<string, string> BOLookupCollection
        {
            get
            {
                if (itsBOLookupCollection == null)
                {
                    Sample sample1 = new Sample();
                    sample1.Save();
                    Sample sample2 = new Sample();
                    sample2.Save();
                    Sample sample3 = new Sample();
                    sample3.Save();
                    itsBOLookupCollection = new Dictionary<string, string>
                                {
                                    {"Test3", sample3.ID.GetAsValue().ToString()},
                                    {"Test2", sample2.ID.GetAsValue().ToString()},
                                    {"Test1", sample1.ID.GetAsValue().ToString()}
                                };
                }
                return itsBOLookupCollection;
            }
            set { itsBOLookupCollection = value; }
        }

        public DateTime SampleDate
        {
            get { return (DateTime) this.GetPropertyValue("SampleDate"); }
            set { this.SetPropertyValue("SampleDate", value); }
        }

		public DateTime? SampleDateNullable
        {
			get { return (DateTime?)this.GetPropertyValue("SampleDateNullable"); }
			set { this.SetPropertyValue("SampleDateNullable", value); }
        }

        public Guid? SampleLookupID
        {
            get { return (Guid?) this.GetPropertyValue("SampleLookupID"); }
            set { this.SetPropertyValue("SampleLookupID", value); }
        }

        public string SampleText
        {
            get { return (string) this.GetPropertyValue("SampleText"); }
            set { this.SetPropertyValue("SampleText", value); }
        }

        public bool SampleBoolean
        {
            get { return (bool) this.GetPropertyValue("SampleBoolean"); }
            set { this.SetPropertyValue("SampleBoolean", value); }
        }

        public int SampleInt
        {
            set { this.SetPropertyValue("SampleInt", value); }
            get { return (int) this.GetPropertyValue("SampleInt"); }
        }

        public decimal SampleMoney
        {
            get { return (decimal)this.GetPropertyValue("SampleMoney"); }
            set { this.SetPropertyValue("SampleMoney", value); }
        }

        #region Old methods for the purpose of not breaking other tests

        public static UIForm SampleUserInterfaceMapper3Props()
        {
            return new SampleUserInterfaceMapperWin().SampleUserInterfaceMapper3Props();
        }

        public static UIForm SampleUserInterfaceMapperPrivatePropOnly()
        {
            return new SampleUserInterfaceMapperWin().SampleUserInterfaceMapperPrivatePropOnly();
        }

        public static UIForm SampleUserInterfaceMapperDescribedPropOnly(string toolTipText)
        {
            return new SampleUserInterfaceMapperWin().SampleUserInterfaceMapperDescribedPropOnly(toolTipText);
        }

        public static UIForm SampleUserInterfaceMapper2Cols()
        {
            return new SampleUserInterfaceMapperWin().SampleUserInterfaceMapper2Cols();
        }


        public static UIForm SampleUserInterfaceMapper2Tabs()
        {
            return new SampleUserInterfaceMapperWin().SampleUserInterfaceMapper2Tabs();
        }

        public static UIForm SampleUserInterfaceMapperColSpanning()
        {
            return new SampleUserInterfaceMapperWin().SampleUserInterfaceMapperColSpanning();
        }

        public static UIForm SampleUserInterfaceMapperRowSpanning()
        {
            return new SampleUserInterfaceMapperWin().SampleUserInterfaceMapperRowSpanning();
        }

        #endregion
        
        /// <summary>
        /// Summary description for SampleUserInterfaceMapper.
        /// </summary>
        public abstract class SampleUserInterfaceMapper: ISampleUserInterfaceMapper
        {
            protected string _textBoxTypeName;
            protected string _textBoxAssemblyName;

            protected string _dateTimePickerTypeName;
            protected string _dateTimePickerAssemblyName;
            protected string _dateTimePickerMapperName;
            
            protected SampleUserInterfaceMapper()
            {
                SetupTypeNameVariables();
            }

            public UIDef GetUIDef()
            {
                return new UIDef("default", GetUIFormProperties(), GetUIGridProperties());
            }

            protected abstract void SetupTypeNameVariables();

            public UIForm GetSimpleUIFormDef()
            {
               XmlUIFormLoader loader = CreateXmlUIFormLoader();
               return 
                    (UIForm) loader.LoadUIFormDef(
                                 @"<form>
							<tab name=""Tab1"">
								<columnLayout width=""150"">
									<field label=""Text:"" property=""SampleText"" type=""TextBox"" mapperType=""TextBoxMapper"" />
								</columnLayout>
							</tab>
						</form>");
            }

            private static XmlUIFormLoader CreateXmlUIFormLoader()
            {
                return new XmlUIFormLoader(new DtdLoader(), new DefClassFactory());
            }

            private static IUIForm GetSimpleUIFormDefWithReadWriteRuleValueReadOnly()
            {
                XmlUIFormLoader loader = CreateXmlUIFormLoader();
                return
                     loader.LoadUIFormDef(
                         @"<form>
							<tab name=""Tab1"">
								<columnLayout width=""150"">
									<field label=""Text:"" property=""SampleText"" type=""TextBox"" mapperType=""TextBoxMapper"" >
                                            <parameter name=""readWriteRule"" value=""ReadOnly""/>
                                    </field>
								</columnLayout>
							</tab>
						</form>");
            }

            public IUIForm GetSimpleUIFormDef_NoColumnWidth()
            {
                XmlUIFormLoader loader = CreateXmlUIFormLoader();
                return
                     loader.LoadUIFormDef(
                         @"<form>
							<tab name=""Tab1"">
								<columnLayout>
									<field label=""Text:"" property=""SampleText"" type=""TextBox"" mapperType=""TextBoxMapper"" />
								</columnLayout>
							</tab>
						</form>");
            }

            public IUIForm GetSimpleUIFormTabOneFieldRowAndColSpan()
            {
                XmlUIFormLoader loader = CreateXmlUIFormLoader();
                return
                     loader.LoadUIFormDef(
                         @"<form>
							<tab name=""Tab1"">
								<columnLayout>
									<field label=""Text:"" property=""SampleText"" type=""TextBox"" mapperType=""TextBoxMapper"" >
                                            <parameter name=""colSpan"" value=""2"" />
                                            <parameter name=""rowSpan"" value=""2"" />
                                    </field>
								</columnLayout>
                                <columnLayout>
									<field label=""Integer:"" property=""SampleInt"" type=""NumericUpDown"" mapperType=""NumericUpDownIntegerMapper"" />
								</columnLayout>
							</tab>
						</form>");
            }

            public IUIForm GetSimpleUIFormDefInt()
            {
                XmlUIFormLoader loader = CreateXmlUIFormLoader();
                return
                     loader.LoadUIFormDef(
                         @"<form>
							<tab name=""Tab1"">
								<columnLayout>
									<field label=""Integer:"" property=""SampleInt"" type=""NumericUpDown"" mapperType=""NumericUpDownIntegerMapper"" />
								</columnLayout>
							</tab>
						</form>");
            }

            public IUIForm GetSimpleUIFormDefTwoRows()
            {
                XmlUIFormLoader loader = CreateXmlUIFormLoader();
                return
                     loader.LoadUIFormDef(
                         @"<form>
							<tab name=""Tab1"">
								<columnLayout>
                                    <field label=""Text:"" property=""SampleText"" type=""TextBox"" mapperType=""TextBoxMapper"" />
									<field label=""Integer:"" property=""SampleInt"" type=""NumericUpDown"" mapperType=""NumericUpDownIntegerMapper"" />
								</columnLayout>
							</tab>
						</form>");
            }

            public IUIForm GetSimpleUIFormDef1Row2Columns()
            {
                XmlUIFormLoader loader = CreateXmlUIFormLoader();
                return
                     loader.LoadUIFormDef(
                         @"<form>
							<tab name=""Tab1"">
								<columnLayout>
                                    <field label=""Text:"" property=""SampleText"" type=""TextBox"" mapperType=""TextBoxMapper"" />
                                </columnLayout>
                                <columnLayout>
									<field label=""Integer:"" property=""SampleInt"" type=""NumericUpDown"" mapperType=""NumericUpDownIntegerMapper"" />
								</columnLayout>
							</tab>
						</form>");
            }

            public IUIForm GetSimpleUIFormDef2Row2Columns1RowWithMoreControls()
            {
                XmlUIFormLoader loader = CreateXmlUIFormLoader();
                return
                     loader.LoadUIFormDef(
                         @"<form>
							<tab name=""Tab1"">
								<columnLayout>
                                    <field label=""Text:"" property=""SampleText"" type=""TextBox"" mapperType=""TextBoxMapper"" />
                                </columnLayout>
                                <columnLayout>
									<field label=""Integer:"" property=""SampleInt"" type=""NumericUpDown"" mapperType=""NumericUpDownIntegerMapper"" />
                                    <field label=""Date:"" property=""SampleDate"" type=""DateTimePicker"" mapperType=""DateTimePickerMapper"" />
								</columnLayout>
							</tab>
						</form>");
            }
            public IUIForm GetSimpleUIFormDef_1Column3RowsWithRowSpan()
            {
                XmlUIFormLoader loader = CreateXmlUIFormLoader();
                return
                     loader.LoadUIFormDef(
                         @"<form>
							<tab name=""Tab1"">
								<columnLayout>
                                    <field label=""Text:"" property=""SampleText"" type=""TextBox"" mapperType=""TextBoxMapper"" />
                                    <field label=""Text1:"" property=""SampleText"" type=""TextBox"" mapperType=""TextBoxMapper"" >
                                        <parameter name=""rowSpan"" value=""2""/>
                                    </field>
                                    <field label=""Text2:"" property=""SampleText"" type=""TextBox"" mapperType=""TextBoxMapper"" />
                                </columnLayout>
							</tab>
						</form>");
            }
            public IUIForm GetSimpleUIFormDef1Row2Columns1Row()
            {
                XmlUIFormLoader loader = CreateXmlUIFormLoader();
                return
                     loader.LoadUIFormDef(
                         @"<form>
							<tab name=""Tab1"">
								<columnLayout width=""200"">
                                    <field label=""Text:"" property=""SampleText"" type=""TextBox"" mapperType=""TextBoxMapper"" />
                                </columnLayout>
                                <columnLayout width=""150"">
									<field label=""Text2:"" property=""SampleText"" type=""TextBox"" mapperType=""TextBoxMapper"" />
                                </columnLayout>
							</tab>
						</form>");
            }

            public IUIForm GetSimpleUIFormDef1Row1Column1Row()
            {
                XmlUIFormLoader loader = CreateXmlUIFormLoader();
                return
                     loader.LoadUIFormDef(
                         @"<form>
							<tab name=""Tab1"">
								<columnLayout width=""150"">
                                    <field label=""Text:"" property=""SampleText"" type=""TextBox"" mapperType=""TextBoxMapper"" />
                                </columnLayout>
							</tab>
						</form>");
            }

            private static IUIForm GetSimpleUIFormDef1Row3Columns()
            {
                XmlUIFormLoader loader = CreateXmlUIFormLoader();
                return
                     loader.LoadUIFormDef(
                         @"<form>
							<tab name=""Tab1"">
								<columnLayout width=""200"">
                                    <field label=""Text:"" property=""SampleText"" type=""TextBox"" mapperType=""TextBoxMapper"" />
                                </columnLayout>
                                <columnLayout width=""150"">
									<field label=""Text2:"" property=""SampleText"" type=""TextBox"" mapperType=""TextBoxMapper"" />
                                </columnLayout>
                                <columnLayout width=""125"">
									<field label=""Text3:"" property=""SampleText"" type=""TextBox"" mapperType=""TextBoxMapper"" />
                                </columnLayout>
							</tab>
						</form>");
            }

            private static IUIForm GetSimpleUIFormDefTwoRowsOneHasCompulsoryProp()
            {
                XmlUIFormLoader loader = CreateXmlUIFormLoader();
                return
                     loader.LoadUIFormDef(
                         @"<form>
							<tab name=""Tab1"">
								<columnLayout width=""200"">
                                    <field label=""CompulsorySampleText:"" property=""SampleText"" type=""TextBox"" mapperType=""TextBoxMapper"" />
                                    <field label=""SampleTextNotCompulsory:"" property=""SampleText2"" type=""TextBox"" mapperType=""TextBoxMapper"" />
                                </columnLayout>
							</tab>
						</form>");
            }

            private static IUIForm GetSimpleUIFormOneFieldHasToolTip()
            {
                XmlUIFormLoader loader = CreateXmlUIFormLoader();
                return
                     loader.LoadUIFormDef(
                         @"<form>
							<tab name=""Tab1"">
								<columnLayout width=""200"">
                                    <field label=""CompulsorySampleText:"" property=""SampleText"" type=""TextBox"" mapperType=""TextBoxMapper"" toolTipText=""ToolTip""/>
                                    <field label=""SampleTextNotCompulsory:"" property=""SampleText2"" type=""TextBox"" mapperType=""TextBoxMapper"" toolTipText=""ToolTip""/>
                                </columnLayout>
							</tab>
						</form>");
            }

            private static IUIForm GetSimpleUIFormDef_3Columns_1Column_2RowSpan()
            {
                XmlUIFormLoader loader = CreateXmlUIFormLoader();
                return
                    loader.LoadUIFormDef(
                        @"<form>
								<columnLayout width=""450"">
                                    <field label=""TextBoxRowSpan2"" property=""SampleText"">
                                        <parameter name=""rowSpan"" value=""2"" />
                                    </field>
                                  </columnLayout>
                                  <columnLayout width=""250"">
									  <field label=""Col2TextBox1"" property=""SampleText2"" type=""TextBox"" >
                                         <parameter name=""dateFormat"" value=""yyyy/MM/dd"" />
                                       </field>
                                    <field label=""Col2TextBox2"" property=""SampleText2"" type=""TextBox"" />
                                </columnLayout>
                                <columnLayout width=""250"">
                                     <field label=""Col3TextBox1"" property=""SampleText2"" type=""TextBox"" />
                                     <field label=""Col3TextBox2"" property=""SampleText2"" type=""TextBox"" />
                                  </columnLayout>
                            </form>");
            }

            private static IUIForm GetSimpleUIFormDef_2Columns_2_1_ColSpan()
            {
                XmlUIFormLoader loader = CreateXmlUIFormLoader();
                return
                     loader.LoadUIFormDef(
                         @"<form>
							<tab name=""Tab1"">
								<columnLayout width=""200"">
                                    <field label=""Text1:"" property=""SampleText"" type=""TextBox"" mapperType=""TextBoxMapper"" >
                                        <parameter name=""colSpan"" value=""2""/>
                                    </field>
                                    <field label=""Text2:"" property=""SampleText"" type=""TextBox"" mapperType=""TextBoxMapper"" />
                                </columnLayout>
                                <columnLayout width=""150"">
									<field label=""Text3:"" property=""SampleText"" type=""TextBox"" mapperType=""TextBoxMapper"" />
                                </columnLayout>
							</tab>
						</form>");
            }

            public UIForm GetSimpleUIFormProperties()
            {
                
                UIForm def = new UIForm();
                def.Height = 300;
                def.Width = 350;
                UIFormTab tab = new UIFormTab();
                UIFormColumn col = new UIFormColumn(100);
                Hashtable propertyAttributes = new Hashtable();
                col.Add(
                    new UIFormField("Text:", "SampleText", _textBoxTypeName, _textBoxAssemblyName, "TextBoxMapper", "", false, null,
                                       propertyAttributes, LayoutStyle.Label));
                tab.Add(col);
                def.Add(tab);
                return def;
            }
            
            public UIForm GetUIFormProperties()
            {
                UIForm def = new UIForm();
                def.Height = 300;
                def.Width = 350;
                UIFormTab tab = new UIFormTab();
                UIFormColumn col = new UIFormColumn(100);
                Hashtable propertyAttributes = new Hashtable();
                propertyAttributes.Add("numLines", 3);
                col.Add(
                    new UIFormField("Text:", "SampleText", _textBoxTypeName, _textBoxAssemblyName, "TextBoxMapper", "", false, null,
                                       propertyAttributes,  LayoutStyle.Label));
                tab.Add(col);
                def.Add(tab);
                return def;
            }

            public UIGrid GetUIGridProperties()
            {
                UIGrid col = new UIGrid();
                col.Add(
                    new UIGridColumn("Text:", "SampleText", "DataGridViewTextBoxColumn", null, true, 100,
                                       PropAlignment.left, null));
                return col;
            }

            public UIForm SampleUserInterfaceMapper3Props()
            {
                UIForm def = new UIForm();
                def.Height = 300;
                def.Width = 350;
                UIFormTab tab = new UIFormTab();
                UIFormColumn col = new UIFormColumn(100);
                col.Add(new UIFormField("Text:", "SampleText", _textBoxTypeName, _textBoxAssemblyName, "TextBoxMapper", "", true, null, new Hashtable(),  LayoutStyle.Label));
                col.Add(
                    new UIFormField("Date:", "SampleDate", _dateTimePickerTypeName, _dateTimePickerAssemblyName, _dateTimePickerMapperName, "", true, null,
                                       new Hashtable(),  LayoutStyle.Label));
                col.Add(
                    new UIFormField("Text2:", "SampleText2", _textBoxTypeName, _textBoxAssemblyName, "TextBoxMapper", "", true, null, new Hashtable(), LayoutStyle.Label));
                tab.Add(col);
                def.Add(tab);
                return def;
            }

            public UIForm SampleUserInterfaceMapperPrivatePropOnly()
            {
                UIForm def = new UIForm();
                def.Height = 300;
                def.Width = 350;
                UIFormTab tab = new UIFormTab();
                def.Add(tab);
                UIFormColumn col = new UIFormColumn(100);
                tab.Add(col);
                col.Add(new UIFormField("Private Text:", "SampleTextPrivate", _textBoxTypeName, _textBoxAssemblyName, "TextBoxMapper", "", true, null, new Hashtable(),  LayoutStyle.Label));
                return def;
            }

            public UIForm SampleUserInterfaceMapperDescribedPropOnly(string toolTipText)
            {
                UIForm def = new UIForm();
                def.Height = 300;
                def.Width = 350;
                UIFormTab tab = new UIFormTab();
                def.Add(tab);
                UIFormColumn col = new UIFormColumn(100);
                tab.Add(col);
                col.Add(new UIFormField("Described Text:", "SampleTextDescribed", _textBoxTypeName, _textBoxAssemblyName, "TextBoxMapper", "", true, toolTipText, new Hashtable(),  LayoutStyle.Label));
                return def;
            }

            public UIForm SampleUserInterfaceMapper2Cols()
            {
                UIForm def = new UIForm();
                def.Height = 300;
                def.Width = 350;
                UIFormTab tab = new UIFormTab();
                UIFormColumn col1 = new UIFormColumn(100);
                UIFormColumn col2 = new UIFormColumn(150);
                col1.Add(
                    new UIFormField("Text:", "SampleText", _textBoxTypeName, _textBoxAssemblyName, "TextBoxMapper", "", true, null, new Hashtable(),  LayoutStyle.Label));
                col1.Add(
                    new UIFormField("Date:", "SampleDate", _dateTimePickerTypeName, _dateTimePickerAssemblyName, _dateTimePickerMapperName, "", true, null,
                                       new Hashtable(),  LayoutStyle.Label));
                col2.Add(
                    new UIFormField("Text2:", "SampleText2", _textBoxTypeName, _textBoxAssemblyName, "TextBoxMapper", "", true, null, new Hashtable(),  LayoutStyle.Label));
                tab.Add(col1);
                tab.Add(col2);
                def.Add(tab);
                return def;
            }


            public UIForm SampleUserInterfaceMapper2Tabs()
            {
                UIForm def = new UIForm();
                def.Height = 300;
                def.Width = 350;
                UIFormTab tab1 = new UIFormTab("mytab1");
                UIFormTab tab2 = new UIFormTab("mytab2");
                UIFormColumn col1 = new UIFormColumn(100);
                UIFormColumn col2 = new UIFormColumn(150);
                col1.Add(
                    new UIFormField("Text:", "SampleText", _textBoxTypeName, _textBoxAssemblyName, "TextBoxMapper", "", true, null, new Hashtable(), LayoutStyle.Label));
                col1.Add(
                    new UIFormField("Date:", "SampleDate", _dateTimePickerTypeName, _dateTimePickerAssemblyName, _dateTimePickerMapperName, "", true, null,
                                       new Hashtable(),  LayoutStyle.Label));
                col2.Add(
                    new UIFormField("Text2:", "SampleText2", _textBoxTypeName, _textBoxAssemblyName, "TextBoxMapper", "", true, null, new Hashtable(), LayoutStyle.Label));
                tab1.Add(col1);
                tab2.Add(col2);
                def.Add(tab1);
                def.Add(tab2);
                return def;
            }

            public UIForm SampleUserInterfaceMapperColSpanning()
            {
                UIForm def = new UIForm();
                def.Height = 300;
                def.Width = 350;
                UIFormTab tab = new UIFormTab();
                UIFormColumn col = new UIFormColumn(100);
                Hashtable propertyAttributes = new Hashtable();
                propertyAttributes.Add("numLines", 3);
                propertyAttributes.Add("colSpan", 2);
                col.Add(
                    new UIFormField("Text:", "SampleText", _textBoxTypeName, _textBoxAssemblyName, "TextBoxMapper", "", false, null, propertyAttributes, LayoutStyle.Label));
                col.Add(
                    new UIFormField("Text2:", "SampleText2", _textBoxTypeName, _textBoxAssemblyName, "TextBoxMapper", "", false, null, new Hashtable(), LayoutStyle.Label));
                tab.Add(col);
                UIFormColumn col2 = new UIFormColumn(100);
                col2.Add(
                    new UIFormField("Text2:", "SampleText2", _textBoxTypeName, _textBoxAssemblyName, "TextBoxMapper", "", false, null, new Hashtable(), LayoutStyle.Label));
                tab.Add(col2);
                def.Add(tab);
                return def;
            }

            public UIForm SampleUserInterfaceMapperRowSpanning()
            {
                UIForm def = new UIForm();
                def.Height = 300;
                def.Width = 350;
                UIFormTab tab = new UIFormTab();
                UIFormColumn col = new UIFormColumn(100);
                Hashtable propertyAttributes = new Hashtable();
                propertyAttributes.Add("numLines", 3);
                propertyAttributes.Add("rowSpan", 2);
                col.Add(
                    new UIFormField("Text:", "SampleText", _textBoxTypeName, _textBoxAssemblyName, "TextBoxMapper", "", false, null, propertyAttributes, LayoutStyle.Label));
                tab.Add(col);
                UIFormColumn col2 = new UIFormColumn(100);
                col2.Add(
                    new UIFormField("Text2:", "SampleText2", _textBoxTypeName, _textBoxAssemblyName, "TextBoxMapper", "", false, null, new Hashtable(), LayoutStyle.Label));
                col2.Add(
                    new UIFormField("Text2:", "SampleText2", _textBoxTypeName, _textBoxAssemblyName, "TextBoxMapper", "", false, null, new Hashtable(), LayoutStyle.Label));
                tab.Add(col2);
                def.Add(tab);
                return def;
            }

            public UIForm SampleUserInterface_ReadWriteRule()
            {
                UIForm def = new UIForm();
                def.Height = 300;
                def.Width = 350;
                UIFormTab tab = new UIFormTab();
                UIFormColumn col = new UIFormColumn(100);
                Hashtable propertyAttributes = new Hashtable();
                propertyAttributes.Add("readWriteRule", "ReadOnly");
                col.Add(
                    new UIFormField("Text:", "SampleText", _textBoxTypeName, _textBoxAssemblyName, "TextBoxMapper", "", true, null, propertyAttributes, LayoutStyle.Label));
                col.Add(
                    new UIFormField("Text2:", "SampleText2", _textBoxTypeName, _textBoxAssemblyName, "TextBoxMapper", "", true, null, new Hashtable(), LayoutStyle.Label));
                tab.Add(col);
                def.Add(tab);

                return def;
            }

            public UIForm SampleUserInterface_CustomMapper_WithAttributes(string mapperTypeName, string mapperAssemblyName, string attributeName, string attributeValue)
            {
                UIForm def = new UIForm();
                def.Height = 300;
                def.Width = 350;
                UIFormTab tab = new UIFormTab();
                UIFormColumn col = new UIFormColumn(100);
                Hashtable propertyAttributes = new Hashtable();
                propertyAttributes.Add(attributeName, attributeValue);
                col.Add(new UIFormField("Text:", "SampleText", _textBoxTypeName, _textBoxAssemblyName, mapperTypeName, mapperAssemblyName, false, null, propertyAttributes, LayoutStyle.Label));
                tab.Add(col);
                def.Add(tab);
                return def;
            }

            public UIForm SampleUserInterface_WriteNewRule()
            {
                UIForm def = new UIForm();
                def.Height = 300;
                def.Width = 350;
                UIFormTab tab = new UIFormTab();
                UIFormColumn col = new UIFormColumn(100);
                Hashtable propertyAttributes = new Hashtable();
                propertyAttributes.Add("readWriteRule", "WriteNew");
                col.Add(
                    new UIFormField("Text:", "SampleText", _textBoxTypeName, _textBoxAssemblyName, "TextBoxMapper", "", true, null, propertyAttributes, LayoutStyle.Label));
                col.Add(
                    new UIFormField("Text2:", "SampleText2", _textBoxTypeName, _textBoxAssemblyName, "TextBoxMapper", "", true, null, new Hashtable(),  LayoutStyle.Label));
                tab.Add(col);
                def.Add(tab);


                return def;
            }

            public UIFormTab GetFormTabOneField()
            {
                return (UIFormTab) GetSimpleUIFormDef()[0];
            }

            public UIFormTab GetFormTabOneIntegerField()
            {
                return (UIFormTab) GetSimpleUIFormDefInt()[0];
            }

            public UIFormTab GetFormTabTwoFields()
            {
                return (UIFormTab) GetSimpleUIFormDefTwoRows()[0];
            }

            public UIFormTab GetFormTabTwoColumns_1_1()
            {
                return (UIFormTab) GetSimpleUIFormDef1Row2Columns()[0];
            }

            public UIFormTab GetFormTabTwoColumns_1_2()
            {
                return (UIFormTab) GetSimpleUIFormDef2Row2Columns1RowWithMoreControls()[0];
            }

            public UIFormTab GetFormTabTwoColumnsOneRowWithWidths()
            {
                return (UIFormTab) GetSimpleUIFormDef1Row2Columns1Row()[0];
            }

            public UIFormTab GetFormTabOneColumnOneRowWithWidth()
            {
                return (UIFormTab) GetSimpleUIFormDef1Row1Column1Row()[0];
            }

            public UIFormTab GetFormTabThreeColumnsOneRowWithWidths()
            {
                return (UIFormTab) GetSimpleUIFormDef1Row3Columns()[0];
            }


            public UIFormTab GetFormTabOneFieldNoColumnWidth()
            {
                return (UIFormTab) GetSimpleUIFormDef_NoColumnWidth()[0];
            }

            public UIFormTab GetFormTabThreeColumnsOneColumnWithRowSpan()
            {
                return (UIFormTab) GetSimpleUIFormDef_3Columns_1Column_2RowSpan()[0];
            }

            public UIFormTab GetFormTabOneColumnThreeRowsWithRowSpan()
            {
                return (UIFormTab) GetSimpleUIFormDef_1Column3RowsWithRowSpan()[0];
            }


            public UIFormTab GetFormTabTwoColumns_2_1_ColSpan()
            {
                return (UIFormTab) GetSimpleUIFormDef_2Columns_2_1_ColSpan()[0];
            }


            public UIFormTab GetFormTabTwoFields_OneHasCompulsoryProp()
            {
                return (UIFormTab) GetSimpleUIFormDefTwoRowsOneHasCompulsoryProp()[0];
            }

            public UIFormTab GetFormTabOneField_ReadWriteParameter_ReadOnly()
            {
                return (UIFormTab) GetSimpleUIFormDefWithReadWriteRuleValueReadOnly()[0];
                
            }


            public UIFormTab GetFormTabTwoFields_OneHasToolTip()
            {
                return (UIFormTab) GetSimpleUIFormOneFieldHasToolTip()[0];
            }


            public UIFormTab GetFormTabTwoFieldsWithNoAlignment()
            {
                return (UIFormTab) GetSimpleUIFormDef_NoAlignment()[0];
            }

            private static IUIForm GetSimpleUIFormDef_NoAlignment()
            {
                XmlUIFormLoader loader = CreateXmlUIFormLoader();
                return
                     loader.LoadUIFormDef(
                         @"<form>
							<tab name=""Tab1"">
								<columnLayout width=""150"">
									<field label=""Text:"" property=""SampleText"" type=""TextBox"" mapperType=""TextBoxMapper"" />
                                    <field label=""Integer:"" property=""SampleInt"" type=""NumericUpDown"" mapperType=""NumericUpDownIntegerMapper"" />
								</columnLayout>
							</tab>
						</form>");
            }

            public UIFormTab GetFormTabOneFieldWithRightAlignment()
            {
                return (UIFormTab) GetSimpleUIFormDef_RightAlignment()[0];
            }

            private static IUIForm GetSimpleUIFormDef_RightAlignment()
            {
                XmlUIFormLoader loader = CreateXmlUIFormLoader();
                return
                     loader.LoadUIFormDef(
                         @"<form>
							<tab name=""Tab1"">
								<columnLayout width=""150"">
									<field label=""Text:"" property=""SampleText"" type=""TextBox"" mapperType=""TextBoxMapper"" >
                                        <parameter name=""alignment"" value=""right""/>
                                    </field>
								</columnLayout>
							</tab>
						</form>");
            }

            public UIFormTab GetFormTabOneFieldWithCenterAlignment()
            {
                return (UIFormTab) GetSimpleUIFormDef_CenterAlignment()[0];
            }

            private static IUIForm GetSimpleUIFormDef_CenterAlignment()
            {
                XmlUIFormLoader loader = CreateXmlUIFormLoader();
                return
                     loader.LoadUIFormDef(
                         @"<form>
							<tab name=""Tab1"">
								<columnLayout width=""150"">
									<field label=""Text:"" property=""SampleText"" type=""TextBox"" mapperType=""TextBoxMapper"" >
                                        <parameter name=""alignment"" value=""center""/>
                                    </field>
								</columnLayout>
							</tab>
						</form>");
            }

            public UIFormTab GetFormTabOneFieldWithInvalidAlignment()
            {
                return (UIFormTab) GetSimpleUIFormDef_InvalidAlignment()[0];
            }

            private static IUIForm GetSimpleUIFormDef_InvalidAlignment()
            {
                XmlUIFormLoader loader = CreateXmlUIFormLoader();
                return
                     loader.LoadUIFormDef(
                         @"<form>
							<tab name=""Tab1"">
								<columnLayout width=""150"">
									<field label=""Text:"" property=""SampleText"" type=""TextBox"" mapperType=""TextBoxMapper"" >
                                        <parameter name=""alignment"" value=""Top""/>
                                    </field>
								</columnLayout>
							</tab>
						</form>");
            }

            public UIFormTab GetFormTabOneFieldsWithAlignment_NumericUpDown()
            {
                return (UIFormTab) GetSimpleUIFormDef_WithAlignmentAndNumericUpdown()[0];
            }

            private static IUIForm GetSimpleUIFormDef_WithAlignmentAndNumericUpdown()
            {
                XmlUIFormLoader loader = CreateXmlUIFormLoader();
                return
                     loader.LoadUIFormDef(
                         @"<form>
							<tab name=""Tab1"">
								<columnLayout width=""150"">
                                    <field label=""Integer:"" property=""SampleInt"" type=""NumericUpDown"" mapperType=""NumericUpDownIntegerMapper"" >
                                        <parameter name=""alignment"" value=""left""/>
                                    </field>
                                    <field label=""Integer:"" property=""SampleInt"" type=""NumericUpDown"" mapperType=""NumericUpDownIntegerMapper"" >
                                        <parameter name=""alignment"" value=""right""/>
                                    </field>
                                    <field label=""Integer:"" property=""SampleInt"" type=""NumericUpDown"" mapperType=""NumericUpDownIntegerMapper"" >
                                        <parameter name=""alignment"" value=""center""/>
                                    </field>
                                    <field label=""Integer:"" property=""SampleInt"" type=""NumericUpDown"" mapperType=""NumericUpDownIntegerMapper"" >
                                        <parameter name=""alignment"" value=""centre""/>
                                    </field>
								</columnLayout>
							</tab>
						</form>");
            }

            public UIFormTab GetFormTabOneFieldsWithNumericUpDown()
            {
                return (UIFormTab) GetSimpleUIFormDef_WithNumericUpDown()[0];
            }

            private static IUIForm GetSimpleUIFormDef_WithNumericUpDown()
            {
                XmlUIFormLoader loader = CreateXmlUIFormLoader();
                return
                     loader.LoadUIFormDef(
                         @"<form>
							<tab name=""Tab1"">
								<columnLayout width=""150"">
                                    <field label=""Integer:"" property=""SampleInt"" type=""NumericUpDown"" mapperType=""NumericUpDownIntegerMapper"" >
                                        <parameter name=""alignment"" value=""right""/>
                                    </field>
                                </columnLayout>
							</tab>
						</form>");
            }


            public UIFormTab GetFormTabOneFieldWithMultiLineParameter()
            {
                return (UIFormTab) GetSimpleUIFormDef_WithMultiLineParameter()[0];
            }

            private static IUIForm GetSimpleUIFormDef_WithMultiLineParameter()
            {
                XmlUIFormLoader loader = CreateXmlUIFormLoader();
                return
                     loader.LoadUIFormDef(
                         @"<form>
							<tab name=""Tab1"">
								<columnLayout width=""150"">
									<field label=""Text:"" property=""SampleText"" type=""TextBox"" mapperType=""TextBoxMapper"" >
                                        <parameter name=""numLines"" value=""3""/>
                                    </field>
								</columnLayout>
							</tab>
						</form>");
            }

            public UIFormTab GetFormTabOneFieldWithInvalidMultiLineParameter()
            {
                return (UIFormTab) GetSimpleUIFormDef_WithInvalidMultiLineParameter()[0];
            }

            private static IUIForm GetSimpleUIFormDef_WithInvalidMultiLineParameter()
            {
               
                   XmlUIFormLoader loader = CreateXmlUIFormLoader();
                return
                     loader.LoadUIFormDef(
                         @"<form>
							<tab name=""Tab1"">
								<columnLayout width=""150"">
									<field label=""Text:"" property=""SampleText"" type=""TextBox"" mapperType=""TextBoxMapper"" >
                                        <parameter name=""numLines"" value=""Invalid""/>
                                    </field>
								</columnLayout>
							</tab>
						</form>");
            }

            public UIFormTab GetFormTabOneFieldWithDecimalPlacesParameter()
            {
                return (UIFormTab) GetSimpleUIFormDef_WithDecimalPlacesParameter()[0];
            }

            private static IUIForm GetSimpleUIFormDef_WithDecimalPlacesParameter()
            {
                XmlUIFormLoader loader = CreateXmlUIFormLoader();
                return
                     loader.LoadUIFormDef(
                         @"<form>
							<tab name=""Tab1"">
								<columnLayout width=""150"">
									<field label=""Integer:"" property=""SampleInt"" type=""NumericUpDown"" mapperType=""NumericUpDownCurrencyMapper"" >
                                        <parameter name=""decimalPlaces"" value=""3""/>
                                    </field>
								</columnLayout>
							</tab>
						</form>");
            }

            public UIFormTab GetFormTabOneFieldWithOptionsParameter()
            {
                return (UIFormTab) GetSimpleUIFormDef_WithOptionsParameter()[0];
            }

            private static IUIForm GetSimpleUIFormDef_WithOptionsParameter()
            {
                XmlUIFormLoader loader = CreateXmlUIFormLoader();
                return
                     loader.LoadUIFormDef(
                         @"<form>
							<tab name=""Tab1"">
								<columnLayout width=""150"">
									<field label=""Text:"" property=""SampleText"" type=""ComboBox"" mapperType=""ListComboBoxMapper"" >
                                        <parameter name=""options"" value=""M|F""/>
                                    </field>
								</columnLayout>
							</tab>
						</form>");
            }

            public UIFormTab GetFormTabOneFieldWithIsEmailParameter()
            {
                return (UIFormTab) GetSimpleUIFormDef_WithIsEmailParameter()[0];
            }

            private static IUIForm GetSimpleUIFormDef_WithIsEmailParameter()
            {
                XmlUIFormLoader loader = CreateXmlUIFormLoader();
                return
                     loader.LoadUIFormDef(
                         @"<form>
							<tab name=""Tab1"">
								<columnLayout width=""150"">
									<field label=""Text:"" property=""SampleText"" type=""TextBox"" mapperType=""TextBoxMapper"" >
                                        <parameter name=""isEmail"" value=""true""/>
                                    </field>
                                    <field label=""Text:"" property=""SampleText"" type=""TextBox"" mapperType=""TextBoxMapper"" >
                                        <parameter name=""isEmail"" value=""false""/>
                                    </field>
   								</columnLayout>
							</tab>
						</form>");
            }

            public UIFormTab GetFormTabOneFieldWithDateFormatParameter()
            {
                return (UIFormTab) GetSimpleUIFormDef_WithDateFormatParameter()[0];
            }

            private static IUIForm GetSimpleUIFormDef_WithDateFormatParameter()
            {
                XmlUIFormLoader loader = CreateXmlUIFormLoader();
                return
                     loader.LoadUIFormDef(
                         @"<form>
							<tab name=""Tab1"">
								<columnLayout width=""150"">
									<field label=""DateTime:"" property=""SampleDateTime"" type=""DateTimePicker"" mapperType=""DateTimePickerMapper"" >
                                        <parameter name=""dateFormat"" value=""d""/>
                                    </field>
                                    <field label=""DateTime:"" property=""SampleDateTime"" type=""DateTimePicker"" mapperType=""DateTimePickerMapper"" >
                                        <parameter name=""dateFormat"" value=""dd MM yy""/>
                                    </field>
   								</columnLayout>
							</tab>
						</form>");
            }
            public UIFormTab GetFormTabOneFieldHasRowAndColSpan()
            {
                return (UIFormTab) GetSimpleUIFormTabOneFieldRowAndColSpan()[0];
            }
        }


        

        public class SampleUserInterfaceMapperWin : SampleUserInterfaceMapper
        {
            protected override void SetupTypeNameVariables()
            {
                _textBoxTypeName = "TextBox";
                _textBoxAssemblyName = "System.Windows.Forms";
                _dateTimePickerTypeName = "DateTimePicker";
                _dateTimePickerAssemblyName = "System.Windows.Forms";
                _dateTimePickerMapperName = "DateTimePickerMapper";
            }
        }


        /// <summary>
        /// Summary description for SampleUserInterfaceMapper.
        /// </summary>
        public class SampleUserInterfaceMapperVWG : SampleUserInterfaceMapper
        {

            protected override void SetupTypeNameVariables()
            {
                _textBoxTypeName = "TextBoxVWG";
                _textBoxAssemblyName = "Habanero.UI.VWG";
                _dateTimePickerTypeName = "DateTimePickerVWG";
                _dateTimePickerAssemblyName = "Habanero.UI.VWG";
                _dateTimePickerMapperName = "DateTimePickerMapper";
            }
            
        }
    }
}
