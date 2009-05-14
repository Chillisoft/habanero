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
using Habanero.BO.ClassDefinition;
using Habanero.DB;

namespace Habanero.Test.General
{
    public abstract class TestInheritanceBase : TestUsingDatabase
    {
        protected BusinessObject objCircle;
        protected SqlStatementCollection itsInsertSql;
        protected SqlStatementCollection itsUpdateSql;
        protected SqlStatementCollection itsDeleteSql;
        protected string strID;

        public void SetupTest()
        {
            ClassDef.ClassDefs.Clear();

            this.SetupDBConnection();
            DatabaseConnection.CurrentConnection.ExecuteRawSql("delete from filledcircle_table;");
            DatabaseConnection.CurrentConnection.ExecuteRawSql("delete from circle_table;");
            DatabaseConnection.CurrentConnection.ExecuteRawSql("delete from circle_concrete;");
            DatabaseConnection.CurrentConnection.ExecuteRawSql("delete from shape_table;");
            SetupInheritanceSpecifics();
            objCircle = new Circle();
            SetStrID();
            objCircle.SetPropertyValue("ShapeName", "MyShape");
            objCircle.SetPropertyValue("Radius", 10);
            itsInsertSql =
                new InsertStatementGenerator(objCircle, DatabaseConnection.CurrentConnection).Generate();
            itsUpdateSql = new UpdateStatementGenerator(objCircle, DatabaseConnection.CurrentConnection).Generate();
            itsDeleteSql = new DeleteStatementGenerator(objCircle, DatabaseConnection.CurrentConnection).Generate();
        }

        public void SetupTestWithoutPrimaryKey()
        {
            ClassDef.ClassDefs.Clear();
            this.SetupDBConnection();
            DatabaseConnection.CurrentConnection.ExecuteRawSql("delete from filledcircle_table;");
            DatabaseConnection.CurrentConnection.ExecuteRawSql("delete from circle_table;");
            DatabaseConnection.CurrentConnection.ExecuteRawSql("delete from circle_concrete;");
            DatabaseConnection.CurrentConnection.ExecuteRawSql("delete from shape_table;");
            SetupInheritanceSpecifics();
            objCircle = new CircleNoPrimaryKey();
            SetStrID();
            objCircle.SetPropertyValue("ShapeName", "MyShape");
            objCircle.SetPropertyValue("Radius", 10);
            itsInsertSql =
                new InsertStatementGenerator(objCircle, DatabaseConnection.CurrentConnection).Generate();
            itsUpdateSql = new UpdateStatementGenerator(objCircle, DatabaseConnection.CurrentConnection).Generate();
            itsDeleteSql = new DeleteStatementGenerator(objCircle, DatabaseConnection.CurrentConnection).Generate();
        }

        protected abstract void SetupInheritanceSpecifics();
        protected abstract void SetStrID();
    }
}