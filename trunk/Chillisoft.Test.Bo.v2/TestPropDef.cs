using System;
using Chillisoft.Bo.ClassDefinition.v2;
using NUnit.Framework;

namespace Chillisoft.Test.Bo.v2
{
    /// <summary>
    /// Summary description for TestPropertyDef.
    /// </summary>
    [TestFixture]
    public class TestPropDef
    {
        [
            Test,
                ExpectedException(typeof (ArgumentException),
                    "A property name cannot contain any of the following characters: [.-|]  Invalid property name This.That"
                    )]
        public void TestDotIsNotAllowedInName()
        {
            PropDef def = new PropDef("This.That", typeof (string), cbsPropReadWriteRule.ReadManyWriteMany, "");
        }


        [
            Test,
                ExpectedException(typeof (ArgumentException),
                    "A property name cannot contain any of the following characters: [.-|]  Invalid property name This-That"
                    )]
        public void TestDashIsNotAllowedInName()
        {
            PropDef def = new PropDef("This-That", typeof (string), cbsPropReadWriteRule.ReadManyWriteMany, "");
        }

        [
            Test,
                ExpectedException(typeof (ArgumentException),
                    "A property name cannot contain any of the following characters: [.-|]  Invalid property name This|That"
                    )]
        public void TestPipeIsNotAllowedInName()
        {
            PropDef def = new PropDef("This|That", typeof (string), cbsPropReadWriteRule.ReadManyWriteMany, "");
        }
    }
}