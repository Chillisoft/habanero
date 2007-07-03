using System;
using Habanero.Bo.ClassDefinition;
using Habanero.Bo.Loaders;
using Habanero.Bo;
using Habanero.Test;
using Habanero.Util;
using NUnit.Framework;

namespace Habanero.Test.Bo
{
    /// <summary>
    /// Summary description for TestClassDef.
    /// </summary>
    [TestFixture]
    public class TestClassDef
    {
        private ClassDef itsClassDef;

        [Test]
        public void TestCreateBusinessObject()
        {
            ClassDef.ClassDefs.Clear();
            XmlClassLoader loader = new XmlClassLoader();
            itsClassDef =
                loader.LoadClass(
                    @"
				<class name=""MyBo"" assembly=""Habanero.Test"">
					<property  name=""MyBoID"" type=""Guid"" />
					<property  name=""TestProp"" />
					<primaryKey>
						<prop name=""MyBoID"" />
					</primaryKey>
				</class>
			");
            BusinessObject bo = itsClassDef.CreateNewBusinessObject();
            Assert.AreSame(typeof (MyBo), bo.GetType());
            bo.SetPropertyValue("TestProp", "TestValue");
            Assert.AreEqual("TestValue", bo.GetPropertyValue("TestProp"));
        }

        [Test]
        public void TestLoadClassDefs()
        {
            XmlClassDefsLoader loader =
                new XmlClassDefsLoader(
                    GetTestClassDefinition(""),
                     new DtdLoader());
            ClassDef.ClassDefs.Clear();
            ClassDef.LoadClassDefs(loader);
            Assert.AreEqual(2, ClassDef.ClassDefs.Count);
        }

    	private string GetTestClassDefinition(string suffix)
    	{
    		string classDefString = String.Format(
                @"
					<classes>
						<class name=""TestClass{0}"" assembly=""Habanero.Test.Bo.Loaders"" >
							<property  name=""TestClass{0}ID"" />
                            <primaryKey>
                                <prop name=""TestClass{0}ID""/>
                            </primaryKey>
						</class>
						<class name=""TestRelatedClass{0}"" assembly=""Habanero.Test.Bo.Loaders"" >
							<property  name=""TestRelatedClass{0}ID"" />
                            <primaryKey>
                                <prop name=""TestRelatedClass{0}ID""/>
                            </primaryKey>
						</class>
					</classes>
			", suffix);
    		return classDefString;
    	}

		[Test]
		public void TestLoadRepeatedClassDefs()
		{
			XmlClassDefsLoader loader;
            loader = new XmlClassDefsLoader(GetTestClassDefinition(""), new DtdLoader());
    		ClassDef.ClassDefs.Clear();
			ClassDef.LoadClassDefs(loader);
			Assert.AreEqual(2, ClassDef.ClassDefs.Count);
			//Now load the same again.
            loader = new XmlClassDefsLoader(GetTestClassDefinition(""), new DtdLoader());
			ClassDef.LoadClassDefs(loader);
			Assert.AreEqual(2, ClassDef.ClassDefs.Count);
		}

		[Test]
		public void TestLoadMultipleClassDefs()
		{
			XmlClassDefsLoader loader;
            loader = new XmlClassDefsLoader(GetTestClassDefinition(""), new DtdLoader());
			ClassDef.ClassDefs.Clear();
			ClassDef.LoadClassDefs(loader);
			Assert.AreEqual(2, ClassDef.ClassDefs.Count);
			// Now load some more classes
            loader = new XmlClassDefsLoader(GetTestClassDefinition("Other"), new DtdLoader());
			ClassDef.LoadClassDefs(loader);
			Assert.AreEqual(4, ClassDef.ClassDefs.Count);
		}
    }
}