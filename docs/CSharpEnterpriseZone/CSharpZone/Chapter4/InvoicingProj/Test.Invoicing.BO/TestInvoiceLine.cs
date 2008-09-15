using System.IO;
using Habanero.Base;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;
using Invoicing.BO;
using NUnit.Framework;

namespace Test.Invoicing.BO
{
    [TestFixture]
    public class TestInvoiceLine
    {
        [SetUp]
        public void SetupTest()
        {
            //Runs every time that any testmethod is executed
            ClassDef.LoadClassDefs(new XmlClassDefsLoader(new StreamReader("Classdefs.xml").ReadToEnd(), new DtdLoader()));

        }
        [Test]
        public void TestUpdateInvoiceLineProperty()
        {
            //---------------Set up test pack-------------------
            InvoiceLine invLine = new InvoiceLine();

            //---------------Assert Precondition----------------
            Assert.IsTrue(invLine.State.IsNew);
            Assert.IsFalse(invLine.State.IsValid());
            Assert.IsFalse(invLine.State.IsDeleted);
            Assert.IsFalse(invLine.State.IsDirty);
            Assert.IsFalse(invLine.State.IsEditing);

            Assert.IsTrue(invLine.State.IsValidMessage.Contains("'Invoice Line Value' is a compulsory field and has no value"));
            string inValidReason;
            Assert.IsFalse(invLine.IsValid(out inValidReason));
            Assert.IsTrue(inValidReason.Contains("'Invoice Line Value' is a compulsory field and has no value"));

            Assert.IsNull(invLine.InvoiceLineValue);
            IBOProp prop = invLine.Props["InvoiceLineValue"];
            Assert.IsFalse(prop.IsDirty);
            Assert.IsTrue(prop.IsObjectNew);
            Assert.IsFalse(prop.IsValid);

            //---------------Execute Test ----------------------
            invLine.InvoiceLineValue = 12.00m;

            //---------------Test Result -----------------------
            Assert.IsTrue(invLine.State.IsNew);
            Assert.IsFalse(invLine.State.IsValid());
            Assert.IsFalse(invLine.State.IsDeleted);
            Assert.IsTrue(invLine.State.IsDirty);
            Assert.IsTrue(invLine.State.IsEditing);
            Assert.IsFalse(invLine.State.IsValidMessage.Contains("'Invoice Line Value' is a compulsory field and has no value"));
            Assert.AreEqual(12m, invLine.InvoiceLineValue);

            Assert.IsTrue(prop.IsDirty);
            Assert.IsTrue(prop.IsObjectNew);
            Assert.IsTrue(prop.IsValid);
        }
    }
}
