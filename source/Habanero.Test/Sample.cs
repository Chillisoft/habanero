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
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using Habanero.BO.ClassDefinition;
using Habanero.BO;
using Habanero.Base;
using BusinessObject=Habanero.BO.BusinessObject;

namespace Habanero.Test
{
    /// <summary>
    /// Summary description for Sample.
    /// </summary>
    public class Sample : BusinessObject
    {
        private static Dictionary<string, object> itsLookupCollection;
        private static Dictionary<string, object> itsBOLookupCollection;

        public Sample() : base()
        {
        }

        public Sample(IDatabaseConnection conn) : base(conn)
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
                return CreateClassDef();
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

        private static ClassDef CreateClassDef()
        {
            PropDefCol lPropDefCol = new PropDefCol();
            lPropDefCol.Add(
                new PropDef("SampleText", typeof (String), PropReadWriteRule.ReadWrite, "SampleText",  null));
            lPropDefCol.Add(
                new PropDef("SampleText2", typeof (String), PropReadWriteRule.ReadWrite, "SampleText2", null));
			lPropDefCol.Add(
				new PropDef("SampleDate", typeof(DateTime), PropReadWriteRule.ReadWrite, "SampleDate", null));
			lPropDefCol.Add(
				new PropDef("SampleDateNullable", typeof(DateTime), PropReadWriteRule.ReadWrite, "SampleDate", null));
			lPropDefCol.Add(
                new PropDef("SampleBoolean", typeof (Boolean), PropReadWriteRule.ReadWrite, "SampleBoolean",
                            null));
            lPropDefCol.Add(
                new PropDef("SampleLookupID", typeof (Guid), PropReadWriteRule.ReadWrite, "SampleLookupID",
                            null));
            lPropDefCol.Add(
                new PropDef("SampleInt", typeof (int), PropReadWriteRule.ReadWrite, "SampleInt", 0));
            lPropDefCol.Add(
                new PropDef("SampleMoney", typeof (Decimal), PropReadWriteRule.ReadWrite, "SampleInt",
                            new Decimal(0)));
            PropDef lPropDef =
                new PropDef("SampleLookup2ID", typeof (Guid), PropReadWriteRule.ReadWrite, "SampleLookup2ID",
                            null);
            itsLookupCollection = new Dictionary<string, object>();
            itsLookupCollection.Add("Test1", new Guid("{6E8B3DDB-1B13-4566-868D-57478C1F4BEE}"));
            itsLookupCollection.Add("Test2", new Guid("{7209B956-96A0-4720-8E49-DE154FA0E096}"));
            itsLookupCollection.Add("Test3", new Guid("{F45DE850-C693-44d8-AC39-8CEE5435B21A}"));
            lPropDef.LookupList = new SimpleLookupList(itsLookupCollection);
            lPropDefCol.Add(lPropDef);
                lPropDefCol.Add(new PropDef("SampleLookup3ID", typeof (String), PropReadWriteRule.ReadWrite, "SampleLookup3ID",
                                            null));
            PropDef def = new PropDef("SampleID", typeof (Guid), PropReadWriteRule.WriteOnce, null);
            lPropDefCol.Add(def);
            PrimaryKeyDef primaryKey = new PrimaryKeyDef();
            primaryKey.IsObjectID = true;
            primaryKey.Add(lPropDefCol["SampleID"]);
            KeyDefCol keysCol = new KeyDefCol();
            RelationshipDefCol relDefCol = new RelationshipDefCol();
            ClassDef lClassDef = new ClassDef(typeof (Sample), primaryKey, lPropDefCol, keysCol, relDefCol);
            lClassDef.UIDefCol.Add(new SampleUserInterfaceMapper().GetUIDef()); 
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

        /// <summary>
        /// Summary description for SampleUserInterfaceMapper.
        /// </summary>
        public class SampleUserInterfaceMapper 
        {
            public UIDef GetUIDef()
            {
                return new UIDef("default", GetUIFormProperties(), GetUIGridProperties());
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
                    new UIFormField("Text:", "SampleText", typeof(TextBox), "TextBoxMapper", "", false,
                                       propertyAttributes));
                tab.Add(col);
                def.Add(tab);
                return def;
            }

            public UIGrid GetUIGridProperties()
            {
                UIGrid col = new UIGrid();
                col.Add(
                    new UIGridColumn("Text:", "SampleText", typeof (DataGridTextBoxColumn), true, 100,
                                       UIGridColumn.PropAlignment.left));
                return col;
            }
        }


