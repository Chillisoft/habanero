//---------------------------------------------------------------------------------
// Copyright (C) 2008 Chillisoft Solutions
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

using System.Windows.Forms;
using Habanero.UI.Forms;
using NUnit.Framework;

namespace Habanero.Test.UI.Forms
{
    /// <summary>
    /// Summary description for TestCheckBoxMapper.
    /// </summary>
    [TestFixture]
    public class TestCheckBoxMapper : TestMapperBase
    {
        private CheckBox cb;
        private CheckBoxMapper mapper;
        private Sample s;


        [SetUp]
        public void SetupTest()
        {
            cb = new CheckBox();
            mapper = new CheckBoxMapper(cb, "SampleBoolean", false);
            s = new Sample();
        }


        [Test]
        public void TestConstructor()
        {
            Assert.AreSame(cb, mapper.Control);
            Assert.AreSame("SampleBoolean", mapper.PropertyName);
        }

        [Test]
        public void TestCheckBoxValue()
        {
            s.SampleBoolean = false;
            mapper.BusinessObject = s;
            Assert.IsFalse(cb.Checked);
            s.SampleBoolean = true;
            Assert.IsTrue(cb.Checked);
        }

        [Test]
        public void TestSettingCheckBoxCheckedUpdatesBO()
        {
            s.SampleBoolean = false;
            mapper.BusinessObject = s;
            cb.Checked = true;
            Assert.IsTrue(s.SampleBoolean);
        }

        [Test]
        public void TestDisplayingRelatedProperty()
        {
            SetupClassDefs(true);
            cb = new CheckBox();
            mapper = new CheckBoxMapper(cb, "MyRelationship.MyRelatedTestProp", true);
            mapper.BusinessObject = itsMyBo;
            Assert.IsNotNull(mapper.BusinessObject);
            Assert.AreEqual(true, cb.Checked);
        }
    }
}