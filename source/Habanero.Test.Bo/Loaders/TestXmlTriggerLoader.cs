// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2010 Chillisoft Solutions
//  
//  This file is part of the Habanero framework.
//  
//      Habanero is a free framework: you can redistribute it and/or modify
//      it under the terms of the GNU Lesser General Public License as published by
//      the Free Software Foundation, either version 3 of the License, or
//      (at your option) any later version.
//  
//      The Habanero framework is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//      GNU Lesser General Public License for more details.
//  
//      You should have received a copy of the GNU Lesser General Public License
//      along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
// ---------------------------------------------------------------------------------
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;
using NUnit.Framework;

namespace Habanero.Test.BO.Loaders
{
#pragma warning disable 612,618

    [Ignore("Triggers are not included in Habanero at the moment")]
    [TestFixture]
    public class TestXmlTriggerLoader
    {
        private XmlTriggerLoader loader;

        [SetUp]
        public virtual void SetupTest()
        {
            loader = new XmlTriggerLoader(new DtdLoader(), GetDefClassFactory());
        }

        protected virtual IDefClassFactory GetDefClassFactory()
        {
            return new DefClassFactory();
        }

        [Test]
        public void TestDefaults()
        {
            ITrigger trigger =
                loader.LoadTrigger(@"<trigger action=""action"" value=""value"" />");
            Assert.IsNull(trigger.TriggeredBy);
            Assert.IsNull(trigger.Target);
            Assert.IsNull(trigger.ConditionValue);
            Assert.AreEqual("action", trigger.Action);
            Assert.AreEqual("value", trigger.Value);
        }

        [Test]
        public void TestTriggerWithTriggeredBy()
        {
            ITrigger trigger =
                loader.LoadTrigger(
                    @"<trigger triggeredBy=""prop"" conditionValue=""value"" action=""action"" value=""value"" />");
            Assert.AreEqual("prop", trigger.TriggeredBy);
            Assert.IsNull(trigger.Target);
            Assert.AreEqual("value", trigger.ConditionValue);
            Assert.AreEqual("action", trigger.Action);
            Assert.AreEqual("value", trigger.Value);
        }

        [Test]
        public void TestTriggerWithTarget()
        {
            ITrigger trigger =
                loader.LoadTrigger(
                    @"<trigger target=""prop"" conditionValue=""value"" action=""action"" value=""value"" />");
            Assert.IsNull(trigger.TriggeredBy); 
            Assert.AreEqual("prop", trigger.Target);
            Assert.AreEqual("value", trigger.ConditionValue);
            Assert.AreEqual("action", trigger.Action);
            Assert.AreEqual("value", trigger.Value);
        }

        [Test]
        public void TestCantSetBothTriggeredByAndTarget()
        {
            try
            {
                
                    loader.LoadTrigger(
                        @"<trigger triggeredBy=""prop"" target=""prop"" action=""action"" value=""value"" />");
                Assert.Fail("Expected to throw an InvalidXmlDefinitionException");
            }
                //---------------Test Result -----------------------
            catch (InvalidXmlDefinitionException ex)
            {
                StringAssert.Contains("MESSAGE", ex.Message);
            }
        }
    }
#pragma warning restore 612,618
}