        public static UIForm SampleUserInterfaceMapper3Props()
        {
            UIForm def = new UIForm();
            def.Height = 300;
            def.Width = 350;
            UIFormTab tab = new UIFormTab();
            UIFormColumn col = new UIFormColumn(100);
            col.Add(new UIFormField("Text:", "SampleText", typeof (TextBox), "TextBoxMapper", "", true, new Hashtable()));
            col.Add(
                new UIFormField("Date:", "SampleDate", typeof(DateTimePicker), "DateTimePickerMapper", "", true,
                                   new Hashtable()));
            col.Add(
                new UIFormField("Text2:", "SampleText2", typeof(TextBox), "TextBoxMapper", "", true, new Hashtable()));
            tab.Add(col);
            def.Add(tab);
            return def;
        }


        public static UIForm SampleUserInterfaceMapper2Cols()
        {
            UIForm def = new UIForm();
            def.Height = 300;
            def.Width = 350;
            UIFormTab tab = new UIFormTab();
            UIFormColumn col1 = new UIFormColumn(100);
            UIFormColumn col2 = new UIFormColumn(150);
            col1.Add(
                new UIFormField("Text:", "SampleText", typeof(TextBox), "TextBoxMapper", "", true, new Hashtable()));
            col1.Add(
                new UIFormField("Date:", "SampleDate", typeof(DateTimePicker), "DateTimePickerMapper", "", true,
                                   new Hashtable()));
            col2.Add(
                new UIFormField("Text2:", "SampleText2", typeof(TextBox), "TextBoxMapper", "", true, new Hashtable()));
            tab.Add(col1);
            tab.Add(col2);
            def.Add(tab);
            return def;
        }


        public static UIForm SampleUserInterfaceMapper2Tabs()
        {
            UIForm def = new UIForm();
            def.Height = 300;
            def.Width = 350;
            UIFormTab tab1 = new UIFormTab("mytab1");
            UIFormTab tab2 = new UIFormTab("mytab2");
            UIFormColumn col1 = new UIFormColumn(100);
            UIFormColumn col2 = new UIFormColumn(150);
            col1.Add(
                new UIFormField("Text:", "SampleText", typeof(TextBox), "TextBoxMapper", "", true, new Hashtable()));
            col1.Add(
                new UIFormField("Date:", "SampleDate", typeof(DateTimePicker), "DateTimePickerMapper", "", true,
                                   new Hashtable()));
            col2.Add(
                new UIFormField("Text2:", "SampleText2", typeof(TextBox), "TextBoxMapper", "", true, new Hashtable()));
            tab1.Add(col1);
            tab2.Add(col2);
            def.Add(tab1);
            def.Add(tab2);
            return def;
        }

        public static UIForm SampleUserInterfaceMapperColSpanning()
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
                new UIFormField("Text:", "SampleText", typeof(TextBox), "TextBoxMapper", "", false, propertyAttributes));
            col.Add(
                new UIFormField("Text2:", "SampleText2", typeof(TextBox), "TextBoxMapper", "", false, new Hashtable()));
            tab.Add(col);
            UIFormColumn col2 = new UIFormColumn(100);
            col2.Add(
                new UIFormField("Text2:", "SampleText2", typeof(TextBox), "TextBoxMapper", "", false, new Hashtable()));
            tab.Add(col2);
            def.Add(tab);
            return def;
        }

        public static UIForm SampleUserInterfaceMapperRowSpanning()
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
                new UIFormField("Text:", "SampleText", typeof(TextBox), "TextBoxMapper", "", false, propertyAttributes));
            tab.Add(col);
            UIFormColumn col2 = new UIFormColumn(100);
            col2.Add(
                new UIFormField("Text2:", "SampleText2", typeof(TextBox), "TextBoxMapper", "", false, new Hashtable()));
            col2.Add(
                new UIFormField("Text2:", "SampleText2", typeof(TextBox), "TextBoxMapper", "", false, new Hashtable()));
            tab.Add(col2);
            def.Add(tab);
            return def;
        }


        public decimal SampleMoney
        {
            get { return (decimal) this.GetPropertyValue("SampleMoney"); }
            set { this.SetPropertyValue("SampleMoney", value); }
        }
    }
}