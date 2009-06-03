using System.Collections;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;
using Habanero.Test.Structure;
using Habanero.UI.Base;
using Habanero.UI.Win;
using Habanero.Util;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    [TestFixture]
    public class TestTreeViewController
    {
        [TestFixtureSetUp]
        public void SetupFixture()
        {

            //UITestUtils.SetupFixture();
            TestUtil.WaitForGC();
            BORegistry.DataAccessor = new DataAccessorInMemory();
        }

        [SetUp]
        public virtual void SetupTest()
        {
            //-------------------------------------------------------------------------------------
            // You can choose here whether to run against a database or whether to use an in-memory
            // database for all the tests, which runs quickly. It doesn't however check that
            // your database has the correct structure, which is partly the purpose of these tests.
            // The generated tests do already use an in-memory database where possible.
            // In your custom tests, you can set them to use an in-memory database by copying the
            // line to the first line of your test.
            //-------------------------------------------------------------------------------------
            //UITestUtils.SetupTest();

            ClassDef.ClassDefs.Clear();
            ClassDef.LoadClassDefs(new XmlClassDefsLoader(BOBroker.GetClassDefsXml(), new DtdLoader()));
            GlobalUIRegistry.ControlFactory = new ControlFactoryWin();
            //ClassDefModifier classDefModifier = new ClassDefModifier();
            //classDefModifier.InsertAssemblyRelationshipMapper();
            //classDefModifier.InsertRelatedClassRelationshipMapper();
            GlobalRegistry.UIExceptionNotifier = new RethrowingExceptionNotifier();

        }




        //[Test]
        //public void Test_Acceptance_LoadBO_TwoChildren_OneWithNoChildrenAndOneWith2Children_Expanded_ShouldLoadCorrectChildren()
        //{
        //    //---------------Set up test pack-------------------
        //    TreeViewWin treeView = new TreeViewWin();
        //    TreeViewController treeViewController = new TreeViewController(treeView);

        //    DBDatabase dbDatabase = new DBDatabase { DatabaseName = TestUtil.GetRandomString() };
        //    DBTable table1 = CreateDBTable(dbDatabase, "1111");
        //    DBTable table2 = CreateDBTable(dbDatabase, "2220");
        //    table2.Columns.Add(new DBColumn(TestUtil.GetRandomString()));
        //    table2.Columns.Add(new DBColumn(TestUtil.GetRandomString()));
        //    //-------------Assert Preconditions -------------
        //    Assert.AreEqual(0, treeView.Nodes.Count);
        //    Assert.AreEqual(2, dbDatabase.Tables.Count);
        //    Assert.AreEqual(0, table1.Columns.Count);
        //    Assert.AreEqual(2, table2.Columns.Count);
        //    //---------------Execute Test ----------------------
        //    treeViewController.LoadTreeView(dbDatabase, 4);
        //    //---------------Test Result -----------------------
        //    Assert.AreEqual(1, treeView.Nodes.Count);
        //    ITreeNode dbNode = treeView.Nodes[0];
        //    ITreeNode relationshipNode = dbNode.Nodes[0];
        //    Assert.AreEqual(2, relationshipNode.Nodes.Count, "Should have two tables");
        //    AssertChildNodeLoadedInTree(dbDatabase.Tables, relationshipNode, 0);
        //    AssertChildNodeLoadedInTree(dbDatabase.Tables, relationshipNode, 1);
        //    ITreeNode table1Node = relationshipNode.Nodes[0];

        //    ITreeNode columnRelationshipNode = table1Node.Nodes[0];
        //    Assert.AreEqual(0, columnRelationshipNode.Nodes.Count);


        //    ITreeNode table2Node = relationshipNode.Nodes[1];
        //    Assert.AreEqual(table2.ToString(), table2Node.Text);
        //    ITreeNode table2ColumnRelationshipNode = table2Node.Nodes[0];
        //    Assert.AreEqual("Columns", table2ColumnRelationshipNode.Text);
        //    Assert.AreEqual(2, table2.Columns.Count);
        //    Assert.AreEqual(2, table2ColumnRelationshipNode.Nodes.Count);
        //    AssertChildNodeLoadedInTree(table2.Columns, table2ColumnRelationshipNode, 0);
        //    AssertChildNodeLoadedInTree(table2.Columns, table2ColumnRelationshipNode, 1);
        //}

        //    [Test]
        //    public void
        //        Test_Acceptance_LoadBO_TwoChildren_OneWithOneChild_AndOneWith2Children_Expanded_ShouldLoadCorrectChildren()
        //    {
        //        //---------------Set up test pack-------------------
        //        TreeViewWin treeView = new TreeViewWin();
        //        TreeViewController treeViewController = new TreeViewController(treeView);
        //        DBDatabase dbDatabase = new DBDatabase { DatabaseName = TestUtilsShared.GetRandomString() };
        //        DBTable table1 = CreateDBTable(dbDatabase, "1111");
        //        table1.Columns.Add(new DBColumn(TestUtilsShared.GetRandomString()));
        //        DBTable table2 = CreateDBTable(dbDatabase, "2220");
        //        table2.Columns.Add(new DBColumn(TestUtilsShared.GetRandomString()));
        //        table2.Columns.Add(new DBColumn(TestUtilsShared.GetRandomString()));
        //        //-------------Assert Preconditions -------------
        //        Assert.AreEqual(0, treeView.Nodes.Count);
        //        Assert.AreEqual(2, dbDatabase.Tables.Count);
        //        Assert.AreEqual(1, table1.Columns.Count);
        //        Assert.AreEqual(2, table2.Columns.Count);
        //        //---------------Execute Test ----------------------
        //        treeViewController.LoadTreeView(dbDatabase, 4);
        //        //---------------Test Result -----------------------
        //        Assert.AreEqual(1, treeView.Nodes.Count);
        //        ITreeNode dbNode = treeView.Nodes[0];
        //        ITreeNode relationshipNode = dbNode.Nodes[0];
        //        Assert.AreEqual(2, relationshipNode.Nodes.Count, "Should have two tables");
        //        AssertChildNodeLoadedInTree(dbDatabase.Tables, relationshipNode, 0);
        //        AssertChildNodeLoadedInTree(dbDatabase.Tables, relationshipNode, 1);
        //        ITreeNode table1Node = relationshipNode.Nodes[0];

        //        ITreeNode columnRelationshipNode = table1Node.Nodes[0];
        //        Assert.AreEqual(1, columnRelationshipNode.Nodes.Count);
        //        AssertChildNodeLoadedInTree(table1.Columns, columnRelationshipNode, 0);

        //        ITreeNode table2Node = relationshipNode.Nodes[1];
        //        Assert.AreEqual(table2.ToString(), table2Node.Text);
        //        ITreeNode table2ColumnRelationshipNode = table2Node.Nodes[0];
        //        Assert.AreEqual("Columns", table2ColumnRelationshipNode.Text);
        //        Assert.AreEqual(2, table2.Columns.Count);
        //        Assert.AreEqual(2, table2ColumnRelationshipNode.Nodes.Count);
        //        AssertChildNodeLoadedInTree(table2.Columns, table2ColumnRelationshipNode, 0);
        //        AssertChildNodeLoadedInTree(table2.Columns, table2ColumnRelationshipNode, 1);
        //    }

        //    [Test]
        //    public void
        //        Test_Acceptance_ExpandChild_WithTwoChildren_OneWithOneChild_AndOneWith2Children_WhenNotExpandedOnLoad_ShouldLoadCorrectChildren
        //        ()
        //    {
        //        //---------------Set up test pack-------------------
        //        TreeViewWin treeView = new TreeViewWin();
        //        TreeViewController treeViewController = new TreeViewController(treeView);
        //        DBDatabase dbDatabase = new DBDatabase { DatabaseName = TestUtilsShared.GetRandomString() };
        //        DBTable table1 = CreateDBTable(dbDatabase, "1111");
        //        table1.Columns.Add(new DBColumn(TestUtilsShared.GetRandomString()));
        //        DBTable table2 = CreateDBTable(dbDatabase, "2220");
        //        table2.Columns.Add(new DBColumn(TestUtilsShared.GetRandomString()));
        //        table2.Columns.Add(new DBColumn(TestUtilsShared.GetRandomString()));
        //        treeViewController.LoadTreeView(dbDatabase, 3);
        //        //-------------Assert Preconditions -------------
        //        Assert.AreEqual(2, dbDatabase.Tables.Count);
        //        Assert.AreEqual(1, table1.Columns.Count);
        //        Assert.AreEqual(2, table2.Columns.Count);
        //        Assert.AreEqual(1, treeView.Nodes.Count);
        //        ITreeNode dbNode = treeView.Nodes[0];
        //        ITreeNode tablesRelationshipNode = dbNode.Nodes[0];
        //        Assert.AreEqual(2, tablesRelationshipNode.Nodes.Count, "Should have two tables");
        //        AssertChildNodeLoadedInTree(dbDatabase.Tables, tablesRelationshipNode, 0);
        //        AssertChildNodeLoadedInTree(dbDatabase.Tables, tablesRelationshipNode, 1);
        //        ITreeNode table1Node = tablesRelationshipNode.Nodes[0];
        //        ITreeNode table1ColumnRelationshipNode = table1Node.Nodes[0];
        //        Assert.AreEqual("Columns", table1ColumnRelationshipNode.Text);
        //        Assert.AreEqual
        //            (1, table1ColumnRelationshipNode.Nodes.Count,
        //             "Only the dummy should be present because the node is not expanded");
        //        ITreeNode table2Node = tablesRelationshipNode.Nodes[1];
        //        ITreeNode table2ColumnRelationshipNode = table2Node.Nodes[0];
        //        Assert.AreEqual("Columns", table2ColumnRelationshipNode.Text);
        //        Assert.AreEqual
        //            (1, table2ColumnRelationshipNode.Nodes.Count,
        //             "Only the dummy should be present because the node is not expanded");
        //        //---------------Execute Test ----------------------
        //        table2ColumnRelationshipNode.Expand();
        //        ReflectionUtilities.ExecutePrivateMethod(treeViewController, "ExpandNode", table2ColumnRelationshipNode);
        //        //---------------Test Result -----------------------
        //        Assert.AreEqual(2, table2.Columns.Count);
        //        Assert.AreEqual
        //            (1, table1ColumnRelationshipNode.Nodes.Count, "Columns in table 1 should still be the dummy node");
        //        Assert.AreNotEqual(table1.Columns[0].ToString(), table1ColumnRelationshipNode.Nodes[0].Text);
        //        AssertChildNodeLoadedInTree(table2.Columns, table2ColumnRelationshipNode, 0);
        //        Assert.AreEqual
        //            (2, table2ColumnRelationshipNode.Nodes.Count,
        //             "Both columns in table 2 should now be shown since it is expanded");
        //        AssertChildNodeLoadedInTree(table2.Columns, table2ColumnRelationshipNode, 1);
        //    }

        //    [Test]
        //    public void Test_Acceptance_WhenAddBusinessObjectToChildCollection_ShouldAddToTree()
        //    {
        //        //---------------Set up test pack-------------------
        //        TreeViewWin treeView = new TreeViewWin();
        //        TreeViewController treeViewController = new TreeViewController(treeView);
        //        DBDatabase dbDatabase = new DBDatabase { DatabaseName = TestUtilsShared.GetRandomString() };
        //        DBTable table1 = CreateDBTable(dbDatabase, "1111");
        //        table1.Columns.Add(new DBColumn(TestUtilsShared.GetRandomString()));
        //        DBTable table2 = CreateDBTable(dbDatabase, "2220");
        //        table2.Columns.Add(new DBColumn(TestUtilsShared.GetRandomString()));
        //        table2.Columns.Add(new DBColumn(TestUtilsShared.GetRandomString()));
        //        treeViewController.LoadTreeView(dbDatabase, 4);
        //        ITreeNode dbNode = treeView.Nodes[0];
        //        ITreeNode relationshipNode = dbNode.Nodes[0];
        //        ITreeNode table2Node = relationshipNode.Nodes[1];
        //        //-------------Assert Preconditions -------------
        //        Assert.AreEqual(table2.ToString(), table2Node.Text);
        //        ITreeNode table2ColumnRelationshipNode = table2Node.Nodes[0];
        //        Assert.AreEqual("Columns", table2ColumnRelationshipNode.Text);
        //        Assert.AreEqual(2, table2.Columns.Count);
        //        Assert.AreEqual(2, table2ColumnRelationshipNode.Nodes.Count);
        //        AssertChildNodeLoadedInTree(table2.Columns, table2ColumnRelationshipNode, 0);
        //        AssertChildNodeLoadedInTree(table2.Columns, table2ColumnRelationshipNode, 1);
        //        //---------------Execute Test ----------------------

        //        table2.Columns.Add(new DBColumn(TestUtilsShared.GetRandomString()));
        //        //---------------Test Result -----------------------
        //        Assert.AreEqual(3, table2ColumnRelationshipNode.Nodes.Count);
        //    }

        //    [Test]
        //    public void Test_Acceptance_WhenRemoveBusinessObjectFromChildCollection_ShouldRemoveFromTree()
        //    {
        //        //---------------Set up test pack-------------------
        //        TreeViewWin treeView = new TreeViewWin();
        //        TreeViewController treeViewController = new TreeViewController(treeView);
        //        DBDatabase dbDatabase = new DBDatabase { DatabaseName = TestUtilsShared.GetRandomString() };
        //        DBTable table1 = CreateDBTable(dbDatabase, "1111");
        //        table1.Columns.Add(new DBColumn(TestUtilsShared.GetRandomString()));
        //        DBTable table2 = CreateDBTable(dbDatabase, "2220");
        //        table2.Columns.Add(new DBColumn(TestUtilsShared.GetRandomString()));
        //        DBColumn column2 = new DBColumn(TestUtilsShared.GetRandomString());
        //        table2.Columns.Add(column2);
        //        treeViewController.LoadTreeView(dbDatabase, 4);
        //        ITreeNode dbNode = treeView.Nodes[0];
        //        ITreeNode relationshipNode = dbNode.Nodes[0];
        //        ITreeNode table2Node = relationshipNode.Nodes[1];
        //        //-------------Assert Preconditions -------------
        //        Assert.AreEqual(table2.ToString(), table2Node.Text);
        //        ITreeNode table2ColumnRelationshipNode = table2Node.Nodes[0];
        //        Assert.AreEqual("Columns", table2ColumnRelationshipNode.Text);
        //        Assert.AreEqual(2, table2.Columns.Count);
        //        Assert.AreEqual(2, table2ColumnRelationshipNode.Nodes.Count);
        //        AssertChildNodeLoadedInTree(table2.Columns, table2ColumnRelationshipNode, 0);
        //        AssertChildNodeLoadedInTree(table2.Columns, table2ColumnRelationshipNode, 1);
        //        //---------------Execute Test ----------------------
        //        table2.Columns.Remove(column2);
        //        //---------------Test Result -----------------------
        //        Assert.AreEqual(1, table2ColumnRelationshipNode.Nodes.Count);
        //    }

        //    [Test]
        //    public void Test_Acceptance_WhenRemoveBusinessObjectFromChildCollectionAndThenAddBack_ShouldAddBackToTree()
        //    {
        //        //---------------Set up test pack-------------------
        //        TreeViewWin treeView = new TreeViewWin();
        //        TreeViewController treeViewController = new TreeViewController(treeView);
        //        DBDatabase dbDatabase = new DBDatabase { DatabaseName = TestUtilsShared.GetRandomString() };
        //        DBTable table1 = CreateDBTable(dbDatabase, "1111");
        //        table1.Columns.Add(new DBColumn(TestUtilsShared.GetRandomString()));
        //        DBTable table2 = CreateDBTable(dbDatabase, "2220");
        //        table2.Columns.Add(new DBColumn(TestUtilsShared.GetRandomString()));
        //        DBColumn column2 = new DBColumn(TestUtilsShared.GetRandomString());
        //        table2.Columns.Add(column2);
        //        treeViewController.LoadTreeView(dbDatabase, 4);
        //        ITreeNode dbNode = treeView.Nodes[0];
        //        ITreeNode relationshipNode = dbNode.Nodes[0];
        //        ITreeNode table2Node = relationshipNode.Nodes[1];
        //        //-------------Assert Preconditions -------------
        //        Assert.AreEqual(table2.ToString(), table2Node.Text);
        //        ITreeNode table2ColumnRelationshipNode = table2Node.Nodes[0];
        //        Assert.AreEqual("Columns", table2ColumnRelationshipNode.Text);
        //        Assert.AreEqual(2, table2.Columns.Count);
        //        Assert.AreEqual(2, table2ColumnRelationshipNode.Nodes.Count);
        //        AssertChildNodeLoadedInTree(table2.Columns, table2ColumnRelationshipNode, 0);
        //        AssertChildNodeLoadedInTree(table2.Columns, table2ColumnRelationshipNode, 1);
        //        //---------------Execute Test ----------------------
        //        table2.Columns.Remove(column2);
        //        table2.Columns.Add(column2);
        //        //---------------Test Result -----------------------
        //        Assert.AreEqual(table2.ToString(), table2Node.Text);
        //        table2ColumnRelationshipNode = table2Node.Nodes[0];
        //        Assert.AreEqual("Columns", table2ColumnRelationshipNode.Text);
        //        Assert.AreEqual(2, table2.Columns.Count);
        //        Assert.AreEqual(2, table2ColumnRelationshipNode.Nodes.Count);
        //        AssertChildNodeLoadedInTree(table2.Columns, table2ColumnRelationshipNode, 0);
        //        AssertChildNodeLoadedInTree(table2.Columns, table2ColumnRelationshipNode, 1);
        //    }

        //    [Test]
        //    public void
        //        Test_Acceptance_WhenRemoveBusinessObjectThatHasChildrenThenAddBack_ShouldAddBackToTree_AndAddChildrenBack()
        //    {
        //        //---------------Set up test pack-------------------
        //        TreeViewWin treeView = new TreeViewWin();
        //        TreeViewController treeViewController = new TreeViewController(treeView);
        //        DBDatabase dbDatabase = new DBDatabase { DatabaseName = TestUtilsShared.GetRandomString() };
        //        DBTable table1 = CreateDBTable(dbDatabase, "1111");
        //        table1.Columns.Add(new DBColumn(TestUtilsShared.GetRandomString()));
        //        DBTable table2 = CreateDBTable(dbDatabase, "2220");
        //        table2.Columns.Add(new DBColumn(TestUtilsShared.GetRandomString()));
        //        DBColumn column2 = new DBColumn(TestUtilsShared.GetRandomString());
        //        table2.Columns.Add(column2);
        //        treeViewController.LoadTreeView(dbDatabase, 4);
        //        ITreeNode dbNode = treeView.Nodes[0];
        //        ITreeNode relationshipNode = dbNode.Nodes[0];

        //        dbDatabase.Tables.Remove(table2);
        //        //-------------Assert Preconditions -------------
        //        Assert.AreEqual(1, relationshipNode.Nodes.Count);
        //        Assert.AreEqual(2, table2.Columns.Count);
        //        //---------------Execute Test ----------------------

        //        dbDatabase.Tables.Add(table2);
        //        ITreeNode table2Node = relationshipNode.Nodes[1];
        //        table2Node.ExpandAll();
        //        ReflectionUtilities.ExecutePrivateMethod(treeViewController, "ExpandNode", table2Node);

        //        //---------------Test Result -----------------------
        //        Assert.AreEqual(2, relationshipNode.Nodes.Count);
        //        table2Node = relationshipNode.Nodes[1];
        //        Assert.AreEqual(table2.ToString(), table2Node.Text);
        //        Assert.AreEqual(4, table2Node.Nodes.Count);
        //        ITreeNode table2ColumnRelationshipNode = table2Node.Nodes[0];
        //        Assert.AreEqual("Columns", table2ColumnRelationshipNode.Text);
        //        Assert.AreEqual(2, table2.Columns.Count);
        //        ReflectionUtilities.ExecutePrivateMethod(treeViewController, "ExpandNode", table2ColumnRelationshipNode);
        //        Assert.AreEqual(2, table2ColumnRelationshipNode.Nodes.Count);
        //        AssertChildNodeLoadedInTree(table2.Columns, table2ColumnRelationshipNode, 0);
        //        AssertChildNodeLoadedInTree(table2.Columns, table2ColumnRelationshipNode, 1);
        //    }

        //    [Test]
        //    public void Test_WhenRemoveBusinessObject_ShouldRemoveChildrenFromNodeStates()
        //    {
        //        //---------------Set up test pack-------------------
        //        TreeViewWin treeView = new TreeViewWin();
        //        TreeViewController treeViewController = new TreeViewController(treeView);
        //        DBDatabase dbDatabase = new DBDatabase { DatabaseName = TestUtilsShared.GetRandomString() };
        //        DBTable table1 = CreateDBTable(dbDatabase, "1111");
        //        table1.Columns.Add(new DBColumn(TestUtilsShared.GetRandomString()));
        //        DBTable table2 = CreateDBTable(dbDatabase, "2220");
        //        table2.Columns.Add(new DBColumn(TestUtilsShared.GetRandomString()));
        //        DBColumn column2 = new DBColumn(TestUtilsShared.GetRandomString());
        //        table2.Columns.Add(column2);
        //        treeViewController.LoadTreeView(dbDatabase, 4);
        //        ITreeNode dbNode = treeView.Nodes[0];
        //        ITreeNode relationshipNode = dbNode.Nodes[0];
        //        //-------------Assert Preconditions -------------
        //        //---------------Execute Test ----------------------
        //        dbDatabase.Tables.Remove(table2);
        //        //---------------Test Result -----------------------
        //        Assert.AreEqual(1, relationshipNode.Nodes.Count);
        //        IDictionary objectNodes =
        //            (IDictionary)ReflectionUtilities.GetPrivatePropertyValue(treeViewController, "ObjectNodes");

        //        Assert.IsFalse(objectNodes.Contains(table2));
        //        Assert.IsFalse(objectNodes.Contains(table2.Columns[0]));
        //        Assert.IsFalse(objectNodes.Contains(table2.Columns[1]));

        //        IDictionary relationshipNodes =
        //            (IDictionary)ReflectionUtilities.GetPrivatePropertyValue(treeViewController, "RelationshipNodes");
        //        Assert.IsFalse(relationshipNodes.Contains(table2.Relationships["Columns"]));


        //        IDictionary childCollectionNodes =
        //            (IDictionary)ReflectionUtilities.GetPrivatePropertyValue(treeViewController, "ChildCollectionNodes");
        //        Assert.IsFalse(childCollectionNodes.Contains(table2.Columns));
        //    }

        //    [Test]
        //    public void Test_RootNodeBusinessObject()
        //    {
        //        //---------------Set up test pack-------------------
        //        TreeViewWin treeView = new TreeViewWin();
        //        TreeViewController treeViewController = new TreeViewController(treeView);
        //        DBColumn dbColumn = new DBColumn(TestUtilsShared.GetRandomString());
        //        treeViewController.LoadTreeView(dbColumn);
        //        //-------------Assert Preconditions -------------
        //        Assert.AreEqual(1, treeView.Nodes.Count);
        //        //---------------Execute Test ----------------------
        //        IBusinessObject businessObject = treeViewController.RootNodeBusinessObject;
        //        //---------------Test Result -----------------------
        //        Assert.AreSame(dbColumn, businessObject);
        //    }

        //    //TODO: Soriya 2009/03/18
        //    //[Test]
        //    //public void Test_LoadNull()
        //    //{
        //    //    //---------------Set up test pack-------------------
        //    //    TreeViewWin treeView = new TreeViewWin();
        //    //    TreeViewController treeViewController = new TreeViewController(treeView);
        //    //    //-------------Assert Preconditions -------------
        //    //    Assert.AreEqual(0, treeView.Nodes.Count);
        //    //    //---------------Execute Test ----------------------
        //    //    treeViewController.LoadTreeView(null);
        //    //    //---------------Test Result -----------------------
        //    //    Assert.AreEqual(0, treeView.Nodes.Count);
        //    //}

        //    [Test]
        //    public void Test_LoadSingleBO_NoChildren()
        //    {
        //        //---------------Set up test pack-------------------
        //        TreeViewWin treeView = new TreeViewWin();
        //        TreeViewController treeViewController = new TreeViewController(treeView);
        //        DBColumn dbColumn = new DBColumn(TestUtilsShared.GetRandomString());
        //        //-------------Assert Preconditions -------------
        //        Assert.AreEqual(0, treeView.Nodes.Count);
        //        //---------------Execute Test ----------------------
        //        treeViewController.LoadTreeView(dbColumn);
        //        //---------------Test Result -----------------------
        //        Assert.AreEqual(1, treeView.Nodes.Count);
        //        ITreeNode node = treeView.Nodes[0];
        //        Assert.AreEqual(dbColumn.ToString(), node.Text);
        //        Assert.AreEqual(0, node.Nodes.Count);
        //    }

        //    [Test]
        //    public void Test_LoadSingleBO_OneChildRelationship_NotExpanded_HasADummyNode()
        //    {
        //        //---------------Set up test pack-------------------
        //        TreeViewWin treeView = new TreeViewWin();
        //        TreeViewController treeViewController = new TreeViewController(treeView);
        //        DBDatabase dbDatabase = new DBDatabase { DatabaseName = TestUtilsShared.GetRandomString() };
        //        //-------------Assert Preconditions -------------
        //        Assert.AreEqual(0, treeView.Nodes.Count);
        //        //---------------Execute Test ----------------------
        //        treeViewController.LoadTreeView(dbDatabase);
        //        //---------------Test Result -----------------------
        //        Assert.AreEqual(1, treeView.Nodes.Count);
        //        ITreeNode dbNode = treeView.Nodes[0];
        //        Assert.AreEqual(dbDatabase.ToString(), dbNode.Text);
        //        Assert.AreEqual(1, dbNode.Nodes.Count);
        //        ITreeNode relationshipNode = dbNode.Nodes[0];
        //        Assert.AreEqual
        //            ("$DUMMY$", relationshipNode.Text,
        //             "Dummy node present so that the tree view has the + icon for expanding");
        //    }

        //    [Test]
        //    public void Test_LoadSingleBO_OneChildRelationship_Expanded()
        //    {
        //        //---------------Set up test pack-------------------
        //        TreeViewWin treeView = new TreeViewWin();
        //        TreeViewController treeViewController = new TreeViewController(treeView);
        //        DBDatabase dbDatabase = new DBDatabase { DatabaseName = TestUtilsShared.GetRandomString() };
        //        //-------------Assert Preconditions -------------
        //        Assert.AreEqual(0, treeView.Nodes.Count);
        //        //---------------Execute Test ----------------------
        //        treeViewController.LoadTreeView(dbDatabase, 1);
        //        //---------------Test Result -----------------------
        //        Assert.AreEqual(1, treeView.Nodes.Count);
        //        ITreeNode dbNode = treeView.Nodes[0];
        //        Assert.AreEqual(dbDatabase.ToString(), dbNode.Text);
        //        Assert.AreEqual(1, dbNode.Nodes.Count);
        //        ITreeNode tablesRelationshipNode = dbNode.Nodes[0];
        //        Assert.AreEqual
        //            ("Tables", tablesRelationshipNode.Text,
        //             "This is expanded so the Tables Relationship should be loaded into the tree view");
        //    }

        //    [Test]
        //    public void Test_LoadSingleBO_OneChild_Expanded()
        //    {
        //        //---------------Set up test pack-------------------
        //        TreeViewWin treeView = new TreeViewWin();
        //        TreeViewController treeViewController = new TreeViewController(treeView);
        //        DBDatabase dbDatabase = new DBDatabase { DatabaseName = TestUtilsShared.GetRandomString() };
        //        DBTable dbTable = new DBTable(TestUtilsShared.GetRandomString());
        //        dbDatabase.Tables.Add(dbTable);
        //        //-------------Assert Preconditions -------------
        //        Assert.AreEqual(0, treeView.Nodes.Count);
        //        //---------------Execute Test ----------------------
        //        treeViewController.LoadTreeView(dbDatabase, 2);
        //        //---------------Test Result -----------------------
        //        Assert.AreEqual(1, treeView.Nodes.Count);
        //        ITreeNode dbNode = treeView.Nodes[0];
        //        Assert.AreEqual(dbDatabase.ToString(), dbNode.Text);
        //        Assert.AreEqual(1, dbNode.Nodes.Count);
        //        ITreeNode relationshipNode = dbNode.Nodes[0];
        //        Assert.AreEqual("Tables", relationshipNode.Text);
        //        Assert.AreEqual(1, relationshipNode.Nodes.Count);
        //        ITreeNode tableBONode = relationshipNode.Nodes[0];
        //        Assert.AreEqual(dbTable.ToString(), tableBONode.Text);
        //    }

        //    [Test]
        //    public void Test_LoadSingleBO_TwoChildren_Expanded()
        //    {
        //        //---------------Set up test pack-------------------
        //        TreeViewWin treeView = new TreeViewWin();
        //        TreeViewController treeViewController = new TreeViewController(treeView);
        //        DBDatabase dbDatabase = new DBDatabase { DatabaseName = TestUtilsShared.GetRandomString() };
        //        DBTable dbTable = new DBTable("1111");
        //        dbDatabase.Tables.Add(dbTable);
        //        dbDatabase.Tables.Add(new DBTable("2222"));
        //        //-------------Assert Preconditions -------------
        //        Assert.AreEqual(0, treeView.Nodes.Count);
        //        Assert.AreEqual(2, dbDatabase.Tables.Count);
        //        //---------------Execute Test ----------------------
        //        treeViewController.LoadTreeView(dbDatabase, 2);
        //        //---------------Test Result -----------------------
        //        Assert.AreEqual(1, treeView.Nodes.Count);
        //        ITreeNode dbNode = treeView.Nodes[0];
        //        ITreeNode relationshipNode = dbNode.Nodes[0];
        //        Assert.AreEqual(2, relationshipNode.Nodes.Count, "Should have two tables");
        //        AssertChildNodeLoadedInTree(dbDatabase.Tables, relationshipNode, 0);

        //        AssertChildNodeLoadedInTree(dbDatabase.Tables, relationshipNode, 1);
        //    }

        //    private static DBTable CreateDBTable(DBDatabase dbDatabase, string tableName)
        //    {
        //        DBTable dbTable = new DBTable(tableName);
        //        dbDatabase.Tables.Add(dbTable);
        //        return dbTable;
        //    }

        //    /// <summary>
        //    /// Verifies that the item in the tree nodes collection identified by the index is matched to the the 
        //    ///  item in the Business object collection identified by the index. I.e. the ordering of the nodes collection matches the 
        //    ///  business object collection.
        //    /// </summary>
        //    /// <param name="relationship"></param>
        //    /// <param name="relationshipNode"></param>
        //    /// <param name="index"></param>
        //    private static void AssertChildNodeLoadedInTree
        //        (IBusinessObjectCollection relationship, ITreeNode relationshipNode, int index)
        //    {
        //        Assert.AreEqual(relationship[index].ToString(), relationshipNode.Nodes[index].Text);
        //    }

        //    [Test]
        //    public void Test_SetupNodeWithBusinessObject_LoadSingleBo_CustomDisplayValue()
        //    {
        //        //---------------Set up test pack-------------------
        //        TreeViewWin treeView = new TreeViewWin();
        //        TreeViewController treeViewController = new TreeViewController(treeView);
        //        string customDisplayValue = TestUtilsShared.GetRandomString();
        //        DBColumn dbColumn = new DBColumn("ColumnName");
        //        //-------------Assert Preconditions -------------
        //        Assert.AreEqual(0, treeView.Nodes.Count);
        //        //---------------Execute Test ----------------------
        //        treeViewController.SetupNodeWithBusinessObject +=
        //            delegate(ITreeNode treeNode, IBusinessObject businessObject) { treeNode.Text = customDisplayValue; };
        //        treeViewController.LoadTreeView(dbColumn);
        //        //---------------Test Result -----------------------
        //        Assert.AreEqual(1, treeView.Nodes.Count);
        //        ITreeNode node = treeView.Nodes[0];
        //        Assert.AreEqual(customDisplayValue, node.Text);
        //        Assert.AreEqual(0, node.Nodes.Count);
        //    }

        //    [Test]
        //    public void Test_GetBusinessObjectTreeNode_RootNode()
        //    {
        //        //---------------Set up test pack-------------------
        //        TreeViewWin treeView = new TreeViewWin();
        //        TreeViewController treeViewController = new TreeViewController(treeView);
        //        DBColumn dbColumn = new DBColumn("ColumnName");
        //        treeViewController.LoadTreeView(dbColumn);
        //        //-------------Assert Preconditions -------------
        //        Assert.AreEqual(1, treeView.Nodes.Count);
        //        //---------------Execute Test ----------------------
        //        ITreeNode fetchedTreeNode = treeViewController.GetBusinessObjectTreeNode(dbColumn);
        //        //---------------Test Result -----------------------
        //        Assert.IsNotNull(fetchedTreeNode);
        //        Assert.AreSame(fetchedTreeNode, treeView.Nodes[0]);
        //    }

        //    [Test]
        //    public void Test_GetBusinessObjectTreeNode_ChildNode()
        //    {
        //        //---------------Set up test pack-------------------
        //        TreeViewWin treeView = new TreeViewWin();
        //        TreeViewController treeViewController = new TreeViewController(treeView);
        //        DBTable dbTable = new DBTable("TableName");
        //        DBColumn dbColumn = new DBColumn("ColumnName");
        //        dbTable.Columns.Add(dbColumn);
        //        treeViewController.LoadTreeView(dbTable, 2);
        //        //-------------Assert Preconditions -------------
        //        Assert.AreEqual(1, treeView.Nodes.Count);
        //        Assert.AreEqual(4, treeView.Nodes[0].Nodes.Count);
        //        Assert.AreEqual(1, treeView.Nodes[0].Nodes[0].Nodes.Count);
        //        //---------------Execute Test ----------------------
        //        ITreeNode fetchedTreeNode = treeViewController.GetBusinessObjectTreeNode(dbColumn);
        //        //---------------Test Result -----------------------
        //        Assert.IsNotNull(fetchedTreeNode);
        //        Assert.AreSame(fetchedTreeNode, treeView.Nodes[0].Nodes[0].Nodes[0]);
        //    }

        //    [Test]
        //    public void Test_GetBusinessObjectTreeNode_NotInTree()
        //    {
        //        //---------------Set up test pack-------------------
        //        TreeViewWin treeView = new TreeViewWin();
        //        TreeViewController treeViewController = new TreeViewController(treeView);
        //        DBColumn dbColumn = new DBColumn("ColumnName");
        //        treeViewController.LoadTreeView(dbColumn);
        //        //-------------Assert Preconditions -------------
        //        Assert.AreEqual(1, treeView.Nodes.Count);
        //        //---------------Execute Test ----------------------
        //        ITreeNode fetchedTreeNode = treeViewController.GetBusinessObjectTreeNode(new DBColumn());
        //        //---------------Test Result -----------------------
        //        Assert.IsNull(fetchedTreeNode);
        //    }

        //    [Test]
        //    public void Test_SetBusinessObjectVisibility_False_ShouldHideNode()
        //    {
        //        //---------------Set up test pack-------------------
        //        TreeViewWin treeView = new TreeViewWin();
        //        TreeViewController treeViewController = new TreeViewController(treeView);
        //        DBDatabase dbDatabase = new DBDatabase { DatabaseName = TestUtilsShared.GetRandomString() };
        //        DBTable dbTable = new DBTable(TestUtilsShared.GetRandomString());
        //        dbDatabase.Tables.Add(dbTable);
        //        treeViewController.LoadTreeView(dbDatabase, 2);
        //        //-------------Assert Preconditions -------------
        //        Assert.AreEqual(1, treeView.Nodes.Count);
        //        ITreeNode rootNode = treeView.Nodes[0];
        //        Assert.AreEqual(dbDatabase.ToString(), rootNode.Text);
        //        Assert.AreEqual(1, rootNode.Nodes.Count);
        //        ITreeNode relationshipNode = rootNode.Nodes[0];
        //        Assert.AreEqual("Tables", relationshipNode.Text);
        //        Assert.AreEqual(1, relationshipNode.Nodes.Count);
        //        ITreeNode childNode = relationshipNode.Nodes[0];
        //        Assert.AreEqual(dbTable.ToString(), childNode.Text);
        //        //---------------Execute Test ----------------------
        //        treeViewController.SetVisibility(dbTable, false);
        //        //---------------Test Result -----------------------
        //        Assert.AreEqual(1, treeView.Nodes.Count);
        //        Assert.AreEqual(dbDatabase.ToString(), rootNode.Text);
        //        Assert.AreEqual(1, rootNode.Nodes.Count);
        //        Assert.AreEqual("Tables", relationshipNode.Text);
        //        Assert.AreEqual(0, relationshipNode.Nodes.Count);
        //    }

        //    [Test]
        //    public void Test_SetBusinessObjectVisibility_True_ShouldShowNode()
        //    {
        //        //---------------Set up test pack-------------------
        //        TreeViewWin treeView = new TreeViewWin();
        //        TreeViewController treeViewController = new TreeViewController(treeView);
        //        DBDatabase dbDatabase = new DBDatabase { DatabaseName = TestUtilsShared.GetRandomString() };
        //        DBTable dbTable = new DBTable(TestUtilsShared.GetRandomString());
        //        dbDatabase.Tables.Add(dbTable);
        //        treeViewController.LoadTreeView(dbDatabase, 2);
        //        //-------------Assert Preconditions -------------
        //        Assert.AreEqual(1, treeView.Nodes.Count);
        //        ITreeNode rootNode = treeView.Nodes[0];
        //        Assert.AreEqual(dbDatabase.ToString(), rootNode.Text);
        //        Assert.AreEqual(1, rootNode.Nodes.Count);
        //        ITreeNode relationshipNode = rootNode.Nodes[0];
        //        Assert.AreEqual("Tables", relationshipNode.Text);
        //        Assert.AreEqual(1, relationshipNode.Nodes.Count);
        //        ITreeNode childNode = relationshipNode.Nodes[0];
        //        Assert.AreEqual(dbTable.ToString(), childNode.Text);
        //        //---------------Execute Test ----------------------
        //        treeViewController.SetVisibility(dbTable, true);
        //        //---------------Test Result -----------------------
        //        Assert.AreEqual(1, treeView.Nodes.Count);
        //        Assert.AreEqual(dbDatabase.ToString(), rootNode.Text);
        //        Assert.AreEqual(1, rootNode.Nodes.Count);
        //        Assert.AreEqual("Tables", relationshipNode.Text);
        //        Assert.AreEqual(1, relationshipNode.Nodes.Count);
        //    }

        //    [Test]
        //    public void Test_SetVisibility_False_ThenTrue()
        //    {
        //        //---------------Set up test pack-------------------
        //        TreeViewWin treeView = new TreeViewWin();
        //        TreeViewController treeViewController = new TreeViewController(treeView);
        //        DBDatabase dbDatabase = new DBDatabase { DatabaseName = TestUtilsShared.GetRandomString() };
        //        DBTable dbTable = new DBTable(TestUtilsShared.GetRandomString());
        //        dbDatabase.Tables.Add(dbTable);
        //        treeViewController.LoadTreeView(dbDatabase, 2);
        //        treeViewController.SetVisibility(dbTable, false);
        //        //-------------Assert Preconditions -------------
        //        Assert.AreEqual(1, treeView.Nodes.Count);
        //        ITreeNode rootNode = treeView.Nodes[0];
        //        Assert.AreEqual(dbDatabase.ToString(), rootNode.Text);
        //        Assert.AreEqual(1, rootNode.Nodes.Count);
        //        ITreeNode relationshipNode = rootNode.Nodes[0];
        //        Assert.AreEqual("Tables", relationshipNode.Text);
        //        Assert.AreEqual(0, relationshipNode.Nodes.Count);
        //        //---------------Execute Test ----------------------
        //        treeViewController.SetVisibility(dbTable, true);
        //        //---------------Test Result -----------------------
        //        Assert.AreEqual(1, treeView.Nodes.Count);
        //        Assert.AreEqual(dbDatabase.ToString(), rootNode.Text);
        //        Assert.AreEqual(1, rootNode.Nodes.Count);
        //        Assert.AreEqual("Tables", relationshipNode.Text);
        //        Assert.AreEqual(1, relationshipNode.Nodes.Count);
        //    }

        //    [Test]
        //    public void Test_SetVisibility_False_ThenTrue_CorrectOrder()
        //    {
        //        //---------------Set up test pack-------------------
        //        TreeViewWin treeView = new TreeViewWin();
        //        TreeViewController treeViewController = new TreeViewController(treeView);
        //        DBDatabase dbDatabase = new DBDatabase { DatabaseName = TestUtilsShared.GetRandomString() };
        //        DBTable dbTable = new DBTable(TestUtilsShared.GetRandomString());
        //        dbDatabase.Tables.Add(dbTable);
        //        DBTable dbTableInfo2 = new DBTable(TestUtilsShared.GetRandomString());
        //        dbDatabase.Tables.Add(dbTableInfo2);
        //        DBTable dbTableInfo3 = new DBTable(TestUtilsShared.GetRandomString());
        //        dbDatabase.Tables.Add(dbTableInfo3);
        //        treeViewController.LoadTreeView(dbDatabase, 2);
        //        treeViewController.SetVisibility(dbTableInfo2, false);
        //        //-------------Assert Preconditions -------------
        //        Assert.AreEqual(1, treeView.Nodes.Count);
        //        ITreeNode rootNode = treeView.Nodes[0];
        //        Assert.AreEqual(dbDatabase.ToString(), rootNode.Text);
        //        Assert.AreEqual(1, rootNode.Nodes.Count);
        //        ITreeNode relationshipNode = rootNode.Nodes[0];
        //        Assert.AreEqual("Tables", relationshipNode.Text);
        //        Assert.AreEqual(2, relationshipNode.Nodes.Count);
        //        //---------------Execute Test ----------------------
        //        treeViewController.SetVisibility(dbTableInfo2, true);
        //        //---------------Test Result -----------------------
        //        Assert.AreEqual(1, treeView.Nodes.Count);
        //        Assert.AreEqual(dbDatabase.ToString(), rootNode.Text);
        //        Assert.AreEqual(1, rootNode.Nodes.Count);
        //        Assert.AreEqual("Tables", relationshipNode.Text);
        //        Assert.AreEqual(3, relationshipNode.Nodes.Count);
        //        Assert.AreEqual(treeViewController.GetBusinessObjectTreeNode(dbTable), relationshipNode.Nodes[0]);
        //        Assert.AreEqual(treeViewController.GetBusinessObjectTreeNode(dbTableInfo2), relationshipNode.Nodes[1]);
        //        Assert.AreEqual(treeViewController.GetBusinessObjectTreeNode(dbTableInfo3), relationshipNode.Nodes[2]);
        //    }

        //    [Test]
        //    public void Test_LoadBusinessObjectCollection()
        //    {
        //        //---------------Set up test pack-------------------
        //        TreeViewWin treeView = new TreeViewWin();
        //        TreeViewController treeViewController = new TreeViewController(treeView);
        //        DBColumn dbColumn = new DBColumn(TestUtilsShared.GetRandomString());
        //        IBusinessObjectCollection dbColumns = new BusinessObjectCollection<DBColumn> { dbColumn };
        //        //-------------Assert Preconditions -------------
        //        Assert.AreEqual(1, dbColumns.Count);
        //        Assert.AreEqual(0, treeView.Nodes.Count);
        //        //---------------Execute Test ----------------------
        //        treeViewController.LoadTreeView(dbColumns);
        //        //---------------Test Result -----------------------
        //        Assert.AreEqual(1, treeView.Nodes.Count);
        //        ITreeNode node = treeView.Nodes[0];
        //        Assert.AreEqual(dbColumn.ToString(), node.Text);
        //        Assert.AreEqual(0, node.Nodes.Count);

        //        //Assert.AreEqual(1, treeView.Nodes.Count);
        //        //ITreeNode node = treeView.Nodes[0];
        //        //Assert.AreEqual("DBColumns", node.Text);
        //        //Assert.AreEqual(1, node.Nodes.Count);
        //        //ITreeNode relationshipNode = node.Nodes[0];
        //        //Assert.AreEqual(dbColumn.ToString(), relationshipNode.Text);
        //    }

        //    [Test]
        //    public void Test_LoadBusinessObjectCollection_TwoItems()
        //    {
        //        //---------------Set up test pack-------------------
        //        TreeViewWin treeView = new TreeViewWin();
        //        TreeViewController treeViewController = new TreeViewController(treeView);
        //        DBColumn dbColumn = new DBColumn(TestUtilsShared.GetRandomString());
        //        DBColumn dbColumn2 = new DBColumn(TestUtilsShared.GetRandomString());
        //        IBusinessObjectCollection dbColumns = new BusinessObjectCollection<DBColumn> { dbColumn, dbColumn2 };
        //        //-------------Assert Preconditions -------------
        //        Assert.AreEqual(2, dbColumns.Count);
        //        Assert.AreEqual(0, treeView.Nodes.Count);
        //        //---------------Execute Test ----------------------
        //        treeViewController.LoadTreeView(dbColumns);
        //        //---------------Test Result -----------------------
        //        Assert.AreEqual(2, treeView.Nodes.Count);
        //        ITreeNode node = treeView.Nodes[0];
        //        Assert.AreEqual(dbColumn.ToString(), node.Text);
        //        Assert.AreEqual(0, node.Nodes.Count);
        //        ITreeNode node2 = treeView.Nodes[1];
        //        Assert.AreEqual(dbColumn2.ToString(), node2.Text);
        //        Assert.AreEqual(0, node2.Nodes.Count);
        //    }

        //    [Test]
        //    public void Test_LoadBusinessObjectCollection_OneChild_Expanded()
        //    {
        //        //---------------Set up test pack-------------------
        //        TreeViewWin treeView = new TreeViewWin();
        //        TreeViewController treeViewController = new TreeViewController(treeView);
        //        DBDatabase dbDatabase = new DBDatabase { DatabaseName = "Database" + TestUtilsShared.GetRandomString() };
        //        DBTable dbTable = new DBTable("Table" + TestUtilsShared.GetRandomString());
        //        dbDatabase.Tables.Add(dbTable);
        //        IBusinessObjectCollection dbDatabases = new BusinessObjectCollection<DBDatabase> { dbDatabase };
        //        //-------------Assert Preconditions -------------
        //        Assert.AreEqual(1, dbDatabases.Count);
        //        Assert.AreEqual(0, treeView.Nodes.Count);
        //        //---------------Execute Test ----------------------
        //        treeViewController.LoadTreeView(dbDatabases, 2);
        //        //---------------Test Result -----------------------
        //        Assert.AreEqual(1, treeView.Nodes.Count);
        //        ITreeNode node = treeView.Nodes[0];
        //        Assert.AreEqual(dbDatabase.ToString(), node.Text);
        //        Assert.AreEqual(1, node.Nodes.Count);
        //        ITreeNode relationshipNode = node.Nodes[0];
        //        Assert.AreEqual("Tables", relationshipNode.Text);
        //        Assert.AreEqual(1, relationshipNode.Nodes.Count);
        //        ITreeNode childNode = relationshipNode.Nodes[0];
        //        Assert.AreEqual(dbTable.ToString(), childNode.Text);
        //    }

        //    [Test]
        //    [Ignore("Need to implement")] //TODO Brett 18 Mar 2009: Ignored Test - Need to implement
        //    public void Test_LoadBusinessObjectCollection_Expanded_ChildAdded()
        //    {
        //        //---------------Set up test pack-------------------
        //        TreeViewWin treeView = new TreeViewWin();
        //        TreeViewController treeViewController = new TreeViewController(treeView);
        //        DBDatabase dbDatabase = new DBDatabase { DatabaseName = "Database" + TestUtilsShared.GetRandomString() };
        //        DBDatabase dbDatabase2 = new DBDatabase { DatabaseName = "Database(2)" + TestUtilsShared.GetRandomString() };
        //        DBTable dbTable = new DBTable("Table" + TestUtilsShared.GetRandomString());
        //        dbDatabase.Tables.Add(dbTable);
        //        IBusinessObjectCollection dbDatabases = new BusinessObjectCollection<DBDatabase> { dbDatabase };
        //        treeViewController.LoadTreeView(dbDatabases, 2);
        //        bool busObjectAddedEvent = false;
        //        dbDatabases.BusinessObjectAdded += (sender, e) => busObjectAddedEvent = true;
        //        //-------------Assert Preconditions -------------
        //        Assert.AreEqual(1, dbDatabases.Count);
        //        Assert.AreEqual(1, treeView.Nodes.Count);
        //        ITreeNode databaseNode = treeView.Nodes[0];
        //        Assert.AreEqual(dbDatabase.ToString(), databaseNode.Text);
        //        Assert.AreEqual(1, databaseNode.Nodes.Count);
        //        ITreeNode relationshipNode = databaseNode.Nodes[0];
        //        Assert.AreEqual("Tables", relationshipNode.Text);
        //        Assert.AreEqual(1, relationshipNode.Nodes.Count);
        //        ITreeNode childNode = relationshipNode.Nodes[0];
        //        Assert.AreEqual(dbTable.ToString(), childNode.Text);
        //        Assert.IsFalse(busObjectAddedEvent);
        //        //---------------Execute Test ----------------------
        //        dbDatabases.Add(dbDatabase2);
        //        //---------------Test Result -----------------------
        //        Assert.IsTrue(busObjectAddedEvent);
        //        Assert.AreEqual(2, treeView.Nodes.Count);
        //        ITreeNode databaseNode2 = treeView.Nodes[1];
        //        Assert.AreEqual(dbDatabase2.ToString(), databaseNode2.Text);
        //        Assert.AreEqual(1, databaseNode2.Nodes.Count);
        //    }

        //    [Test]
        //    [Ignore("//TODO Brett 18 Mar 2009: Woking on this")]
        //    public void Test_LoadBusinessObjectCollection_LevelsToDisplay()
        //    {
        //        //---------------Set up test pack-------------------
        //        TreeViewWin treeView = new TreeViewWin();
        //        TreeViewController treeViewController = new TreeViewController(treeView);
        //        DBDatabase dbDatabase = new DBDatabase { DatabaseName = TestUtilsShared.GetRandomString() };
        //        DBTable dbTable = new DBTable(TestUtilsShared.GetRandomString());
        //        dbDatabase.Tables.Add(dbTable);
        //        DBColumn dbColumn = new DBColumn();
        //        dbTable.Columns.Add(dbColumn);
        //        IBusinessObjectCollection dbDatabases = new BusinessObjectCollection<DBDatabase> { dbDatabase };
        //        //-------------Assert Preconditions -------------
        //        Assert.AreEqual(1, dbDatabases.Count);
        //        Assert.AreEqual(0, treeView.Nodes.Count);
        //        //---------------Execute Test ----------------------
        //        treeViewController.LoadTreeView(dbDatabases, 3, 2);
        //        //---------------Test Result -----------------------
        //        Assert.AreEqual(1, treeView.Nodes.Count);
        //        ITreeNode node = treeView.Nodes[0];
        //        Assert.AreEqual(dbDatabase.ToString(), node.Text);
        //        Assert.AreEqual(1, node.Nodes.Count);
        //        ITreeNode relationshipNode = node.Nodes[0];
        //        Assert.AreEqual("Tables", relationshipNode.Text);
        //        Assert.AreEqual(1, relationshipNode.Nodes.Count);
        //        ITreeNode childNode = relationshipNode.Nodes[0];
        //        Assert.AreEqual(dbTable.ToString(), childNode.Text);
        //        Assert.AreEqual(0, childNode.Nodes.Count);
        //    }

        //    [Test]
        //    public void Test_LoadTreeView_WithRelationship()
        //    {
        //        //---------------Set up test pack-------------------
        //        TreeViewWin treeView = new TreeViewWin();
        //        TreeViewController treeViewController = new TreeViewController(treeView);
        //        DBColumn dbColumn = new DBColumn("Column" + TestUtilsShared.GetRandomString());
        //        DBTable dbTable = new DBTable("Table" + TestUtilsShared.GetRandomString());
        //        dbTable.Columns.Add(dbColumn);
        //        IRelationship relationship = dbTable.Relationships["Columns"];
        //        //-------------Assert Preconditions -------------
        //        Assert.IsInstanceOfType(typeof(IMultipleRelationship), relationship);
        //        IMultipleRelationship multipleRelationship = (IMultipleRelationship)relationship;
        //        Assert.AreEqual(1, multipleRelationship.BusinessObjectCollection.Count);
        //        Assert.AreEqual(0, treeView.Nodes.Count);
        //        //---------------Execute Test ----------------------
        //        treeViewController.LoadTreeView(relationship);
        //        //---------------Test Result -----------------------
        //        Assert.AreEqual(1, treeView.Nodes.Count);
        //        ITreeNode relationshipNode = treeView.Nodes[0];
        //        Assert.AreEqual(relationship.RelationshipName, relationshipNode.Text);
        //        Assert.AreEqual(1, relationshipNode.Nodes.Count);
        //    }

        //    [Test]
        //    public void Test_LoadTreeView_WithRelationship_TwoChildren()
        //    {
        //        //---------------Set up test pack-------------------
        //        TreeViewWin treeView = new TreeViewWin();
        //        TreeViewController treeViewController = new TreeViewController(treeView);
        //        DBTable dbTable = new DBTable("Table" + TestUtilsShared.GetRandomString());
        //        DBColumn dbColumn = new DBColumn("Column" + TestUtilsShared.GetRandomString());
        //        DBColumn dbColumn2 = new DBColumn("Column(2)" + TestUtilsShared.GetRandomString());
        //        dbTable.Columns.Add(dbColumn);
        //        dbTable.Columns.Add(dbColumn2);
        //        IRelationship relationship = dbTable.Relationships["Columns"];
        //        //-------------Assert Preconditions -------------
        //        Assert.IsInstanceOfType(typeof(IMultipleRelationship), relationship);
        //        IMultipleRelationship multipleRelationship = (IMultipleRelationship)relationship;
        //        Assert.AreEqual(2, multipleRelationship.BusinessObjectCollection.Count);
        //        Assert.AreEqual(0, treeView.Nodes.Count);
        //        //---------------Execute Test ----------------------
        //        treeViewController.LoadTreeView(relationship, 2);
        //        //---------------Test Result -----------------------
        //        Assert.AreEqual(1, treeView.Nodes.Count);
        //        ITreeNode relationshipNode = treeView.Nodes[0];
        //        Assert.AreEqual(relationship.RelationshipName, relationshipNode.Text);
        //        Assert.AreEqual(2, relationshipNode.Nodes.Count);
        //    }

        //    [Test]
        //    public void Test_LoadTreeView_WithRelationship_WhenExpandLevelsSpecified()
        //    {
        //        //---------------Set up test pack-------------------
        //        TreeViewWin treeView = new TreeViewWin();
        //        TreeViewController treeViewController = new TreeViewController(treeView);
        //        DBColumn dbColumn = new DBColumn("Column" + TestUtilsShared.GetRandomString());
        //        DBTable dbTable = new DBTable("Table" + TestUtilsShared.GetRandomString());
        //        dbTable.Columns.Add(dbColumn);
        //        IRelationship relationship = dbTable.Relationships["Columns"];

        //        //-------------Assert Preconditions -------------
        //        Assert.IsInstanceOfType(typeof(IMultipleRelationship), relationship);
        //        IMultipleRelationship multipleRelationship = (IMultipleRelationship)relationship;
        //        Assert.AreEqual(1, multipleRelationship.BusinessObjectCollection.Count);
        //        Assert.AreEqual(0, treeView.Nodes.Count);
        //        //---------------Execute Test ----------------------
        //        treeViewController.LoadTreeView(relationship, 2);
        //        //---------------Test Result -----------------------
        //        Assert.AreEqual(1, treeView.Nodes.Count);
        //        ITreeNode relationshipNode = treeView.Nodes[0];
        //        Assert.AreEqual(relationship.RelationshipName, relationshipNode.Text);
        //        Assert.AreEqual(1, relationshipNode.Nodes.Count);
        //        ITreeNode childNode = relationshipNode.Nodes[0];
        //        Assert.AreEqual(dbColumn.ToString(), childNode.Text);
        //        Assert.AreEqual(0, childNode.Nodes.Count);
        //    }

        //    [Test]
        //    public void Test_LoadTreeView_WithRelationship_WhenExpandLevelsSpecified_WithDisplayLevelZero()
        //    {
        //        //---------------Set up test pack-------------------
        //        TreeViewWin treeView = new TreeViewWin();
        //        TreeViewController treeViewController = new TreeViewController(treeView);


        //        DBDatabase dbDatabase = new DBDatabase { DatabaseName = TestUtilsShared.GetRandomString() };
        //        DBColumn dbColumn = new DBColumn("Column" + TestUtilsShared.GetRandomString());
        //        DBTable dbTable = new DBTable("Table" + TestUtilsShared.GetRandomString());
        //        dbTable.Columns.Add(dbColumn);
        //        dbDatabase.Tables.Add(dbTable);
        //        IRelationship relationship = dbDatabase.Relationships["Tables"];
        //        //-------------Assert Preconditions -------------
        //        Assert.IsInstanceOfType(typeof(IMultipleRelationship), relationship);
        //        IMultipleRelationship multipleRelationship = (IMultipleRelationship)relationship;
        //        Assert.AreEqual(1, multipleRelationship.BusinessObjectCollection.Count);
        //        Assert.AreEqual(0, treeView.Nodes.Count);
        //        //---------------Execute Test ----------------------
        //        treeViewController.LoadTreeView(relationship, 2, 0);
        //        //---------------Test Result -----------------------
        //        Assert.AreEqual(1, treeView.Nodes.Count);
        //        ITreeNode relationshipNode = treeView.Nodes[0];
        //        Assert.AreEqual(relationship.RelationshipName, relationshipNode.Text);
        //        Assert.AreEqual(0, relationshipNode.Nodes.Count);
        //    }

        //    [Test]
        //    public void Test_LoadTreeView_WithRelationship_WhenExpandLevelsSpecified_WithDisplayLevelOne()
        //    {
        //        //---------------Set up test pack-------------------
        //        TreeViewWin treeView = new TreeViewWin();
        //        TreeViewController treeViewController = new TreeViewController(treeView);


        //        DBDatabase dbDatabase = new DBDatabase { DatabaseName = TestUtilsShared.GetRandomString() };
        //        DBColumn dbColumn = new DBColumn("Column" + TestUtilsShared.GetRandomString());
        //        DBTable dbTable = new DBTable("Table" + TestUtilsShared.GetRandomString());
        //        dbTable.Columns.Add(dbColumn);
        //        dbDatabase.Tables.Add(dbTable);
        //        IRelationship relationship = dbDatabase.Relationships["Tables"];
        //        //-------------Assert Preconditions -------------
        //        Assert.IsInstanceOfType(typeof(IMultipleRelationship), relationship);
        //        IMultipleRelationship multipleRelationship = (IMultipleRelationship)relationship;
        //        Assert.AreEqual(1, multipleRelationship.BusinessObjectCollection.Count);
        //        Assert.AreEqual(0, treeView.Nodes.Count);
        //        //---------------Execute Test ----------------------
        //        treeViewController.LoadTreeView(relationship, 3, 1);
        //        //---------------Test Result -----------------------
        //        Assert.AreEqual(1, treeView.Nodes.Count);
        //        ITreeNode relationshipNode = treeView.Nodes[0];
        //        Assert.AreEqual(relationship.RelationshipName, relationshipNode.Text);
        //        Assert.AreEqual(1, relationshipNode.Nodes.Count);
        //        ITreeNode tableChildNode = relationshipNode.Nodes[0];
        //        Assert.AreEqual(dbTable.ToString(), tableChildNode.Text);
        //        Assert.AreEqual(0, tableChildNode.Nodes.Count);
        //    }

        //    [Test]
        //    public void Test_LoadTreeView_WithRelationship_WhenExpandLevelsSpecified_WithDisplayLevel()
        //    {
        //        //---------------Set up test pack-------------------
        //        TreeViewWin treeView = new TreeViewWin();
        //        TreeViewController treeViewController = new TreeViewController(treeView);


        //        DBDatabase dbDatabase = new DBDatabase { DatabaseName = TestUtilsShared.GetRandomString() };
        //        DBColumn dbColumn = new DBColumn("Column" + TestUtilsShared.GetRandomString());
        //        DBTable dbTable = new DBTable("Table" + TestUtilsShared.GetRandomString());
        //        dbTable.Columns.Add(dbColumn);
        //        dbDatabase.Tables.Add(dbTable);
        //        IRelationship relationship = dbDatabase.Relationships["Tables"];
        //        //-------------Assert Preconditions -------------
        //        Assert.IsInstanceOfType(typeof(IMultipleRelationship), relationship);
        //        IMultipleRelationship multipleRelationship = (IMultipleRelationship)relationship;
        //        Assert.AreEqual(1, multipleRelationship.BusinessObjectCollection.Count);
        //        Assert.AreEqual(0, treeView.Nodes.Count);
        //        //---------------Execute Test ----------------------
        //        treeViewController.LoadTreeView(relationship, 3, 2);
        //        //---------------Test Result -----------------------
        //        Assert.AreEqual(1, treeView.Nodes.Count);
        //        ITreeNode relationshipNode = treeView.Nodes[0];
        //        Assert.AreEqual(relationship.RelationshipName, relationshipNode.Text);
        //        Assert.AreEqual(1, relationshipNode.Nodes.Count);
        //        ITreeNode tableChildNode = relationshipNode.Nodes[0];
        //        Assert.AreEqual(dbTable.ToString(), tableChildNode.Text);

        //        Assert.AreEqual(4, tableChildNode.Nodes.Count);
        //        ITreeNode columnRelationshipNode = tableChildNode.Nodes[0];
        //        Assert.AreEqual("Columns", columnRelationshipNode.Text);
        //        Assert.AreEqual(0, columnRelationshipNode.Nodes.Count);
        //        //ReflectionUtilities.ExecutePrivateMethod(treeViewController, "ExpandNode", columnRelationshipNode);
        //        //ITreeNode columnChildNode = columnRelationshipNode.Nodes[0];
        //        //Assert.AreEqual(dbColumn.ToString(), columnChildNode.Text);
        //        //ReflectionUtilities.ExecutePrivateMethod(treeViewController, "ExpandNode", columnChildNode);
        //        //Assert.AreEqual(1, columnChildNode.Nodes.Count);
        //    }




        //    [Test]
        //    public void Test_AddNewParentToTreeView_ThenAddChildren_ThenExpandParent_ShouldHaveChildren()
        //    {
        //        //---------------Set up test pack-------------------
        //        TreeViewWin treeView = new TreeViewWin();
        //        TreeViewController treeViewController = new TreeViewController(treeView);
        //        DBDatabase dbDatabase = new DBDatabase { DatabaseName = "Database" + TestUtilsShared.GetRandomString() };
        //        DBTable dbTable = new DBTable("Table" + TestUtilsShared.GetRandomString());
        //        dbDatabase.Tables.Add(dbTable);
        //        IBusinessObjectCollection dbDatabases = new BusinessObjectCollection<DBDatabase> { dbDatabase };
        //        treeViewController.LoadTreeView(dbDatabases, 3);

        //        //-------------Assert Preconditions -------------
        //        Assert.AreEqual(1, treeView.Nodes.Count);
        //        ITreeNode dbNode = treeView.Nodes[0];
        //        Assert.AreEqual(dbDatabase.ToString(), dbNode.Text);
        //        Assert.AreEqual(1, dbNode.Nodes.Count);
        //        ITreeNode tableRelationshipNode = dbNode.Nodes[0];
        //        Assert.AreEqual("Tables", tableRelationshipNode.Text);
        //        Assert.AreEqual(1, tableRelationshipNode.Nodes.Count);
        //        ITreeNode tableNode = tableRelationshipNode.Nodes[0];
        //        Assert.AreEqual(dbTable.ToString(), tableNode.Text);
        //        ITreeNode columnRelationshipNode = tableNode.Nodes[0];
        //        Assert.AreEqual("Columns", columnRelationshipNode.Text);
        //        //---------------Execute Test ----------------------
        //        DBTable newTable = dbDatabase.Tables.CreateBusinessObject();
        //        DBColumn newColumn = newTable.Columns.CreateBusinessObject();
        //        //            DBColumn anotherNewColumn = newTable.Columns.CreateBusinessObject();
        //        ITreeNode newTablenode = tableRelationshipNode.Nodes[1];
        //        newTablenode.Expand();
        //        ReflectionUtilities.ExecutePrivateMethod(treeViewController, "ExpandNode", newTablenode);

        //        ITreeNode newColumnsRelationshipNode = newTablenode.Nodes[0];
        //        newColumnsRelationshipNode.Expand();
        //        ReflectionUtilities.ExecutePrivateMethod(treeViewController, "ExpandNode", newColumnsRelationshipNode);
        //        //---------------Test Result -----------------------
        //        Assert.AreEqual(2, tableRelationshipNode.Nodes.Count);

        //        Assert.AreEqual(newTable.ToString(), newTablenode.Text);
        //        //            Assert.AreEqual(1, newTable.Columns.Count);

        //        Assert.AreEqual("Columns", newColumnsRelationshipNode.Text);
        //        Assert.AreEqual(1, newColumnsRelationshipNode.Nodes.Count);
        //        ITreeNode newColumNode = newColumnsRelationshipNode.Nodes[0];
        //        Assert.AreEqual(newColumn.ToString(), newColumNode.Text);
        //        //            ITreeNode secondNewColumnNode = newColumnsRelationshipNode.Nodes[1];
        //        //            Assert.AreEqual(anotherNewColumn.ToString(), secondNewColumnNode.Text);

        //    }
        //    //        [Test]
        //    //        public void Test_AddNewParentToTreeView_ThenAddChildren_ChangeChildToAnotherParent_ThenExpandParent_ShouldHaveChildren()
        //    //        {
        //    //            //---------------Set up test pack-------------------
        //    //            TreeViewWin treeView = new TreeViewWin();
        //    //            TreeViewController treeViewController = new TreeViewController(treeView);
        //    //            DBDatabase dbDatabase = new DBDatabase { DatabaseName = "Database" + TestUtilsShared.GetRandomString() };
        //    //            DBTable dbTable = new DBTable("Table" + TestUtilsShared.GetRandomString());
        //    //            dbDatabase.Tables.Add(dbTable);
        //    //            IBusinessObjectCollection dbDatabases = new BusinessObjectCollection<DBDatabase> { dbDatabase };
        //    //            DBDatabase newDB = (DBDatabase) dbDatabases.CreateBusinessObject();
        //    //            treeViewController.LoadTreeView(dbDatabases, 3);
        //    //            //-------------Assert Preconditions -------------
        //    //            Assert.AreEqual(2, treeView.Nodes.Count);
        //    //            ITreeNode newDBNode = treeView.Nodes[1];
        //    //            Assert.AreEqual(dbDatabase.ToString(), newDBNode.Text);
        //    ////            Assert.AreEqual(1, newDBNode.Nodes.Count);
        //    ////            ITreeNode tableRelationshipNode = newDBNode.Nodes[0];
        //    ////            Assert.AreEqual("Tables", tableRelationshipNode.Text);
        //    ////            Assert.AreEqual(1, tableRelationshipNode.Nodes.Count);
        //    ////            ITreeNode tableNode = tableRelationshipNode.Nodes[0];
        //    ////            Assert.AreEqual(dbTable.ToString(), tableNode.Text);
        //    ////            ITreeNode columnRelationshipNode = tableNode.Nodes[0];
        //    ////            Assert.AreEqual("Columns", columnRelationshipNode.Text);
        //    //            //---------------Execute Test ----------------------
        //    //            
        //    //            DBTable newTable = dbDatabase.Tables.CreateBusinessObject();
        //    //            newTable.Database = newDB;
        //    //            DBColumn newColumn = newTable.Columns.CreateBusinessObject();
        //    ////            DBColumn anotherNewColumn = newTable.Columns.CreateBusinessObject();
        //    //            ITreeNode newTablenode = tableRelationshipNode.Nodes[1];
        //    //            newTablenode.Expand();
        //    //            ReflectionUtilities.ExecutePrivateMethod(treeViewController, "ExpandNode", newTablenode);
        //    //
        //    //            ITreeNode newColumnsRelationshipNode = newTablenode.Nodes[0];
        //    //            newColumnsRelationshipNode.Expand();
        //    //            ReflectionUtilities.ExecutePrivateMethod(treeViewController, "ExpandNode", newColumnsRelationshipNode);
        //    //            //---------------Test Result -----------------------
        //    //            Assert.AreEqual(2, tableRelationshipNode.Nodes.Count); 
        //    //            
        //    //            Assert.AreEqual(newTable.ToString(), newTablenode.Text);
        //    //            Assert.AreEqual(1, newTable.Columns.Count);
        //    //            
        //    //            Assert.AreEqual("Columns", newColumnsRelationshipNode.Text);
        //    //            Assert.AreEqual(1, newColumnsRelationshipNode.Nodes.Count);
        //    //            ITreeNode newColumNode = newColumnsRelationshipNode.Nodes[0];
        //    //            Assert.AreEqual(newColumn.ToString(), newColumNode.Text);
        //    ////            ITreeNode secondNewColumnNode = newColumnsRelationshipNode.Nodes[1];
        //    ////            Assert.AreEqual(anotherNewColumn.ToString(), secondNewColumnNode.Text);
        //    //
        //    //        }
        //    [Test]
        //    public void Test_LoadRelationships_AddNewParentToTreeView_ThenAddChildren_ThenExpandParent_ShouldHaveChildre()
        //    {
        //        //---------------Set up test pack-------------------
        //        TreeViewWin treeView = new TreeViewWin();
        //        TreeViewController treeViewController = new TreeViewController(treeView);
        //        DBDatabase dbDatabase = new DBDatabase { DatabaseName = "Database" + TestUtilsShared.GetRandomString() };
        //        DBTable dbTable = new DBTable("Table" + TestUtilsShared.GetRandomString());
        //        dbDatabase.Tables.Add(dbTable);
        //        IBusinessObjectCollection dbDatabases = new BusinessObjectCollection<DBDatabase> { dbDatabase };
        //        treeViewController.LoadTreeView(dbDatabase.Relationships["Tables"], 2);

        //        //-------------Assert Preconditions -------------
        //        Assert.AreEqual(1, treeView.Nodes.Count);
        //        ITreeNode tableRelationshipNode = treeView.Nodes[0];
        //        Assert.AreEqual("Tables", tableRelationshipNode.Text);
        //        Assert.AreEqual(1, tableRelationshipNode.Nodes.Count);
        //        ITreeNode tableNode = tableRelationshipNode.Nodes[0];
        //        Assert.AreEqual(dbTable.ToString(), tableNode.Text);
        //        ITreeNode columnRelationshipNode = tableNode.Nodes[0];
        //        Assert.AreEqual("Columns", columnRelationshipNode.Text);
        //        //---------------Execute Test ----------------------
        //        BusinessObjectCollection<DBTable> tables = dbDatabase.Tables;
        //        DBTable newTable = tables.CreateBusinessObject();
        //        newTable.TableNameDB = "New Name";
        //        //            newTable.Save();
        //        DBColumn newColumn = dbTable.Columns.CreateBusinessObject();
        //        newColumn.Table = newTable;
        //        newColumn.ColumnName = "New Name";
        //        DBColumn anotherNewColumn = dbTable.Columns.CreateBusinessObject();
        //        anotherNewColumn.Table = newTable;
        //        ITreeNode newTablenode = tableRelationshipNode.Nodes[1];
        //        newTablenode.Expand();
        //        ReflectionUtilities.ExecutePrivateMethod(treeViewController, "ExpandNode", newTablenode);

        //        ITreeNode newColumnsRelationshipNode = newTablenode.Nodes[0];
        //        newColumnsRelationshipNode.Expand();
        //        ReflectionUtilities.ExecutePrivateMethod(treeViewController, "ExpandNode", newColumnsRelationshipNode);
        //        //---------------Test Result -----------------------
        //        Assert.AreEqual(2, tableRelationshipNode.Nodes.Count);

        //        Assert.AreEqual(newTable.ToString(), newTablenode.Text);
        //        //Assert.AreEqual(2, newTable.Columns.Count);

        //        Assert.AreEqual("Columns", newColumnsRelationshipNode.Text);
        //        Assert.AreEqual(2, newColumnsRelationshipNode.Nodes.Count);
        //        ITreeNode newColumNode = newColumnsRelationshipNode.Nodes[0];
        //        Assert.AreEqual(newColumn.ToString(), newColumNode.Text);
        //        ITreeNode secondNewColumnNode = newColumnsRelationshipNode.Nodes[1];
        //        Assert.AreEqual(anotherNewColumn.ToString(), secondNewColumnNode.Text);

        //    }

        //    [Test]
        //    public void Test_BusinessObjectCollection_ChildRemoved_SetsSelectedNodeToNull()
        //    {
        //        //---------------Set up test pack-------------------
        //        TreeViewWin treeView = new TreeViewWin();
        //        TreeViewController treeViewController = new TreeViewController(treeView);
        //        DBDatabase dbDatabase = new DBDatabase { DatabaseName = "Database" + TestUtilsShared.GetRandomString() };
        //        DBDatabase dbDatabase2 = new DBDatabase { DatabaseName = "Database" + TestUtilsShared.GetRandomString() };
        //        DBTable dbTable = new DBTable("Table" + TestUtilsShared.GetRandomString());
        //        DBTable dbTable2 = new DBTable("Table" + TestUtilsShared.GetRandomString());
        //        dbDatabase.Tables.Add(dbTable);
        //        dbDatabase.Tables.Add(dbTable2);
        //        IBusinessObjectCollection dbDatabases = new BusinessObjectCollection<DBDatabase> { dbDatabase };
        //        treeViewController.LoadTreeView(dbDatabases, 4);
        //        treeViewController.SelectObject(dbTable2);
        //        //---------------Assert Precondition----------------        
        //        Assert.IsNotNull(treeViewController.TreeView.SelectedNode);
        //        //---------------Execute Test ----------------------
        //        dbTable2.Database = dbDatabase2;
        //        //---------------Test Result -----------------------
        //        Assert.IsNull(treeViewController.TreeView.SelectedNode);
        //    }

        //    //[Test]
        //    //public void Test_LoadTreeView_WithRelationship_OneChild_Expanded()
        //    //{
        //    //    //---------------Set up test pack-------------------
        //    //    TreeViewWin treeView = new TreeViewWin();
        //    //    TreeViewController treeViewController = new TreeViewController(treeView);
        //    //    DBDatabase dbDatabase = new DBDatabase() { DatabaseName = "Database" + TestUtilsShared.GetRandomString() };
        //    //    DBTable dbTable = new DBTable("Table" + TestUtilsShared.GetRandomString());
        //    //    dbDatabase.Tables.Add(dbTable);
        //    //    IBusinessObjectCollection dbDatabases = new BusinessObjectCollection<DBDatabase>();
        //    //    dbDatabases.Add(dbDatabase);
        //    //    //-------------Assert Preconditions -------------
        //    //    Assert.AreEqual(1, dbDatabases.Count);
        //    //    Assert.AreEqual(0, treeView.Nodes.Count);
        //    //    //---------------Execute Test ----------------------
        //    //    treeViewController.LoadTreeView(dbDatabases, 2);
        //    //    //---------------Test Result -----------------------
        //    //    Assert.AreEqual(1, treeView.Nodes.Count);
        //    //    ITreeNode node = treeView.Nodes[0];
        //    //    Assert.AreEqual(dbDatabase.ToString(), node.Text);
        //    //    Assert.AreEqual(1, node.Nodes.Count);
        //    //    ITreeNode relationshipNode = node.Nodes[0];
        //    //    Assert.AreEqual("Tables", relationshipNode.Text);
        //    //    Assert.AreEqual(1, relationshipNode.Nodes.Count);
        //    //    ITreeNode childNode = relationshipNode.Nodes[0];
        //    //    Assert.AreEqual(dbTable.ToString(), childNode.Text);
        //    //}
        //}
    }

    internal class TreeViewController
    {
        public TreeViewController(TreeViewWin treeView)
        {
        }
    }
}
