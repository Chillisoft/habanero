using System.Xml;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;
using NUnit.Framework;

namespace Habanero.Test.BO.Loaders
{
    /// <summary>
    /// Summary description for TestXmlClassDefsLoader.
    /// </summary>
//    [TestFixture]
    public class TestClassDefValidator
    {
        [TestFixtureSetUp]
        public void SetupFixture()
        {
            ClassDef.ClassDefs.Clear();
        }

//        [Test]
        public void Test_Valid_Relationship()
        {
            //----------------------Test Setup ----------------------
            const string classDefsString = @"
					<classes>
						<class name=""TestClass"" assembly=""Habanero.Test.BO.Loaders"" >
							<property  name=""TestClassID"" />
                            <primaryKey>
                                <prop name=""TestClassID""/>
                            </primaryKey>
					        <relationship name=""TestRelatedClass"" type=""single"" relatedClass=""TestRelatedClass"" relatedAssembly=""Habanero.Test.BO.Loaders"">
						        <relatedProperty property=""TestClassID"" relatedProperty=""TestClassID"" />
					        </relationship>
						</class>
						<class name=""TestRelatedClass"" assembly=""Habanero.Test.BO.Loaders"" >
							<property  name=""TestRelatedClassID"" />
							<property  name=""TestClassID"" />
                            <primaryKey>
                                <prop name=""TestRelatedClassID""/>
                            </primaryKey>
						</class>
					</classes>
			";
            XmlClassDefsLoader loader = new XmlClassDefsLoader();
            ClassDefCol classDefList = loader.LoadClassDefs(classDefsString);
            ClassDefValidator defValidator = new ClassDefValidator();
            //--------------------Assert Preconditions-----------------
            Assert.AreEqual(2, classDefList.Count);
            Assert.IsTrue(classDefList.Contains("Habanero.Test.BO.Loaders", "TestClass"), "Class 'TestClass' should have been loaded.");
            Assert.IsTrue(classDefList.Contains("Habanero.Test.BO.Loaders", "TestRelatedClass"), "Class 'TestRelatedClass' should have been loaded.");
            //--------------------Execute Test-------------------------
            string errorMessage;
            bool areClassDefsValid = defValidator.AreClassDefsValid(classDefList, out errorMessage);
            //--------------------Assert Results ----------------------
            Assert.IsTrue(string.IsNullOrEmpty(errorMessage), "The err msg should be null but was :" + errorMessage);
            Assert.IsTrue(areClassDefsValid);
        }


