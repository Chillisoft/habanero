//---------------------------------------------------------------------------------
// Copyright (C) 2009 Chillisoft Solutions
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
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Rhino.Mocks;
//using NMock;

namespace Habanero.Test.UI.Base
{
    /// <summary>
    /// Summary description for TestMapperBase.
    /// </summary>
    public class TestMapperBase //: TestUsingDatabase
    {
        protected MyBO itsMyBo;

        public TestMapperBase()
        {
            //base.SetupDBConnection();
            BORegistry.DataAccessor = new DataAccessorInMemory();
        }

        protected void SetupClassDefs(object propValue)
        {
            ClassDef.ClassDefs.Clear();
            IClassDef itsClassDef = MyBO.LoadClassDefWithRelationship();
            IClassDef itsRelatedClassDef = MyRelatedBo.LoadClassDef();
            itsMyBo = (MyBO)itsClassDef.CreateNewBusinessObject();
            MyRelatedBo relatedBo = (MyRelatedBo)itsRelatedClassDef.CreateNewBusinessObject();
            Guid myRelatedBoGuid = relatedBo.ID.GetAsGuid();
            itsMyBo.SetPropertyValue("RelatedID", myRelatedBoGuid);
            relatedBo.SetPropertyValue("MyRelatedTestProp", propValue);
            itsMyBo.Save();
            relatedBo.Save();
        }
        //protected void SetupClassDefs(object propValue)
        //{
        //    MockRepository mock = new MockRepository();
        //    IDatabaseConnection connection = mock.StrictMock<IDatabaseConnection>();
        //    IRelationshipCol mockRelCol = mock.StrictMock<IRelationshipCol>();

        //    ClassDef.ClassDefs.Clear();
        //    IClassDef itsClassDef = MyBO.LoadClassDefWithRelationship();
        //    IClassDef itsRelatedClassDef = MyRelatedBo.LoadClassDef();
        //    itsMyBo = (MyBO)itsClassDef.CreateNewBusinessObject();
        //    MyRelatedBo relatedBo = (MyRelatedBo)itsRelatedClassDef.CreateNewBusinessObject();
        //    Guid myRelatedBoGuid = relatedBo.ID.GetAsGuid();
        //    itsMyBo.SetPropertyValue("RelatedID", myRelatedBoGuid);
        //    relatedBo.SetPropertyValue("MyRelatedTestProp", propValue);
        //    ((IBusinessObject)itsMyBo).Relationships = mockRelCol;

        //    Expect.Call(mockRelCol.GetRelatedObject("MyRelationship")).Return(relatedBo).Repeat.Any();
        //    Expect.Call(connection.GetConnection()).Return(DatabaseConnection.CurrentConnection.GetConnection()).Repeat.Any();

        //    mock.ReplayAll();
        //}
        }
}