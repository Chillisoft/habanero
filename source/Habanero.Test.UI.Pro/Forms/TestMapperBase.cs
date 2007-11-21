using System;
using Habanero.BO.ClassDefinition;
using Habanero.BO;
using Habanero.DB;
using Habanero.Base;
using Habanero.Test;
//using NMock;
using Rhino.Mocks;

namespace Habanero.Test.UI.Forms
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
            itsMyBo = (MyBO) itsClassDef.CreateNewBusinessObject(connection);
            MyRelatedBo relatedBo = (MyRelatedBo) itsRelatedClassDef.CreateNewBusinessObject();
            Guid myRelatedBoGuid = new Guid(relatedBo.ID.GetObjectId().Substring(3, 38));
            itsMyBo.SetPropertyValue("RelatedID", myRelatedBoGuid);
            relatedBo.SetPropertyValue("MyRelatedTestProp", propValue);
            itsMyBo.Relationships = mockRelCol;

            //relColControl.ExpectAndReturn("GetRelatedObject", relatedBo, new object[] {"MyRelationship"});
            Expect.Call(mockRelCol.GetRelatedObject("MyRelationship")).Return(relatedBo).Repeat.Any();
			
            //mockDbConnection.ExpectAndReturn("GetConnection", DatabaseConnection.CurrentConnection.GetConnection(), new object[] {});
            Expect.Call(connection.GetConnection()).Return(DatabaseConnection.CurrentConnection.GetConnection()).Repeat.Any();

            mock.ReplayAll();
        }
    }
}