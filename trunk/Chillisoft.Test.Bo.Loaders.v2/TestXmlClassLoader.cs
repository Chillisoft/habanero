using System;
using System.IO;
using Chillisoft.Bo.ClassDefinition.v2;
using Chillisoft.Bo.Loaders.v2;
using Chillisoft.Bo.v2;
using Chillisoft.Generic.v2;
using NUnit.Framework;
using BusinessObject=Chillisoft.Bo.v2.BusinessObject;

namespace Chillisoft.Test.Bo.Loaders.v2
{
    /// <summary>
    /// Summary description for TestXmlLoader.
    /// </summary>
    [TestFixture]
    public class TestXmlClassLoader
    {
        private XmlClassLoader loader;

        [SetUp]
        public void SetupTest()
        {
            loader = new XmlClassLoader();
            ClassDef.GetClassDefCol.Clear();
        }

        [Test, ExpectedException(typeof(FileNotFoundException), "The Document Type Definition (DTD) for " +
                    "the XML element 'class' was not found in the application's output/execution directory (eg. bin/debug). " +
                    "Ensure that you have a .DTD file for each of the XML class " +
                    "definition elements you will be using, and that they are being copied to the " +
                    "application's output directory (eg. bin/debug).  Alternatively, check that " +
                    "the element name was spelt correctly and has the correct capitalisation.")]
        public void TestInvalidXmlFormatWrongRootElement()
        {
            loader.LoadClass("<class name=\"TestClass\" assembly=\"Chillisoft.Test.Bo.Loaders.v2\" />");
        }

        //[
        //    Test,
        //        ExpectedException(typeof (UnknownTypeNameException),
        //            "The type TestClassNoExist does not exist in assembly Chillisoft.Test.Bo.Loaders.v2")]
        //public void TestLoadingNonExistantClass()
        //{
        //    ClassDef def = loader.LoadClass("<classDef name=\"TestClassNoExist\" assembly=\"Chillisoft.Test.Bo.Loaders.v2\" />");
        //    Assert.IsNull( def.)
        //}

        //[
        //    Test,
        //        ExpectedException(typeof (UnknownTypeNameException),
        //            "The assembly Chillisoft.Test.Bo.v2.NoExist could not be found")]
        //public void TestLoadingNonExistantAssembly()
        //{
        //    loader.LoadClass("<classDef name=\"TestClass\" assembly=\"Chillisoft.Test.Bo.v2.NoExist\" />");
        //}

