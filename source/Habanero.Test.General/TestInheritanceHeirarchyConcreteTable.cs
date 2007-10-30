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
using Habanero.BO.ClassDefinition;
using Habanero.DB;
using NUnit.Framework;

namespace Habanero.Test.General
{
    /// <summary>
    /// Summary description for TestInheritanceHeirarchyConcreteTable.
    /// </summary>
    [TestFixture]
    public class TestInheritanceHeirarchyConcreteTable : TestInheritanceHeirarchyBase
    {
        protected override void SetupInheritanceSpecifics()
        {
            Circle.GetClassDef().SuperClassDef =
                new SuperClassDef(Shape.GetClassDef(), ORMapping.ConcreteTableInheritance);
            FilledCircle.GetClassDef().SuperClassDef =
                new SuperClassDef(Circle.GetClassDef(), ORMapping.ConcreteTableInheritance);
        }

        protected override void SetStrID()
        {
            itsFilledCircleId = (string) DatabaseUtil.PrepareValue(itsFilledCircle.GetPropertyValue("FilledCircleID"));
        }

        [Test]
        public void TestCircleIsUsingConcreteTableInheritance()
        {
            Assert.AreEqual(ORMapping.ConcreteTableInheritance, Circle.GetClassDef().SuperClassDef.ORMapping);
            Assert.AreEqual(ORMapping.ConcreteTableInheritance, FilledCircle.GetClassDef().SuperClassDef.ORMapping);
        }

        [Test]
        public void TestObjCircleHasCorrectProperties()
        {
            itsFilledCircle.GetPropertyValue("ShapeName");
            itsFilledCircle.GetPropertyValue("FilledCircleID");
            itsFilledCircle.GetPropertyValue("Radius");
            itsFilledCircle.GetPropertyValue("Colour");
        }


        [Test]
        public void TestCircleSelectSql()
        {
            Assert.AreEqual(
                "SELECT FilledCircle.Colour, FilledCircle.FilledCircleID, FilledCircle.Radius, FilledCircle.ShapeName FROM FilledCircle WHERE FilledCircleID = ?Param0",
                itsSelectSql.Statement.ToString(), "select statement is incorrect for Concrete Table inheritance");
        }

        [Test]
        public void TestCircleInsertSql()
        {
            Assert.AreEqual(1, itsInsertSql.Count,
                            "There should only be one insert statement for concrete table inheritance.");
            Assert.AreEqual(
                "INSERT INTO FilledCircle (Colour, FilledCircleID, Radius, ShapeName) VALUES (?Param0, ?Param1, ?Param2, ?Param3)",
                itsInsertSql[0].Statement.ToString(), "Concrete Table Inheritance insert Sql seems to be incorrect.");
            Assert.AreEqual(itsFilledCircleId, ((IDbDataParameter) itsInsertSql[0].Parameters[1]).Value,
                            "Parameter FilledCircleID has incorrect value");
            Assert.AreEqual(3, ((IDbDataParameter) itsInsertSql[0].Parameters[0]).Value,
                            "Parameter Colour has incorrect value");
            Assert.AreEqual("MyFilledCircle", ((IDbDataParameter) itsInsertSql[0].Parameters[3]).Value,
                            "Parameter ShapeName has incorrect value");
            Assert.AreEqual(10, ((IDbDataParameter) itsInsertSql[0].Parameters[2]).Value,
                            "Parameter Radius has incorrect value");
        }

        [Test]
        public void TestCircleUpdateSql()
        {
            Assert.AreEqual(1, itsUpdateSql.Count,
                            "There should only be one update statement for concrete table inheritance.");
            Assert.AreEqual(
                "UPDATE FilledCircle SET Colour = ?Param0, Radius = ?Param1, ShapeName = ?Param2 WHERE FilledCircleID = ?Param3",
                itsUpdateSql[0].Statement.ToString(), "Concrete Table Inheritance update Sql seems to be incorrect.");
            Assert.AreEqual(3, ((IDbDataParameter) itsUpdateSql[0].Parameters[0]).Value,
                            "Parameter Colour has incorrect value");
            Assert.AreEqual("MyFilledCircle", ((IDbDataParameter) itsUpdateSql[0].Parameters[2]).Value,
                            "Parameter ShapeName has incorrect value");
            Assert.AreEqual(10, ((IDbDataParameter) itsUpdateSql[0].Parameters[1]).Value,
                            "Parameter Radius has incorrect value");
            Assert.AreEqual(itsFilledCircleId, ((IDbDataParameter) itsUpdateSql[0].Parameters[3]).Value,
                            "Parameter ShapeID has incorrect value");
        }

        [Test]
        public void TestCircleDeleteSql()
        {
            Assert.AreEqual(1, itsDeleteSql.Count,
                            "There should only be one delete statement for concrete table inheritance.");
            Assert.AreEqual("DELETE FROM FilledCircle WHERE FilledCircleID = ?Param0",
                            itsDeleteSql[0].Statement.ToString(),
                            "Concrete Table Inheritance delete Sql seems to be incorrect.");
            Assert.AreEqual(itsFilledCircleId, ((IDbDataParameter) itsDeleteSql[0].Parameters[0]).Value,
                            "Parameter FilledCircleID has incorrect value in Delete Sql statement for concrete table inheritance.");
        }

        [Test]
        public void TestSelectSql()
        {
            Assert.AreEqual(
                "SELECT FilledCircle.Colour, FilledCircle.FilledCircleID, FilledCircle.Radius, FilledCircle.ShapeName FROM FilledCircle WHERE FilledCircleID = ?Param0",
                itsSelectSql.Statement.ToString(), "Select sql is incorrect for concrete table inheritance.");
            Assert.AreEqual(itsFilledCircleId, ((IDbDataParameter) itsSelectSql.Parameters[0]).Value,
                            "Parameter CircleID is incorrect in select where clause for concrete table inheritance.");
        }
    }
}
