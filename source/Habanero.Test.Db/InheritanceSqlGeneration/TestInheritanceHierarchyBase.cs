//---------------------------------------------------------------------------------
// Copyright (C) 2008 Chillisoft Solutions
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

using Habanero.BO;
using Habanero.DB;

namespace Habanero.Test.DB.InheritanceSqlGeneration
{
    public abstract class TestInheritanceHierarchyBase : TestUsingDatabase
    {
        protected BusinessObject _filledCircle;
        protected SqlStatementCollection _insertSql;
        protected SqlStatementCollection _updateSql;
        protected SqlStatementCollection _deleteSql;
        protected string _filledCircleId;
        protected SqlStatement _loadSql;

        public void SetupTest()
        {
            this.SetupDBConnection();
            ClearTables();
            SetupInheritanceSpecifics();
            _filledCircle = new FilledCircle();
            SetStrID();
            _filledCircle.SetPropertyValue("Colour", 3);
            _filledCircle.SetPropertyValue("Radius", 10);
            _filledCircle.SetPropertyValue("ShapeName", "MyFilledCircle");

            _insertSql =
                new InsertStatementGenerator(_filledCircle, DatabaseConnection.CurrentConnection).Generate();
            _updateSql = new UpdateStatementGenerator(_filledCircle, DatabaseConnection.CurrentConnection).Generate();
            _deleteSql = new DeleteStatementGenerator(_filledCircle, DatabaseConnection.CurrentConnection).Generate();
        }

        private void ClearTables()
        {
            DatabaseConnection.CurrentConnection.ExecuteRawSql("delete from filledcircle_table;");
            DatabaseConnection.CurrentConnection.ExecuteRawSql("delete from circle_table;");
            DatabaseConnection.CurrentConnection.ExecuteRawSql("delete from circle_concrete;");
            DatabaseConnection.CurrentConnection.ExecuteRawSql("delete from shape_table;");
        }

        public void SetupTestForFilledCircleNoPK()
        {
            this.SetupDBConnection();
            ClearTables();
            SetupInheritanceSpecifics();
            _filledCircle = new FilledCircleNoPrimaryKey();
            SetStrID();
            _filledCircle.SetPropertyValue("Colour", 3);
            _filledCircle.SetPropertyValue("Radius", 10);
            _filledCircle.SetPropertyValue("ShapeName", "MyFilledCircle");

            _insertSql =
                new InsertStatementGenerator(_filledCircle, DatabaseConnection.CurrentConnection).Generate();
            _updateSql = new UpdateStatementGenerator(_filledCircle, DatabaseConnection.CurrentConnection).Generate();
            _deleteSql = new DeleteStatementGenerator(_filledCircle, DatabaseConnection.CurrentConnection).Generate();
        }

        public void SetupTestForFilledCircleInheritsCircleNoPK()
        {
            this.SetupDBConnection();
            ClearTables();
            SetupInheritanceSpecifics();
            _filledCircle = new FilledCircleInheritsCircleNoPK();
            SetStrID();
            _filledCircle.SetPropertyValue("Colour", 3);
            _filledCircle.SetPropertyValue("Radius", 10);
            _filledCircle.SetPropertyValue("ShapeName", "MyFilledCircle");

            _insertSql =
                new InsertStatementGenerator(_filledCircle, DatabaseConnection.CurrentConnection).Generate();
            _updateSql = new UpdateStatementGenerator(_filledCircle, DatabaseConnection.CurrentConnection).Generate();
            _deleteSql = new DeleteStatementGenerator(_filledCircle, DatabaseConnection.CurrentConnection).Generate();
        }

        public void SetupTestForFilledCircleNoPrimaryKeyInheritsCircle()
        {
            this.SetupDBConnection();
            ClearTables();
            SetupInheritanceSpecifics();
            _filledCircle = new FilledCircleNoPrimaryKeyInheritsCircle();
            SetStrID();
            _filledCircle.SetPropertyValue("Colour", 3);
            _filledCircle.SetPropertyValue("Radius", 10);
            _filledCircle.SetPropertyValue("ShapeName", "MyFilledCircle");

            _insertSql =
                new InsertStatementGenerator(_filledCircle, DatabaseConnection.CurrentConnection).Generate();
            _updateSql = new UpdateStatementGenerator(_filledCircle, DatabaseConnection.CurrentConnection).Generate();
            _deleteSql = new DeleteStatementGenerator(_filledCircle, DatabaseConnection.CurrentConnection).Generate();
        }

        protected static Shape CreateSavedShape()
        {
            Shape shape = new Shape();
            shape.ShapeName = "MyShape";
            shape.Save();
            return shape;
        }

        protected static CircleNoPrimaryKey CreateSavedCircle()
        {
            CircleNoPrimaryKey circle = new CircleNoPrimaryKey();
            circle.Radius = 5;
            circle.ShapeName = "Circle";
            circle.Save();
            return circle;
        }

        protected static FilledCircleInheritsCircleNoPK CreateSavedFilledCircle()
        {
            FilledCircleInheritsCircleNoPK filledCircle = new FilledCircleInheritsCircleNoPK();
            filledCircle.Colour = 3;
            filledCircle.Radius = 7;
            filledCircle.ShapeName = "FilledCircle";
            filledCircle.Save();
            return filledCircle;
        }

        protected abstract void SetupInheritanceSpecifics();
        protected abstract void SetStrID();
    }
}