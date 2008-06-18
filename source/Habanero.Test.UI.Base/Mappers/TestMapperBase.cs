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


using System;
using Habanero.Base;
using Habanero.BO.ClassDefinition;
using Habanero.DB;
using Rhino.Mocks;
//using NMock;

namespace Habanero.Test.UI.Base
{
    /// <summary>
    /// Summary description for TestMapperBase.
    /// </summary>
    public class TestMapperBase : TestUsingDatabase
    {
        protected MyBO itsMyBo;

        public TestMapperBase()
        {
            base.SetupDBConnection();
        }

        protected void SetupClassDefs(object propValue)
        {
            MockRepository mock = new MockRepository();

            //Mock mockDbConnection = new DynamicMock(typeof (IDatabaseConnection));
            //IDatabaseConnection connection = (IDatabaseConnection) mockDbConnection.MockInstance;
            IDatabaseConnection connection = mock.CreateMock<IDatabaseConnection>();

            //Mock relColControl = new DynamicMock(typeof (IRelationshipCol));
            //IRelationshipCol mockRelCol = (IRelationshipCol) relColControl.MockInstance;
            IRelationshipCol mockRelCol = mock.CreateMock<IRelationshipCol>();

            ClassDef.ClassDefs.Clear();
            ClassDef itsClassDef = MyBO.LoadClassDefWithRelationship();
            ClassDef itsRelatedClassDef = MyRelatedBo.LoadClassDef();
            itsMyBo = (MyBO)itsClassDef.CreateNewBusinessObject(connection);
            MyRelatedBo relatedBo = (MyRelatedBo)itsRelatedClassDef.CreateNewBusinessObject();
            Guid myRelatedBoGuid = new Guid(relatedBo.ID.GetObjectId().Substring(3, 38));
            itsMyBo.SetPropertyValue("RelatedID", myRelatedBoGuid);
            relatedBo.SetPropertyValue("MyRelatedTestProp", propValue);
            ((IBusinessObject)itsMyBo).Relationships = mockRelCol;

            //relColControl.ExpectAndReturn("GetRelatedObject", relatedBo, new object[] {"MyRelationship"});
            Expect.Call(mockRelCol.GetRelatedObject("MyRelationship")).Return(relatedBo).Repeat.Any();

            //mockDbConnection.ExpectAndReturn("GetConnection", DatabaseConnection.CurrentConnection.GetConnection(), new object[] {});
            Expect.Call(connection.GetConnection()).Return(DatabaseConnection.CurrentConnection.GetConnection()).Repeat.Any();

            mock.ReplayAll();
        }
    }
}