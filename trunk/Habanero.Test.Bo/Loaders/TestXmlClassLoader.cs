using System;
using System.IO;
using Habanero.Bo.ClassDefinition;
using Habanero.Bo.Loaders;
using Habanero.Bo;
using Habanero.Base;
using Habanero.Util;
using NUnit.Framework;
using BusinessObject=Habanero.Bo.BusinessObject;

namespace Habanero.Test.Bo.Loaders
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

        [Test, ExpectedException(typeof(InvalidXmlDefinitionException), "An invalid node 'class' was encountered when loading the class definitions.")]
        public void TestInvalidXmlFormatWrongRootElement()
        {
            loader.LoadClass("<class name=\"TestClass\" assembly=\"Habanero.Test.Bo.Loaders\" />");
        }

        //[
        //    Test,
        //        ExpectedException(typeof (UnknownTypeNameException),
        //            "The type TestClassNoExist does not exist in assembly Habanero.Test.Bo.Loaders")]
        //public void TestLoadingNonExistantClass()
        //{
        //    ClassDef def = loader.LoadClass("<classDef name=\"TestClassNoExist\" assembly=\"Habanero.Test.Bo.Loaders\" />");
        //    Assert.IsNull( def.)
        //}

        //[
        //    Test,
        //        ExpectedException(typeof (UnknownTypeNameException),
        //            "The assembly Habanero.Test.Bo.NoExist could not be found")]
        //public void TestLoadingNonExistantAssembly()
        //{
        //    loader.LoadClass("<classDef name=\"TestClass\" assembly=\"Habanero.Test.Bo.NoExist\" />");
        //}

        [Test]
        public void TestNoTableName()
        {
            ClassDef def =
                loader.LoadClass(
                    @"
				<classDef name=""TestClass"" assembly=""Habanero.Test.Bo.Loaders"">
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
				<classDef name=""TestClass"" assembly=""Habanero.Test.Bo.Loaders"" tableName=""myTable"">
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
				<classDef name=""TestClass"" assembly=""Habanero.Test.Bo.Loaders"" supportsSynchronising=""true"">
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
				<classDef name=""TestClass"" assembly=""Habanero.Test.Bo.Loaders"" >
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
				<classDef name=""TestClass"" assembly=""Habanero.Test.Bo.Loaders"">
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
				<classDef name=""TestClass"" assembly=""Habanero.Test.Bo.Loaders"">
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
				<classDef name=""TestClass"" assembly=""Habanero.Test.Bo.Loaders"">
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
				<classDef name=""TestClass"" assembly=""Habanero.Test.Bo.Loaders"">
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
				<classDef name=""TestClass"" assembly=""Habanero.Test.Bo.Loaders"">
					<propertyDef name=""TestProp"" />
                    <primaryKeyDef>
                        <prop name=""TestProp""/>
                    </primaryKeyDef>
					<relationshipDef 
						name=""TestRelationship"" 
						type=""single"" 
						relatedType=""TestRelatedClass"" 
						relatedAssembly=""Habanero.Test.Bo.Loaders""
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
						<classDef name=""TestClass"" assembly=""Habanero.Test.Bo.Loaders"" >
							<propertyDef name=""TestClassID"" />
                            <primaryKeyDef>
                                <prop name=""TestClassID""/>
                            </primaryKeyDef>
						</classDef>
					</classDefs>",
                                 new DtdLoader()));
            ClassDef def =
                loader.LoadClass(
                    @"
				<classDef name=""TestRelatedClass"" assembly=""Habanero.Test.Bo.Loaders"">
					<superClassDesc className=""Habanero.Test.Bo.Loaders.TestClass"" assemblyName=""Habanero.Test.Bo"" />
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
				<classDef name=""TestClass"" assembly=""Habanero.Test.Bo.Loaders"">
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