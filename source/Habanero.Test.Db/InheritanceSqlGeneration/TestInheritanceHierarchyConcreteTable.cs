// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2010 Chillisoft Solutions
//  
//  This file is part of the Habanero framework.
//  
//      Habanero is a free framework: you can redistribute it and/or modify
//      it under the terms of the GNU Lesser General Public License as published by
//      the Free Software Foundation, either version 3 of the License, or
//      (at your option) any later version.
//  
//      The Habanero framework is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//      GNU Lesser General Public License for more details.
//  
//      You should have received a copy of the GNU Lesser General Public License
//      along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
// ---------------------------------------------------------------------------------
using System;
using System.Data;
using System.Globalization;
using System.Linq;
using Habanero.Base;
using Habanero.BO.ClassDefinition;
using Habanero.DB;
using NUnit.Framework;

namespace Habanero.Test.DB.InheritanceSqlGeneration
{
    [TestFixture]
    public class TestInheritanceHierarchyConcreteTable : TestInheritanceHierarchyBase
    {
        [TestFixtureSetUp]
        public void SetupFixture()
        {
            SetupTest();
        }

        protected override void SetupInheritanceSpecifics()
        {
            Circle.GetClassDef().SuperClassDef =
                new SuperClassDef(Shape.GetClassDef(), ORMapping.ConcreteTableInheritance);
            FilledCircle.GetClassDef().SuperClassDef =
                new SuperClassDef(Circle.GetClassDef(), ORMapping.ConcreteTableInheritance);
        }

        protected override void SetStrID()
        {
            _filledCircleId = ((Guid)_filledCircle.GetPropertyValue("FilledCircleID")).ToString("B").ToUpper(CultureInfo.InvariantCulture);
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
            _filledCircle.GetPropertyValue("ShapeName");
            _filledCircle.GetPropertyValue("FilledCircleID");
            _filledCircle.GetPropertyValue("Radius");
            _filledCircle.GetPropertyValue("Colour");
        }


        //[Test]
        //public void TestCircleSelectSql()
        //{
        //    Assert.AreEqual(
        //        "SELECT `FilledCircle_table`.`Colour`, `FilledCircle_table`.`FilledCircleID_field`, `FilledCircle_table`.`Radius`, `FilledCircle_table`.`ShapeName` FROM `FilledCircle_table` WHERE `FilledCircleID_field` = ?Param0",
        //        _selectSql.Statement.ToString(), "select statement is incorrect for Concrete Table inheritance");
        //}

        [Test]
        public void TestCircleInsertSql()
        {
            var sqlStatements = _insertSql.ToList();
            Assert.AreEqual(1, sqlStatements.Count,
                            "There should only be one insert statement for concrete table inheritance.");
            Assert.AreEqual(
                "INSERT INTO `FilledCircle_table` (`Colour`, `FilledCircleID_field`, `Radius`, `ShapeName`) VALUES (?Param0, ?Param1, ?Param2, ?Param3)",
                sqlStatements[0].Statement.ToString(), "Concrete Table Inheritance insert Sql seems to be incorrect.");
            Assert.AreEqual(_filledCircleId, ((IDbDataParameter) sqlStatements[0].Parameters[1]).Value,
                            "Parameter FilledCircleID has incorrect value");
            Assert.AreEqual(3, ((IDbDataParameter) sqlStatements[0].Parameters[0]).Value,
                            "Parameter Colour has incorrect value");
            Assert.AreEqual("MyFilledCircle", ((IDbDataParameter) sqlStatements[0].Parameters[3]).Value,
                            "Parameter ShapeName has incorrect value");
            Assert.AreEqual(10, ((IDbDataParameter) sqlStatements[0].Parameters[2]).Value,
                            "Parameter Radius has incorrect value");
        }

        [Test]
        public void TestCircleUpdateSql()
        {
            var sqlStatements = _updateSql.ToList();
            Assert.AreEqual(1, sqlStatements.Count,
                            "There should only be one update statement for concrete table inheritance.");
            Assert.AreEqual(
                "UPDATE `FilledCircle_table` SET `Colour` = ?Param0, `Radius` = ?Param1, `ShapeName` = ?Param2 WHERE `FilledCircleID_field` = ?Param3",
                sqlStatements[0].Statement.ToString(), "Concrete Table Inheritance update Sql seems to be incorrect.");
            Assert.AreEqual(3, ((IDbDataParameter) sqlStatements[0].Parameters[0]).Value,
                            "Parameter Colour has incorrect value");
            Assert.AreEqual("MyFilledCircle", ((IDbDataParameter) sqlStatements[0].Parameters[2]).Value,
                            "Parameter ShapeName has incorrect value");
            Assert.AreEqual(10, ((IDbDataParameter) sqlStatements[0].Parameters[1]).Value,
                            "Parameter Radius has incorrect value");
            Assert.AreEqual(_filledCircleId, ((IDbDataParameter) sqlStatements[0].Parameters[3]).Value,
                            "Parameter ShapeID has incorrect value");
        }

        [Test]
        public void TestCircleDeleteSql()
        {
            var sqlStatements = _deleteSql.ToList();
            Assert.AreEqual(1, sqlStatements.Count,
                            "There should only be one delete statement for concrete table inheritance.");
            Assert.AreEqual("DELETE FROM `FilledCircle_table` WHERE `FilledCircleID_field` = ?Param0",
                            sqlStatements[0].Statement.ToString(),
                            "Concrete Table Inheritance delete Sql seems to be incorrect.");
            Assert.AreEqual(_filledCircleId, ((IDbDataParameter) sqlStatements[0].Parameters[0]).Value,
                            "Parameter FilledCircleID has incorrect value in Delete Sql statement for concrete table inheritance.");
        }

    }
}