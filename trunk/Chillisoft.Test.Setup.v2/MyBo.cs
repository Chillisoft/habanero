using Chillisoft.Bo.ClassDefinition.v2;
using Chillisoft.Bo.Loaders.v2;
using Chillisoft.Bo.v2;
using Chillisoft.Generic.v2;

namespace Chillisoft.Test.Setup.v2
{
    /// <summary>
    /// Summary description for MyBo.
    /// </summary>
    public class MyBo : BusinessObjectBase
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
				<classDef name=""MyBo"" assembly=""Chillisoft.Test.Setup.v2"">
					<propertyDef name=""MyBoID"" />
					<propertyDef name=""TestProp"" />
					<propertyDef name=""TestProp2"" />
					<primaryKeyDef>
						<prop name=""MyBoID"" />
					</primaryKeyDef>
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




				</classDef>


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
				<classDef name=""MyBo"" assembly=""Chillisoft.Test.Setup.v2"">
					<propertyDef name=""MyBoID"" />
					<propertyDef name=""TestProp"" />
					<propertyDef name=""TestProp2"" />
					<primaryKeyDef>
						<prop name=""MyBoID"" />
					</primaryKeyDef>
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
				</classDef>

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
				<classDef name=""MyBo"" assembly=""Chillisoft.Test.Setup.v2"">
					<propertyDef name=""MyBoID"" />
					<propertyDef name=""TestProp"" />
					<propertyDef name=""TestProp2"" />
					<propertyDef name=""TestBoolean"" type=""Boolean"" />
					<primaryKeyDef>
						<prop name=""MyBoID"" />
					</primaryKeyDef>
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
				</classDef>
				
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
				<classDef name=""MyBo"" assembly=""Chillisoft.Test.Setup.v2"">
					<propertyDef name=""MyBoID"" />
					<propertyDef name=""TestProp"" />
					<propertyDef name=""TestProp2"" type=""Guid"" >
						<simpleLookupListSource>
							<stringGuidPair string=""s1"" guid=""{E6E8DC44-59EA-4e24-8D53-4A43DC2F25E7}"" />
							<stringGuidPair string=""s2"" guid=""{F428FADC-3740-412c-91A7-ECEB4D414414}"" />
						</simpleLookupListSource>
					</propertyDef>
					<primaryKeyDef>
						<prop name=""MyBoID"" />
					</primaryKeyDef>
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
				</classDef>


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
				<classDef name=""MyBo"" assembly=""Chillisoft.Test.Setup.v2"">
					<propertyDef name=""MyBoID"" />
					<propertyDef name=""TestProp"" />
					<propertyDef name=""TestProp2"" type=""Guid"" >
						<simpleLookupListSource>
							<stringGuidPair string=""s1"" guid=""{E6E8DC44-59EA-4e24-8D53-4A43DC2F25E7}"" />
							<stringGuidPair string=""s2"" guid=""{F428FADC-3740-412c-91A7-ECEB4D414414}"" />
						</simpleLookupListSource>
					</propertyDef>
					<propertyDef name=""RelatedID"" type=""Guid"" />
					<primaryKeyDef>
						<prop name=""MyBoID"" />
					</primaryKeyDef>
					<relationshipDef name=""MyRelationship"" type=""single"" relatedType=""MyRelatedBo"" relatedAssembly=""Chillisoft.Test.Setup.v2"">
						<relKeyDef>
							<relProp name=""RelatedID"" relatedPropName=""MyRelatedBoID"" />
						</relKeyDef>
					</relationshipDef>
					<relationshipDef name=""MyMultipleRelationship"" type=""multiple"" relatedType=""MyRelatedBo"" relatedAssembly=""Chillisoft.Test.Setup.v2"">
						<relKeyDef>
							<relProp name=""MyBoID"" relatedPropName=""MyBoID"" />
						</relKeyDef>
					</relationshipDef>
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
				</classDef>


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

    public class MyRelatedBo : BusinessObjectBase
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
				<classDef name=""MyRelatedBo"" assembly=""Chillisoft.Test.Setup.v2"">
					<propertyDef name=""MyRelatedBoID"" />
					<propertyDef name=""MyRelatedTestProp"" />
					<propertyDef name=""MyBoID"" />
					<primaryKeyDef>
						<prop name=""MyRelatedBoID"" />
					</primaryKeyDef>
				</classDef>
			");
            return itsClassDef;
        }
    }
}