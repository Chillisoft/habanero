using System;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.Test.UI.Base;
using Habanero.UI.Base;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Win.Mappers
{
    [TestFixture]
    public class TestTextBoxMapperWin : TestTextBoxMapper
    {
        protected override IControlFactory GetControlFactory()
        {
            return new ControlFactoryWin();
        }

        [Test]
        public void TestIsValidChar_ReturnsTrueIfBOPropNotSet()
        {
            //---------------Set up test pack-------------------
            TextBoxMapperStrategyWin strategy =
                (TextBoxMapperStrategyWin)GetControlFactory().CreateTextBoxMapperStrategy();

            //---------------Assert pre-condition---------------
            //---------------Execute Test ----------------------

            //---------------Test Result -----------------------
            Assert.IsTrue(strategy.IsValidCharacter('a'));
            Assert.IsTrue(strategy.IsValidCharacter(' '));
            Assert.IsTrue(strategy.IsValidCharacter('.'));
            Assert.IsTrue(strategy.IsValidCharacter('-'));
            //---------------Tear down -------------------------
        }

        [Test]
        public void TestIsValidChar_ReturnsTrueIfTextBoxNotSet()
        {
            //---------------Set up test pack-------------------
            TextBoxMapperStrategyWin strategy =
                (TextBoxMapperStrategyWin)GetControlFactory().CreateTextBoxMapperStrategy();

            //---------------Assert pre-condition---------------
            //---------------Execute Test ----------------------

            //---------------Test Result -----------------------
            Assert.IsTrue(strategy.IsValidCharacter('a'));
            Assert.IsTrue(strategy.IsValidCharacter(' '));
            Assert.IsTrue(strategy.IsValidCharacter('.'));
            Assert.IsTrue(strategy.IsValidCharacter('-'));
            //---------------Tear down -------------------------
        }

        [Test]
        public void TestIsValidChar_WithString_ReturnsTrueForNonNumericTypes()
        {
            //---------------Set up test pack-------------------
            TextBoxMapperStrategyWin strategy =
                (TextBoxMapperStrategyWin)GetControlFactory().CreateTextBoxMapperStrategy();
            BOProp boProp = CreateBOPropForType(typeof(string));
            strategy.AddKeyPressEventHandler(_mapper, boProp);
            //---------------Execute Test ----------------------

            //---------------Test Result -----------------------
            Assert.IsTrue(strategy.IsValidCharacter('a'));
            Assert.IsTrue(strategy.IsValidCharacter(' '));
            Assert.IsTrue(strategy.IsValidCharacter('.'));
            Assert.IsTrue(strategy.IsValidCharacter('-'));
            //---------------Tear down -------------------------
        }

        [Test]
        public void TestIsValidChar_WithInt_ReturnsTrueForNumber()
        {
            //---------------Set up test pack-------------------
            TextBoxMapperStrategyWin strategy =
                (TextBoxMapperStrategyWin)GetControlFactory().CreateTextBoxMapperStrategy();
            BOProp boProp = CreateBOPropForType(typeof(int));
            strategy.AddKeyPressEventHandler(_mapper, boProp);
            //---------------Execute Test ----------------------

            //---------------Test Result -----------------------
            Assert.IsTrue(strategy.IsValidCharacter('0'));
            Assert.IsTrue(strategy.IsValidCharacter('9'));
            Assert.IsTrue(strategy.IsValidCharacter('-'));
            Assert.IsTrue(strategy.IsValidCharacter(Convert.ToChar(8)));
            //---------------Tear down -------------------------
        }

        [Test]
        public void TestIsValidChar_WithInt_ReturnsFalseForNonNumber()
        {
            //---------------Set up test pack-------------------
            TextBoxMapperStrategyWin strategy =
                (TextBoxMapperStrategyWin)GetControlFactory().CreateTextBoxMapperStrategy();
            BOProp boProp = CreateBOPropForType(typeof(int));
            strategy.AddKeyPressEventHandler(_mapper, boProp);
            //---------------Execute Test ----------------------

            //---------------Test Result -----------------------
            Assert.IsFalse(strategy.IsValidCharacter('a'));
            Assert.IsFalse(strategy.IsValidCharacter('A'));
            Assert.IsFalse(strategy.IsValidCharacter('+'));
            Assert.IsFalse(strategy.IsValidCharacter('.'));
            Assert.IsFalse(strategy.IsValidCharacter(Convert.ToChar(7)));
            //---------------Tear down -------------------------
        }

        [Test]
        public void TestIsValidChar_WithInt_ReturnsTrueForNegativeAtStart()
        {
            //---------------Set up test pack-------------------
            TextBoxMapperStrategyWin strategy =
                (TextBoxMapperStrategyWin)GetControlFactory().CreateTextBoxMapperStrategy();
            BOProp boProp = CreateBOPropForType(typeof(int));
            strategy.AddKeyPressEventHandler(_mapper, boProp);
            _mapper.Control.Text = "123";
            ((TextBoxWin)_mapper.Control).SelectionStart = 0;
            //---------------Execute Test ----------------------

            //---------------Test Result -----------------------
            Assert.IsTrue(strategy.IsValidCharacter('-'));
            //---------------Tear down -------------------------
        }

        [Test]
        public void TestIsValidChar_WithInt_ReturnsFalseForNegativeAfterStart()
        {
            //---------------Set up test pack-------------------
            TextBoxMapperStrategyWin strategy =
                (TextBoxMapperStrategyWin)GetControlFactory().CreateTextBoxMapperStrategy();
            BOProp boProp = CreateBOPropForType(typeof(int));
            strategy.AddKeyPressEventHandler(_mapper, boProp);
            _mapper.Control.Text = "123";
            ((TextBoxWin)_mapper.Control).SelectionStart = 2;
            //---------------Execute Test ----------------------

            //---------------Test Result -----------------------
            Assert.IsFalse(strategy.IsValidCharacter('-'));
            //---------------Tear down -------------------------
        }

        [Test]
        public void TestIsValidChar_WithDecimal_ReturnsTrueForNumber()
        {
            //---------------Set up test pack-------------------
            TextBoxMapperStrategyWin strategy =
                (TextBoxMapperStrategyWin)GetControlFactory().CreateTextBoxMapperStrategy();
            BOProp boProp = CreateBOPropForType(typeof(decimal));
            strategy.AddKeyPressEventHandler(_mapper, boProp);
            //---------------Execute Test ----------------------

            //---------------Test Result -----------------------
            Assert.IsTrue(strategy.IsValidCharacter('0'));
            Assert.IsTrue(strategy.IsValidCharacter('9'));
            Assert.IsTrue(strategy.IsValidCharacter('-'));
            Assert.IsTrue(strategy.IsValidCharacter(Convert.ToChar(8)));
            //---------------Tear down -------------------------
        }

        [Test]
        public void TestIsValidChar_WithDecimal_ReturnsFalseForNonNumber()
        {
            //---------------Set up test pack-------------------
            TextBoxMapperStrategyWin strategy =
                (TextBoxMapperStrategyWin)GetControlFactory().CreateTextBoxMapperStrategy();
            BOProp boProp = CreateBOPropForType(typeof(decimal));
            strategy.AddKeyPressEventHandler(_mapper, boProp);
            //---------------Execute Test ----------------------

            //---------------Test Result -----------------------
            Assert.IsFalse(strategy.IsValidCharacter('a'));
            Assert.IsFalse(strategy.IsValidCharacter('A'));
            Assert.IsFalse(strategy.IsValidCharacter('+'));
            Assert.IsFalse(strategy.IsValidCharacter(Convert.ToChar(7)));
            //---------------Tear down -------------------------
        }

        [Test]
        public void TestIsValidChar_WithDecimal_ReturnsTrueForNegativeAtStart()
        {
            //---------------Set up test pack-------------------
            TextBoxMapperStrategyWin strategy =
                (TextBoxMapperStrategyWin)GetControlFactory().CreateTextBoxMapperStrategy();
            BOProp boProp = CreateBOPropForType(typeof(decimal));
            strategy.AddKeyPressEventHandler(_mapper, boProp);
            _mapper.Control.Text = "123";
            ((TextBoxWin)_mapper.Control).SelectionStart = 0;
            //---------------Execute Test ----------------------

            //---------------Test Result -----------------------
            Assert.IsTrue(strategy.IsValidCharacter('-'));
            //---------------Tear down -------------------------
        }

        [Test]
        public void TestBoProp_ChangesWhen_TextBoxTextChanges()
        {
            //---------------Set up test pack-------------------
            _mapper.BusinessObject = _shape;
            TextBoxMapperStrategyWin strategy =
                (TextBoxMapperStrategyWin)GetControlFactory().CreateTextBoxMapperStrategy();
            IBOProp boProp = _shape.Props["ShapeName"];
            strategy.AddKeyPressEventHandler(_mapper, boProp);
            strategy.AddUpdateBoPropOnTextChangedHandler(_mapper, boProp);
            _mapper.Control.Text = "TestString";
            //---------------Execute Test ----------------------

            //---------------Test Result -----------------------
            Assert.AreEqual("TestString", boProp.Value);
            //---------------Tear down -------------------------
        }

        [Test]
        public void TestIsValidChar_WithDecimal_ReturnsFalseForNegativeAfterStart()
        {
            //---------------Set up test pack-------------------
            TextBoxMapperStrategyWin strategy =
                (TextBoxMapperStrategyWin)GetControlFactory().CreateTextBoxMapperStrategy();
            BOProp boProp = CreateBOPropForType(typeof(decimal));
            strategy.AddKeyPressEventHandler(_mapper, boProp);
            _mapper.Control.Text = "123";
            ((TextBoxWin)_mapper.Control).SelectionStart = 2;
            //---------------Execute Test ----------------------

            //---------------Test Result -----------------------
            Assert.IsFalse(strategy.IsValidCharacter('-'));
            //---------------Tear down -------------------------
        }

        [Test]
        public void TestIsValidChar_WithDecimal_ReturnsTrueForDotNotAtStart()
        {
            //---------------Set up test pack-------------------
            TextBoxMapperStrategyWin strategy =
                (TextBoxMapperStrategyWin)GetControlFactory().CreateTextBoxMapperStrategy();
            BOProp boProp = CreateBOPropForType(typeof(decimal));
            strategy.AddKeyPressEventHandler(_mapper, boProp);
            _mapper.Control.Text = "123";
            ((TextBoxWin)_mapper.Control).SelectionStart = 3;
            //---------------Execute Test ----------------------

            //---------------Test Result -----------------------
            Assert.IsTrue(strategy.IsValidCharacter('.'));
            //---------------Tear down -------------------------
        }

        [Test]
        public void TestIsValidChar_WithDecimal_ReturnsFalseForMultipleDots()
        {
            //---------------Set up test pack-------------------
            TextBoxMapperStrategyWin strategy =
                (TextBoxMapperStrategyWin)GetControlFactory().CreateTextBoxMapperStrategy();
            BOProp boProp = CreateBOPropForType(typeof(decimal));
            strategy.AddKeyPressEventHandler(_mapper, boProp);
            _mapper.Control.Text = "12.3";
            //---------------Execute Test ----------------------

            //---------------Test Result -----------------------
            Assert.IsFalse(strategy.IsValidCharacter('.'));
            //---------------Tear down -------------------------
        }

        [Test]
        public void TestIsValidChar_WithDecimal_AddsZeroForDotAtStart()
        {
            //---------------Set up test pack-------------------
            TextBoxMapperStrategyWin strategy =
                (TextBoxMapperStrategyWin)GetControlFactory().CreateTextBoxMapperStrategy();
            BOProp boProp = CreateBOPropForType(typeof(decimal));
            strategy.AddKeyPressEventHandler(_mapper, boProp);
            _mapper.Control.Text = "";
            TextBoxWin textBox = ((TextBoxWin)_mapper.Control);
            textBox.SelectionStart = 0;
            //---------------Execute Test ----------------------

            //---------------Test Result -----------------------
            Assert.IsFalse(strategy.IsValidCharacter('.'));
            Assert.AreEqual("0.", textBox.Text);
            Assert.AreEqual(2, textBox.SelectionStart);
            Assert.AreEqual(0, textBox.SelectionLength);
            //---------------Tear down -------------------------
        }

        [Test]
        public void Test_TextBoxHasMapperStrategy()
        {
            //---------------Test Result -----------------------
            Assert.IsNotNull(_mapper.TextBoxMapperStrategy);
        }

        [Test]
        public void Test_CorrectTextBoxMapperStrategy()
        {
            //---------------Execute Test ----------------------
            ITextBoxMapperStrategy strategy = _mapper.TextBoxMapperStrategy;
            //---------------Test Result -----------------------
            Assert.AreSame(typeof(TextBoxMapperStrategyWin), strategy.GetType());
            //---------------Tear down -------------------------
        }

        [Test]
        public void Test_MapperStrategy_Returns_Correct_BoProp_WhenChangingBOs()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            MyBO.LoadClassDefWithIntegerRule();
            MyBO myBo = new MyBO();
            _mapper = new TextBoxMapper(_textBox, "TestProp2", false, GetControlFactory());
            _mapper.BusinessObject = myBo;
            _textBox.Name = "TestTextBox";
            //---------------Assert pre-conditions--------------
            Assert.AreEqual(_mapper.CurrentBOProp(), ((TextBoxMapperStrategyWin)_mapper.TextBoxMapperStrategy).BoProp);
            Assert.AreEqual(_mapper.Control, ((TextBoxMapperStrategyWin)_mapper.TextBoxMapperStrategy).TextBoxControl);
            //---------------Execute Test ----------------------
            ClassDef.ClassDefs.Clear();
            MyBO.LoadDefaultClassDef();
            MyBO myNewBo = new MyBO();
            _mapper.BusinessObject = myNewBo;
            //---------------Test Result -----------------------
            Assert.AreEqual(_mapper.CurrentBOProp(), ((TextBoxMapperStrategyWin)_mapper.TextBoxMapperStrategy).BoProp);
            //---------------Tear down -------------------------
        }

        private static BOProp CreateBOPropForType(Type type)
        {
            PropDef propDef = new PropDef("Prop", type, PropReadWriteRule.ReadWrite, null);
            return new BOProp(propDef);
        }


    }
}