        [Test]
        public void TestNoTableName()
        {
            ClassDef def =
                loader.LoadClass(
                    @"
				<classDef name=""TestClass"" assembly=""Chillisoft.Test.Bo.Loaders.v2"">
                    <propertyDef name=""TestProp"" />
                    <primaryKeyDef>
                        <prop name=""TestProp""/>
                    </primaryKeyDef>
				</classDef>
			");
            Assert.AreEqual("TestClass", def.TableName);
        }

        [Test]
        public void TestTableName()
        {
            ClassDef def =
                loader.LoadClass(
                    @"
				<classDef name=""TestClass"" assembly=""Chillisoft.Test.Bo.Loaders.v2"" tableName=""myTable"">
                    <propertyDef name=""TestProp"" />
                    <primaryKeyDef>
                        <prop name=""TestProp""/>
                    </primaryKeyDef>
				</classDef>
			");
            Assert.AreEqual("myTable", def.TableName);
        }

        [Test]
        public void TestSupportsSynchronisation()
        {
            ClassDef def =
                loader.LoadClass(
                    @"
				<classDef name=""TestClass"" assembly=""Chillisoft.Test.Bo.Loaders.v2"" supportsSynchronising=""true"">
                    <propertyDef name=""TestProp"" />
                    <primaryKeyDef>
                        <prop name=""TestProp""/>
                    </primaryKeyDef>
				</classDef>
			");
            Assert.IsTrue(def.SupportsSynchronising);
        }


        [Test]
        public void TestSupportsSynchronisationDefault()
        {
            ClassDef def =
                loader.LoadClass(
                    @"
				<classDef name=""TestClass"" assembly=""Chillisoft.Test.Bo.Loaders.v2"" >
                    <propertyDef name=""TestProp"" />
                    <primaryKeyDef>
                        <prop name=""TestProp""/>
                    </primaryKeyDef>
				</classDef>
			");
            Assert.IsFalse(def.SupportsSynchronising);
        }

        [Test]
        public void TestTwoPropClass()
        {
            ClassDef def =
                loader.LoadClass(
                    @"
				<classDef name=""TestClass"" assembly=""Chillisoft.Test.Bo.Loaders.v2"">
					<propertyDef name=""TestProp"" />
					<propertyDef name=""TestProp2"" />
                    <primaryKeyDef>
                        <prop name=""TestProp""/>
                    </primaryKeyDef>
				</classDef>
			");
            Assert.AreEqual(2, def.PropDefcol.Count);
            Assert.AreEqual("TestClass", def.ClassName);
        }

        [Test]
        public void TestClassWithPrimaryKeyDef()
        {
            ClassDef def =
                loader.LoadClass(
                    @"
				<classDef name=""TestClass"" assembly=""Chillisoft.Test.Bo.Loaders.v2"">
					<propertyDef name=""TestProp"" />
					<primaryKeyDef>
						<prop name=""TestProp"" />
					</primaryKeyDef>
				</classDef>
			");
            Assert.IsNotNull(def.PrimaryKeyDef);
            Assert.AreEqual(1, def.PrimaryKeyDef.Count);
        }

        [Test, ExpectedException(typeof (InvalidXmlDefinitionException))]
        public void TestClassWithMoreThanOnePrimaryKeyDef()
        {
            loader.LoadClass(
                @"
				<classDef name=""TestClass"" assembly=""Chillisoft.Test.Bo.Loaders.v2"">
					<propertyDef name=""TestProp"" />
					<propertyDef name=""TestProp2"" />
					<primaryKeyDef>
						<prop name=""TestProp"" />
					</primaryKeyDef>
					<primaryKeyDef>
						<prop name=""TestProp2"" />
					</primaryKeyDef>
				</classDef>
			");
        }

        [Test]
        public void TestClassWithKeyDefs()
        {
            ClassDef def =
                loader.LoadClass(
                    @"
				<classDef name=""TestClass"" assembly=""Chillisoft.Test.Bo.Loaders.v2"">
					<propertyDef name=""TestProp"" />
					<propertyDef name=""TestProp2"" />
					<propertyDef name=""TestProp3"" />
					<keyDef>
						<prop name=""TestProp"" />
					</keyDef>
					<keyDef>
						<prop name=""TestProp2"" />
						<prop name=""TestProp3"" />
					</keyDef>
                    <primaryKeyDef>
                        <prop name=""TestProp""/>
                    </primaryKeyDef>
				</classDef>
			");
            Assert.AreEqual(2, def.KeysCol.Count);
        }

        [Test]
        public void TestClassWithSingleRelationship()
        {
            ClassDef def =
                loader.LoadClass(
                    @"
				<classDef name=""TestClass"" assembly=""Chillisoft.Test.Bo.Loaders.v2"">
					<propertyDef name=""TestProp"" />
                    <primaryKeyDef>
                        <prop name=""TestProp""/>
                    </primaryKeyDef>
					<relationshipDef 
						name=""TestRelationship"" 
						type=""single"" 
						relatedType=""TestRelatedClass"" 
						relatedAssembly=""Chillisoft.Test.Bo.Loaders.v2""
					>
						<relKeyDef>
							<relProp name=""TestProp"" relatedPropName=""TestRelatedProp"" />
						</relKeyDef>
					</relationshipDef>
				</classDef>
			");
            RelationshipDefCol relDefCol = def.RelationshipDefCol;
            Assert.AreEqual(1, relDefCol.Count, "There should be one relationship def from the given xml definition");
            Assert.IsNotNull(relDefCol["TestRelationship"],
                             "'TestRelationship' should be the name of the relationship created");
        }

        [Test]
        public void TestClassWithSuperClass()
        {
            ClassDef.GetClassDefCol.Clear();
            ClassDef.LoadClassDefs(
                new XmlClassDefsLoader(
                    @"
					<classDefs>
						<classDef name=""TestClass"" assembly=""Chillisoft.Test.Bo.Loaders.v2"" >
							<propertyDef name=""TestClassID"" />
                            <primaryKeyDef>
                                <prop name=""TestClassID""/>
                            </primaryKeyDef>
						</classDef>
					</classDefs>",
                    ""));
            ClassDef def =
                loader.LoadClass(
                    @"
				<classDef name=""TestRelatedClass"" assembly=""Chillisoft.Test.Bo.Loaders.v2"">
					<superClassDesc className=""TestClass"" assemblyName=""Chillisoft.Test.Bo.Loaders.v2"" />
					<propertyDef name=""TestProp"" />
                    <primaryKeyDef>
                        <prop name=""TestProp""/>
                    </primaryKeyDef>
				</classDef>
			");
            Assert.IsNotNull(def.SuperClassDesc);
            Assert.AreSame(ClassDef.GetClassDefCol[typeof (TestClass)], def.SuperClassDesc.SuperClassDef);
        }

        [Test]
        public void TestClassWithUIDef()
        {
            ClassDef def =
                loader.LoadClass(
                    @"
				<classDef name=""TestClass"" assembly=""Chillisoft.Test.Bo.Loaders.v2"">
					<propertyDef name=""TestProp"" />
					<propertyDef name=""TestProp2"" />
					<primaryKeyDef>
						<prop name=""TestProp"" />
					</primaryKeyDef>
					<uiDef>
						<uiFormDef>
							<uiFormTab name=""testtab"">
								<uiFormColumn>
									<uiFormProperty label=""Test Prop"" propertyName=""TestProp"" />
								</uiFormColumn>
							</uiFormTab>
						</uiFormDef>
					</uiDef>
				</classDef>
			");
            UIDef uiDef = def.UIDefCol["default"];
            Assert.IsNotNull(uiDef);
            Assert.IsNotNull(uiDef.UIFormDef);
            Assert.IsNull(uiDef.UIGridDef);
        }
    }

    public class TestClass : BusinessObject
    {
        protected override ClassDef ConstructClassDef()
        {
            throw new NotImplementedException();
        }
    }

    public class TestRelatedClass : BusinessObject
    {
        protected override ClassDef ConstructClassDef()
        {
            throw new NotImplementedException();
        }
    }
}