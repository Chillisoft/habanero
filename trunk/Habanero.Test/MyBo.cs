using Habanero.Bo.ClassDefinition;
using Habanero.Bo.Loaders;
using Habanero.Bo;
using Habanero.Base;
using BusinessObject=Habanero.Bo.BusinessObject;

namespace Habanero.Test
{
    /// <summary>
    /// Summary description for MyBo.
    /// </summary>
    public class MyBo : BusinessObject
    {
        public MyBo(ClassDef def) : base(def)
        {
        }

        public MyBo(ClassDef def, IDatabaseConnection conn) : base(def, conn)
        {
        }

        protected override ClassDef ConstructClassDef()
        {
            return _classDef;
        }

        public string MyName
        {
            get { return "MyNameIsMyBo"; }
        }

        public static ClassDef LoadDefaultClassDef()
        {
            XmlClassLoader itsLoader = new XmlClassLoader();
            ClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""MyBo"" assembly=""Habanero.Test"">
					<property  name=""MyBoID"" />
					<property  name=""TestProp"" />
					<property  name=""TestProp2"" />
					<primaryKey>
						<prop name=""MyBoID"" />
					</primaryKey>
					<uiDef>
						<uiGridDef>
							<uiGridProperty heading=""Test Prop"" propertyName=""TestProp"" gridControlTypeName=""DataGridViewTextBoxColumn"" />
							<uiGridProperty heading=""Test Prop 2"" propertyName=""TestProp2"" gridControlTypeName=""DataGridViewComboBoxColumn"" />
						</uiGridDef>
						<uiFormDef>
							<uiFormTab name=""Tab1"">
								<uiFormColumn>
									<uiFormProperty label=""Test Prop"" propertyName=""TestProp"" controlTypeName=""TextBox"" mapperTypeName=""TextBoxMapper"" />
									<uiFormProperty label=""Test Prop 2"" propertyName=""TestProp2"" controlTypeName=""TextBox"" mapperTypeName=""TextBoxMapper"" />
								</uiFormColumn>
							</uiFormTab>
						</uiFormDef>
					</uiDef>
					<uiDef name=""Alternate"">
						<uiGridDef>
							<uiGridProperty heading=""Test Prop"" propertyName=""TestProp"" gridControlTypeName=""DataGridViewTextBoxColumn"" />
						</uiGridDef>
						<uiFormDef>
							<uiFormTab name=""Tab1"">
								<uiFormColumn>
									<uiFormProperty label=""Test Prop"" propertyName=""TestProp"" controlTypeName=""TextBox"" mapperTypeName=""TextBoxMapper"" />
								</uiFormColumn>
							</uiFormTab>
						</uiFormDef>
					</uiDef>




				</class>


			");
			ClassDef.GetClassDefCol.Add(itsClassDef);
			return itsClassDef;
        }

        public static ClassDef LoadClassDefWithNoLookup()
        {
            XmlClassLoader itsLoader = new XmlClassLoader();
            ClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""MyBo"" assembly=""Habanero.Test"">
					<property  name=""MyBoID"" />
					<property  name=""TestProp"" />
					<property  name=""TestProp2"" />
					<primaryKey>
						<prop name=""MyBoID"" />
					</primaryKey>
					<uiDef>
						<uiGridDef>
							<uiGridProperty heading=""Test Prop"" propertyName=""TestProp"" gridControlTypeName=""DataGridViewTextBoxColumn"" />
							<uiGridProperty heading=""Test Prop 2"" propertyName=""TestProp2"" gridControlTypeName=""DataGridViewTextBoxColumn"" />
						</uiGridDef>
						<uiFormDef>
							<uiFormTab name=""Tab1"">
								<uiFormColumn>
									<uiFormProperty label=""Test Prop"" propertyName=""TestProp"" controlTypeName=""TextBox"" mapperTypeName=""TextBoxMapper"" />
									<uiFormProperty label=""Test Prop 2"" propertyName=""TestProp2"" controlTypeName=""TextBox"" mapperTypeName=""TextBoxMapper"" />
								</uiFormColumn>
							</uiFormTab>
						</uiFormDef>
					</uiDef>
				</class>

			");
			ClassDef.GetClassDefCol.Add(itsClassDef);
			return itsClassDef;
        }