        [Test]
        public void TestLoadClassDefs_InheritedClassWithNoPrimaryKey()
        {
            XmlClassDefsLoader loader = new XmlClassDefsLoader();
            ClassDefCol classDefList =
                loader.LoadClassDefs(
                    @"
					<classes>
						<class name=""TestClass"" assembly=""Habanero.Test.BO.Loaders"" >
							<property  name=""TestClassID"" />
                            <primaryKey>
                                <prop name=""TestClassID""/>
                            </primaryKey>
						</class>
						<class name=""TestClass2"" assembly=""Habanero.Test.BO.Loaders"" >
							<property  name=""TestClass2ID"" />
                            <primaryKey>
                                <prop name=""TestClass2ID""/>
                            </primaryKey>
						</class>
						<class name=""TestClassInherited"" assembly=""Habanero.Test.BO.Loaders"" >							
                            <superClass class=""TestClass"" assembly=""Habanero.Test.BO.Loaders"" />
						</class>
					</classes>
			");
            Assert.AreEqual(3, classDefList.Count);
            Assert.IsTrue(classDefList.Contains("Habanero.Test.BO.Loaders", "TestClass"), "Class 'TestClass' should have been loaded.");
            Assert.IsTrue(classDefList.Contains("Habanero.Test.BO.Loaders", "TestClass2"), "Class 'TestClass2' should have been loaded.");
            Assert.IsTrue(classDefList.Contains("Habanero.Test.BO.Loaders", "TestClassInherited"), "Class 'TestClassInherited' should have been loaded.");
            ClassDef classDefTestClass = classDefList["Habanero.Test.BO.Loaders", "TestClass"];
            ClassDef classDefInherited = classDefList["Habanero.Test.BO.Loaders", "TestClassInherited"];
            Assert.IsNotNull(classDefTestClass);
            Assert.IsNotNull(classDefInherited.SuperClassDef);
            Assert.IsNull(classDefInherited.PrimaryKeyDef);
        }

        [Test]
        public void TestLoadClassDefs_KeyDefinedWithInheritedProperties()
        {
            //-------------Setup Test Pack ------------------
            const string xml = @"
					<classes>
						<class name=""TestClass"" assembly=""Habanero.Test.BO.Loaders"" >
							<property  name=""TestClassID"" />
							<property  name=""TestClassName"" />
                            <primaryKey>
                                <prop name=""TestClassID""/>
                            </primaryKey>
						</class>
						<class name=""TestClassInherited"" assembly=""Habanero.Test.BO.Loaders"" >							
                            <superClass class=""TestClass"" assembly=""Habanero.Test.BO.Loaders"" />
                            <key>
                                <prop name=""TestClassName""/>
                            </key>
						</class>
					</classes>
			";
            //-------------Execute test ---------------------
            XmlClassDefsLoader loader = new XmlClassDefsLoader();
            ClassDefCol classDefList = loader.LoadClassDefs(xml);
            //-------------Test Result ----------------------
            Assert.AreEqual(2, classDefList.Count);
            Assert.IsTrue(classDefList.Contains("Habanero.Test.BO.Loaders", "TestClass"), "Class 'TestClass' should have been loaded.");
            Assert.IsTrue(classDefList.Contains("Habanero.Test.BO.Loaders", "TestClassInherited"), "Class 'TestClassInherited' should have been loaded.");
            ClassDef classDefTestClass = classDefList["Habanero.Test.BO.Loaders", "TestClass"];
            IPropDef propDef = classDefTestClass.PropDefcol["TestClassName"];
            ClassDef classDefInherited = classDefList["Habanero.Test.BO.Loaders", "TestClassInherited"];
            Assert.IsNotNull(classDefInherited.SuperClassDef);
            Assert.AreEqual(1, classDefInherited.KeysCol.Count);
            KeyDef keyDef = classDefInherited.KeysCol.GetKeyDefAtIndex(0);
            IPropDef keyDefPropDef = keyDef["TestClassName"];
            Assert.AreSame(propDef, keyDefPropDef, "The key's property should have been resolved to be the property of the superclass by the loader.");
        }

        [Test]
        public void TestLoadClassDefs_KeyDefinedWithNonExistantProperty()
        {
            //-------------Setup Test Pack ------------------
            const string xml = @"
				<classes>
					<class name=""TestClass"" assembly=""Habanero.Test.BO.Loaders"" >
						<property  name=""TestClassID"" />
						<property  name=""TestClassName"" />
                        <primaryKey>
                            <prop name=""TestClassID""/>
                        </primaryKey>
					</class>
					<class name=""TestClassInherited"" assembly=""Habanero.Test.BO.Loaders"" >							
                        <superClass class=""TestClass"" assembly=""Habanero.Test.BO.Loaders"" />
                        <key>
                            <prop name=""DoesNotExist""/>
                        </key>
					</class>
				</classes>
			";
            XmlClassDefsLoader loader = new XmlClassDefsLoader();

            //-------------Execute test ---------------------
            try
            {
                loader.LoadClassDefs(xml);
                Assert.Fail("expected Err");
            }
                //---------------Test Result -----------------------
            catch (InvalidXmlDefinitionException ex)
            {
                StringAssert.Contains("In a 'prop' element for the '' key of the 'TestClassInherited' class, the propery 'DoesNotExist' given in the 'name' attribute does not exist for the class or for any of it's superclasses. Either add the property definition or check the spelling and capitalisation of the specified property", ex.Message);
            }
        }

        [Test, ExpectedException(typeof(XmlException))]
        public void TestNoRootNodeException()
        {
            XmlClassDefsLoader loader = new XmlClassDefsLoader();
            loader.LoadClassDefs(@"<invalidRootNode>");
        }
    }
}