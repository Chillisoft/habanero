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

using Habanero.BO;
using Habanero.DB;
using NUnit.Framework;

namespace Habanero.Test.General
{
    public abstract class TestInheritanceHeirarchyBase : TestUsingDatabase
    {
        protected BusinessObject itsFilledCircle;
        protected SqlStatementCollection itsInsertSql;
        protected SqlStatementCollection itsUpdateSql;
        protected SqlStatementCollection itsDeleteSql;
        protected SqlStatement itsSelectSql;
        protected string itsFilledCircleId;
        protected SqlStatement itsLoadSql;

        public void SetupTest()
        {
            this.SetupDBConnection();
            SetupInheritanceSpecifics();
            itsFilledCircle = new FilledCircle();
            SetStrID();
            itsFilledCircle.SetPropertyValue("Colour", 3);
            itsFilledCircle.SetPropertyValue("Radius", 10);
            itsFilledCircle.SetPropertyValue("ShapeName", "MyFilledCircle");

            itsInsertSql = itsFilledCircle.GetInsertSql();
            itsUpdateSql = itsFilledCircle.GetUpdateSql();
            itsDeleteSql = itsFilledCircle.GetDeleteSql();
            itsSelectSql = new SqlStatement(DatabaseConnection.CurrentConnection.GetConnection());
            itsSelectSql.Statement.Append(itsFilledCircle.SelectSqlStatement(itsSelectSql));
        }

        public void SetupTestForFilledCircleNoPK()
        {
            this.SetupDBConnection();
            SetupInheritanceSpecifics();
            itsFilledCircle = new FilledCircleNoPrimaryKey();
            SetStrID();
            itsFilledCircle.SetPropertyValue("Colour", 3);
            itsFilledCircle.SetPropertyValue("Radius", 10);
            itsFilledCircle.SetPropertyValue("ShapeName", "MyFilledCircle");

            itsInsertSql = itsFilledCircle.GetInsertSql();
            itsUpdateSql = itsFilledCircle.GetUpdateSql();
            itsDeleteSql = itsFilledCircle.GetDeleteSql();
            itsSelectSql = new SqlStatement(DatabaseConnection.CurrentConnection.GetConnection());
            itsSelectSql.Statement.Append(itsFilledCircle.SelectSqlStatement(itsSelectSql));
        }

        public void SetupTestForFilledCircleInheritsCircleNoPK()
        {
            this.SetupDBConnection();
            SetupInheritanceSpecifics();
            itsFilledCircle = new FilledCircleInheritsCircleNoPK();
            SetStrID();
            itsFilledCircle.SetPropertyValue("Colour", 3);
            itsFilledCircle.SetPropertyValue("Radius", 10);
            itsFilledCircle.SetPropertyValue("ShapeName", "MyFilledCircle");

            itsInsertSql = itsFilledCircle.GetInsertSql();
            itsUpdateSql = itsFilledCircle.GetUpdateSql();
            itsDeleteSql = itsFilledCircle.GetDeleteSql();
            //itsSelectSql = new SqlStatement(DatabaseConnection.CurrentConnection.GetConnection());
            //itsSelectSql.Statement.Append(itsFilledCircle.SelectSqlStatement(itsSelectSql));
        }

        protected abstract void SetupInheritanceSpecifics();
        protected abstract void SetStrID();
    }
}