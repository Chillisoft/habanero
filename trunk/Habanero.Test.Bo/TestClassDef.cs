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
            ClassDef.GetClassDefCol.Clear();
            XmlClassLoader loader = new XmlClassLoader();
            itsClassDef =
                loader.LoadClass(
                    @"
				<classDef name=""MyBo"" assembly=""Habanero.Test"">
					<propertyDef name=""MyBoID"" type=""Guid"" />
					<propertyDef name=""TestProp"" />
					<primaryKeyDef>
						<prop name=""MyBoID"" />
					</primaryKeyDef>
				</classDef>
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
            ClassDef.GetClassDefCol.Clear();
            ClassDef.LoadClassDefs(loader);
            Assert.AreEqual(2, ClassDef.GetClassDefCol.Count);
        }

    	private string GetTestClassDefinition(string suffix)
    	{
    		string classDefString = String.Format(
				@"
					<classDefs>
						<classDef name=""TestClass{0}"" assembly=""Habanero.Test.Bo.Loaders"" >
							<propertyDef name=""TestClass{0}ID"" />
                            <primaryKeyDef>
                                <prop name=""TestClass{0}ID""/>
                            </primaryKeyDef>
						</classDef>
						<classDef name=""TestRelatedClass{0}"" assembly=""Habanero.Test.Bo.Loaders"" >
							<propertyDef name=""TestRelatedClass{0}ID"" />
                            <primaryKeyDef>
                                <prop name=""TestRelatedClass{0}ID""/>
                            </primaryKeyDef>
						</classDef>
					</classDefs>
			", suffix);
    		return classDefString;
    	}

    	public void TestLoadRepeatedClassDefs()
		{
			XmlClassDefsLoader loader;
            loader = new XmlClassDefsLoader(GetTestClassDefinition(""), new DtdLoader());
    		ClassDef.GetClassDefCol.Clear();
			ClassDef.LoadClassDefs(loader);
			Assert.AreEqual(2, ClassDef.GetClassDefCol.Count);
			//Now load the same again.
            loader = new XmlClassDefsLoader(GetTestClassDefinition(""), new DtdLoader());
			ClassDef.LoadClassDefs(loader);
			Assert.AreEqual(2, ClassDef.GetClassDefCol.Count);
		}

		public void TestLoadMultipleClassDefs()
		{
			XmlClassDefsLoader loader;
            loader = new XmlClassDefsLoader(GetTestClassDefinition(""), new DtdLoader());
			ClassDef.GetClassDefCol.Clear();
			ClassDef.LoadClassDefs(loader);
			Assert.AreEqual(2, ClassDef.GetClassDefCol.Count);
			// Now load some more classes
            loader = new XmlClassDefsLoader(GetTestClassDefinition("Other"), new DtdLoader());
			ClassDef.LoadClassDefs(loader);
			Assert.AreEqual(4, ClassDef.GetClassDefCol.Count);
		}
    }
}