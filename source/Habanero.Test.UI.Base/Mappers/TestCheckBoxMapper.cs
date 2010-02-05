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

using Habanero.UI.Base;

using NUnit.Extensions.Forms;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    /// <summary>
    /// Summary description for TestCheckBoxMapper.
    /// </summary>
    public abstract class TestCheckBoxMapper : TestMapperBase
    {
        protected abstract IControlFactory GetControlFactory();


        protected ICheckBox _cb;
        protected CheckBoxMapper _mapper;
        protected Sample _sampleBusinessObject;

        [SetUp]
        public void SetupTest()
        {
            _cb = GetControlFactory().CreateCheckBox();
            _mapper = new CheckBoxMapper(_cb, "SampleBoolean", false, GetControlFactory());
            _sampleBusinessObject = new Sample();
        }


        [Test]
        public void TestConstructor()
        {
            Assert.AreSame(_cb, _mapper.Control);
            Assert.AreSame("SampleBoolean", _mapper.PropertyName);
        }


        [Test]
        public void TestDisplayingRelatedProperty()
        {
            SetupClassDefs(true);
            _cb = GetControlFactory().CreateCheckBox();
            _mapper = new CheckBoxMapper(_cb, "MyRelationship.MyRelatedTestProp", true, GetControlFactory());
            _mapper.BusinessObject = itsMyBo;
            Assert.IsNotNull(_mapper.BusinessObject);
            Assert.AreEqual(true, _cb.Checked);
        }
    }
}