using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;
using NUnit.Framework;

namespace Habanero.Test.BO.ClassDefinition
{
    [TestFixture]
    public class TestClassDefHelper
    {
        [SetUp]
        public void SetupTest()
        {
            //Runs every time that any testmethod is executed
        }

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            //Code that is executed before any test is run in this class. If multiple tests
            // are executed then it will still only be called once.
        }

        [TearDown]
        public void TearDownTest()
        {
            //runs every time any testmethod is complete
        }

        [Test]
        public void Test_GetSuperClassClassDef()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ClassDefCol classDefCol = new ClassDefCol();
            ClassDef classDef = new ClassDef("Habanero.Test.BO", "UnknownClass", null, null, null, null, null);
            classDefCol.Add(classDef);
            SuperClassDef superClassDef = new SuperClassDef(classDef.AssemblyName, classDef.ClassName, ORMapping.ClassTableInheritance, null, null);
            //---------------Assert Precondition----------------
            Assert.AreEqual(0, ClassDef.ClassDefs.Count);
            Assert.AreEqual(1, classDefCol.Count);
            //---------------Execute Test ----------------------
            IClassDef def = ClassDefHelper.GetSuperClassClassDef(superClassDef, classDefCol);
            //---------------Test Result -----------------------
            Assert.IsNotNull(def);
            Assert.AreSame(classDef, def);
        }

        [Test]
        public void Test_GetSuperClassClassDef_NotFound()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ClassDef classDef = new ClassDef("Habanero.Test.BO", "UnknownClass", null, null, null, null, null);
            ClassDef.ClassDefs.Add(classDef);
            SuperClassDef superClassDef = new SuperClassDef(classDef.AssemblyName, classDef.ClassName, ORMapping.ClassTableInheritance, null, null);
            ClassDefCol classDefCol = new ClassDefCol();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            try
            {
                IClassDef def = ClassDefHelper.GetSuperClassClassDef(superClassDef, classDefCol);
                //---------------Test Result -----------------------
                Assert.Fail("Expected to throw an InvalidXmlDefinitionException");
                Assert.IsNull(def);// This will never get hit. It is here to state an expectation and to avoid a resharper warning.
            }
            catch (InvalidXmlDefinitionException ex)
            {
                StringAssert.Contains("The class definition for the super class with the type " +
                    "'Habanero.Test.BO.UnknownClass' was not found. Check that the class definition " +
                    "exists or that spelling and capitalisation are correct. " +
                    "There are 0 class definitions currently loaded.", ex.Message);
            }
        }

        [Test]
        public void Test_GetPrimaryKeyDef_ClassWithPK()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ClassDef classDef = new XmlClassLoader().LoadClass(
                    @"	<class name=""TestClass"" assembly=""Habanero.Test.BO.Loaders"" >
							<property  name=""TestClassID"" type=""Guid"" />
                            <primaryKey>
                                <prop name=""TestClassID""/>
                            </primaryKey>
						</class>");
            ClassDefCol classDefCol = new ClassDefCol();
            classDefCol.Add(classDef);
            //---------------Assert Precondition----------------
            Assert.IsNotNull(classDef.PrimaryKeyDef);
            //---------------Execute Test ----------------------
            IPrimaryKeyDef primaryKeyDef = ClassDefHelper.GetPrimaryKeyDef(classDef, classDefCol);
            //---------------Test Result -----------------------
            Assert.IsNotNull(primaryKeyDef);
            Assert.AreSame(classDef.PrimaryKeyDef, primaryKeyDef);
        }

        [Test]
        public void Test_GetPrimaryKeyDef_ClassWithPKFromSuperClass()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ClassDef parentDef =  new XmlClassLoader().LoadClass(
                    @"	<class name=""TestClass"" assembly=""Habanero.Test.BO.Loaders"" >
							<property  name=""TestClassID"" type=""Guid"" />
                            <primaryKey>
                                <prop name=""TestClassID""/>
                            </primaryKey>
						</class>");
            ClassDef def = new XmlClassLoader().LoadClass(
                    @"
				<class name=""TestRelatedClass"" assembly=""Habanero.Test.BO.Loaders"">
					<superClass class=""TestClass"" assembly=""Habanero.Test.BO.Loaders"" />
					<property  name=""TestProp"" type=""Guid"" />                    
				</class>
			");
            ClassDefCol classDefCol = new ClassDefCol();
            classDefCol.Add(parentDef);
            //---------------Assert Precondition----------------
            Assert.IsNotNull(parentDef.PrimaryKeyDef);
            Assert.IsNotNull(def.SuperClassDef);
            Assert.IsNull(def.PrimaryKeyDef);
            //---------------Execute Test ----------------------
            IPrimaryKeyDef primaryKeyDef = ClassDefHelper.GetPrimaryKeyDef(def, classDefCol);
            //---------------Test Result -----------------------
            Assert.IsNotNull(primaryKeyDef);
            Assert.AreSame(parentDef.PrimaryKeyDef, primaryKeyDef);
        }

    }
}