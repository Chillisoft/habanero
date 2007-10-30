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

using System;
using System.Collections.Generic;
using System.Text;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.DB;
using NUnit.Framework;

namespace Habanero.Test.General
{
	[TestFixture]
	public class TestShapeCircleRelationship : TestUsingDatabase
	{
		[TestFixtureSetUp]
		public void SetupTestFixture()
		{
			ClassDef.ClassDefs.Clear();
			SetupDBConnection();
			ClassDef classDef = Shape.GetClassDef();

			RelKeyDef relKeyDef;
			PropDef propDef;
			RelPropDef lRelPropDef;
			RelationshipDef relDef;

			relKeyDef = new RelKeyDef();
			propDef = classDef.PropDefcol["ShapeID"]; 
			lRelPropDef = new RelPropDef(propDef, "ShapeID");
			relKeyDef.Add(lRelPropDef);
			relDef = new SingleRelationshipDef("Circle", typeof(Circle), relKeyDef, false);
			classDef.RelationshipDefCol.Add(relDef);
		}

		[Test]
		public void TestRelationshipSQL()
		{
			Shape shape;
			Circle circle = new Circle();
			circle.Radius = 10;
			shape = circle;
			Relationship circleRelationship = shape.Relationships["Circle"];
			RelKey relKey = circleRelationship._relKey;
			ISqlStatement sqlStatement = BusinessObjectCollection<Circle>.CreateLoadSqlStatement(
				circle, Circle.GetClassDef(), relKey.RelationshipExpression(), -1, "");
			string sql = sqlStatement.Statement.ToString();
			Assert.AreEqual("SELECT Circle.CircleID, Circle.Radius, Shape.ShapeID, Shape.ShapeName FROM Circle, Shape " + 
				"WHERE Shape.ShapeID = Circle.ShapeID AND Shape.ShapeID = ?Param0", sql);
		}

	}
}
