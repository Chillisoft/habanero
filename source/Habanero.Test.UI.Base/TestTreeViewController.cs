using System.Collections;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.Test.BO;
using Habanero.UI.Base;
using Habanero.UI.VWG;
using Habanero.UI.Win;
using Habanero.Util;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    [TestFixture]
    public class TestTreeViewControllerWin
    {

        protected virtual IControlFactory GetControlFactory()
        {
            IControlFactory controlFactory = new ControlFactoryWin();
            GlobalUIRegistry.ControlFactory = controlFactory;
            return controlFactory;
        }

        [TestFixtureSetUp]
        public void SetupFixture()
        {
           // base.SetupFixture();
            ClassDef.ClassDefs.Clear();
            ClassDef organisationClassDef = OrganisationTestBO.LoadDefaultClassDef();
            organisationClassDef.RelationshipDefCol["ContactPeople"].RelationshipType = RelationshipType.Composition;
            AddressTestBO.LoadDefaultClassDef();
            ClassDef contactPersonTestBOClassDef = ContactPersonTestBO.LoadClassDefWithOrganisationAndAddressRelationships();
            contactPersonTestBOClassDef.RelationshipDefCol["Addresses"].RelationshipType = RelationshipType.Composition;
            GetControlFactory();
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
            BORegistry.DataAccessor = new DataAccessorInMemory();
            
        }

        [Test]
        public void Test_Acceptance_LoadBO_TwoChildren_OneWithNoChildrenAndOneWith2Children_Expanded_ShouldLoadCorrectChildren()
        {
            //---------------Set up test pack-------------------
            ITreeView treeView = GlobalUIRegistry.ControlFactory.CreateTreeView();
            //TreeViewWin treeView = new TreeViewWin();
            TreeViewController treeViewController = new TreeViewController(treeView);
            OrganisationTestBO organisation = OrganisationTestBO.CreateSavedOrganisation();
            ContactPersonTestBO contactPerson1 = CreateUnsavedContactPerson(organisation);
            ContactPersonTestBO contactPerson2 = CreateUnsavedContactPerson(organisation);
            AddressTestBO address1 = AddressTestBO.CreateUnsavedAddress(contactPerson2);
            AddressTestBO address2 = AddressTestBO.CreateUnsavedAddress(contactPerson2);
            
            
            //-------------Assert Preconditions -------------
            Assert.AreEqual(0, treeView.Nodes.Count);
            Assert.AreEqual(2, organisation.ContactPeople.Count);
            Assert.AreEqual(0, contactPerson1.Addresses.Count);
            Assert.AreEqual(2, contactPerson2.Addresses.Count);
            //---------------Execute Test ----------------------
            treeViewController.LoadTreeView(organisation, 4);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, treeView.Nodes.Count);
            ITreeNode dbNode = treeView.Nodes[0];
            ITreeNode relationshipNode = dbNode.Nodes[0];
            Assert.AreEqual(2, relationshipNode.Nodes.Count, "Should have two Contact People");
            AssertChildNodeLoadedInTree(organisation.ContactPeople, relationshipNode, 0);
            AssertChildNodeLoadedInTree(organisation.ContactPeople, relationshipNode, 1);
            ITreeNode table1Node = relationshipNode.Nodes[0];

            ITreeNode addressRelationshipNode = table1Node.Nodes[0];
            Assert.AreEqual(0, addressRelationshipNode.Nodes.Count);


            ITreeNode contactPerson2Node = relationshipNode.Nodes[1];
            Assert.AreEqual(contactPerson2.ToString(), contactPerson2Node.Text);
            ITreeNode contactPerson2AddressRelationshipNode = contactPerson2Node.Nodes[0];
            Assert.AreEqual("Addresses", contactPerson2AddressRelationshipNode.Text);
            Assert.AreEqual(2, contactPerson2.Addresses.Count);
            Assert.AreEqual(2, contactPerson2AddressRelationshipNode.Nodes.Count);
            AssertChildNodeLoadedInTree(contactPerson2.Addresses, contactPerson2AddressRelationshipNode, 0);
            AssertChildNodeLoadedInTree(contactPerson2.Addresses, contactPerson2AddressRelationshipNode, 1);
        }

        [Test]
        public void
            Test_Acceptance_LoadBO_TwoChildren_OneWithOneChild_AndOneWith2Children_Expanded_ShouldLoadCorrectChildren()
        {
            //---------------Set up test pack-------------------
            ITreeView treeView = GlobalUIRegistry.ControlFactory.CreateTreeView();
            TreeViewController treeViewController = new TreeViewController(treeView);
            OrganisationTestBO organisation = OrganisationTestBO.CreateSavedOrganisation();
            ContactPersonTestBO contactPerson1 = CreateUnsavedContactPerson(organisation);
            ContactPersonTestBO contactPerson2 = CreateUnsavedContactPerson(organisation);
            AddressTestBO address1 = AddressTestBO.CreateUnsavedAddress(contactPerson2);
            AddressTestBO address2 = AddressTestBO.CreateUnsavedAddress(contactPerson2);
            AddressTestBO address3 = AddressTestBO.CreateUnsavedAddress(contactPerson1);
            //-------------Assert Preconditions -------------
            Assert.AreEqual(0, treeView.Nodes.Count);
            Assert.AreEqual(2, organisation.ContactPeople.Count);
            Assert.AreEqual(1, contactPerson1.Addresses.Count);
            Assert.AreEqual(2, contactPerson2.Addresses.Count);
            //---------------Execute Test ----------------------
            treeViewController.LoadTreeView(organisation, 4);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, treeView.Nodes.Count);
            ITreeNode dbNode = treeView.Nodes[0];
            ITreeNode relationshipNode = dbNode.Nodes[0];
            Assert.AreEqual(2, relationshipNode.Nodes.Count, "Should have two Contact People");
            AssertChildNodeLoadedInTree(organisation.ContactPeople, relationshipNode, 0);
            AssertChildNodeLoadedInTree(organisation.ContactPeople, relationshipNode, 1);
            ITreeNode table1Node = relationshipNode.Nodes[0];

            ITreeNode addressRelationshipNode = table1Node.Nodes[0];
            Assert.AreEqual(1, addressRelationshipNode.Nodes.Count);
            AssertChildNodeLoadedInTree(contactPerson1.Addresses, addressRelationshipNode, 0);

            ITreeNode table2Node = relationshipNode.Nodes[1];
            Assert.AreEqual(contactPerson2.ToString(), table2Node.Text);
            ITreeNode contactPerson2AddressRelationshipNode = table2Node.Nodes[0];
            Assert.AreEqual("Addresses", contactPerson2AddressRelationshipNode.Text);
            Assert.AreEqual(2, contactPerson2.Addresses.Count);
            Assert.AreEqual(2, contactPerson2AddressRelationshipNode.Nodes.Count);
            AssertChildNodeLoadedInTree(contactPerson2.Addresses, contactPerson2AddressRelationshipNode, 0);
            AssertChildNodeLoadedInTree(contactPerson2.Addresses, contactPerson2AddressRelationshipNode, 1);
        }

        [Test]
        public void
            Test_Acceptance_ExpandChild_WithTwoChildren_OneWithOneChild_AndOneWith2Children_WhenNotExpandedOnLoad_ShouldLoadCorrectChildren
            ()
        {
            //---------------Set up test pack-------------------
            ITreeView treeView = GlobalUIRegistry.ControlFactory.CreateTreeView();
            TreeViewController treeViewController = new TreeViewController(treeView);
            OrganisationTestBO organisation = OrganisationTestBO.CreateSavedOrganisation();
            ContactPersonTestBO contactPerson1 = CreateUnsavedContactPerson(organisation);
            ContactPersonTestBO contactPerson2 = CreateUnsavedContactPerson(organisation);
            AddressTestBO address1 = AddressTestBO.CreateUnsavedAddress(contactPerson2);
            AddressTestBO address2 = AddressTestBO.CreateUnsavedAddress(contactPerson2);
            AddressTestBO address3 = AddressTestBO.CreateUnsavedAddress(contactPerson1);
            treeViewController.LoadTreeView(organisation, 3);
            //-------------Assert Preconditions -------------
            Assert.AreEqual(2, organisation.ContactPeople.Count);
            Assert.AreEqual(1, contactPerson1.Addresses.Count);
            Assert.AreEqual(2, contactPerson2.Addresses.Count);
            Assert.AreEqual(1, treeView.Nodes.Count);
            ITreeNode dbNode = treeView.Nodes[0];
            ITreeNode contactPersonsRelationshipNode = dbNode.Nodes[0];
            Assert.AreEqual(2, contactPersonsRelationshipNode.Nodes.Count, "Should have two tables");
            AssertChildNodeLoadedInTree(organisation.ContactPeople, contactPersonsRelationshipNode, 0);
            AssertChildNodeLoadedInTree(organisation.ContactPeople, contactPersonsRelationshipNode, 1);
            ITreeNode contactPerson1Node = contactPersonsRelationshipNode.Nodes[0];
            ITreeNode contactPerson1AddressRelationshipNode = contactPerson1Node.Nodes[0];
            Assert.AreEqual("Addresses", contactPerson1AddressRelationshipNode.Text);
            Assert.AreEqual
                (1, contactPerson1AddressRelationshipNode.Nodes.Count,
                 "Only the dummy should be present because the node is not expanded");
            ITreeNode contactPerson2Node = contactPersonsRelationshipNode.Nodes[1];
            ITreeNode contactPerson2AddressRelationshipNode = contactPerson2Node.Nodes[0];
            Assert.AreEqual("Addresses", contactPerson2AddressRelationshipNode.Text);
            Assert.AreEqual
                (1, contactPerson2AddressRelationshipNode.Nodes.Count,
                 "Only the dummy should be present because the node is not expanded");
            //---------------Execute Test ----------------------
            contactPerson2AddressRelationshipNode.Expand();
            ReflectionUtilities.ExecutePrivateMethod(treeViewController, "ExpandNode", contactPerson2AddressRelationshipNode);
            //---------------Test Result -----------------------
            Assert.AreEqual(2, contactPerson2.Addresses.Count);
            Assert.AreEqual
                (1, contactPerson1AddressRelationshipNode.Nodes.Count, "Addresses in table 1 should still be the dummy node");
            Assert.AreNotEqual(contactPerson1.Addresses[0].ToString(), contactPerson1AddressRelationshipNode.Nodes[0].Text);
            AssertChildNodeLoadedInTree(contactPerson2.Addresses, contactPerson2AddressRelationshipNode, 0);
            Assert.AreEqual
                (2, contactPerson2AddressRelationshipNode.Nodes.Count,
                 "Both columns in table 2 should now be shown since it is expanded");
            AssertChildNodeLoadedInTree(contactPerson2.Addresses, contactPerson2AddressRelationshipNode, 1);
        }

        [Test]
        public void Test_Acceptance_WhenAddBusinessObjectToChildCollection_ShouldAddToTree()
        {
            //---------------Set up test pack-------------------
            ITreeView treeView = GlobalUIRegistry.ControlFactory.CreateTreeView();
            TreeViewController treeViewController = new TreeViewController(treeView);
            OrganisationTestBO organisation = OrganisationTestBO.CreateSavedOrganisation();
            ContactPersonTestBO contactPerson1 = CreateUnsavedContactPerson(organisation);
            ContactPersonTestBO contactPerson2 = CreateUnsavedContactPerson(organisation);
            AddressTestBO address1 = AddressTestBO.CreateUnsavedAddress(contactPerson2);
            AddressTestBO address2 = AddressTestBO.CreateUnsavedAddress(contactPerson2);
            AddressTestBO address3 = AddressTestBO.CreateUnsavedAddress(contactPerson1);
            treeViewController.LoadTreeView(organisation, 4);
            ITreeNode dbNode = treeView.Nodes[0];
            ITreeNode relationshipNode = dbNode.Nodes[0];
            ITreeNode contactPerson2Node = relationshipNode.Nodes[1];
            //-------------Assert Preconditions -------------
            Assert.AreEqual(contactPerson2.ToString(), contactPerson2Node.Text);
            ITreeNode contactPerson2AddressRelationshipNode = contactPerson2Node.Nodes[0];
            Assert.AreEqual("Addresses", contactPerson2AddressRelationshipNode.Text);
            Assert.AreEqual(2, contactPerson2.Addresses.Count);
            Assert.AreEqual(2, contactPerson2AddressRelationshipNode.Nodes.Count);
            AssertChildNodeLoadedInTree(contactPerson2.Addresses, contactPerson2AddressRelationshipNode, 0);
            AssertChildNodeLoadedInTree(contactPerson2.Addresses, contactPerson2AddressRelationshipNode, 1);
            //---------------Execute Test ----------------------

            contactPerson2.Addresses.Add(new AddressTestBO());
            //---------------Test Result -----------------------
            Assert.AreEqual(3, contactPerson2AddressRelationshipNode.Nodes.Count);
        }

        [Test]
        public void Test_Acceptance_WhenRemoveBusinessObjectFromChildCollection_ShouldRemoveFromTree()
        {
            //---------------Set up test pack-------------------
            ITreeView treeView = GlobalUIRegistry.ControlFactory.CreateTreeView();
            TreeViewController treeViewController = new TreeViewController(treeView);
            OrganisationTestBO organisation = OrganisationTestBO.CreateSavedOrganisation();
            ContactPersonTestBO contactPerson1 = CreateUnsavedContactPerson(organisation);
            ContactPersonTestBO contactPerson2 = CreateUnsavedContactPerson(organisation);
            AddressTestBO address1 = AddressTestBO.CreateUnsavedAddress(contactPerson2);
            AddressTestBO address2 = AddressTestBO.CreateUnsavedAddress(contactPerson2);
            AddressTestBO address3 = AddressTestBO.CreateUnsavedAddress(contactPerson1);
            treeViewController.LoadTreeView(organisation, 4);
            ITreeNode dbNode = treeView.Nodes[0];
            ITreeNode relationshipNode = dbNode.Nodes[0];
            ITreeNode table2Node = relationshipNode.Nodes[1];
            //-------------Assert Preconditions -------------
            Assert.AreEqual(contactPerson2.ToString(), table2Node.Text);
            ITreeNode contactPerson2AddressRelationshipNode = table2Node.Nodes[0];
            Assert.AreEqual("Addresses", contactPerson2AddressRelationshipNode.Text);
            Assert.AreEqual(2, contactPerson2.Addresses.Count);
            Assert.AreEqual(2, contactPerson2AddressRelationshipNode.Nodes.Count);
            AssertChildNodeLoadedInTree(contactPerson2.Addresses, contactPerson2AddressRelationshipNode, 0);
            AssertChildNodeLoadedInTree(contactPerson2.Addresses, contactPerson2AddressRelationshipNode, 1);
            //---------------Execute Test ----------------------
            contactPerson2.Addresses.Remove(address2);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, contactPerson2AddressRelationshipNode.Nodes.Count);
        }

        [Test]
        public void Test_Acceptance_WhenRemoveBusinessObjectFromChildCollectionAndThenAddBack_ShouldAddBackToTree()
        {
            //---------------Set up test pack-------------------
            ITreeView treeView = GlobalUIRegistry.ControlFactory.CreateTreeView();
            TreeViewController treeViewController = new TreeViewController(treeView);
            OrganisationTestBO organisation = OrganisationTestBO.CreateSavedOrganisation();
            ContactPersonTestBO contactPerson1 = CreateUnsavedContactPerson(organisation);
            ContactPersonTestBO contactPerson2 = CreateUnsavedContactPerson(organisation);
            AddressTestBO address1 = AddressTestBO.CreateUnsavedAddress(contactPerson2);
            AddressTestBO address2 = AddressTestBO.CreateUnsavedAddress(contactPerson2);
            AddressTestBO address3 = AddressTestBO.CreateUnsavedAddress(contactPerson1);
            treeViewController.LoadTreeView(organisation, 4);
            ITreeNode dbNode = treeView.Nodes[0];
            ITreeNode relationshipNode = dbNode.Nodes[0];
            ITreeNode contactPerson2Node = relationshipNode.Nodes[1];
            //-------------Assert Preconditions -------------
            Assert.AreEqual(contactPerson2.ToString(), contactPerson2Node.Text);
            ITreeNode contactPerson2AddressRelationshipNode = contactPerson2Node.Nodes[0];
            Assert.AreEqual("Addresses", contactPerson2AddressRelationshipNode.Text);
            Assert.AreEqual(2, contactPerson2.Addresses.Count);
            Assert.AreEqual(2, contactPerson2AddressRelationshipNode.Nodes.Count);
            AssertChildNodeLoadedInTree(contactPerson2.Addresses, contactPerson2AddressRelationshipNode, 0);
            AssertChildNodeLoadedInTree(contactPerson2.Addresses, contactPerson2AddressRelationshipNode, 1);
            //---------------Execute Test ----------------------
            contactPerson2.Addresses.Remove(address2);
            contactPerson2.Addresses.Add(address2);
            //---------------Test Result -----------------------
            Assert.AreEqual(contactPerson2.ToString(), contactPerson2Node.Text);
            contactPerson2AddressRelationshipNode = contactPerson2Node.Nodes[0];
            Assert.AreEqual("Addresses", contactPerson2AddressRelationshipNode.Text);
            Assert.AreEqual(2, contactPerson2.Addresses.Count);
            Assert.AreEqual(2, contactPerson2AddressRelationshipNode.Nodes.Count);
            AssertChildNodeLoadedInTree(contactPerson2.Addresses, contactPerson2AddressRelationshipNode, 0);
            AssertChildNodeLoadedInTree(contactPerson2.Addresses, contactPerson2AddressRelationshipNode, 1);
        }

        [Test]
        public void
            Test_Acceptance_WhenRemoveBusinessObjectThatHasChildrenThenAddBack_ShouldAddBackToTree_AndAddChildrenBack()
        {
            //---------------Set up test pack-------------------
            ITreeView treeView = GlobalUIRegistry.ControlFactory.CreateTreeView();
            TreeViewController treeViewController = new TreeViewController(treeView);
            OrganisationTestBO organisation = OrganisationTestBO.CreateSavedOrganisation();
            ContactPersonTestBO contactPerson1 = CreateUnsavedContactPerson(organisation);
            ContactPersonTestBO contactPerson2 = CreateUnsavedContactPerson(organisation);
            AddressTestBO address1 = AddressTestBO.CreateUnsavedAddress(contactPerson2);
            AddressTestBO address2 = AddressTestBO.CreateUnsavedAddress(contactPerson2);
            AddressTestBO address3 = AddressTestBO.CreateUnsavedAddress(contactPerson1);
            treeViewController.LoadTreeView(organisation, 4);
            ITreeNode dbNode = treeView.Nodes[0];
            ITreeNode relationshipNode = dbNode.Nodes[0];

            organisation.ContactPeople.Remove(contactPerson2);
            //-------------Assert Preconditions -------------
            Assert.AreEqual(1, relationshipNode.Nodes.Count);
            Assert.AreEqual(2, contactPerson2.Addresses.Count);
            //---------------Execute Test ----------------------

            organisation.ContactPeople.Add(contactPerson2);
            ITreeNode contactPerson2Node = relationshipNode.Nodes[1];
            contactPerson2Node.ExpandAll();
            ReflectionUtilities.ExecutePrivateMethod(treeViewController, "ExpandNode", contactPerson2Node);

            //---------------Test Result -----------------------
            Assert.AreEqual(2, relationshipNode.Nodes.Count);
            contactPerson2Node = relationshipNode.Nodes[1];
            Assert.AreEqual(contactPerson2.ToString(), contactPerson2Node.Text);
            Assert.AreEqual(1, contactPerson2Node.Nodes.Count);
            ITreeNode contactPerson2AddressRelationshipNode = contactPerson2Node.Nodes[0];
            Assert.AreEqual("Addresses", contactPerson2AddressRelationshipNode.Text);
            Assert.AreEqual(2, contactPerson2.Addresses.Count);
            ReflectionUtilities.ExecutePrivateMethod(treeViewController, "ExpandNode", contactPerson2AddressRelationshipNode);
            Assert.AreEqual(2, contactPerson2AddressRelationshipNode.Nodes.Count);
            AssertChildNodeLoadedInTree(contactPerson2.Addresses, contactPerson2AddressRelationshipNode, 0);
            AssertChildNodeLoadedInTree(contactPerson2.Addresses, contactPerson2AddressRelationshipNode, 1);
        }

        [Test]
        public void Test_WhenRemoveBusinessObject_ShouldRemoveChildrenFromNodeStates()
        {
            //---------------Set up test pack-------------------
            ITreeView treeView = GlobalUIRegistry.ControlFactory.CreateTreeView();
            TreeViewController treeViewController = new TreeViewController(treeView);
            OrganisationTestBO organisation = OrganisationTestBO.CreateSavedOrganisation();
            ContactPersonTestBO contactPerson1 = CreateUnsavedContactPerson(organisation);
            ContactPersonTestBO contactPerson2 = CreateUnsavedContactPerson(organisation);
            AddressTestBO address1 = AddressTestBO.CreateUnsavedAddress(contactPerson2);
            AddressTestBO address2 = AddressTestBO.CreateUnsavedAddress(contactPerson2);
            AddressTestBO address3 = AddressTestBO.CreateUnsavedAddress(contactPerson1);
            treeViewController.LoadTreeView(organisation, 4);
            ITreeNode dbNode = treeView.Nodes[0];
            ITreeNode relationshipNode = dbNode.Nodes[0];
            //-------------Assert Preconditions -------------
            //---------------Execute Test ----------------------
            organisation.ContactPeople.Remove(contactPerson2);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, relationshipNode.Nodes.Count);
            System.Collections.IDictionary objectNodes =
                (System.Collections.IDictionary)ReflectionUtilities.GetPrivatePropertyValue(treeViewController, "ObjectNodes");

            Assert.IsFalse(objectNodes.Contains(contactPerson2));
            Assert.IsFalse(objectNodes.Contains(contactPerson2.Addresses[0]));
            Assert.IsFalse(objectNodes.Contains(contactPerson2.Addresses[1]));

            System.Collections.IDictionary relationshipNodes =
                (System.Collections.IDictionary)ReflectionUtilities.GetPrivatePropertyValue(treeViewController, "RelationshipNodes");
            Assert.IsFalse(relationshipNodes.Contains(contactPerson2.Relationships["Addresses"]));


            IDictionary childCollectionNodes =
                (System.Collections.IDictionary)ReflectionUtilities.GetPrivatePropertyValue(treeViewController, "ChildCollectionNodes");
            Assert.IsFalse(childCollectionNodes.Contains(contactPerson2.Addresses));
        }

        [Test]
        public void Test_RootNodeBusinessObject()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisation = OrganisationTestBO.CreateSavedOrganisation();


            ITreeView treeView = GlobalUIRegistry.ControlFactory.CreateTreeView();
            TreeViewController treeViewController = new TreeViewController(treeView);
            treeViewController.LoadTreeView(organisation);
            //-------------Assert Preconditions -------------
            Assert.AreEqual(1, treeView.Nodes.Count);
            //---------------Execute Test ----------------------
            IBusinessObject businessObject = treeViewController.RootNodeBusinessObject;
            //---------------Test Result -----------------------
            Assert.AreSame(organisation, businessObject);
        }

       

        [Test]
        public void Test_LoadSingleBO_NoChildren()
        {
            //---------------Set up test pack-------------------

            AddressTestBO addressTestBO = AddressTestBO.CreateUnsavedAddress(ContactPersonTestBO.CreateUnsavedContactPerson());

            ITreeView treeView = GlobalUIRegistry.ControlFactory.CreateTreeView();
            TreeViewController treeViewController = new TreeViewController(treeView);

            //-------------Assert Preconditions -------------
            Assert.AreEqual(0, treeView.Nodes.Count);
            //---------------Execute Test ----------------------
            treeViewController.LoadTreeView(addressTestBO);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, treeView.Nodes.Count);
            ITreeNode node = treeView.Nodes[0];
            Assert.AreEqual(addressTestBO.ToString(), node.Text);
            Assert.AreEqual(0, node.Nodes.Count);
        }

        [Test]
        public void Test_LoadSingleBO_OneChildRelationship_NotExpanded_HasADummyNode()
        {
            //---------------Set up test pack-------------------

            OrganisationTestBO organisation = OrganisationTestBO.CreateSavedOrganisation();
            ITreeView treeView = GlobalUIRegistry.ControlFactory.CreateTreeView();
            TreeViewController treeViewController = new TreeViewController(treeView);
            //-------------Assert Preconditions -------------
            Assert.AreEqual(0, treeView.Nodes.Count);
            //---------------Execute Test ----------------------
            treeViewController.LoadTreeView(organisation);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, treeView.Nodes.Count);
            ITreeNode dbNode = treeView.Nodes[0];
            Assert.AreEqual(organisation.ToString(), dbNode.Text);
            Assert.AreEqual(1, dbNode.Nodes.Count);
            ITreeNode relationshipNode = dbNode.Nodes[0];
            Assert.AreEqual
                ("$DUMMY$", relationshipNode.Text,
                 "Dummy node present so that the tree view has the + icon for expanding");
        }

        [Test]
        public void Test_LoadSingleBO_OneChildRelationship_Expanded()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisation = OrganisationTestBO.CreateSavedOrganisation();
            ITreeView treeView = GlobalUIRegistry.ControlFactory.CreateTreeView();
            TreeViewController treeViewController = new TreeViewController(treeView);
            //-------------Assert Preconditions -------------
            Assert.AreEqual(0, treeView.Nodes.Count);
            //---------------Execute Test ----------------------
            treeViewController.LoadTreeView(organisation, 1);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, treeView.Nodes.Count);
            ITreeNode dbNode = treeView.Nodes[0];
            Assert.AreEqual(organisation.ToString(), dbNode.Text);
            Assert.AreEqual(1, dbNode.Nodes.Count);
            ITreeNode tablesRelationshipNode = dbNode.Nodes[0];
            Assert.AreEqual
                ("ContactPeople", tablesRelationshipNode.Text,
                 "This is expanded so the ContactPeople Relationship should be loaded into the tree view");
        }

        [Test]
        public void Test_LoadSingleBO_OneChild_Expanded()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisation = OrganisationTestBO.CreateSavedOrganisation();
            ContactPersonTestBO contactPersonTestBO = organisation.ContactPeople.CreateBusinessObject();
            contactPersonTestBO.FirstName = TestUtil.GetRandomString();
            contactPersonTestBO.Surname = TestUtil.GetRandomString();

            ITreeView treeView = GlobalUIRegistry.ControlFactory.CreateTreeView();
            TreeViewController treeViewController = new TreeViewController(treeView);
            //-------------Assert Preconditions -------------
            Assert.AreEqual(0, treeView.Nodes.Count);
            //---------------Execute Test ----------------------
            treeViewController.LoadTreeView(organisation, 2);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, treeView.Nodes.Count);
            ITreeNode dbNode = treeView.Nodes[0];
            Assert.AreEqual(organisation.ToString(), dbNode.Text);
            Assert.AreEqual(1, dbNode.Nodes.Count);
            ITreeNode relationshipNode = dbNode.Nodes[0];
            Assert.AreEqual("ContactPeople", relationshipNode.Text);
            Assert.AreEqual(1, relationshipNode.Nodes.Count);
            ITreeNode contactPersonBONode = relationshipNode.Nodes[0];
            Assert.AreEqual(contactPersonTestBO.ToString(), contactPersonBONode.Text);
        }

        [Test]
        public void Test_LoadSingleBO_TwoChildren_Expanded()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisation = OrganisationTestBO.CreateSavedOrganisation();
            CreateUnsavedContactPerson(organisation);
            CreateUnsavedContactPerson(organisation);

            ITreeView treeView = GlobalUIRegistry.ControlFactory.CreateTreeView();
            TreeViewController treeViewController = new TreeViewController(treeView);
           
            //-------------Assert Preconditions -------------
            Assert.AreEqual(0, treeView.Nodes.Count);
            Assert.AreEqual(2, organisation.ContactPeople.Count);
            //---------------Execute Test ----------------------
            treeViewController.LoadTreeView(organisation, 2);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, treeView.Nodes.Count);
            ITreeNode dbNode = treeView.Nodes[0];
            ITreeNode relationshipNode = dbNode.Nodes[0];
            Assert.AreEqual(2, relationshipNode.Nodes.Count, "Should have two Contact People");
            AssertChildNodeLoadedInTree(organisation.ContactPeople, relationshipNode, 0);

            AssertChildNodeLoadedInTree(organisation.ContactPeople, relationshipNode, 1);
        }

        private ContactPersonTestBO CreateUnsavedContactPerson(OrganisationTestBO organisation)
        {
            ContactPersonTestBO contactPersonTestBO = organisation.ContactPeople.CreateBusinessObject();
            contactPersonTestBO.FirstName = TestUtil.GetRandomString();
            contactPersonTestBO.Surname = TestUtil.GetRandomString();
            return contactPersonTestBO;
        }

     

        /// <summary>
        /// Verifies that the item in the tree nodes collection identified by the index is matched to the the 
        ///  item in the Business object collection identified by the index. I.e. the ordering of the nodes collection matches the 
        ///  business object collection.
        /// </summary>
        /// <param name="relationship"></param>
        /// <param name="relationshipNode"></param>
        /// <param name="index"></param>
        private static void AssertChildNodeLoadedInTree
            (IBusinessObjectCollection relationship, ITreeNode relationshipNode, int index)
        {
            Assert.AreEqual(relationship[index].ToString(), relationshipNode.Nodes[index].Text);
        }

        [Test]
        public void Test_SetupNodeWithBusinessObject_LoadSingleBo_CustomDisplayValue()
        {
            //---------------Set up test pack-------------------
            AddressTestBO addressTestBO = AddressTestBO.CreateUnsavedAddress(ContactPersonTestBO.CreateUnsavedContactPerson());
            string customDisplayValue = TestUtil.GetRandomString();
            ITreeView treeView = GlobalUIRegistry.ControlFactory.CreateTreeView();
            TreeViewController treeViewController = new TreeViewController(treeView);
            
            //-------------Assert Preconditions -------------
            Assert.AreEqual(0, treeView.Nodes.Count);
            //---------------Execute Test ----------------------
            treeViewController.SetupNodeWithBusinessObject +=
                delegate(ITreeNode treeNode, IBusinessObject businessObject) { treeNode.Text = customDisplayValue; };
            treeViewController.LoadTreeView(addressTestBO);
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
            OrganisationTestBO organisation = OrganisationTestBO.CreateSavedOrganisation();
            ITreeView treeView = GlobalUIRegistry.ControlFactory.CreateTreeView();
            TreeViewController treeViewController = new TreeViewController(treeView);
           
            treeViewController.LoadTreeView(organisation);
            //-------------Assert Preconditions -------------
            Assert.AreEqual(1, treeView.Nodes.Count);
            //---------------Execute Test ----------------------
            ITreeNode fetchedTreeNode = treeViewController.GetBusinessObjectTreeNode(organisation);
            //---------------Test Result -----------------------
            Assert.IsNotNull(fetchedTreeNode);
            Assert.AreSame(fetchedTreeNode, treeView.Nodes[0]);
        }

        [Test]
        public void Test_GetBusinessObjectTreeNode_ChildNode()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisation = OrganisationTestBO.CreateSavedOrganisation();
            ContactPersonTestBO contactPerson = CreateUnsavedContactPerson(organisation);

            ITreeView treeView = GlobalUIRegistry.ControlFactory.CreateTreeView();
            TreeViewController treeViewController = new TreeViewController(treeView);

            treeViewController.LoadTreeView(organisation, 2);
            //-------------Assert Preconditions -------------
            Assert.AreEqual(1, treeView.Nodes.Count);
            Assert.AreEqual(1, treeView.Nodes[0].Nodes.Count);
            Assert.AreEqual(1, treeView.Nodes[0].Nodes[0].Nodes.Count);
            //---------------Execute Test ----------------------
            ITreeNode fetchedTreeNode = treeViewController.GetBusinessObjectTreeNode(contactPerson);
            //---------------Test Result -----------------------
            Assert.IsNotNull(fetchedTreeNode);
            Assert.AreSame(fetchedTreeNode, treeView.Nodes[0].Nodes[0].Nodes[0]);
        }

        [Test]
        public void Test_GetBusinessObjectTreeNode_NotInTree()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisation = OrganisationTestBO.CreateSavedOrganisation();
            ITreeView treeView = GlobalUIRegistry.ControlFactory.CreateTreeView();
            TreeViewController treeViewController = new TreeViewController(treeView);

            treeViewController.LoadTreeView(organisation);
            //-------------Assert Preconditions -------------
            Assert.AreEqual(1, treeView.Nodes.Count);
            //---------------Execute Test ----------------------
            ITreeNode fetchedTreeNode = treeViewController.GetBusinessObjectTreeNode(new OrganisationTestBO());
            //---------------Test Result -----------------------
            Assert.IsNull(fetchedTreeNode);
        }

        [Test]
        public void Test_SetBusinessObjectVisibility_False_ShouldHideNode()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisation = OrganisationTestBO.CreateSavedOrganisation();
            ContactPersonTestBO contactPerson1 = CreateUnsavedContactPerson(organisation);


            ITreeView treeView = GlobalUIRegistry.ControlFactory.CreateTreeView();
            TreeViewController treeViewController = new TreeViewController(treeView);

            treeViewController.LoadTreeView(organisation, 2);
            //-------------Assert Preconditions -------------
            Assert.AreEqual(1, treeView.Nodes.Count);
            ITreeNode rootNode = treeView.Nodes[0];
            Assert.AreEqual(organisation.ToString(), rootNode.Text);
            Assert.AreEqual(1, rootNode.Nodes.Count);
            ITreeNode relationshipNode = rootNode.Nodes[0];
            Assert.AreEqual("ContactPeople", relationshipNode.Text);
            Assert.AreEqual(1, relationshipNode.Nodes.Count);
            ITreeNode childNode = relationshipNode.Nodes[0];
            Assert.AreEqual(contactPerson1.ToString(), childNode.Text);
            //---------------Execute Test ----------------------
            treeViewController.SetVisibility(contactPerson1, false);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, treeView.Nodes.Count);
            Assert.AreEqual(organisation.ToString(), rootNode.Text);
            Assert.AreEqual(1, rootNode.Nodes.Count);
            Assert.AreEqual("ContactPeople", relationshipNode.Text);
            Assert.AreEqual(0, relationshipNode.Nodes.Count);
        }

        [Test]
        public void Test_SetBusinessObjectVisibility_True_ShouldShowNode()
        {
            //---------------Set up test pack-------------------
            ITreeView treeView = GlobalUIRegistry.ControlFactory.CreateTreeView();
            TreeViewController treeViewController = new TreeViewController(treeView);
            OrganisationTestBO organisation = OrganisationTestBO.CreateSavedOrganisation();
            ContactPersonTestBO contactPerson = CreateUnsavedContactPerson(organisation);
    
            treeViewController.LoadTreeView(organisation, 2);
            //-------------Assert Preconditions -------------
            Assert.AreEqual(1, treeView.Nodes.Count);
            ITreeNode rootNode = treeView.Nodes[0];
            Assert.AreEqual(organisation.ToString(), rootNode.Text);
            Assert.AreEqual(1, rootNode.Nodes.Count);
            ITreeNode relationshipNode = rootNode.Nodes[0];
            Assert.AreEqual("ContactPeople", relationshipNode.Text);
            Assert.AreEqual(1, relationshipNode.Nodes.Count);
            ITreeNode childNode = relationshipNode.Nodes[0];
            Assert.AreEqual(contactPerson.ToString(), childNode.Text);
            //---------------Execute Test ----------------------
            treeViewController.SetVisibility(contactPerson, true);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, treeView.Nodes.Count);
            Assert.AreEqual(organisation.ToString(), rootNode.Text);
            Assert.AreEqual(1, rootNode.Nodes.Count);
            Assert.AreEqual("ContactPeople", relationshipNode.Text);
            Assert.AreEqual(1, relationshipNode.Nodes.Count);
        }

        [Test]
        public void Test_SetVisibility_False_ThenTrue()
        {
            //---------------Set up test pack-------------------
            ITreeView treeView = GlobalUIRegistry.ControlFactory.CreateTreeView();
            TreeViewController treeViewController = new TreeViewController(treeView);
            OrganisationTestBO organisation = OrganisationTestBO.CreateSavedOrganisation();
            ContactPersonTestBO contactPerson = CreateUnsavedContactPerson(organisation);
    
            treeViewController.LoadTreeView(organisation, 2);
            treeViewController.SetVisibility(contactPerson, false);
            //-------------Assert Preconditions -------------
            Assert.AreEqual(1, treeView.Nodes.Count);
            ITreeNode rootNode = treeView.Nodes[0];
            Assert.AreEqual(organisation.ToString(), rootNode.Text);
            Assert.AreEqual(1, rootNode.Nodes.Count);
            ITreeNode relationshipNode = rootNode.Nodes[0];
            Assert.AreEqual("ContactPeople", relationshipNode.Text);
            Assert.AreEqual(0, relationshipNode.Nodes.Count);
            //---------------Execute Test ----------------------
            treeViewController.SetVisibility(contactPerson, true);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, treeView.Nodes.Count);
            Assert.AreEqual(organisation.ToString(), rootNode.Text);
            Assert.AreEqual(1, rootNode.Nodes.Count);
            Assert.AreEqual("ContactPeople", relationshipNode.Text);
            Assert.AreEqual(1, relationshipNode.Nodes.Count);
        }

        [Test]
        public void Test_SetVisibility_False_ThenTrue_CorrectOrder()
        {
            //---------------Set up test pack-------------------
            ITreeView treeView = GlobalUIRegistry.ControlFactory.CreateTreeView();
            TreeViewController treeViewController = new TreeViewController(treeView);
            OrganisationTestBO organisation = OrganisationTestBO.CreateSavedOrganisation();
            ContactPersonTestBO contactPerson1 = CreateUnsavedContactPerson(organisation);
            ContactPersonTestBO contactPerson2 = CreateUnsavedContactPerson(organisation);
            ContactPersonTestBO contactPerson3 = CreateUnsavedContactPerson(organisation);
            treeViewController.LoadTreeView(organisation, 2);
            treeViewController.SetVisibility(contactPerson2, false);
            //-------------Assert Preconditions -------------
            Assert.AreEqual(1, treeView.Nodes.Count);
            ITreeNode rootNode = treeView.Nodes[0];
            Assert.AreEqual(organisation.ToString(), rootNode.Text);
            Assert.AreEqual(1, rootNode.Nodes.Count);
            ITreeNode relationshipNode = rootNode.Nodes[0];
            Assert.AreEqual("ContactPeople", relationshipNode.Text);
            Assert.AreEqual(2, relationshipNode.Nodes.Count);
            //---------------Execute Test ----------------------
            treeViewController.SetVisibility(contactPerson2, true);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, treeView.Nodes.Count);
            Assert.AreEqual(organisation.ToString(), rootNode.Text);
            Assert.AreEqual(1, rootNode.Nodes.Count);
            Assert.AreEqual("ContactPeople", relationshipNode.Text);
            Assert.AreEqual(3, relationshipNode.Nodes.Count);
            Assert.AreEqual(treeViewController.GetBusinessObjectTreeNode(contactPerson1), relationshipNode.Nodes[0]);
            Assert.AreEqual(treeViewController.GetBusinessObjectTreeNode(contactPerson2), relationshipNode.Nodes[1]);
            Assert.AreEqual(treeViewController.GetBusinessObjectTreeNode(contactPerson3), relationshipNode.Nodes[2]);
        }

        [Test]
        public void Test_LoadBusinessObjectCollection()
        {
            //---------------Set up test pack-------------------
            ITreeView treeView = GlobalUIRegistry.ControlFactory.CreateTreeView();
            TreeViewController treeViewController = new TreeViewController(treeView);
            AddressTestBO address = AddressTestBO.CreateUnsavedAddress(ContactPersonTestBO.CreateUnsavedContactPerson());
            IBusinessObjectCollection addresses = new BusinessObjectCollection<AddressTestBO> { address };
            //-------------Assert Preconditions -------------
            Assert.AreEqual(1, addresses.Count);
            Assert.AreEqual(0, treeView.Nodes.Count);
            //---------------Execute Test ----------------------
            treeViewController.LoadTreeView(addresses);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, treeView.Nodes.Count);
            ITreeNode node = treeView.Nodes[0];
            Assert.AreEqual(address.ToString(), node.Text);
            Assert.AreEqual(0, node.Nodes.Count);

        }

        [Test]
        public void Test_LoadBusinessObjectCollection_TwoItems()
        {
            //---------------Set up test pack-------------------
            ITreeView treeView = GlobalUIRegistry.ControlFactory.CreateTreeView();
            TreeViewController treeViewController = new TreeViewController(treeView);
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateUnsavedContactPerson();
            AddressTestBO address1 = AddressTestBO.CreateUnsavedAddress(contactPerson);
            AddressTestBO address2 = AddressTestBO.CreateUnsavedAddress(contactPerson);
            IBusinessObjectCollection addresses = new BusinessObjectCollection<AddressTestBO> { address1, address2 };
 
            //-------------Assert Preconditions -------------
            Assert.AreEqual(2, addresses.Count);
            Assert.AreEqual(0, treeView.Nodes.Count);
            //---------------Execute Test ----------------------
            treeViewController.LoadTreeView(addresses);
            //---------------Test Result -----------------------
            Assert.AreEqual(2, treeView.Nodes.Count);
            ITreeNode node = treeView.Nodes[0];
            Assert.AreEqual(address1.ToString(), node.Text);
            Assert.AreEqual(0, node.Nodes.Count);
            ITreeNode node2 = treeView.Nodes[1];
            Assert.AreEqual(address2.ToString(), node2.Text);
            Assert.AreEqual(0, node2.Nodes.Count);
        }

        [Test]
        public void Test_LoadBusinessObjectCollection_OneChild_Expanded()
        {
            //---------------Set up test pack-------------------
            ITreeView treeView = GlobalUIRegistry.ControlFactory.CreateTreeView();
            TreeViewController treeViewController = new TreeViewController(treeView);
            OrganisationTestBO organisation = OrganisationTestBO.CreateSavedOrganisation();
            ContactPersonTestBO contactPerson = CreateUnsavedContactPerson(organisation);

            IBusinessObjectCollection organisations = new BusinessObjectCollection<OrganisationTestBO> { organisation };
            //-------------Assert Preconditions -------------
            Assert.AreEqual(1, organisations.Count);
            Assert.AreEqual(0, treeView.Nodes.Count);
            //---------------Execute Test ----------------------
            treeViewController.LoadTreeView(organisations, 2);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, treeView.Nodes.Count);
            ITreeNode node = treeView.Nodes[0];
            Assert.AreEqual(organisation.ToString(), node.Text);
            Assert.AreEqual(1, node.Nodes.Count);
            ITreeNode relationshipNode = node.Nodes[0];
            Assert.AreEqual("ContactPeople", relationshipNode.Text);
            Assert.AreEqual(1, relationshipNode.Nodes.Count);
            ITreeNode childNode = relationshipNode.Nodes[0];
            Assert.AreEqual(contactPerson.ToString(), childNode.Text);
        }

        //[Test]
        //[Ignore("Need to implement")] //TODO Brett 18 Mar 2009: Ignored Test - Need to implement
        //public void Test_LoadBusinessObjectCollection_Expanded_ChildAdded()
        //{
        //    //---------------Set up test pack-------------------
        //    TreeViewWin treeView = new TreeViewWin();
        //    TreeViewController treeViewController = new TreeViewController(treeView);
        //    DBDatabase organisation = new DBDatabase { DatabaseName = "Database" + TestUtilsShared.GetRandomString() };
        //    DBDatabase dbDatabase2 = new DBDatabase { DatabaseName = "Database(2)" + TestUtilsShared.GetRandomString() };
        //    DBTable contactPerson = new DBTable("Table" + TestUtilsShared.GetRandomString());
        //    organisation.ContactPeople.Add(contactPerson);
        //    IBusinessObjectCollection dbDatabases = new BusinessObjectCollection<DBDatabase> { organisation };
        //    treeViewController.LoadTreeView(dbDatabases, 2);
        //    bool busObjectAddedEvent = false;
        //    dbDatabases.BusinessObjectAdded += (sender, e) => busObjectAddedEvent = true;
        //    //-------------Assert Preconditions -------------
        //    Assert.AreEqual(1, dbDatabases.Count);
        //    Assert.AreEqual(1, treeView.Nodes.Count);
        //    ITreeNode databaseNode = treeView.Nodes[0];
        //    Assert.AreEqual(organisation.ToString(), databaseNode.Text);
        //    Assert.AreEqual(1, databaseNode.Nodes.Count);
        //    ITreeNode relationshipNode = databaseNode.Nodes[0];
        //    Assert.AreEqual("ContactPeople", relationshipNode.Text);
        //    Assert.AreEqual(1, relationshipNode.Nodes.Count);
        //    ITreeNode childNode = relationshipNode.Nodes[0];
        //    Assert.AreEqual(contactPerson.ToString(), childNode.Text);
        //    Assert.IsFalse(busObjectAddedEvent);
        //    //---------------Execute Test ----------------------
        //    dbDatabases.Add(dbDatabase2);
        //    //---------------Test Result -----------------------
        //    Assert.IsTrue(busObjectAddedEvent);
        //    Assert.AreEqual(2, treeView.Nodes.Count);
        //    ITreeNode databaseNode2 = treeView.Nodes[1];
        //    Assert.AreEqual(dbDatabase2.ToString(), databaseNode2.Text);
        //    Assert.AreEqual(1, databaseNode2.Nodes.Count);
        //}

        //[Test]
        //[Ignore("//TODO Brett 18 Mar 2009: Woking on this")]
        //public void Test_LoadBusinessObjectCollection_LevelsToDisplay()
        //{
        //    //---------------Set up test pack-------------------
        //    TreeViewWin treeView = new TreeViewWin();
        //    TreeViewController treeViewController = new TreeViewController(treeView);
        //    DBDatabase organisation = new DBDatabase { DatabaseName = TestUtilsShared.GetRandomString() };
        //    DBTable contactPerson = new DBTable(TestUtilsShared.GetRandomString());
        //    organisation.ContactPeople.Add(contactPerson);
        //    DBColumn dbColumn = new DBColumn();
        //    contactPerson.Addresses.Add(dbColumn);
        //    IBusinessObjectCollection dbDatabases = new BusinessObjectCollection<DBDatabase> { organisation };
        //    //-------------Assert Preconditions -------------
        //    Assert.AreEqual(1, dbDatabases.Count);
        //    Assert.AreEqual(0, treeView.Nodes.Count);
        //    //---------------Execute Test ----------------------
        //    treeViewController.LoadTreeView(dbDatabases, 3, 2);
        //    //---------------Test Result -----------------------
        //    Assert.AreEqual(1, treeView.Nodes.Count);
        //    ITreeNode node = treeView.Nodes[0];
        //    Assert.AreEqual(organisation.ToString(), node.Text);
        //    Assert.AreEqual(1, node.Nodes.Count);
        //    ITreeNode relationshipNode = node.Nodes[0];
        //    Assert.AreEqual("ContactPeople", relationshipNode.Text);
        //    Assert.AreEqual(1, relationshipNode.Nodes.Count);
        //    ITreeNode childNode = relationshipNode.Nodes[0];
        //    Assert.AreEqual(contactPerson.ToString(), childNode.Text);
        //    Assert.AreEqual(0, childNode.Nodes.Count);
        //}

        [Test]
        public void Test_LoadTreeView_WithRelationship()
        {
            //---------------Set up test pack-------------------
            ITreeView treeView = GlobalUIRegistry.ControlFactory.CreateTreeView();
            TreeViewController treeViewController = new TreeViewController(treeView);
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateUnsavedContactPerson();
            AddressTestBO address = AddressTestBO.CreateUnsavedAddress(contactPerson);
            IRelationship relationship = contactPerson.Relationships["Addresses"];
            //-------------Assert Preconditions -------------
            Assert.IsInstanceOfType(typeof(IMultipleRelationship), relationship);
            IMultipleRelationship multipleRelationship = (IMultipleRelationship)relationship;
            Assert.AreEqual(1, multipleRelationship.BusinessObjectCollection.Count);
            Assert.AreEqual(1, contactPerson.Addresses.Count);
            Assert.AreEqual(0, treeView.Nodes.Count);
            //---------------Execute Test ----------------------
            treeViewController.LoadTreeView(relationship);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, treeView.Nodes.Count);
            ITreeNode relationshipNode = treeView.Nodes[0];
            Assert.AreEqual(relationship.RelationshipName, relationshipNode.Text);
            Assert.AreEqual(1, relationshipNode.Nodes.Count);
        }

        [Test]
        public void Test_LoadTreeView_WithRelationship_TwoChildren()
        {
            //---------------Set up test pack-------------------
            ITreeView treeView = GlobalUIRegistry.ControlFactory.CreateTreeView();
            TreeViewController treeViewController = new TreeViewController(treeView);

            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateUnsavedContactPerson();
            AddressTestBO address1 = AddressTestBO.CreateUnsavedAddress(contactPerson);
            AddressTestBO address2 = AddressTestBO.CreateUnsavedAddress(contactPerson);
            IRelationship relationship = contactPerson.Relationships["Addresses"];
            //-------------Assert Preconditions -------------
            Assert.IsInstanceOfType(typeof(IMultipleRelationship), relationship);
            IMultipleRelationship multipleRelationship = (IMultipleRelationship)relationship;
            Assert.AreEqual(2, multipleRelationship.BusinessObjectCollection.Count);
            Assert.AreEqual(0, treeView.Nodes.Count);
            //---------------Execute Test ----------------------
            treeViewController.LoadTreeView(relationship, 2);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, treeView.Nodes.Count);
            ITreeNode relationshipNode = treeView.Nodes[0];
            Assert.AreEqual(relationship.RelationshipName, relationshipNode.Text);
            Assert.AreEqual(2, relationshipNode.Nodes.Count);
        }

        [Test]
        public void Test_LoadTreeView_WithRelationship_WhenExpandLevelsSpecified()
        {
            //---------------Set up test pack-------------------
            ITreeView treeView = GlobalUIRegistry.ControlFactory.CreateTreeView();
            TreeViewController treeViewController = new TreeViewController(treeView);
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateUnsavedContactPerson();
            AddressTestBO address = AddressTestBO.CreateUnsavedAddress(contactPerson);
            IRelationship relationship = contactPerson.Relationships["Addresses"];

            //-------------Assert Preconditions -------------
            Assert.IsInstanceOfType(typeof(IMultipleRelationship), relationship);
            IMultipleRelationship multipleRelationship = (IMultipleRelationship)relationship;
            Assert.AreEqual(1, multipleRelationship.BusinessObjectCollection.Count);
            Assert.AreEqual(0, treeView.Nodes.Count);
            //---------------Execute Test ----------------------
            treeViewController.LoadTreeView(relationship, 2);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, treeView.Nodes.Count);
            ITreeNode relationshipNode = treeView.Nodes[0];
            Assert.AreEqual(relationship.RelationshipName, relationshipNode.Text);
            Assert.AreEqual(1, relationshipNode.Nodes.Count);
            ITreeNode childNode = relationshipNode.Nodes[0];
            Assert.AreEqual(address.ToString(), childNode.Text);
            Assert.AreEqual(0, childNode.Nodes.Count);
        }

        [Test]
        public void Test_LoadTreeView_WithRelationship_WhenExpandLevelsSpecified_WithDisplayLevelZero()
        {
            //---------------Set up test pack-------------------
            ITreeView treeView = GlobalUIRegistry.ControlFactory.CreateTreeView();
            TreeViewController treeViewController = new TreeViewController(treeView);
            OrganisationTestBO organisation = OrganisationTestBO.CreateSavedOrganisation();
            ContactPersonTestBO contactPerson = CreateUnsavedContactPerson(organisation);
            AddressTestBO address = AddressTestBO.CreateUnsavedAddress(contactPerson);
            IRelationship relationship = organisation.Relationships["ContactPeople"];
            //-------------Assert Preconditions -------------
            Assert.IsInstanceOfType(typeof(IMultipleRelationship), relationship);
            IMultipleRelationship multipleRelationship = (IMultipleRelationship)relationship;
            Assert.AreEqual(1, multipleRelationship.BusinessObjectCollection.Count);
            Assert.AreEqual(0, treeView.Nodes.Count);
            //---------------Execute Test ----------------------
            treeViewController.LoadTreeView(relationship, 2, 0);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, treeView.Nodes.Count);
            ITreeNode relationshipNode = treeView.Nodes[0];
            Assert.AreEqual(relationship.RelationshipName, relationshipNode.Text);
            Assert.AreEqual(0, relationshipNode.Nodes.Count);
        }

        [Test]
        public void Test_LoadTreeView_WithRelationship_WhenExpandLevelsSpecified_WithDisplayLevelOne()
        {
            //---------------Set up test pack-------------------
            ITreeView treeView = GlobalUIRegistry.ControlFactory.CreateTreeView();
            TreeViewController treeViewController = new TreeViewController(treeView);
            OrganisationTestBO organisation = OrganisationTestBO.CreateSavedOrganisation();
            ContactPersonTestBO contactPerson = CreateUnsavedContactPerson(organisation);
            AddressTestBO address = AddressTestBO.CreateUnsavedAddress(contactPerson);
            IRelationship relationship = organisation.Relationships["ContactPeople"];
            //-------------Assert Preconditions -------------
            Assert.IsInstanceOfType(typeof(IMultipleRelationship), relationship);
            IMultipleRelationship multipleRelationship = (IMultipleRelationship)relationship;
            Assert.AreEqual(1, multipleRelationship.BusinessObjectCollection.Count);
            Assert.AreEqual(0, treeView.Nodes.Count);
            //---------------Execute Test ----------------------
            treeViewController.LoadTreeView(relationship, 3, 1);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, treeView.Nodes.Count);
            ITreeNode relationshipNode = treeView.Nodes[0];
            Assert.AreEqual(relationship.RelationshipName, relationshipNode.Text);
            Assert.AreEqual(1, relationshipNode.Nodes.Count);
            ITreeNode tableChildNode = relationshipNode.Nodes[0];
            Assert.AreEqual(contactPerson.ToString(), tableChildNode.Text);
            Assert.AreEqual(0, tableChildNode.Nodes.Count);
        }

        [Test]
        public void Test_LoadTreeView_WithRelationship_WhenExpandLevelsSpecified_WithDisplayLevel()
        {
            //---------------Set up test pack-------------------
            ITreeView treeView = GlobalUIRegistry.ControlFactory.CreateTreeView();
            TreeViewController treeViewController = new TreeViewController(treeView);


            OrganisationTestBO organisation = OrganisationTestBO.CreateSavedOrganisation();
            ContactPersonTestBO contactPerson = CreateUnsavedContactPerson(organisation);
            AddressTestBO address = AddressTestBO.CreateUnsavedAddress(contactPerson);
            IRelationship relationship = organisation.Relationships["ContactPeople"];
            //-------------Assert Preconditions -------------
            Assert.IsInstanceOfType(typeof(IMultipleRelationship), relationship);
            IMultipleRelationship multipleRelationship = (IMultipleRelationship)relationship;
            Assert.AreEqual(1, multipleRelationship.BusinessObjectCollection.Count);
            Assert.AreEqual(0, treeView.Nodes.Count);
            //---------------Execute Test ----------------------
            treeViewController.LoadTreeView(relationship, 3, 2);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, treeView.Nodes.Count);
            ITreeNode relationshipNode = treeView.Nodes[0];
            Assert.AreEqual(relationship.RelationshipName, relationshipNode.Text);
            Assert.AreEqual(1, relationshipNode.Nodes.Count);
            ITreeNode tableChildNode = relationshipNode.Nodes[0];
            Assert.AreEqual(contactPerson.ToString(), tableChildNode.Text);

            Assert.AreEqual(1, tableChildNode.Nodes.Count);
            ITreeNode columnRelationshipNode = tableChildNode.Nodes[0];
            Assert.AreEqual("Addresses", columnRelationshipNode.Text);
            Assert.AreEqual(0, columnRelationshipNode.Nodes.Count);
           
        }




        [Test]
        public void Test_AddNewParentToTreeView_ThenAddChildren_ThenExpandParent_ShouldHaveChildren()
        {
            //---------------Set up test pack-------------------
            ITreeView treeView = GlobalUIRegistry.ControlFactory.CreateTreeView();
            TreeViewController treeViewController = new TreeViewController(treeView);
            OrganisationTestBO organisation = OrganisationTestBO.CreateSavedOrganisation();
            ContactPersonTestBO contactPerson = CreateUnsavedContactPerson(organisation);

            IBusinessObjectCollection organisations = new BusinessObjectCollection<OrganisationTestBO> { organisation };
            treeViewController.LoadTreeView(organisations, 3);

            //-------------Assert Preconditions -------------
            Assert.AreEqual(1, treeView.Nodes.Count);
            ITreeNode dbNode = treeView.Nodes[0];
            Assert.AreEqual(organisation.ToString(), dbNode.Text);
            Assert.AreEqual(1, dbNode.Nodes.Count);
            ITreeNode cpRelationshipNode = dbNode.Nodes[0];
            Assert.AreEqual("ContactPeople", cpRelationshipNode.Text);
            Assert.AreEqual(1, cpRelationshipNode.Nodes.Count);
            ITreeNode tableNode = cpRelationshipNode.Nodes[0];
            Assert.AreEqual(contactPerson.ToString(), tableNode.Text);
            ITreeNode columnRelationshipNode = tableNode.Nodes[0];
            Assert.AreEqual("Addresses", columnRelationshipNode.Text);
            //---------------Execute Test ----------------------
            ContactPersonTestBO newContactPerson = organisation.ContactPeople.CreateBusinessObject();
            newContactPerson.Surname = TestUtil.GetRandomString();
            AddressTestBO newAddress = newContactPerson.Addresses.CreateBusinessObject();
           
            ITreeNode newCpNode = cpRelationshipNode.Nodes[1];
            newCpNode.Expand();
            ReflectionUtilities.ExecutePrivateMethod(treeViewController, "ExpandNode", newCpNode);

            ITreeNode newColumnsRelationshipNode = newCpNode.Nodes[0];
            newColumnsRelationshipNode.Expand();
            ReflectionUtilities.ExecutePrivateMethod(treeViewController, "ExpandNode", newColumnsRelationshipNode);
            //---------------Test Result -----------------------
            Assert.AreEqual(2, cpRelationshipNode.Nodes.Count);

            Assert.AreEqual(newContactPerson.ToString(), newCpNode.Text);
           
            Assert.AreEqual("Addresses", newColumnsRelationshipNode.Text);
            Assert.AreEqual(1, newColumnsRelationshipNode.Nodes.Count);
            ITreeNode newColumNode = newColumnsRelationshipNode.Nodes[0];
            Assert.AreEqual(newAddress.ToString(), newColumNode.Text);
           
        }

        [Test]
        public void Test_LoadRelationships_AddNewParentToTreeView_ThenAddChildren_ThenExpandParent_ShouldHaveChildre()
        {
            //---------------Set up test pack-------------------
            ITreeView treeView = GlobalUIRegistry.ControlFactory.CreateTreeView();
            TreeViewController treeViewController = new TreeViewController(treeView);
            OrganisationTestBO organisation = OrganisationTestBO.CreateSavedOrganisation();
            ContactPersonTestBO contactPerson = CreateUnsavedContactPerson(organisation);
            IBusinessObjectCollection organisations = new BusinessObjectCollection<OrganisationTestBO> { organisation };
            treeViewController.LoadTreeView(organisation.Relationships["ContactPeople"], 2);

            //-------------Assert Preconditions -------------
            Assert.AreEqual(1, treeView.Nodes.Count);
            ITreeNode cpRelationshipNode = treeView.Nodes[0];
            Assert.AreEqual("ContactPeople", cpRelationshipNode.Text);
            Assert.AreEqual(1, cpRelationshipNode.Nodes.Count);
            ITreeNode cpNode = cpRelationshipNode.Nodes[0];
            Assert.AreEqual(contactPerson.ToString(), cpNode.Text);
            ITreeNode addressRelationshipNode = cpNode.Nodes[0];
            Assert.AreEqual("Addresses", addressRelationshipNode.Text);
            //---------------Execute Test ----------------------
            BusinessObjectCollection<ContactPersonTestBO> contactPeople = organisation.ContactPeople;
            ContactPersonTestBO newContactPerson = contactPeople.CreateBusinessObject();
            newContactPerson.Surname = "New Name";
            //            newTable.Save();
            AddressTestBO newAddress1 = contactPerson.Addresses.CreateBusinessObject();
            newAddress1.ContactPersonTestBO = newContactPerson;
            newAddress1.AddressLine1 = "New Address";
            AddressTestBO newAddress2 = contactPerson.Addresses.CreateBusinessObject();
            newAddress2.ContactPersonTestBO = newContactPerson;
            ITreeNode newCpNode = cpRelationshipNode.Nodes[1];
            newCpNode.Expand();
            ReflectionUtilities.ExecutePrivateMethod(treeViewController, "ExpandNode", newCpNode);

            ITreeNode newAddressesRelationshipNode = newCpNode.Nodes[0];
            newAddressesRelationshipNode.Expand();
            ReflectionUtilities.ExecutePrivateMethod(treeViewController, "ExpandNode", newAddressesRelationshipNode);
            //---------------Test Result -----------------------
            Assert.AreEqual(2, cpRelationshipNode.Nodes.Count);

            Assert.AreEqual(newContactPerson.ToString(), newCpNode.Text);
            //Assert.AreEqual(2, newTable.Addresses.Count);

            Assert.AreEqual("Addresses", newAddressesRelationshipNode.Text);
            Assert.AreEqual(2, newAddressesRelationshipNode.Nodes.Count);
            ITreeNode newAddressNode1 = newAddressesRelationshipNode.Nodes[0];
            Assert.AreEqual(newAddress1.ToString(), newAddressNode1.Text);
            ITreeNode newAddressNode2 = newAddressesRelationshipNode.Nodes[1];
            Assert.AreEqual(newAddress2.ToString(), newAddressNode2.Text);

        }

        [Test]
        public void Test_BusinessObjectCollection_ChildRemoved_SetsSelectedNodeToNull()
        {
            //---------------Set up test pack-------------------
            ITreeView treeView = GlobalUIRegistry.ControlFactory.CreateTreeView();
            TreeViewController treeViewController = new TreeViewController(treeView);
            OrganisationTestBO organisation1 = OrganisationTestBO.CreateSavedOrganisation();
            OrganisationTestBO organisation2 = OrganisationTestBO.CreateSavedOrganisation();
            ContactPersonTestBO contactPerson1 = CreateUnsavedContactPerson(organisation1);
            ContactPersonTestBO contactPerson2 = CreateUnsavedContactPerson(organisation1);

            IBusinessObjectCollection organisations = new BusinessObjectCollection<OrganisationTestBO> { organisation1 };
            treeViewController.LoadTreeView(organisations, 4);
            treeViewController.SelectObject(contactPerson2);
            //---------------Assert Precondition----------------        
            Assert.IsNotNull(treeViewController.TreeView.SelectedNode);
            //---------------Execute Test ----------------------
            contactPerson2.Organisation = organisation2;
            //---------------Test Result -----------------------
            Assert.IsNull(treeViewController.TreeView.SelectedNode);
        }

        [Test]
        public void Test_LoadTreeView_WithRelationship_OneChild_Expanded()
        {
            //---------------Set up test pack-------------------
            ITreeView treeView = GlobalUIRegistry.ControlFactory.CreateTreeView();
            TreeViewController treeViewController = new TreeViewController(treeView);
            OrganisationTestBO organisation = OrganisationTestBO.CreateSavedOrganisation();
            ContactPersonTestBO contactPerson = CreateUnsavedContactPerson(organisation);

            IBusinessObjectCollection organisations = new BusinessObjectCollection<OrganisationTestBO>();
            organisations.Add(organisation);
            //-------------Assert Preconditions -------------
            Assert.AreEqual(1, organisations.Count);
            Assert.AreEqual(0, treeView.Nodes.Count);
            //---------------Execute Test ----------------------
            treeViewController.LoadTreeView(organisations, 2);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, treeView.Nodes.Count);
            ITreeNode node = treeView.Nodes[0];
            Assert.AreEqual(organisation.ToString(), node.Text);
            Assert.AreEqual(1, node.Nodes.Count);
            ITreeNode relationshipNode = node.Nodes[0];
            Assert.AreEqual("ContactPeople", relationshipNode.Text);
            Assert.AreEqual(1, relationshipNode.Nodes.Count);
            ITreeNode childNode = relationshipNode.Nodes[0];
            Assert.AreEqual(contactPerson.ToString(), childNode.Text);
        }
    }

    //public class TestTreeViewControllerVWG : TestTreeViewControllerWin
    //{

    //    protected override IControlFactory GetControlFactory()
    //    {
    //        IControlFactory controlFactory = new ControlFactoryVWG();
    //        GlobalUIRegistry.ControlFactory = controlFactory;
    //        return controlFactory;
    //    }
    //}
}
