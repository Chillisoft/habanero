//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
// 
// This file is part of Habanero Standard.
// 
//     Habanero Standard is free software: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     Habanero Standard is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with Habanero Standard.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO.ClassDefinition
{
    [TestFixture]
    public class TestTrigger
    {
        [Test]
        public void TestConstructors()
        {
            Trigger trigger = new Trigger(
                "triggeredby", null, "condition", "action", "value");

            Assert.AreEqual("triggeredby", trigger.TriggeredBy);
            Assert.IsNull(trigger.Target);
            Assert.AreEqual("condition", trigger.ConditionValue);
            Assert.AreEqual("action", trigger.Action);
            Assert.AreEqual("value", trigger.Value);

            trigger = new Trigger(
                null, "target", "condition", "action", "value");

            Assert.IsNull(trigger.TriggeredBy);
            Assert.AreEqual("target", trigger.Target);
        }

        [Test]
        public void TestPropertySets()
        {
            Trigger trigger = new Trigger(
                "triggeredby", null, "condition", "action", "value");

            trigger.TriggeredBy = null;
            Assert.IsNull(trigger.TriggeredBy);

            trigger.Target = "sometarget";
            Assert.AreEqual("sometarget", trigger.Target);

            trigger.ConditionValue = "true";
            Assert.AreEqual("true", trigger.ConditionValue);

            trigger.Action = "newaction";
            Assert.AreEqual("newaction", trigger.Action);

            trigger.Value = "newvalue";
            Assert.AreEqual("newvalue", trigger.Value);
        }

        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestConstructorSetsTargetAndTriggeredBy()
        {
            Trigger trigger = new Trigger(
                "triggeredby", "target", "condition", "action", "value");
        }

        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestSetTargetWhenTriggeredByExists()
        {
            Trigger trigger = new Trigger(
                "triggeredby", null, "condition", "action", "value");
            trigger.Target = "someprop";
        }

        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestSetTriggeredByWhenTargetExists()
        {
            Trigger trigger = new Trigger(
                null, "target", "condition", "action", "value");
            trigger.TriggeredBy = "someprop";
        }

        [Test]
        public void TestCheckTriggerValid()
        {
            //action names valid
            Assert.IsTrue(Trigger.CheckTriggerValid(new Trigger(null, "prop", null, "assignLiteral", "1")));
            Assert.IsTrue(Trigger.CheckTriggerValid(new Trigger(null, "prop", null, "assignProperty", "1")));
            Assert.IsTrue(Trigger.CheckTriggerValid(new Trigger(null, null, null, "execute", "1")));
            Assert.IsTrue(Trigger.CheckTriggerValid(new Trigger(null, "prop", null, "filter", "1")));
            Assert.IsTrue(Trigger.CheckTriggerValid(new Trigger(null, "prop", null, "filterReverse", "1")));
            Assert.IsTrue(Trigger.CheckTriggerValid(new Trigger(null, "prop", null, "setEditable", "1")));
            Assert.IsTrue(Trigger.CheckTriggerValid(new Trigger(null, "prop", null, "setEditableOnce", "1")));

            //no trig/target given
            Assert.IsTrue(Trigger.CheckTriggerValid(new Trigger(null, null, null, "execute", "1")));
            Assert.IsTrue(Trigger.CheckTriggerValid(new Trigger(null, null, null, "setEditable", "1")));
            Assert.IsTrue(Trigger.CheckTriggerValid(new Trigger(null, null, null, "setEditableOnce", "1")));
            Assert.IsTrue(Trigger.CheckTriggerValid(new Trigger(null, null, null, "assignLiteral", "1")));
            Assert.IsTrue(Trigger.CheckTriggerValid(new Trigger(null, null, null, "assignProperty", "1")));

            //no value given
            Assert.IsTrue(Trigger.CheckTriggerValid(new Trigger(null, "prop", null, "filter", null)));
            Assert.IsTrue(Trigger.CheckTriggerValid(new Trigger(null, "prop", null, "filterReverse", null)));
        }

        [Test]
        public void TestCheckTriggerInvalid()
        {
            TriggerCol col = new TriggerCol();
            
            //action name invalid
            col.Add(new Trigger(null, "prop", null, "invalidaction", "1"));
            
            //no trig/target given
            col.Add(new Trigger(null, null, null, "filter", "1"));
            col.Add(new Trigger(null, null, null, "filterReverse", "1"));

            //no value
            col.Add(new Trigger(null, null, null, "execute", null));
            col.Add(new Trigger(null, null, null, "execute", ""));
            col.Add(new Trigger(null, null, null, "setEditable", null));
            col.Add(new Trigger(null, null, null, "setEditableOnce", null));
            col.Add(new Trigger(null, null, null, "assignLiteral", null));
            col.Add(new Trigger(null, null, null, "assignProperty", null));

            foreach (Trigger trigger in col)
            {
                bool valid = true;
                try
                {
                    Trigger.CheckTriggerValid(trigger);
                }
                catch
                {
                    valid = false;
                }
                Assert.IsFalse(valid);
            }
        }
    }
}