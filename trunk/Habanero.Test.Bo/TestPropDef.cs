using System;
using Habanero.Bo.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.Bo
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
            PropDef def = new PropDef("This.That", typeof (string), PropReadWriteRule.ReadWrite, "");
        }


        [
            Test,
                ExpectedException(typeof (ArgumentException),
                    "A property name cannot contain any of the following characters: [.-|]  Invalid property name This-That"
                    )]
        public void TestDashIsNotAllowedInName()
        {
            PropDef def = new PropDef("This-That", typeof (string), PropReadWriteRule.ReadWrite, "");
        }

        [
            Test,
                ExpectedException(typeof (ArgumentException),
                    "A property name cannot contain any of the following characters: [.-|]  Invalid property name This|That"
                    )]
        public void TestPipeIsNotAllowedInName()
        {
            PropDef def = new PropDef("This|That", typeof (string), PropReadWriteRule.ReadWrite, "");

           
        }
    }
}