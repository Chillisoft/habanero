using System;
using Habanero.Bo.ClassDefinition;
using Habanero.Bo;
using Habanero.DB;
using Habanero.Base;
using Habanero.Test;
using NMock;

namespace Habanero.Test.Ui.BoControls
{
    /// <summary>
    /// Summary description for TestMapperBase.
    /// </summary>
    public class TestMapperBase : TestUsingDatabase
    {
        protected MyBo itsMyBo;

        public TestMapperBase()
        {
            base.SetupDBConnection();
        }

        protected void SetupClassDefs(object propValue)
        {
            Mock mockDbConnection = new DynamicMock(typeof (IDatabaseConnection));
            IDatabaseConnection connection = (IDatabaseConnection) mockDbConnection.MockInstance;

            Mock relColControl = new DynamicMock(typeof (IRelationshipCol));
            IRelationshipCol mockRelCol = (IRelationshipCol) relColControl.MockInstance;

            ClassDef.ClassDefs.Clear();
            ClassDef itsClassDef = MyBo.LoadClassDefWithRelationship();
            ClassDef itsRelatedClassDef = MyRelatedBo.LoadClassDef();
            itsMyBo = (MyBo) itsClassDef.CreateNewBusinessObject(connection);
            MyRelatedBo relatedBo = (MyRelatedBo) itsRelatedClassDef.CreateNewBusinessObject();
            Guid myRelatedBoGuid = new Guid(relatedBo.ID.GetObjectId().Substring(3, 38));
            itsMyBo.SetPropertyValue("RelatedID", myRelatedBoGuid);
            relatedBo.SetPropertyValue("MyRelatedTestProp", propValue);
            relColControl.ExpectAndReturn("GetRelatedObject", relatedBo, new object[] {"MyRelationship"});
            itsMyBo.Relationships = mockRelCol;

            mockDbConnection.ExpectAndReturn("GetConnection", DatabaseConnection.CurrentConnection.GetConnection(),
                                             new object[] {});
        }
    }
}