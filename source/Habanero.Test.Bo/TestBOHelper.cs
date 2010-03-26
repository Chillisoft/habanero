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
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Habanero.Base;
using Habanero.BO;
using Habanero.Test.BO.ClassDefinition;
using NUnit.Framework;
using Rhino.Mocks;

namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestBOHelper
    {
        [Test]
        public void Test_GetBusinessObjectRules_WhenNull_ShouldRaiseError()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            try
            {
                BOHelper.GetBusinessObjectRules(null);
                Assert.Fail("expected ArgumentNullException");
            }
            //---------------Test Result -----------------------
            catch (ArgumentNullException ex)
            {
                StringAssert.Contains("Value cannot be null", ex.Message);
                StringAssert.Contains("businessObject", ex.ParamName);
            }
        }

        [Test]
        public void Test_GetBusinessObjectRules_WhenNoRules_ShouldReturnEmptyCol()
        {
            //---------------Set up test pack-------------------
            MyRulesBo myRulesBo = new MyRulesBo();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            ReadOnlyCollection<IBusinessObjectRule> rules = BOHelper.GetBusinessObjectRules(myRulesBo);
            //---------------Test Result -----------------------
            Assert.IsNotNull(rules);
            Assert.AreEqual(0, rules.Count);
        }

        [Test]
        public void Test_GetBusinessObjectRules_WhenHasRules_ShouldReturnCorrectCol()
        {
            //---------------Set up test pack-------------------
            IBusinessObjectRule rule1 = MockRepository.GenerateStub<IBusinessObjectRule>();
            IBusinessObjectRule rule2 = MockRepository.GenerateStub<IBusinessObjectRule>();
            IBusinessObjectRule rule3 = MockRepository.GenerateStub<IBusinessObjectRule>();
            IBusinessObjectRule rule4 = MockRepository.GenerateStub<IBusinessObjectRule>();
            MyRulesBo myRulesBo = new MyRulesBo(rule1, rule2, rule3, rule4);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            ReadOnlyCollection<IBusinessObjectRule> rules = BOHelper.GetBusinessObjectRules(myRulesBo);
            //---------------Test Result -----------------------
            Assert.IsNotNull(rules);
            Assert.AreEqual(myRulesBo.Rules.Length, rules.Count);
            Assert.Contains(rule1, rules);
            Assert.Contains(rule2, rules);
            Assert.Contains(rule3, rules);
            Assert.Contains(rule4, rules);
        }

        private class MyRulesBo : MockBO
        {
            internal IBusinessObjectRule[] Rules{ get; private set;}

            public MyRulesBo(params IBusinessObjectRule[] rules)
            {
                Rules = rules;
            }

            protected override void LoadBusinessObjectRules(IList<IBusinessObjectRule> boRules)
            {
                foreach (IBusinessObjectRule rule in Rules)
                {
                    boRules.Add(rule);
                }
            }
        }
    }
}