using System.Windows.Forms;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.Test.BO;
using Habanero.UI.Base;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    [TestFixture]
    public class TestTreeViewControllerWin
    {
        [TestFixtureSetUp]
        public  void  TestFixtureSetUp()
        {
            ClassDef.ClassDefs.Clear();
            //ContactPersonTestBO.LoadDefaultClassDef_WOrganisationID();
            //OrganisationTestBO.LoadDefaultClassDef();

            BORegistry.DataAccessor = new DataAccessorInMemory();
            ContactPersonTestBO.LoadClassDefOrganisationTestBORelationship_MultipleReverse();
            OrganisationTestBO.LoadDefaultClassDef_PreventAddChild();
        }

        private static MultipleRelationship<ContactPersonTestBO> GetCompositionRelationship(out BusinessObjectCollection<ContactPersonTestBO> cpCol, OrganisationTestBO organisationTestBO)
        {
            MultipleRelationship<ContactPersonTestBO> compositionRelationship =
                organisationTestBO.Relationships.GetMultiple<ContactPersonTestBO>("ContactPeople");
            RelationshipDef relationshipDef = (RelationshipDef)compositionRelationship.RelationshipDef;
            relationshipDef.RelationshipType = RelationshipType.Composition;
            cpCol = compositionRelationship.BusinessObjectCollection;
            return compositionRelationship;
        }

        [Test]
        public void Test_RootNodeBusinessObject()
        {
            //---------------Set up test pack-------------------
            TreeViewWin treeView = new TreeViewWin();
            TreeViewController treeViewController = new TreeViewController(treeView);
            OrganisationTestBO organisationTestBO = new OrganisationTestBO();
            treeViewController.LoadTreeView(organisationTestBO);
            //-------------Assert Preconditions -------------
            Assert.AreEqual(1, treeView.Nodes.Count);
            //---------------Execute Test ----------------------
            IBusinessObject businessObject = treeViewController.RootNodeBusinessObject;
            //---------------Test Result -----------------------
            Assert.AreSame(organisationTestBO, businessObject);
        }

        [Test]
        public void Test_LoadNull()
        {
            //---------------Set up test pack-------------------
            TreeViewWin treeView = new TreeViewWin();
            TreeViewController treeViewController = new TreeViewController(treeView);
            //-------------Assert Preconditions -------------
            Assert.AreEqual(0, treeView.Nodes.Count);
            //---------------Execute Test ----------------------
            treeViewController.LoadTreeView(null);
            //---------------Test Result -----------------------
            Assert.AreEqual(0, treeView.Nodes.Count);
        }

        [Test]
        public void Test_LoadSingleBO_NoChildren()
        {
            //---------------Set up test pack-------------------
            TreeViewWin treeView = new TreeViewWin();
            TreeViewController treeViewController = new TreeViewController(treeView);
            OrganisationTestBO organisationTestBO = new OrganisationTestBO();
            //-------------Assert Preconditions -------------
            Assert.AreEqual(0, treeView.Nodes.Count);
            //---------------Execute Test ----------------------
            treeViewController.LoadTreeView(organisationTestBO);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, treeView.Nodes.Count);
            ITreeNode node = treeView.Nodes[0];
            Assert.AreEqual(organisationTestBO.ToString(), node.Text);
            Assert.AreEqual(0, node.Nodes.Count);
        }

        [Test]
        public void Test_LoadSingleBO_OneChildRelationship_NotExpanded()
        {
            //---------------Set up test pack-------------------
            TreeViewWin treeView = new TreeViewWin();
            TreeViewController treeViewController = new TreeViewController(treeView);
            OrganisationTestBO organisationTestBO = new OrganisationTestBO();  
            //-------------Assert Preconditions -------------
            Assert.AreEqual(0, treeView.Nodes.Count);
            //---------------Execute Test ----------------------
            treeViewController.LoadTreeView(organisationTestBO);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, treeView.Nodes.Count);
            ITreeNode node = treeView.Nodes[0];
            Assert.AreEqual(organisationTestBO.ToString(), node.Text);
            Assert.AreEqual(1, node.Nodes.Count);
            ITreeNode relationshipNode = node.Nodes[0];
            Assert.AreEqual("$DUMMY$", relationshipNode.Text);
        }

        [Test]
        public void Test_LoadSingleBO_OneChildRelationship_Expanded()
        {
            //---------------Set up test pack-------------------
            TreeViewWin treeView = new TreeViewWin();
            TreeViewController treeViewController = new TreeViewController(treeView);
            OrganisationTestBO organisationTestBO = new OrganisationTestBO();  
            //-------------Assert Preconditions -------------
            Assert.AreEqual(0, treeView.Nodes.Count);
            //---------------Execute Test ----------------------
            treeViewController.LoadTreeView(organisationTestBO, 1);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, treeView.Nodes.Count);
            ITreeNode node = treeView.Nodes[0];
            Assert.AreEqual(organisationTestBO.ToString(), node.Text);
            Assert.AreEqual(1, node.Nodes.Count);
            ITreeNode relationshipNode = node.Nodes[0];
            Assert.AreEqual("ContactPeople", relationshipNode.Text);
        }

        [Test]
        public void Test_LoadSingleBO_OneChild_Expanded()
        {
            //---------------Set up test pack-------------------
            TreeViewWin treeView = new TreeViewWin();
            TreeViewController treeViewController = new TreeViewController(treeView);
            OrganisationTestBO organisationTestBO = new OrganisationTestBO {Name = TestUtil.GetRandomString()};
            ContactPersonTestBO contactPersonTestBO = organisationTestBO.ContactPeople.CreateBusinessObject();
            contactPersonTestBO.Surname = TestUtil.GetRandomString();
            //-------------Assert Preconditions -------------
            Assert.AreEqual(0, treeView.Nodes.Count);
            //---------------Execute Test ----------------------
            treeViewController.LoadTreeView(organisationTestBO, 2);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, treeView.Nodes.Count);
            ITreeNode node = treeView.Nodes[0];
            Assert.AreEqual(organisationTestBO.ToString(), node.Text);
            Assert.AreEqual(1, node.Nodes.Count);
            ITreeNode relationshipNode = node.Nodes[0];
            Assert.AreEqual("ContactPeople", relationshipNode.Text);
            Assert.AreEqual(1, relationshipNode.Nodes.Count);
            ITreeNode childNode = relationshipNode.Nodes[0];
            Assert.AreEqual(contactPersonTestBO.ToString(), childNode.Text);
        }

        [Test]
        public void Test_SetupNodeWithBusinessObject_LoadSingleBo_CustomDisplayValue()
        {
            //---------------Set up test pack-------------------
            TreeViewWin treeView = new TreeViewWin();
            TreeViewController treeViewController = new TreeViewController(treeView);

            string customDisplayValue = TestUtil.GetRandomString();
            ContactPersonTestBO contactPersonTestBO =  new ContactPersonTestBO();
            //-------------Assert Preconditions -------------
            Assert.AreEqual(0, treeView.Nodes.Count);
            //---------------Execute Test ----------------------
            treeViewController.SetupNodeWithBusinessObject += delegate(TreeNode treeNode, IBusinessObject businessObject)
            {
                treeNode.Text = customDisplayValue;
            };
            treeViewController.LoadTreeView(contactPersonTestBO);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, treeView.Nodes.Count);
            ITreeNode node = treeView.Nodes[0];
            Assert.AreEqual(customDisplayValue, node.Text);
            Assert.AreEqual(0, node.Nodes.Count);

        }

        [Test]
        public void Test_GetBusinessObjectTreeNode_RootNode()
        {
            //---------------Set up test pack-------------------
            TreeViewWin treeView = new TreeViewWin();
            TreeViewController treeViewController = new TreeViewController(treeView);
            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO();
            treeViewController.LoadTreeView(contactPersonTestBO);
            //-------------Assert Preconditions -------------
            Assert.AreEqual(1, treeView.Nodes.Count);
            //---------------Execute Test ----------------------
            TreeNode fetchedTreeNode = treeViewController.GetBusinessObjectTreeNode(contactPersonTestBO);
            //---------------Test Result -----------------------
            Assert.IsNotNull(fetchedTreeNode);
            Assert.AreSame(fetchedTreeNode, treeView.Nodes[0].OriginalNode);
        }

        //[Test]
        //public void Test_GetBusinessObjectTreeNode_ChildNode()
        //{
        //    //---------------Set up test pack-------------------
        //    TreeViewWin treeView = new TreeViewWin();
        //    TreeViewController treeViewController = new TreeViewController(treeView);
        //    OrganisationTestBO organisationTestBO = new OrganisationTestBO { Name = TestUtil.GetRandomString() };
        //    ContactPersonTestBO contactPersonTestBO = organisationTestBO.ContactPeople.CreateBusinessObject();
        //    contactPersonTestBO.Surname = TestUtil.GetRandomString();
        //    treeViewController.LoadTreeView(organisationTestBO, 2);
        //    //-------------Assert Preconditions -------------
        //    Assert.AreEqual(1, treeView.Nodes.Count);
        //    Assert.AreEqual(4, treeView.Nodes[0].Nodes.Count);
        //    Assert.AreEqual(1, treeView.Nodes[0].Nodes[0].Nodes.Count);
        //    //---------------Execute Test ----------------------
        //    TreeNode fetchedTreeNode = treeViewController.GetBusinessObjectTreeNode(contactPersonTestBO);
        //    //---------------Test Result -----------------------
        //    Assert.IsNotNull(fetchedTreeNode);
        //    Assert.AreSame(fetchedTreeNode, treeView.Nodes[0].Nodes[0].Nodes[0].OriginalNode);
        //}

        //[Test]
        //public void Test_GetBusinessObjectTreeNode_NotInTree()
        //{
        //    //---------------Set up test pack-------------------
        //    TreeViewWin treeView = new TreeViewWin();
        //    TreeViewController treeViewController = new TreeViewController(treeView);
        //    DBColumn dbColumn = new DBColumn("ColumnName");
        //    treeViewController.LoadTreeView(dbColumn);
        //    //-------------Assert Preconditions -------------
        //    Assert.AreEqual(1, treeView.Nodes.Count);
        //    //---------------Execute Test ----------------------
        //    TreeNode fetchedTreeNode = treeViewController.GetBusinessObjectTreeNode(new DBColumn());
        //    //---------------Test Result -----------------------
        //    Assert.IsNull(fetchedTreeNode);
        //}

        //[Test]
        //public void Test_SetVisibility_False()
        //{
        //    //---------------Set up test pack-------------------
        //    TreeViewWin treeView = new TreeViewWin();
        //    TreeViewController treeViewController = new TreeViewController(treeView);
        //    DBDatabase dbDatabase = new DBDatabase() { DatabaseName = TestUtils.GetRandomString() };
        //    DBTable dbTable = new DBTable(TestUtils.GetRandomString());
        //    dbDatabase.Tables.Add(dbTable);
        //    treeViewController.LoadTreeView(dbDatabase, 2);
        //    //-------------Assert Preconditions -------------
        //    Assert.AreEqual(1, treeView.Nodes.Count);
        //    ITreeNode rootNode = treeView.Nodes[0];
        //    Assert.AreEqual(dbDatabase.ToString(), rootNode.Text);
        //    Assert.AreEqual(1, rootNode.Nodes.Count);
        //    ITreeNode relationshipNode = rootNode.Nodes[0];
        //    Assert.AreEqual("Tables", relationshipNode.Text);
        //    Assert.AreEqual(1, relationshipNode.Nodes.Count);
        //    ITreeNode childNode = relationshipNode.Nodes[0];
        //    Assert.AreEqual(dbTable.ToString(), childNode.Text);
        //    //---------------Execute Test ----------------------
        //    treeViewController.SetVisibility(dbTable, false);
        //    //---------------Test Result -----------------------
        //    Assert.AreEqual(1, treeView.Nodes.Count);
        //    Assert.AreEqual(dbDatabase.ToString(), rootNode.Text);
        //    Assert.AreEqual(1, rootNode.Nodes.Count);
        //    Assert.AreEqual("Tables", relationshipNode.Text);
        //    Assert.AreEqual(0, relationshipNode.Nodes.Count);
        //}

        //[Test]
        //public void Test_SetVisibility_True()
        //{
        //    //---------------Set up test pack-------------------
        //    TreeViewWin treeView = new TreeViewWin();
        //    TreeViewController treeViewController = new TreeViewController(treeView);
        //    DBDatabase dbDatabase = new DBDatabase() { DatabaseName = TestUtils.GetRandomString() };
        //    DBTable dbTable = new DBTable(TestUtils.GetRandomString());
        //    dbDatabase.Tables.Add(dbTable);
        //    treeViewController.LoadTreeView(dbDatabase, 2);
        //    //-------------Assert Preconditions -------------
        //    Assert.AreEqual(1, treeView.Nodes.Count);
        //    ITreeNode rootNode = treeView.Nodes[0];
        //    Assert.AreEqual(dbDatabase.ToString(), rootNode.Text);
        //    Assert.AreEqual(1, rootNode.Nodes.Count);
        //    ITreeNode relationshipNode = rootNode.Nodes[0];
        //    Assert.AreEqual("Tables", relationshipNode.Text);
        //    Assert.AreEqual(1, relationshipNode.Nodes.Count);
        //    ITreeNode childNode = relationshipNode.Nodes[0];
        //    Assert.AreEqual(dbTable.ToString(), childNode.Text);
        //    //---------------Execute Test ----------------------
        //    treeViewController.SetVisibility(dbTable, true);
        //    //---------------Test Result -----------------------
        //    Assert.AreEqual(1, treeView.Nodes.Count);
        //    Assert.AreEqual(dbDatabase.ToString(), rootNode.Text);
        //    Assert.AreEqual(1, rootNode.Nodes.Count);
        //    Assert.AreEqual("Tables", relationshipNode.Text);
        //    Assert.AreEqual(1, relationshipNode.Nodes.Count);
        //}

        //[Test]
        //public void Test_SetVisibility_False_ThenTrue()
        //{
        //    //---------------Set up test pack-------------------
        //    TreeViewWin treeView = new TreeViewWin();
        //    TreeViewController treeViewController = new TreeViewController(treeView);
        //    DBDatabase dbDatabase = new DBDatabase() { DatabaseName = TestUtils.GetRandomString() };
        //    DBTable dbTable = new DBTable(TestUtils.GetRandomString());
        //    dbDatabase.Tables.Add(dbTable);
        //    treeViewController.LoadTreeView(dbDatabase, 2);
        //    treeViewController.SetVisibility(dbTable, false);
        //    //-------------Assert Preconditions -------------
        //    Assert.AreEqual(1, treeView.Nodes.Count);
        //    ITreeNode rootNode = treeView.Nodes[0];
        //    Assert.AreEqual(dbDatabase.ToString(), rootNode.Text);
        //    Assert.AreEqual(1, rootNode.Nodes.Count);
        //    ITreeNode relationshipNode = rootNode.Nodes[0];
        //    Assert.AreEqual("Tables", relationshipNode.Text);
        //    Assert.AreEqual(0, relationshipNode.Nodes.Count);
        //    //---------------Execute Test ----------------------
        //    treeViewController.SetVisibility(dbTable, true);
        //    //---------------Test Result -----------------------
        //    Assert.AreEqual(1, treeView.Nodes.Count);
        //    Assert.AreEqual(dbDatabase.ToString(), rootNode.Text);
        //    Assert.AreEqual(1, rootNode.Nodes.Count);
        //    Assert.AreEqual("Tables", relationshipNode.Text);
        //    Assert.AreEqual(1, relationshipNode.Nodes.Count);
        //}

        //[Test]
        //public void Test_SetVisibility_False_ThenTrue_CorrectOrder()
        //{
        //    //---------------Set up test pack-------------------
        //    TreeViewWin treeView = new TreeViewWin();
        //    TreeViewController treeViewController = new TreeViewController(treeView);
        //    DBDatabase dbDatabase = new DBDatabase() { DatabaseName = TestUtils.GetRandomString() };
        //    DBTable dbTable = new DBTable(TestUtils.GetRandomString());
        //    dbDatabase.Tables.Add(dbTable);
        //    DBTable dbTableInfo2 = new DBTable(TestUtils.GetRandomString());
        //    dbDatabase.Tables.Add(dbTableInfo2);
        //    DBTable dbTableInfo3 = new DBTable(TestUtils.GetRandomString());
        //    dbDatabase.Tables.Add(dbTableInfo3);
        //    treeViewController.LoadTreeView(dbDatabase, 2);
        //    treeViewController.SetVisibility(dbTableInfo2, false);
        //    //-------------Assert Preconditions -------------
        //    Assert.AreEqual(1, treeView.Nodes.Count);
        //    ITreeNode rootNode = treeView.Nodes[0];
        //    Assert.AreEqual(dbDatabase.ToString(), rootNode.Text);
        //    Assert.AreEqual(1, rootNode.Nodes.Count);
        //    ITreeNode relationshipNode = rootNode.Nodes[0];
        //    Assert.AreEqual("Tables", relationshipNode.Text);
        //    Assert.AreEqual(2, relationshipNode.Nodes.Count);
        //    //---------------Execute Test ----------------------
        //    treeViewController.SetVisibility(dbTableInfo2, true);
        //    //---------------Test Result -----------------------
        //    Assert.AreEqual(1, treeView.Nodes.Count);
        //    Assert.AreEqual(dbDatabase.ToString(), rootNode.Text);
        //    Assert.AreEqual(1, rootNode.Nodes.Count);
        //    Assert.AreEqual("Tables", relationshipNode.Text);
        //    Assert.AreEqual(3, relationshipNode.Nodes.Count);
        //    Assert.AreEqual(treeViewController.GetBusinessObjectTreeNode(dbTable), relationshipNode.Nodes[0].OriginalNode);
        //    Assert.AreEqual(treeViewController.GetBusinessObjectTreeNode(dbTableInfo2), relationshipNode.Nodes[1].OriginalNode);
        //    Assert.AreEqual(treeViewController.GetBusinessObjectTreeNode(dbTableInfo3), relationshipNode.Nodes[2].OriginalNode);
        //}

    }
}