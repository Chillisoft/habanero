using System;
using System.Collections;
using System.IO;
using NUnit.Framework;
using Habanero.Util;

namespace Habanero.Test
{
    [TestFixture]
    public class TestSortedStringCollection
    {
        [Test]
        public void TestSorting()
        {
            SortedStringCollection col = new SortedStringCollection();

            Assert.AreEqual(col.Count, 0);
            Assert.IsFalse(col.IsSynchronized);
            Assert.AreEqual(new ArrayList().GetEnumerator().GetType(), col.GetEnumerator().GetType());
            Assert.AreEqual("System.Object", col.SyncRoot.ToString());

            col.Add("c");
            Assert.AreEqual(col.Count, 1);
            Assert.AreEqual(col[0], "c");

            col.Add("a");
            Assert.AreEqual(col.Count, 2);
            Assert.AreEqual(col[0], "a");
            Assert.AreEqual(col[1], "c");

            col.Add("b");
            Assert.AreEqual(col.Count, 3);
            Assert.AreEqual(col[0], "a");
            Assert.AreEqual(col[1], "b");
            Assert.AreEqual(col[2], "c");

            col.Add("d");
            Assert.AreEqual(col.Count, 4);
            Assert.AreEqual(col[0], "a");
            Assert.AreEqual(col[1], "b");
            Assert.AreEqual(col[2], "c");
            Assert.AreEqual(col[3], "d");

            col.Add("b");
            Assert.AreEqual(col.Count, 5);
            Assert.AreEqual(col[0], "a");
            Assert.AreEqual(col[1], "b");
            Assert.AreEqual(col[2], "b");
            Assert.AreEqual(col[3], "c");
            Assert.AreEqual(col[4], "d");

            string[] copy = new string[7];
            col.CopyTo(copy, 1);
            Assert.AreEqual(null, copy[0]);
            Assert.AreEqual(col[0], copy[1]);
            Assert.AreEqual(col[1], copy[2]);
            Assert.AreEqual(col[2], copy[3]);
            Assert.AreEqual(col[3], copy[4]);
            Assert.AreEqual(col[4], copy[5]);
            Assert.AreEqual(null, copy[6]);
        }
    }
}