        public static ClassDef LoadClassDefWithBoolean()
        {
            XmlClassLoader itsLoader = new XmlClassLoader();
            ClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""MyBo"" assembly=""Habanero.Test"">
					<property  name=""MyBoID"" />
					<property  name=""TestProp"" />
					<property  name=""TestProp2"" />
					<property  name=""TestBoolean"" type=""Boolean"" />
					<primaryKey>
						<prop name=""MyBoID"" />
					</primaryKey>
					<uiDef>
						<uiGridDef>
							<uiGridProperty heading=""Test Prop"" propertyName=""TestProp"" gridControlTypeName=""DataGridViewTextBoxColumn"" />
							<uiGridProperty heading=""Test Prop 2"" propertyName=""TestProp2"" gridControlTypeName=""DataGridViewTextBoxColumn"" />
							<uiGridProperty heading=""Test Boolean"" propertyName=""TestBoolean"" gridControlTypeName=""DataGridViewCheckBoxColumn"" />
						</uiGridDef>
						<uiFormDef>
							<uiFormTab name=""Tab1"">
								<uiFormColumn>
									<uiFormProperty label=""Test Prop"" propertyName=""TestProp"" controlTypeName=""TextBox"" mapperTypeName=""TextBoxMapper"" />
									<uiFormProperty label=""Test Prop 2"" propertyName=""TestProp2"" controlTypeName=""TextBox"" mapperTypeName=""TextBoxMapper"" />
									<uiFormProperty label=""Test Boolean"" propertyName=""TestBoolean"" controlTypeName=""CheckBox"" mapperTypeName=""CheckBoxMapper"" />
								</uiFormColumn>
							</uiFormTab>
						</uiFormDef>
					</uiDef>
				</class>
				
			");
			ClassDef.GetClassDefCol.Add(itsClassDef);
			return itsClassDef;
        }

        public static ClassDef LoadClassDefWithLookup()
        {
            XmlClassLoader itsLoader = new XmlClassLoader();
            ClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""MyBo"" assembly=""Habanero.Test"">
					<property  name=""MyBoID"" />
					<property  name=""TestProp"" />
					<property  name=""TestProp2"" type=""Guid"" >
						<simpleLookupList>
							<stringGuidPair string=""s1"" guid=""{E6E8DC44-59EA-4e24-8D53-4A43DC2F25E7}"" />
							<stringGuidPair string=""s2"" guid=""{F428FADC-3740-412c-91A7-ECEB4D414414}"" />
						</simpleLookupList>
					</property>
					<primaryKey>
						<prop name=""MyBoID"" />
					</primaryKey>
					<uiDef>
						<uiGridDef>
							<uiGridProperty heading=""Test Prop"" propertyName=""TestProp"" gridControlTypeName=""DataGridViewTextBoxColumn"" />
							<uiGridProperty heading=""Test Prop 2"" propertyName=""TestProp2"" gridControlTypeName=""DataGridViewComboBoxColumn"" />
						</uiGridDef>
						<uiFormDef>
							<uiFormTab name=""Tab1"">
								<uiFormColumn>
									<uiFormProperty label=""Test Prop"" propertyName=""TestProp"" controlTypeName=""TextBox"" mapperTypeName=""TextBoxMapper"" />
									<uiFormProperty label=""Test Prop 2"" propertyName=""TestProp2"" controlTypeName=""TextBox"" mapperTypeName=""TextBoxMapper"" />
								</uiFormColumn>
							</uiFormTab>
						</uiFormDef>
					</uiDef>
				</class>


			");
			ClassDef.GetClassDefCol.Add(itsClassDef);
			return itsClassDef;
        }

        public static ClassDef LoadClassDefWithRelationship()
        {
            XmlClassLoader itsLoader = new XmlClassLoader();
            ClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""MyBo"" assembly=""Habanero.Test"">
					<property  name=""MyBoID"" />
					<property  name=""TestProp"" />
					<property  name=""TestProp2"" type=""Guid"" >
						<simpleLookupList>
							<stringGuidPair string=""s1"" guid=""{E6E8DC44-59EA-4e24-8D53-4A43DC2F25E7}"" />
							<stringGuidPair string=""s2"" guid=""{F428FADC-3740-412c-91A7-ECEB4D414414}"" />
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
					<uiDef>
						<uiGridDef>
							<uiGridProperty heading=""Test Prop"" propertyName=""TestProp"" gridControlTypeName=""DataGridViewTextBoxColumn"" />
							<uiGridProperty heading=""Test Prop 2"" propertyName=""TestProp2"" gridControlTypeName=""DataGridViewComboBoxColumn"" />
						</uiGridDef>
						<uiFormDef>
							<uiFormTab name=""Tab1"">
								<uiFormColumn>
									<uiFormProperty label=""Test Prop"" propertyName=""TestProp"" controlTypeName=""TextBox"" mapperTypeName=""TextBoxMapper"" />
									<uiFormProperty label=""Test Prop 2"" propertyName=""TestProp2"" controlTypeName=""TextBox"" mapperTypeName=""TextBoxMapper"" />
								</uiFormColumn>
							</uiFormTab>
						</uiFormDef>
					</uiDef>
				</class>


			");
			ClassDef.GetClassDefCol.Add(itsClassDef);
            return itsClassDef;
        }

        public static MyBo Create()
        {
            MyBo bo = new MyBo(ClassDef.GetClassDefCol[typeof (MyBo)]);
            MyBo.AddToLoadedBusinessObjectCol(bo);
            return bo;
        }

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