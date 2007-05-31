using CoreBiz.Bo;
using CoreBiz.Bo.ClassDefinition;
using CoreBiz.Bo.Loaders;
using CoreBiz.Generic;

namespace CoreBiz.Test {
	/// <summary>
	/// Summary description for MyBo.
	/// </summary>
	public class MyBo : BusinessObjectBase {
		public MyBo(ClassDef def) : base(def) {}
		public MyBo(ClassDef def, IDatabaseConnection conn) : base(def, conn) {}

		protected override ClassDef ConstructClassDef() {
			return mClassDef;
		}

		//		public override IUserInterfaceMapper GetUserInterfaceMapper() {
		//		return new MyBoUserInterfaceMapper() ;
		//		}

		public string MyName {
			get { return "MyNameIsMyBo"; }
		}

		public static ClassDef LoadDefaultClassDef() {
			XmlClassLoader itsLoader = new XmlClassLoader();
			ClassDef itsClassDef = itsLoader.LoadClass(@"
				<classDef name=""MyBo"" assembly=""CoreBiz.Test.Bo"">
					<propertyDef name=""MyBoID"" />
					<propertyDef name=""TestProp"" />
					<propertyDef name=""TestProp2"" />
					<primaryKeyDef>
						<prop name=""MyBoID"" />
					</primaryKeyDef>
					<uiDef>
						<uiGridDef>
							<uiGridProperty heading=""Test Prop"" propertyName=""TestProp"" gridControlTypeName=""DataGridTextBoxColumn"" />
							<uiGridProperty heading=""Test Prop 2"" propertyName=""TestProp2"" gridControlTypeName=""DataGridComboBoxColumn"" />
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
			return itsClassDef;
		}

		public static ClassDef LoadClassDefWithNoLookup() {
			XmlClassLoader itsLoader = new XmlClassLoader();
			ClassDef itsClassDef = itsLoader.LoadClass(@"
				<classDef name=""MyBo"" assembly=""CoreBiz.Test.Bo"">
					<propertyDef name=""MyBoID"" />
					<propertyDef name=""TestProp"" />
					<propertyDef name=""TestProp2"" />
					<primaryKeyDef>
						<prop name=""MyBoID"" />
					</primaryKeyDef>
					<uiDef>
						<uiGridDef>
							<uiGridProperty heading=""Test Prop"" propertyName=""TestProp"" gridControlTypeName=""DataGridTextBoxColumn"" />
							<uiGridProperty heading=""Test Prop 2"" propertyName=""TestProp2"" gridControlTypeName=""DataGridTextBoxColumn"" />
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
			return itsClassDef;
		}

		public static ClassDef LoadClassDefWithBoolean() {
			XmlClassLoader itsLoader = new XmlClassLoader();
			ClassDef itsClassDef = itsLoader.LoadClass(@"
				<classDef name=""MyBo"" assembly=""CoreBiz.Test.Bo"">
					<propertyDef name=""MyBoID"" />
					<propertyDef name=""TestProp"" />
					<propertyDef name=""TestProp2"" />
					<propertyDef name=""TestBoolean"" type=""System.Boolean"" />
					<primaryKeyDef>
						<prop name=""MyBoID"" />
					</primaryKeyDef>
					<uiDef>
						<uiGridDef>
							<uiGridProperty heading=""Test Prop"" propertyName=""TestProp"" gridControlTypeName=""DataGridTextBoxColumn"" />
							<uiGridProperty heading=""Test Prop 2"" propertyName=""TestProp2"" gridControlTypeName=""DataGridTextBoxColumn"" />
							<uiGridProperty heading=""Test Boolean"" propertyName=""TestBoolean"" gridControlTypeName=""DataGridBoolColumn"" />
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
			return itsClassDef;
		}

		public static ClassDef LoadClassDefWithLookup() {
			XmlClassLoader itsLoader = new XmlClassLoader();
			ClassDef itsClassDef = itsLoader.LoadClass(@"
				<classDef name=""MyBo"" assembly=""CoreBiz.Test.Bo"">
					<propertyDef name=""MyBoID"" />
					<propertyDef name=""TestProp"" />
					<propertyDef name=""TestProp2"" type=""System.Guid"" >
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
							<uiGridProperty heading=""Test Prop"" propertyName=""TestProp"" gridControlTypeName=""DataGridTextBoxColumn"" />
							<uiGridProperty heading=""Test Prop 2"" propertyName=""TestProp2"" gridControlTypeName=""DataGridComboBoxColumn"" />
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
			return itsClassDef;
		}

		public static ClassDef LoadClassDefWithRelationship() {
			XmlClassLoader itsLoader = new XmlClassLoader();
			ClassDef itsClassDef = itsLoader.LoadClass(@"
				<classDef name=""MyBo"" assembly=""CoreBiz.Test.Bo"">
					<propertyDef name=""MyBoID"" />
					<propertyDef name=""TestProp"" />
					<propertyDef name=""TestProp2"" type=""System.Guid"" >
						<simpleLookupListSource>
							<stringGuidPair string=""s1"" guid=""{E6E8DC44-59EA-4e24-8D53-4A43DC2F25E7}"" />
							<stringGuidPair string=""s2"" guid=""{F428FADC-3740-412c-91A7-ECEB4D414414}"" />
						</simpleLookupListSource>
					</propertyDef>
					<propertyDef name=""RelatedID"" type=""System.Guid"" />
					<primaryKeyDef>
						<prop name=""MyBoID"" />
					</primaryKeyDef>
					<relationshipDef name=""MyRelationship"" type=""single"" relatedType=""MyRelatedBo"" relatedAssembly=""CoreBiz.Test.Bo"">
						<relKeyDef>
							<relProp name=""RelatedID"" relatedPropName=""MyRelatedBoID"" />
						</relKeyDef>
					</relationshipDef>

					<uiDef>
						<uiGridDef>
							<uiGridProperty heading=""Test Prop"" propertyName=""TestProp"" gridControlTypeName=""DataGridTextBoxColumn"" />
							<uiGridProperty heading=""Test Prop 2"" propertyName=""TestProp2"" gridControlTypeName=""DataGridComboBoxColumn"" />
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
			return itsClassDef;
		}


		//		public static ClassDef LoadClassDefWithVirtualUIProp() 
		//		{
		//			XmlClassLoader itsLoader = new XmlClassLoader() ;
		//			ClassDef itsClassDef = itsLoader.LoadClass(@"
		//				<classDef name=""MyBo"" assembly=""CoreBiz.Test.Bo"">
		//					<propertyDef name=""MyBoID"" />
		//					<propertyDef name=""TestProp"" />
		//					<propertyDef name=""TestProp2"" />
		//					<primaryKeyDef>
		//						<prop name=""MyBoID"" />
		//					</primaryKeyDef>
		//					<uiDef>
		//						<uiGridDef>
		//							<uiGridProperty heading=""Test Prop"" propertyName=""TestProp"" gridControlTypeName=""DataGridTextBoxColumn"" />
		//							<uiGridProperty heading=""Test Prop 2"" propertyName=""TestProp2"" gridControlTypeName=""DataGridComboBoxColumn"" />
		//							<uiGridProperty heading=""-MyName-"" propertyName=""-MyName-"" />
		//						</uiGridDef>
		//					</uiDef>
		//				</classDef>
		//			");
		//			return itsClassDef;
		//		}

	}

	public class MyRelatedBo : BusinessObjectBase {
		private static ClassDef itsClassDef;
		public MyRelatedBo() : base() {}
		public MyRelatedBo(ClassDef def) : base(def) {}
		public MyRelatedBo(ClassDef def, IDatabaseConnection conn) : base(def, conn) {}

		protected override ClassDef ConstructClassDef() {
			return itsClassDef;
		}

		public static ClassDef LoadClassDef() {
			XmlClassLoader itsLoader = new XmlClassLoader();
			itsClassDef = itsLoader.LoadClass(@"
				<classDef name=""MyRelatedBo"" assembly=""CoreBiz.Test.Bo"">
					<propertyDef name=""MyRelatedBoID"" />
					<propertyDef name=""MyRelatedTestProp"" />
					<primaryKeyDef>
						<prop name=""MyRelatedBoID"" />
					</primaryKeyDef>
				</classDef>
			");
			return itsClassDef;
		}
	}

	//	public class MyBoUserInterfaceMapper : IUserInterfaceMapper  {
	//		public UIFormDef GetUIFormProperties() {
	//			UIFormDef def = new UIFormDef();
	//			UIFormTab tab = new UIFormTab() ;
	//			UIFormColumn col = new UIFormColumn(100) ;
	//			col.Add(new UIFormProperty("Test Prop", "TestProp", "TextBox", "TextBoxMapper")) ;
	//			col.Add(new UIFormProperty("Test Prop 2", "TestProp2", "TextBox", "TextBoxMapper"));
	//			tab.Add(col);
	//			def.Add(tab);
	//			return def;
	//		}
	//
	//		public UIGridDef GetUIGridProperties() {
	//			UIGridDef col = new UIGridDef();
	//			col.Add(new UIGridProperty("Test Prop", "TestProp", typeof(DataGridTextBoxColumn))) ;
	//			col.Add(new UIGridProperty("Test Prop 2", "TestProp2", typeof(DataGridTextBoxColumn)));
	//			return col;
	//		}
	//	}
}