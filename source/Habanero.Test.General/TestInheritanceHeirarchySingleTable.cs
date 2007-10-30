//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
// 
// This file is part of Habanero Standard.
// 
//     Habanero Standard is free software: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     Habanero Standard is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with Habanero Standard.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using System.Data;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;
using Habanero.DB;
using Habanero.Util;
using Habanero.BO;
using NUnit.Framework;

namespace Habanero.Test.General
{
    /// <summary>
    /// Summary description for TestInheritanceHeirarchySingleTable.
    /// </summary>
    [TestFixture]
    public class TestInheritanceHeirarchySingleTable : TestInheritanceHeirarchyBase
    {
        public TestInheritanceHeirarchySingleTable()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        protected override void SetupInheritanceSpecifics()
        {
            Circle.GetClassDef().SuperClassDef =
                new SuperClassDef(Shape.GetClassDef(), ORMapping.SingleTableInheritance);
            FilledCircle.GetClassDef().SuperClassDef =
                new SuperClassDef(Circle.GetClassDef(), ORMapping.SingleTableInheritance);
        }

        protected override void SetStrID()
        {
            itsFilledCircleId = (string) DatabaseUtil.PrepareValue(itsFilledCircle.GetPropertyValue("ShapeID"));
        }

        [Test]
        public void TestFilledCircleIsUsingSingleTableInheritance()
        {
            Assert.AreEqual(ORMapping.SingleTableInheritance, Circle.GetClassDef().SuperClassDef.ORMapping);
            Assert.AreEqual(ORMapping.SingleTableInheritance, FilledCircle.GetClassDef().SuperClassDef.ORMapping);
        }

        [Test]
        public void TestFilledCircleHasShapeIDAsPrimaryKey()
        {
            try
            {
                itsFilledCircle.ID.Contains("ShapeID");
            }
            catch (HabaneroArgumentException)
            {
                Assert.Fail("An object using SingleTableInheritance should receive its superclass as its primary key.");
            }
        }

        [Test]
        public void TestFilledCircleHasCorrectPropertyNames()
        {
            itsFilledCircle.GetPropertyValue("ShapeName");
            itsFilledCircle.GetPropertyValue("Radius");
            itsFilledCircle.GetPropertyValue("ShapeID");
            itsFilledCircle.GetPropertyValue("Colour");
        }

        [Test, ExpectedException(typeof (InvalidPropertyNameException))]
        public void TestFilledCircleDoesntHaveCircleID()
        {
            itsFilledCircle.GetPropertyValue("CircleID");
            itsFilledCircle.GetPropertyValue("FilledCircleID");
        }


        [Test]
        public void TestCircleSelectSql()
        {
            Assert.AreEqual("SELECT Shape.Colour, Shape.Radius, Shape.ShapeID, Shape.ShapeName FROM Shape",
                            itsSelectSql.Statement.ToString().Substring(0, 76),
                            "select statement is incorrect for Single Table inheritance");
        }

        [Test]
        public void TestCircleInsertSql()
        {
            Assert.AreEqual(1, itsInsertSql.Count,
                            "There should only be one insert Sql statement when using Single Table Inheritance.");
            Assert.AreEqual(
                "INSERT INTO Shape (Colour, Radius, ShapeID, ShapeName) VALUES (?Param0, ?Param1, ?Param2, ?Param3)",
                itsInsertSql[0].Statement.ToString(), "Concrete Table Inheritance insert Sql seems to be incorrect.");
            Assert.AreEqual(3, ((IDbDataParameter) itsInsertSql[0].Parameters[0]).Value,
                            "Parameter Colour has incorrect value");
            Assert.AreEqual("MyFilledCircle", ((IDbDataParameter) itsInsertSql[0].Parameters[3]).Value,
                            "Parameter ShapeName has incorrect value");
            Assert.AreEqual(itsFilledCircleId, ((IDbDataParameter) itsInsertSql[0].Parameters[2]).Value,
                            "Parameter ShapeID has incorrect value");
            Assert.AreEqual(10, ((IDbDataParameter) itsInsertSql[0].Parameters[1]).Value,
                            "Parameter Radius has incorrect value");
        }

        [Test]
        public void TestCircleUpdateSql()
        {
            Assert.AreEqual(1, itsUpdateSql.Count,
                            "There should only be one update sql statement when using single table inheritance.");
            Assert.AreEqual(
                "UPDATE Shape SET Colour = ?Param0, Radius = ?Param1, ShapeID = ?Param2, ShapeName = ?Param3 WHERE ShapeID = ?Param4",
                itsUpdateSql[0].Statement.ToString());
            Assert.AreEqual(3, ((IDbDataParameter) itsUpdateSql[0].Parameters[0]).Value,
                            "Parameter Colour has incorrect value");
            Assert.AreEqual("MyFilledCircle", ((IDbDataParameter) itsUpdateSql[0].Parameters[3]).Value,
                            "Parameter ShapeName has incorrect value");
            Assert.AreEqual(itsFilledCircleId, ((IDbDataParameter) itsUpdateSql[0].Parameters[2]).Value,
                            "Parameter ShapeID has incorrect value");
            Assert.AreEqual(10, ((IDbDataParameter) itsUpdateSql[0].Parameters[1]).Value,
                            "Parameter Radius has incorrect value");
            Assert.AreEqual(itsFilledCircleId, ((IDbDataParameter) itsUpdateSql[0].Parameters[4]).Value,
                            "Parameter ShapeID has incorrect value");
        }

        [Test]
        public void TestCircleDeleteSql()
        {
            Assert.AreEqual(1, itsDeleteSql.Count,
                            "There should only be one delete sql statement when using single table inheritance.");
            Assert.AreEqual("DELETE FROM Shape WHERE ShapeID = ?Param0", itsDeleteSql[0].Statement.ToString(),
                            "Delete Sql for single table inheritance is incorrect.");
            Assert.AreEqual(itsFilledCircleId, ((IDbDataParameter) itsDeleteSql[0].Parameters[0]).Value,
                            "Parameter ShapeID has incorrect value for delete sql when using Single Table inheritance.");
        }
    }
}
