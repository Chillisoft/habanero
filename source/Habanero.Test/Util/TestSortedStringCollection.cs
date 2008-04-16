//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
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

using System.Collections;
using Habanero.Util;
using NUnit.Framework;

namespace Habanero.Test.Util
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

            col.Add("e");
            Assert.AreEqual(col.Count, 4);
            Assert.AreEqual(col[0], "a");
            Assert.AreEqual(col[1], "b");
            Assert.AreEqual(col[2], "c");
            Assert.AreEqual(col[3], "e");

            col.Add("b");
            Assert.AreEqual(col.Count, 5);
            Assert.AreEqual(col[0], "a");
            Assert.AreEqual(col[1], "b");
            Assert.AreEqual(col[2], "b");
            Assert.AreEqual(col[3], "c");
            Assert.AreEqual(col[4], "e");

            col.Add("d");
            Assert.AreEqual(col.Count, 6);
            Assert.AreEqual(col[0], "a");
            Assert.AreEqual(col[1], "b");
            Assert.AreEqual(col[2], "b");
            Assert.AreEqual(col[3], "c");
            Assert.AreEqual(col[4], "d");
            Assert.AreEqual(col[5], "e");

            string[] exactCopy = new string[8];
            col.CopyTo(exactCopy, 0);
            Assert.AreEqual(col[0], exactCopy[0]);
            Assert.AreEqual(col[1], exactCopy[1]);
            Assert.AreEqual(col[2], exactCopy[2]);
            Assert.AreEqual(col[3], exactCopy[3]);
            Assert.AreEqual(col[4], exactCopy[4]);
            Assert.AreEqual(col[5], exactCopy[5]);
            Assert.AreEqual(null, exactCopy[6]);
            Assert.AreEqual(null, exactCopy[7]);

            string[] offsetCopy = new string[8];
            col.CopyTo(offsetCopy, 1);
            Assert.AreEqual(null, offsetCopy[0]);
            Assert.AreEqual(col[0], offsetCopy[1]);
            Assert.AreEqual(col[1], offsetCopy[2]);
            Assert.AreEqual(col[2], offsetCopy[3]);
            Assert.AreEqual(col[3], offsetCopy[4]);
            Assert.AreEqual(col[4], offsetCopy[5]);
            Assert.AreEqual(col[5], offsetCopy[6]);
            Assert.AreEqual(null, offsetCopy[7]);
        }
    }
}
