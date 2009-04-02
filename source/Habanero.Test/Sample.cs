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
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
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
        private static Dictionary<string, object> itsLookupCollection;
        private static Dictionary<string, object> itsBOLookupCollection;

        public Sample()
        {
        }

        public Sample(ClassDef classDef) : this()
        {
            _classDef = classDef;
        }

        public static ClassDef GetClassDef()
        {
            if (!ClassDef.IsDefined(typeof (Sample)))
            {
                return CreateClassDefWin();
            }
            else
            {
                return ClassDef.ClassDefs[typeof (Sample)];
            }
        }

        protected override ClassDef ConstructClassDef()
        {
            _classDef = GetClassDef();
            return _classDef;
        }

        public static ClassDef CreateClassDefWin()
        {
            ClassDef lClassDef = CreateClassDef();
            lClassDef.UIDefCol.Add(new SampleUserInterfaceMapperWin().GetUIDef()); 
            ClassDef.ClassDefs.Add(lClassDef);

            return lClassDef;
        }

        public static ClassDef CreateClassDefWithTwoPropsOneCompulsory()
        {
            XmlClassLoader itsLoader = new XmlClassLoader();
            ClassDef itsClassDef =
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

        public static ClassDef CreateClassDefWithTwoPropsOneWithToolTipText()
        {
                        XmlClassLoader itsLoader = new XmlClassLoader();
            ClassDef itsClassDef =
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

        public static ClassDef CreateClassDefWithTwoPropsOneInteger()
        {
            XmlClassLoader itsLoader = new XmlClassLoader();
            ClassDef itsClassDef =
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
            PropDef propDef;
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
            propDef = new PropDef("SampleLookup2ID", typeof (Guid), PropReadWriteRule.ReadWrite, "SampleLookup2ID", null);
            itsLookupCollection = new Dictionary<string, object>();
            itsLookupCollection.Add("Test1", new Guid("{6E8B3DDB-1B13-4566-868D-57478C1F4BEE}"));
            itsLookupCollection.Add("Test2", new Guid("{7209B956-96A0-4720-8E49-DE154FA0E096}"));
            itsLookupCollection.Add("Test3", new Guid("{F45DE850-C693-44d8-AC39-8CEE5435B21A}"));
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

        public static Dictionary<string, object> LookupCollection
        {
            get { return itsLookupCollection; }
        }

        public static Dictionary<string, object> BOLookupCollection
        {
            get
            {
                if (itsBOLookupCollection == null) {

                    itsBOLookupCollection = new Dictionary<string, object>();
                    itsBOLookupCollection.Add("Test1", new Sample());
                    itsBOLookupCollection.Add("Test2", new Sample());
                    itsBOLookupCollection.Add("Test3", new Sample());
                }
                return itsBOLookupCollection;
            }
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

        public Guid SampleLookupID
        {
            get { return (Guid) this.GetPropertyValue("SampleLookupID"); }
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
               XmlUIFormLoader loader = new XmlUIFormLoader();
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

            private UIForm GetSimpleUIFormDefWithReadWriteRuleValueReadOnly()
            {
                XmlUIFormLoader loader = new XmlUIFormLoader();
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

            public UIForm GetSimpleUIFormDef_NoColumnWidth()
            {
                XmlUIFormLoader loader = new XmlUIFormLoader();
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

            public UIForm GetSimpleUIFormTabOneFieldRowAndColSpan()
            {
                XmlUIFormLoader loader = new XmlUIFormLoader();
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

            public UIForm GetSimpleUIFormDefInt()
            {
                XmlUIFormLoader loader = new XmlUIFormLoader();
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

            public UIForm GetSimpleUIFormDefTwoRows()
            {
                XmlUIFormLoader loader = new XmlUIFormLoader();
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

            public UIForm GetSimpleUIFormDef1Row2Columns()
            {
                XmlUIFormLoader loader = new XmlUIFormLoader();
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

            public UIForm GetSimpleUIFormDef2Row2Columns1RowWithMoreControls()
            {
                XmlUIFormLoader loader = new XmlUIFormLoader();
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
            public UIForm GetSimpleUIFormDef_1Column3RowsWithRowSpan()
            {
                XmlUIFormLoader loader = new XmlUIFormLoader();
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
            public UIForm GetSimpleUIFormDef1Row2Columns1Row()
            {
                XmlUIFormLoader loader = new XmlUIFormLoader();
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

            public UIForm GetSimpleUIFormDef1Row1Column1Row()
            {
                XmlUIFormLoader loader = new XmlUIFormLoader();
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

            private UIForm GetSimpleUIFormDef1Row3Columns()
            {
                XmlUIFormLoader loader = new XmlUIFormLoader();
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

            private UIForm GetSimpleUIFormDefTwoRowsOneHasCompulsoryProp()
            {
                XmlUIFormLoader loader = new XmlUIFormLoader();
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

            private UIForm GetSimpleUIFormOneFieldHasToolTip()
            {
                XmlUIFormLoader loader = new XmlUIFormLoader();
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

            private UIForm GetSimpleUIFormDef_3Columns_1Column_2RowSpan()
            {
                XmlUIFormLoader loader = new XmlUIFormLoader();
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

            private UIForm GetSimpleUIFormDef_2Columns_2_1_ColSpan()
            {
                XmlUIFormLoader loader = new XmlUIFormLoader();
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
                                       propertyAttributes, null));
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
                                       propertyAttributes, null));
                tab.Add(col);
                def.Add(tab);
                return def;
            }

            public UIGrid GetUIGridProperties()
            {
                UIGrid col = new UIGrid();
                col.Add(
                    new UIGridColumn("Text:", "SampleText", "DataGridViewTextBoxColumn", null, true, 100,
                                       UIGridColumn.PropAlignment.left, null));
                return col;
            }

            public UIForm SampleUserInterfaceMapper3Props()
            {
                UIForm def = new UIForm();
                def.Height = 300;
                def.Width = 350;
                UIFormTab tab = new UIFormTab();
                UIFormColumn col = new UIFormColumn(100);
                col.Add(new UIFormField("Text:", "SampleText", _textBoxTypeName, _textBoxAssemblyName, "TextBoxMapper", "", true, null, new Hashtable(), null));
                col.Add(
                    new UIFormField("Date:", "SampleDate", _dateTimePickerTypeName, _dateTimePickerAssemblyName, _dateTimePickerMapperName, "", true, null,
                                       new Hashtable(), null));
                col.Add(
                    new UIFormField("Text2:", "SampleText2", _textBoxTypeName, _textBoxAssemblyName, "TextBoxMapper", "", true, null, new Hashtable(), null));
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
                col.Add(new UIFormField("Private Text:", "SampleTextPrivate", _textBoxTypeName, _textBoxAssemblyName, "TextBoxMapper", "", true, null, new Hashtable(), null));
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
                col.Add(new UIFormField("Described Text:", "SampleTextDescribed", _textBoxTypeName, _textBoxAssemblyName, "TextBoxMapper", "", true, toolTipText, new Hashtable(), null));
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
                    new UIFormField("Text:", "SampleText", _textBoxTypeName, _textBoxAssemblyName, "TextBoxMapper", "", true, null, new Hashtable(), null));
                col1.Add(
                    new UIFormField("Date:", "SampleDate", _dateTimePickerTypeName, _dateTimePickerAssemblyName, _dateTimePickerMapperName, "", true, null,
                                       new Hashtable(), null));
                col2.Add(
                    new UIFormField("Text2:", "SampleText2", _textBoxTypeName, _textBoxAssemblyName, "TextBoxMapper", "", true, null, new Hashtable(), null));
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
                    new UIFormField("Text:", "SampleText", _textBoxTypeName, _textBoxAssemblyName, "TextBoxMapper", "", true, null, new Hashtable(), null));
                col1.Add(
                    new UIFormField("Date:", "SampleDate", _dateTimePickerTypeName, _dateTimePickerAssemblyName, _dateTimePickerMapperName, "", true, null,
                                       new Hashtable(), null));
                col2.Add(
                    new UIFormField("Text2:", "SampleText2", _textBoxTypeName, _textBoxAssemblyName, "TextBoxMapper", "", true, null, new Hashtable(), null));
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
                    new UIFormField("Text:", "SampleText", _textBoxTypeName, _textBoxAssemblyName, "TextBoxMapper", "", false, null, propertyAttributes, null));
                col.Add(
                    new UIFormField("Text2:", "SampleText2", _textBoxTypeName, _textBoxAssemblyName, "TextBoxMapper", "", false, null, new Hashtable(), null));
                tab.Add(col);
                UIFormColumn col2 = new UIFormColumn(100);
                col2.Add(
                    new UIFormField("Text2:", "SampleText2", _textBoxTypeName, _textBoxAssemblyName, "TextBoxMapper", "", false, null, new Hashtable(), null));
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
                    new UIFormField("Text:", "SampleText", _textBoxTypeName, _textBoxAssemblyName, "TextBoxMapper", "", false, null, propertyAttributes, null));
                tab.Add(col);
                UIFormColumn col2 = new UIFormColumn(100);
                col2.Add(
                    new UIFormField("Text2:", "SampleText2", _textBoxTypeName, _textBoxAssemblyName, "TextBoxMapper", "", false, null, new Hashtable(), null));
                col2.Add(
                    new UIFormField("Text2:", "SampleText2", _textBoxTypeName, _textBoxAssemblyName, "TextBoxMapper", "", false, null, new Hashtable(), null));
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
                    new UIFormField("Text:", "SampleText", _textBoxTypeName, _textBoxAssemblyName, "TextBoxMapper", "", true, null, propertyAttributes, null));
                col.Add(
                    new UIFormField("Text2:", "SampleText2", _textBoxTypeName, _textBoxAssemblyName, "TextBoxMapper", "", true, null, new Hashtable(), null));
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
                col.Add(new UIFormField("Text:", "SampleText", _textBoxTypeName, _textBoxAssemblyName, mapperTypeName, mapperAssemblyName, false, null, propertyAttributes, null));
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
                    new UIFormField("Text:", "SampleText", _textBoxTypeName, _textBoxAssemblyName, "TextBoxMapper", "", true, null, propertyAttributes, null));
                col.Add(
                    new UIFormField("Text2:", "SampleText2", _textBoxTypeName, _textBoxAssemblyName, "TextBoxMapper", "", true, null, new Hashtable(), null));
                tab.Add(col);
                def.Add(tab);


                return def;
            }

            public UIFormTab GetFormTabOneField()
            {
                return GetSimpleUIFormDef()[0];
            }

            public UIFormTab GetFormTabOneIntegerField()
            {
                return GetSimpleUIFormDefInt()[0];
            }

            public UIFormTab GetFormTabTwoFields()
            {
                return GetSimpleUIFormDefTwoRows()[0];
            }

            public UIFormTab GetFormTabTwoColumns_1_1()
            {
                return GetSimpleUIFormDef1Row2Columns()[0];
            }

            public UIFormTab GetFormTabTwoColumns_1_2()
            {
                return GetSimpleUIFormDef2Row2Columns1RowWithMoreControls()[0];
            }

            public UIFormTab GetFormTabTwoColumnsOneRowWithWidths()
            {
                return GetSimpleUIFormDef1Row2Columns1Row()[0];
            }

            public UIFormTab GetFormTabOneColumnOneRowWithWidth()
            {
                return GetSimpleUIFormDef1Row1Column1Row()[0];
            }

            public UIFormTab GetFormTabThreeColumnsOneRowWithWidths()
            {
                return GetSimpleUIFormDef1Row3Columns()[0];
            }


            public UIFormTab GetFormTabOneFieldNoColumnWidth()
            {
                return GetSimpleUIFormDef_NoColumnWidth()[0];
            }

            public UIFormTab GetFormTabThreeColumnsOneColumnWithRowSpan()
            {
                return GetSimpleUIFormDef_3Columns_1Column_2RowSpan()[0];
            }

            public UIFormTab GetFormTabOneColumnThreeRowsWithRowSpan()
            {
                return GetSimpleUIFormDef_1Column3RowsWithRowSpan()[0];
            }


            public UIFormTab GetFormTabTwoColumns_2_1_ColSpan()
            {
                return GetSimpleUIFormDef_2Columns_2_1_ColSpan()[0];
            }


            public UIFormTab GetFormTabTwoFields_OneHasCompulsoryProp()
            {
                return GetSimpleUIFormDefTwoRowsOneHasCompulsoryProp()[0];
            }

            public UIFormTab GetFormTabOneField_ReadWriteParameter_ReadOnly()
            {
                return GetSimpleUIFormDefWithReadWriteRuleValueReadOnly()[0];
                
            }


            public UIFormTab GetFormTabTwoFields_OneHasToolTip()
            {
                return GetSimpleUIFormOneFieldHasToolTip()[0];
            }

            public UIFormTab GetFormTabOneFieldHasRowAndColSpan()
            {
                return GetSimpleUIFormTabOneFieldRowAndColSpan()[0];
            }

            public UIFormTab GetFormTabTwoFieldsWithNoAlignment()
            {
                return GetSimpleUIFormDef_NoAlignment()[0];
            }

            private UIForm GetSimpleUIFormDef_NoAlignment()
            {
                XmlUIFormLoader loader = new XmlUIFormLoader();
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
                return GetSimpleUIFormDef_RightAlignment()[0];
            }

            private UIForm GetSimpleUIFormDef_RightAlignment()
            {
                XmlUIFormLoader loader = new XmlUIFormLoader();
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
                return GetSimpleUIFormDef_CenterAlignment()[0];
            }

            private UIForm GetSimpleUIFormDef_CenterAlignment()
            {
                XmlUIFormLoader loader = new XmlUIFormLoader();
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
                return GetSimpleUIFormDef_InvalidAlignment()[0];
            }

            private UIForm GetSimpleUIFormDef_InvalidAlignment()
            {
                XmlUIFormLoader loader = new XmlUIFormLoader();
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
                return GetSimpleUIFormDef_WithAlignmentAndNumericUpdown()[0];
            }

            private UIForm GetSimpleUIFormDef_WithAlignmentAndNumericUpdown()
            {
                XmlUIFormLoader loader = new XmlUIFormLoader();
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
                return GetSimpleUIFormDef_WithNumericUpDown()[0];
            }

            private UIForm GetSimpleUIFormDef_WithNumericUpDown()
            {
                XmlUIFormLoader loader = new XmlUIFormLoader();
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
                return GetSimpleUIFormDef_WithMultiLineParameter()[0];
            }

            private UIForm GetSimpleUIFormDef_WithMultiLineParameter()
            {
                XmlUIFormLoader loader = new XmlUIFormLoader();
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
                return GetSimpleUIFormDef_WithInvalidMultiLineParameter()[0];
            }

            private UIForm GetSimpleUIFormDef_WithInvalidMultiLineParameter()
            {
               
                   XmlUIFormLoader loader = new XmlUIFormLoader();
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
                return GetSimpleUIFormDef_WithDecimalPlacesParameter()[0];
            }

            private UIForm GetSimpleUIFormDef_WithDecimalPlacesParameter()
            {
                XmlUIFormLoader loader = new XmlUIFormLoader();
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
                return GetSimpleUIFormDef_WithOptionsParameter()[0];
            }

            private UIForm GetSimpleUIFormDef_WithOptionsParameter()
            {
                XmlUIFormLoader loader = new XmlUIFormLoader();
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
                return GetSimpleUIFormDef_WithIsEmailParameter()[0];
            }

            private UIForm GetSimpleUIFormDef_WithIsEmailParameter()
            {
                XmlUIFormLoader loader = new XmlUIFormLoader();
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
                return GetSimpleUIFormDef_WithDateFormatParameter()[0];
            }

            private UIForm GetSimpleUIFormDef_WithDateFormatParameter()
            {
                XmlUIFormLoader loader = new XmlUIFormLoader();
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
            //public UIDef GetUIDef()
            //{
            //    return new UIDef("default", GetUIFormProperties(), GetUIGridProperties());
            //}
            //public UIForm GetUIFormProperties()
            //{
            //    UIForm def = new UIForm();
            //    def.Height = 300;
            //    def.Width = 350;
            //    UIFormTab tab = new UIFormTab();
            //    UIFormColumn col = new UIFormColumn(100);
            //    Hashtable propertyAttributes = new Hashtable();
            //    propertyAttributes.Add("numLines", 3);
            //    col.Add(
            //        new UIFormField("Text:", "SampleText",  "TextBoxGiz", "Habanero.UI.WebGUI", "TextBoxMapper", "", false, null,
            //                           propertyAttributes, null));
            //    tab.Add(col);
            //    def.Add(tab);
            //    return def;
            //}

            //public UIGrid GetUIGridProperties()
            //{
            //    UIGrid col = new UIGrid();
            //    col.Add(
            //        new UIGridColumn("Text:", "SampleText",  typeof (DataGridTextBoxColumn), true, 100,
            //                           UIGridColumn.PropAlignment.left, null));
            //    return col;
            //}


            //public UIForm SampleUserInterfaceMapper3Props()
            //{
            //    UIForm def = new UIForm();
            //    def.Height = 300;
            //    def.Width = 350;
            //    UIFormTab tab = new UIFormTab();
            //    UIFormColumn col = new UIFormColumn(100);
            //    col.Add(new UIFormField("Text:", "SampleText", "TextBoxGiz", "Habanero.UI.WebGUI", "TextBoxMapper", "", true, null, new Hashtable(), null));
            //    col.Add(
            //        new UIFormField("Date:", "SampleDate", "TextBoxGiz", "Habanero.UI.WebGUI", "TextBoxMapper", "", true, null,
            //                           new Hashtable(), null));
            //    col.Add(
            //        new UIFormField("Text2:", "SampleText2", "TextBoxGiz", "Habanero.UI.WebGUI", "TextBoxMapper", "", true, null, new Hashtable(), null));
            //    tab.Add(col);
            //    def.Add(tab);
            //    return def;
            //}

            //public UIForm SampleUserInterfaceMapperPrivatePropOnly()
            //{
            //    UIForm def = new UIForm();
            //    def.Height = 300;
            //    def.Width = 350;
            //    UIFormTab tab = new UIFormTab();
            //    def.Add(tab);
            //    UIFormColumn col = new UIFormColumn(100);
            //    tab.Add(col);
            //    col.Add(new UIFormField("Private Text:", "SampleTextPrivate", "TextBoxGiz", "Habanero.UI.WebGUI", "TextBoxMapper", "", true, null, new Hashtable(), null));
            //    return def;
            //}

            //public UIForm SampleUserInterfaceMapperDescribedPropOnly(string toolTipText)
            //{
            //    UIForm def = new UIForm();
            //    def.Height = 300;
            //    def.Width = 350;
            //    UIFormTab tab = new UIFormTab();
            //    def.Add(tab);
            //    UIFormColumn col = new UIFormColumn(100);
            //    tab.Add(col);
            //    col.Add(new UIFormField("Described Text:", "SampleTextDescribed", "TextBoxGiz", "Habanero.UI.WebGUI", "TextBoxMapper", "", true, toolTipText, new Hashtable(), null));
            //    return def;
            //}

            //public UIForm SampleUserInterfaceMapper2Cols()
            //{
            //    UIForm def = new UIForm();
            //    def.Height = 300;
            //    def.Width = 350;
            //    UIFormTab tab = new UIFormTab();
            //    UIFormColumn col1 = new UIFormColumn(100);
            //    UIFormColumn col2 = new UIFormColumn(150);
            //    col1.Add(
            //        new UIFormField("Text:", "SampleText", "TextBoxGiz", "Habanero.UI.WebGUI", "TextBoxMapper", "", true, null, new Hashtable(), null));
            //    col1.Add(
            //        new UIFormField("Date:", "SampleDate", "TextBoxGiz", "Habanero.UI.WebGUI", "TextBoxMapper", "", true, null,
            //                           new Hashtable(), null));
            //    col2.Add(
            //        new UIFormField("Text2:", "SampleText2", "TextBoxGiz", "Habanero.UI.WebGUI", "TextBoxMapper", "", true, null, new Hashtable(), null));
            //    tab.Add(col1);
            //    tab.Add(col2);
            //    def.Add(tab);
            //    return def;
            //}


            //public UIForm SampleUserInterfaceMapper2Tabs()
            //{
            //    UIForm def = new UIForm();
            //    def.Height = 300;
            //    def.Width = 350;
            //    UIFormTab tab1 = new UIFormTab("mytab1");
            //    UIFormTab tab2 = new UIFormTab("mytab2");
            //    UIFormColumn col1 = new UIFormColumn(100);
            //    UIFormColumn col2 = new UIFormColumn(150);
            //    col1.Add(
            //        new UIFormField("Text:", "SampleText", "TextBoxGiz", "Habanero.UI.WebGUI", "TextBoxMapper", "", true, null, new Hashtable(), null));
            //    col1.Add(
            //        new UIFormField("Date:", "SampleDate", "TextBoxGiz", "Habanero.UI.WebGUI", "TextBoxMapper", "", true, null,
            //                           new Hashtable(), null));
            //    col2.Add(
            //        new UIFormField("Text2:", "SampleText2", "TextBoxGiz", "Habanero.UI.WebGUI", "TextBoxMapper", "", true, null, new Hashtable(), null));
            //    tab1.Add(col1);
            //    tab2.Add(col2);
            //    def.Add(tab1);
            //    def.Add(tab2);
            //    return def;
            //}

            //public UIForm SampleUserInterfaceMapperColSpanning()
            //{
            //    UIForm def = new UIForm();
            //    def.Height = 300;
            //    def.Width = 350;
            //    UIFormTab tab = new UIFormTab();
            //    UIFormColumn col = new UIFormColumn(100);
            //    Hashtable propertyAttributes = new Hashtable();
            //    propertyAttributes.Add("numLines", 3);
            //    propertyAttributes.Add("colSpan", 2);
            //    col.Add(
            //        new UIFormField("Text:", "SampleText", "TextBoxGiz", "Habanero.UI.WebGUI", "TextBoxMapper", "", false, null, propertyAttributes, null));
            //    col.Add(
            //        new UIFormField("Text2:", "SampleText2", "TextBoxGiz", "Habanero.UI.WebGUI", "TextBoxMapper", "", false, null, new Hashtable(), null));
            //    tab.Add(col);
            //    UIFormColumn col2 = new UIFormColumn(100);
            //    col2.Add(
            //        new UIFormField("Text2:", "SampleText2", "TextBoxGiz", "Habanero.UI.WebGUI", "TextBoxMapper", "", false, null, new Hashtable(), null));
            //    tab.Add(col2);
            //    def.Add(tab);
            //    return def;
            //}

            //public UIForm SampleUserInterfaceMapperRowSpanning()
            //{
            //    UIForm def = new UIForm();
            //    def.Height = 300;
            //    def.Width = 350;
            //    UIFormTab tab = new UIFormTab();
            //    UIFormColumn col = new UIFormColumn(100);
            //    Hashtable propertyAttributes = new Hashtable();
            //    propertyAttributes.Add("numLines", 3);
            //    propertyAttributes.Add("rowSpan", 2);
            //    col.Add(
            //        new UIFormField("Text:", "SampleText", "TextBoxGiz", "Habanero.UI.WebGUI", "TextBoxMapper", "", false, null, propertyAttributes, null));
            //    tab.Add(col);
            //    UIFormColumn col2 = new UIFormColumn(100);
            //    col2.Add(
            //        new UIFormField("Text2:", "SampleText2", "TextBoxGiz", "Habanero.UI.WebGUI", "TextBoxMapper", "", false, null, new Hashtable(), null));
            //    col2.Add(
            //        new UIFormField("Text2:", "SampleText2", "TextBoxGiz", "Habanero.UI.WebGUI", "TextBoxMapper", "", false, null, new Hashtable(), null));
            //    tab.Add(col2);
            //    def.Add(tab);
            //    return def;
            //}

            //public UIForm SampleUserInterface_ReadWriteRule()
            //{
            //    UIForm def = new UIForm();
            //    def.Height = 300;
            //    def.Width = 350;
            //    UIFormTab tab = new UIFormTab();
            //    UIFormColumn col = new UIFormColumn(100);
            //    Hashtable propertyAttributes = new Hashtable();
            //    propertyAttributes.Add("readWriteRule", "ReadOnly");
            //    col.Add(
            //        new UIFormField("Text:", "SampleText", "TextBoxGiz", "Habanero.UI.WebGUI", "TextBoxMapper", "", true, null, propertyAttributes, null));
            //    col.Add(
            //        new UIFormField("Text2:", "SampleText2", "TextBoxGiz", "Habanero.UI.WebGUI", "TextBoxMapper", "", true, null, new Hashtable(), null));
            //    tab.Add(col);
            //    def.Add(tab);

            //    return def;
            //}

            //public UIForm SampleUserInterface_WriteNewRule()
            //{
            //    UIForm def = new UIForm();
            //    def.Height = 300;
            //    def.Width = 350;
            //    UIFormTab tab = new UIFormTab();
            //    UIFormColumn col = new UIFormColumn(100);
            //    Hashtable propertyAttributes = new Hashtable();
            //    propertyAttributes.Add("readWriteRule", "WriteNew");
            //    col.Add(
            //        new UIFormField("Text:", "SampleText", "TextBoxGiz", "Habanero.UI.WebGUI", "TextBoxMapper", "", true, null, propertyAttributes, null));
            //    col.Add(
            //        new UIFormField("Text2:", "SampleText2", "TextBoxGiz", "Habanero.UI.WebGUI", "TextBoxMapper", "", true, null, new Hashtable(), null));
            //    tab.Add(col);
            //    def.Add(tab);


            //    return def;
            //}
            
        }
    }
}