//---------------------------------------------------------------------------------
// Copyright (C) 2009 Chillisoft Solutions
// 
// This file is part of the Habanero framework.
// 
//     Habanero is a free framework: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     The Habanero framework is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;
using NUnit.Framework;

namespace Habanero.Test.BO.Loaders
{
    /// <summary>
    /// Summary description for TestXmlUIPropertyCollectionLoader.
    /// </summary>
    [TestFixture]
    public class TestXmlUIFormLoader
    {
        private XmlUIFormLoader loader;

        [SetUp]
        public virtual void SetupTest()
        {
            Initialise();
            GlobalRegistry.UIExceptionNotifier = new RethrowingExceptionNotifier();
        }

        protected void Initialise() {
            loader = new XmlUIFormLoader(new DtdLoader(), GetDefClassFactory());
        }

        protected virtual IDefClassFactory GetDefClassFactory()
        {
            return new DefClassFactory();
        }

        [Test]
        public void TestLoadPropertyCollection()
        {
            IUIForm col =
                loader.LoadUIFormDef(
                    @"
					<form width=""100"" height=""120"" title=""testheading"">
						<tab name=""testtab"">
							<columnLayout>
								<field label=""testlabel1"" property=""testpropname1"" type=""Button"" mapperType=""testmappertypename1"" />
								<field label=""testlabel2"" property=""testpropname2"" type=""Button"" mapperType=""testmappertypename2"" />
							</columnLayout>
						</tab>
					</form>");
            //Assert.AreEqual(typeof(MyBO), col.Class);
            //Assert.AreEqual("Habanero.Test.BO.MyBO_testName", col.Name.ToString() );
            Assert.AreEqual(1, col.Count, "There should be 1 tab"); // 1 tab
            Assert.AreEqual(1, col[0].Count, "There should be one column in that tab");
            Assert.AreEqual(2, col[0][0].Count, "There should be two propertys in that column");
            Assert.AreEqual("testlabel1", col[0][0][0].Label);
            Assert.AreEqual("testlabel2", col[0][0][1].Label);
            Assert.AreEqual(100, col.Width);
            Assert.AreEqual(120, col.Height);
            Assert.AreEqual("testheading", col.Title);
        }

        [Test]
        public void TestFormWithFields()
        {
            IUIForm col =
                loader.LoadUIFormDef(
                    @"
					<form width=""100"" height=""120"" title=""testheading"">
						<field label=""testlabel1"" property=""testpropname1"" type=""Button"" mapperType=""testmappertypename1"" />
						<field label=""testlabel2"" property=""testpropname2"" type=""Button"" mapperType=""testmappertypename2"" />
					</form>");
            //Assert.AreEqual(typeof(MyBO), col.Class);
            //Assert.AreEqual("Habanero.Test.BO.MyBO_testName", col.Name.ToString() );
            Assert.AreEqual(1, col.Count, "There should be 1 tab"); // 1 tab
            Assert.AreEqual(1, col[0].Count, "There should be one column in that tab");
            Assert.AreEqual(2, col[0][0].Count, "There should be two propertys in that column");
            Assert.AreEqual("testlabel1", col[0][0][0].Label);
            Assert.AreEqual("testlabel2", col[0][0][1].Label);
            Assert.AreEqual(100, col.Width);
            Assert.AreEqual(120, col.Height);
            Assert.AreEqual("testheading", col.Title);
        }

        [Test]
        public void TestFormWithColumns()
        {
            IUIForm col =
                loader.LoadUIFormDef(
                    @"
					<form width=""100"" height=""120"" title=""testheading"">
						<columnLayout>
						    <field label=""testlabel1"" property=""testpropname1"" type=""Button"" mapperType=""testmappertypename1"" />
                        </columnLayout>
                        <columnLayout>
    						<field label=""testlabel2"" property=""testpropname2"" type=""Button"" mapperType=""testmappertypename2"" />
                        </columnLayout>
                    </form>");
            //Assert.AreEqual(typeof(MyBO), col.Class);
            //Assert.AreEqual("Habanero.Test.BO.MyBO_testName", col.Name.ToString() );
            Assert.AreEqual(1, col.Count, "There should be 1 tab"); // 1 tab
            Assert.AreEqual(2, col[0].Count, "There should be one column in that tab");
            Assert.AreEqual(1, col[0][0].Count, "There should be two propertys in column 1");
            Assert.AreEqual(1, col[0][1].Count, "There should be two propertys in column 1");
            Assert.AreEqual("testlabel1", col[0][0][0].Label);
            Assert.AreEqual("testlabel2", col[0][1][0].Label);
            Assert.AreEqual(100, col.Width);
            Assert.AreEqual(120, col.Height);
            Assert.AreEqual("testheading", col.Title);
        }

        [Test]
        public void TestFormWithTabsAndNoColumns()
        {
            IUIForm col =
                loader.LoadUIFormDef(
                    @"
					<form width=""100"" height=""120"" title=""testheading"">
						<tab name=""testtab"">
							<field label=""testlabel1"" property=""testpropname1"" type=""Button"" mapperType=""testmappertypename1"" />
							<field label=""testlabel2"" property=""testpropname2"" type=""Button"" mapperType=""testmappertypename2"" />
						</tab>
						<tab name=""testtab2"">
							<field label=""testlabel3"" property=""testpropname3"" type=""Button"" mapperType=""testmappertypename3"" />
						</tab>
                    </form>");
            Assert.AreEqual(2, col.Count, "There should be 2 tab"); 
            Assert.AreEqual(1, col[0].Count, "There should be one column in tab 1");
            Assert.AreEqual(1, col[0].Count, "There should be one column in tab 2");
            Assert.AreEqual(2, col[0][0].Count, "There should be two propertys in column 1 (tab 1)");
            Assert.AreEqual(1, col[1][0].Count, "There should be two propertys in column 1 (tab 2)");
            Assert.AreEqual("testlabel1", col[0][0][0].Label);
            Assert.AreEqual("testlabel2", col[0][0][1].Label);
            Assert.AreEqual("testlabel3", col[1][0][0].Label);
            Assert.AreEqual(100, col.Width);
            Assert.AreEqual(120, col.Height);
            Assert.AreEqual("testheading", col.Title);
        }

        [Test]
        public void TestMixedElementsUnderForm()
        {
            try
            {
                loader.LoadUIFormDef(@"
				<form width=""100"" height=""120"" title=""testheading"">
					<tab name=""testtab"">
						<field label=""testlabel1"" property=""testpropname1"" type=""Button"" mapperType=""testmappertypename1"" />
					</tab>
					<field label=""testlabel3"" property=""testpropname3"" type=""Button"" mapperType=""testmappertypename3"" />
                </form>");
                Assert.Fail("Expected to throw an InvalidXmlDefinitionException");
            }
                //---------------Test Result -----------------------
            catch (InvalidXmlDefinitionException ex)
            {
                StringAssert.Contains("A form can have either a set of 'tab', 'columnLayout' or 'field' nodes, but not a mixture", ex.Message);
            }
        }

        [Test]
        public void TestMixedElementsUnderTab()
        {
            try
            {
                loader.LoadUIFormDef(@"
				<form width=""100"" height=""120"" title=""testheading"">
					<tab name=""testtab"">
						<field label=""testlabel1"" property=""testpropname1"" type=""Button"" mapperType=""testmappertypename1"" />
						<columnLayout>
						    <field label=""testlabel1"" property=""testpropname1"" type=""Button"" mapperType=""testmappertypename1"" />
                        </columnLayout>
					</tab>
                </form>");
                Assert.Fail("Expected to throw an InvalidXmlDefinitionException");
            }
                //---------------Test Result -----------------------
            catch (InvalidXmlDefinitionException ex)
            {
                StringAssert.Contains("A 'tab' can have either a set of 'columnLayout' or 'field' nodes or a single 'formGrid' node, but not a mixture", ex.Message);
            }
        }

        [Ignore("CF: Test will fail because there is no DTD validation")] //TODO Andrew Russell 05 Jan 2011: Ignored Test - CF: Test will fail because there is no DTD validation
        [Test]
        public void TestMixedElementsUnderFormColumn()
        {
            try
            {
                loader.LoadUIFormDef(@"
				<form>
					<columnLayout>
					    <field property=""testpropname1"" />
				        <tab name=""testtab"">
					        <field property=""testpropname1"" />
    					</tab>
                    </columnLayout>
                </form>");
                Assert.Fail("Expected to throw an InvalidXmlDefinitionException");
            }
                //---------------Test Result -----------------------
            catch (InvalidXmlDefinitionException ex)
            {
                StringAssert.Contains("The 'columnLayout' node does not conform to its Document Type Definition (DTD). The 'tab' element is not declared.", ex.Message);
            }
        }

        [Test]
        public void TestNoElementsUnderForm()
        {
            try
            {
                loader.LoadUIFormDef(@"<form/>");
                Assert.Fail("Expected to throw an InvalidXmlDefinitionException");
            }
                //---------------Test Result -----------------------
            catch (InvalidXmlDefinitionException ex)
            {
                StringAssert.Contains("A form can have either a set of 'tab', 'columnLayout' or 'field' nodes", ex.Message);
            }
        }

        [Test]
        public void TestInvalidWidth()
        {
            try
            {
                loader.LoadUIFormDef(@"
                <form width=""abc"">
                    <field property=""testpropname1"" />
                </form>");
                Assert.Fail("Expected to throw an InvalidXmlDefinitionException");
            }
                //---------------Test Result -----------------------
            catch (InvalidXmlDefinitionException ex)
            {
                StringAssert.Contains("In a 'form' element, either the 'width' or 'height' attribute has been given an invalid integer pixel value", ex.Message);
            }
        }

        [Test]
        public void TestInvalidHeight()
        {
            try
            {
                loader.LoadUIFormDef(@"
                <form height=""abc"">
                    <field property=""testpropname1"" />
                </form>");
                Assert.Fail("Expected to throw an InvalidXmlDefinitionException");
            }
                //---------------Test Result -----------------------
            catch (InvalidXmlDefinitionException ex)
            {
                StringAssert.Contains("In a 'form' element, either the 'width' or 'height' attribute has been given an invalid integer pixel value", ex.Message);
            }
        }

        [Test]
        public void TestUIFormSetOnUIFormTab()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IUIForm col =
loader.LoadUIFormDef(
@"
					<form width=""100"" height=""120"" title=""testheading"">
						<tab name=""testtab"">
							<field label=""testlabel1"" property=""testpropname1"" type=""Button"" mapperType=""testmappertypename1"" />
						</tab>
						<tab name=""testtab2"">
							<field label=""testlabel2"" property=""testpropname2"" type=""Button"" mapperType=""testmappertypename3"" />
						</tab>
                    </form>");
            //---------------Test Result -----------------------


            Assert.AreSame(col, col[0].UIForm);
            Assert.AreSame(col, col[1].UIForm);

        }
    }
}