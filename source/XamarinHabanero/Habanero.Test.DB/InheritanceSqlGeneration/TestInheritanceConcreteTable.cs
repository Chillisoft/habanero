#region Licensing Header
// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2011 Chillisoft Solutions
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
#endregion
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
    public class TestInheritanceConcreteTable : TestInheritanceBase
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
        }

        protected override void SetStrID()
        {
            strID = ((Guid)objCircle.GetPropertyValue("CircleID")).ToString("B").ToUpper(CultureInfo.InvariantCulture);
        }

        [Test]
        public void TestCircleIsUsingConcreteTableInheritance()
        {
            Assert.AreEqual(ORMapping.ConcreteTableInheritance, Circle.GetClassDef().SuperClassDef.ORMapping);
        }

        [Test]
        public void TestObjCircleHasCorrectProperties()
        {
            objCircle.GetPropertyValue("ShapeName");
            objCircle.GetPropertyValue("CircleID");
            objCircle.GetPropertyValue("Radius");
        }

        [Test]
        public void TestCircleInsertSql()
        {
            var sqlStatements = itsInsertSql.ToList();
            Assert.AreEqual(1, sqlStatements.Count,
                            "There should only be one insert statement for concrete table inheritance.");
            Assert.AreEqual("INSERT INTO `circle_table` (`CircleID_field`, `Radius`, `ShapeName`) VALUES (?Param0, ?Param1, ?Param2)",
                            sqlStatements[0].Statement.ToString(),
                            "Concrete Table Inheritance insert Sql seems to be incorrect.");
            Assert.AreEqual(strID, ((IDbDataParameter) sqlStatements[0].Parameters[0]).Value,
                            "Parameter CircleID has incorrect value");
            Assert.AreEqual("MyShape", ((IDbDataParameter) sqlStatements[0].Parameters[2]).Value,
                            "Parameter ShapeName has incorrect value");
            Assert.AreEqual(10, ((IDbDataParameter) sqlStatements[0].Parameters[1]).Value,
                            "Parameter Radius has incorrect value");
        }

        [Test]
        public void TestCircleUpdateSql()
        {
            var sqlStatements = itsUpdateSql.ToList();
            Assert.AreEqual(1, sqlStatements.Count,
                            "There should only be one update statement for concrete table inheritance.");
            Assert.AreEqual("UPDATE `circle_table` SET `Radius` = ?Param0, `ShapeName` = ?Param1 WHERE `CircleID_field` = ?Param2",
                            sqlStatements[0].Statement.ToString(),
                            "Concrete Table Inheritance update Sql seems to be incorrect.");
            Assert.AreEqual(10, ((IDbDataParameter)sqlStatements[0].Parameters[0]).Value,
                            "Parameter Radius has incorrect value");
            Assert.AreEqual("MyShape", ((IDbDataParameter)sqlStatements[0].Parameters[1]).Value,
                            "Parameter ShapeName incorrect value");
            Assert.AreEqual(strID, ((IDbDataParameter) sqlStatements[0].Parameters[2]).Value,
                            "Parameter CircleID in where clause has incorrect value");
        }

        [Test]
        public void TestCircleDeleteSql()
        {
            var sqlStatements = itsDeleteSql.ToList();
            Assert.AreEqual(1, sqlStatements.Count,
                            "There should only be one delete statement for concrete table inheritance.");
            Assert.AreEqual("DELETE FROM `circle_table` WHERE `CircleID_field` = ?Param0", sqlStatements[0].Statement.ToString(),
                            "Concrete Table Inheritance delete Sql seems to be incorrect.");
            Assert.AreEqual(strID, ((IDbDataParameter) sqlStatements[0].Parameters[0]).Value,
                            "Parameter CircleID has incorrect value in Delete Sql statement for concrete table inheritance.");
        }

    }
}