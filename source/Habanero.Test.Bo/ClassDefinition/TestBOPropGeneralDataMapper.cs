using System;
using System.Collections.Generic;
using Habanero.Base;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;
using Rhino.Mocks;

namespace Habanero.Test.BO.ClassDefinition
{
    [TestFixture]
    public class TestBOPropGeneralDataMapper
    {
        //TODO andrew 03 Jan 2011: CF: Test relies on EmailAddressWithTypeConverter, TypeConverter is not supported in CF
        //[Test]
        //public void Test_TryParseProp_ForCustomTypeWithATypeConverter_WithString_ShouldParseToCustomType()
        //{
        //    //---------------Set up test pack-------------------
        //    const string emailAddToParse = "xxxx.yyyy@ccc.aa.zz";
        //    var propDef = new PropDef("Name", typeof(EmailAddressWithTypeConverter), PropReadWriteRule.ReadWrite, null);

        //    var propMapper = new BOPropGeneralDataMapper(propDef);
        //    //---------------Assert Precondition----------------
        //    //---------------Execute Test ----------------------
        //    object parsedValue;
        //    var tryParsePropValue = propMapper.TryParsePropValue(emailAddToParse, out parsedValue);
        //    //---------------Test Result -----------------------
        //    Assert.IsTrue(tryParsePropValue);
        //    Assert.IsInstanceOf<EmailAddressWithTypeConverter>(parsedValue);
        //    Assert.AreEqual(emailAddToParse, parsedValue.ToString());
        //}
    }
}