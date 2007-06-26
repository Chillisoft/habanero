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

        [Test, ExpectedException(typeof(InvalidXmlDefinitionException), "An invalid node 'class1' was encountered when loading the class definitions.")]
        public void TestInvalidXmlFormatWrongRootElement()
        {
            loader.LoadClass("<class1 name=\"TestClass\" assembly=\"Habanero.Test.Bo.Loaders\" />");
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
				<class name=""TestClass"" assembly=""Habanero.Test.Bo.Loaders"">
                    <property  name=""TestProp"" />
                    <primaryKey>
                        <prop name=""TestProp""/>
                    </primaryKey>
				</class>
			");
            Assert.AreEqual("TestClass", def.TableName);
        }

        [Test]
        public void TestTableName()
        {
            ClassDef def =
                loader.LoadClass(
                    @"
				<class name=""TestClass"" assembly=""Habanero.Test.Bo.Loaders"" table=""myTable"">
                    <property  name=""TestProp"" />
                    <primaryKey>
                        <prop name=""TestProp""/>
                    </primaryKey>
				</class>
			");
            Assert.AreEqual("myTable", def.TableName);
        }

//        [Test]
//        public void TestSupportsSynchronisation()
//        {
//            ClassDef def =
//                loader.LoadClass(
//                    @"
//				<class name=""TestClass"" assembly=""Habanero.Test.Bo.Loaders"" supportsSynchronising=""true"">
//                    <property  name=""TestProp"" />
//                    <primaryKey>
//                        <prop name=""TestProp""/>
//                    </primaryKey>
//				</class>
//			");
//            Assert.IsTrue(def.SupportsSynchronising);
//        }


//        [Test]
//        public void TestSupportsSynchronisationDefault()
//        {
//            ClassDef def =
//                loader.LoadClass(
//                    @"
//				<class name=""TestClass"" assembly=""Habanero.Test.Bo.Loaders"" >
//                    <property  name=""TestProp"" />
//                    <primaryKey>
//                        <prop name=""TestProp""/>
//                    </primaryKey>
//				</class>
//			");
//            Assert.IsFalse(def.SupportsSynchronising);
//        }

        [Test]
        public void TestTwoPropClass()
        {
            ClassDef def =
                loader.LoadClass(
                    @"
				<class name=""TestClass"" assembly=""Habanero.Test.Bo.Loaders"">
					<property  name=""TestProp"" />
					<property  name=""TestProp2"" />
                    <primaryKey>
                        <prop name=""TestProp""/>
                    </primaryKey>
				</class>
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
				<class name=""TestClass"" assembly=""Habanero.Test.Bo.Loaders"">
					<property  name=""TestProp"" />
					<primaryKey>
						<prop name=""TestProp"" />
					</primaryKey>
				</class>
			");
            Assert.IsNotNull(def.PrimaryKeyDef);
            Assert.AreEqual(1, def.PrimaryKeyDef.Count);
        }

        [Test, ExpectedException(typeof (InvalidXmlDefinitionException))]
        public void TestClassWithMoreThanOnePrimaryKeyDef()
        {
            loader.LoadClass(
                @"
				<class name=""TestClass"" assembly=""Habanero.Test.Bo.Loaders"">
					<property  name=""TestProp"" />
					<property  name=""TestProp2"" />
					<primaryKey>
						<prop name=""TestProp"" />
					</primaryKey>
					<primaryKey>
						<prop name=""TestProp2"" />
					</primaryKey>
				</class>
			");
        }

        [Test]
        public void TestClassWithKeyDefs()
        {
            ClassDef def =
                loader.LoadClass(
                    @"
				<class name=""TestClass"" assembly=""Habanero.Test.Bo.Loaders"">
					<property  name=""TestProp"" />
					<property  name=""TestProp2"" />
					<property  name=""TestProp3"" />
					<key>
						<prop name=""TestProp"" />
					</key>
					<key>
						<prop name=""TestProp2"" />
						<prop name=""TestProp3"" />
					</key>
                    <primaryKey>
                        <prop name=""TestProp""/>
                    </primaryKey>
				</class>
			");
            Assert.AreEqual(2, def.KeysCol.Count);
        }

        [Test]
        public void TestClassWithSingleRelationship()
        {
            ClassDef def =
                loader.LoadClass(
                    @"
				<class name=""TestClass"" assembly=""Habanero.Test.Bo.Loaders"">
					<property  name=""TestProp"" />
                    <primaryKey>
                        <prop name=""TestProp""/>
                    </primaryKey>
					<relationship 
						name=""TestRelationship"" 
						type=""single"" 
						relatedClass=""TestRelatedClass"" 
						relatedAssembly=""Habanero.Test.Bo.Loaders""
					>
						<relatedProperty property=""TestProp"" relatedProperty=""TestRelatedProp"" />
					</relationship>
				</class>
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
					<classes>
						<class name=""TestClass"" assembly=""Habanero.Test.Bo.Loaders"" >
							<property  name=""TestClassID"" />
                            <primaryKey>
                                <prop name=""TestClassID""/>
                            </primaryKey>
						</class>
					</classes>",
                                 new DtdLoader()));
            ClassDef def =
                loader.LoadClass(
                    @"
				<class name=""TestRelatedClass"" assembly=""Habanero.Test.Bo.Loaders"">
					<superClass class=""Habanero.Test.Bo.Loaders.TestClass"" assembly=""Habanero.Test.Bo"" />
					<property  name=""TestProp"" />
                    <primaryKey>
                        <prop name=""TestProp""/>
                    </primaryKey>
				</class>
			");
            Assert.IsNotNull(def.SuperClassDef);
            Assert.AreSame(ClassDef.GetClassDefCol[typeof (TestClass)], def.SuperClassDef.SuperClassClassDef);
        }

        [Test]
        public void TestClassWithUIDef()
        {
            ClassDef def =
                loader.LoadClass(
                    @"
				<class name=""TestClass"" assembly=""Habanero.Test.Bo.Loaders"">
					<property  name=""TestProp"" />
					<property  name=""TestProp2"" />
					<primaryKey>
						<prop name=""TestProp"" />
					</primaryKey>
					<uiDef>
						<uiFormDef>
							<uiFormTab name=""testtab"">
								<uiFormColumn>
									<uiFormProperty label=""Test Prop"" propertyName=""TestProp"" />
								</uiFormColumn>
							</uiFormTab>
						</uiFormDef>
					</uiDef>
				</class>
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