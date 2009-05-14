using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Habanero.Base;
using Habanero.BO;
using TreeViewCancelEventArgs=System.Windows.Forms.TreeViewCancelEventArgs;
using TreeViewEventArgs=System.Windows.Forms.TreeViewEventArgs;

namespace Habanero.UI.Win
{
    public class TreeViewController// : ISelectorController
    {
        protected class NodeState
        {
            internal TreeNode Node;
            internal bool IsLoaded;
            internal TreeNode ParentNode;

            public NodeState(TreeNode node)
            {
                Node = node;
                IsLoaded = false;
                ParentNode = node.Parent;
            }

            public bool IsConnected()
            {
                return Node.TreeView != null;
            }

            public void Disconnect()
            {
                if (Node.Level == 0) return;
                if (IsConnected()) Node.Remove();
            }

            public void Connect(int index)
            {
                if (!IsConnected())
                {
                    ParentNode.Nodes.Insert(index, Node);
                }
            }
        }

        private readonly TreeView _treeView;
        protected Dictionary<IBusinessObject, NodeState> _objectNodes;
        private readonly Dictionary<IRelationship, NodeState> _relationshipNodes;
        private readonly Dictionary<IBusinessObjectCollection, NodeState> _childCollectionNodes;
        //private bool _selectCollectionsFirstViewableChild = false;
        private bool _preventSelectionChanged;
        private IBusinessObject _rootNodeBusinessObject;

        public event EventHandler<BOEventArgs> BusinessObjectSelected;

        public delegate void SetupNodeWithBusinessObjectDelegate(TreeNode treeNode, IBusinessObject businessObject);
        public delegate void SetupNodeWithRelationshipDelegate(TreeNode node, IRelationship relationship);

        public SetupNodeWithBusinessObjectDelegate SetupNodeWithBusinessObject { get; set; }
        public SetupNodeWithRelationshipDelegate SetupNodeWithRelationship { get; set; }

        public IBusinessObject RootNodeBusinessObject
        {
            get { return _rootNodeBusinessObject; }
        }

        public TreeView TreeView
        {
            get { return _treeView; }
        }

        public TreeViewController(TreeView treeView)
        {
            _preventSelectionChanged = false;
            _objectNodes = new Dictionary<IBusinessObject, NodeState>(20);
            _relationshipNodes = new Dictionary<IRelationship, NodeState>(5);
            _childCollectionNodes = new Dictionary<IBusinessObjectCollection, NodeState>(5);
            _treeView = treeView;
            _treeView.HideSelection = false;
            _treeView.AfterSelect += TreeView_AfterSelect;
            _treeView.BeforeExpand += TreeView_BeforeExpand;
        }

        ~TreeViewController()
        {
            _treeView.AfterSelect -= TreeView_AfterSelect;
        }

        #region Utility Methods

        private string GetClassDescription(IBusinessObject businessObject)
        {
            return Convert.ToString(businessObject);
        }

        private void RegisterForBusinessObjectEvents(IBusinessObject businessObject)
        {
            businessObject.Updated += BusinessObject_Updated;
            businessObject.Deleted += BusinessObject_Deleted;
            businessObject.PropertyUpdated += BusinessObject_Prop_Updated;
            //businessObject.ChildCollectionChanged += BusinessObject_ChildCollectionChanged;
        }

        private void BusinessObject_Prop_Updated(object sender, BOPropUpdatedEventArgs eventArgs1)
        {
            UpdateBusinessObject(eventArgs1.BusinessObject);
        }

        private void UnRegisterForBusinessObjectEvents(IBusinessObject businessObject)
        {
            businessObject.Updated -= BusinessObject_Updated;
            businessObject.Deleted -= BusinessObject_Deleted;
            businessObject.PropertyUpdated -= BusinessObject_Prop_Updated;
            //businessObject.ChildCollectionChanged -= BusinessObject_ChildCollectionChanged;
        }

        private string GetRelationshipDescription(IRelationship relationship)
        {
            Relationship multipleRelationship = relationship as Relationship;
            if (multipleRelationship != null) return multipleRelationship.RelationshipName;
            return "Unknown";
        }

        private IDictionary<string, IRelationship> GetVisibleRelationships(IBusinessObject businessObject)
        {
            Dictionary<string, IRelationship> relationships = new Dictionary<string, IRelationship>(4);
            foreach (IRelationship relationship in businessObject.Relationships)
            {
                if (MustRelationshipBeVisible(businessObject, relationship))
                {
                    relationships.Add(relationship.RelationshipName, relationship);
                }
            }
            return relationships;
        }

        private static bool MustRelationshipBeVisible(IBusinessObject businessObject, IRelationship relationship)
        {
            IRelationshipDef relationshipDef = relationship.RelationshipDef;
            return relationshipDef.RelationshipType == RelationshipType.Composition
                   || relationshipDef.RelationshipType == RelationshipType.Aggregation;
            //return relationship is IMultipleRelationship;
        }
        
        private void RegisterForBusinessObjectCollectionEvents(IBusinessObjectCollection businessObjectCollection)
        {
            businessObjectCollection.BusinessObjectAdded += BusinessObjectCollection_ChildAdded;
            businessObjectCollection.BusinessObjectRemoved += BusinessObjectCollection_ChildRemoved;
        }

        private void UnRegisterForBusinessObjectCollectionEvents(IBusinessObjectCollection businessObjectCollection)
        {
            businessObjectCollection.BusinessObjectAdded -= BusinessObjectCollection_ChildAdded;
            businessObjectCollection.BusinessObjectRemoved -= BusinessObjectCollection_ChildRemoved;
        }

        #endregion //Utility Methods


        //#region Properties

        //public bool SelectCollectionsFirstViewableChild
        //{
        //    get { return _selectCollectionsFirstViewableChild; }
        //    set { _selectCollectionsFirstViewableChild = value; }
        //}

        //#endregion //Properties

        #region Loading

        public void LoadTreeView(IBusinessObject businessObject)
        {
            LoadTreeView(businessObject, 0);
        }
        public void LoadTreeView(IBusinessObject businessObject, int levelsToExpand)
        {
            CleanUp();
            _rootNodeBusinessObject = businessObject;
            ControlsHelper.SafeGui(_treeView, delegate
            {
                _treeView.BeginUpdate();
                _treeView.Nodes.Clear();
                //LoadChildrenNodes(_treeView.Nodes, businessObject);
                if (businessObject != null)
                    AddBusinessObjectNode(_treeView.Nodes, businessObject);
                _treeView.EndUpdate();
                ExpandLevels(_treeView.Nodes, levelsToExpand);
            });
            Application.DoEvents();
        }

        private void LoadChildrenNodes(TreeNodeCollection nodes, IBusinessObject parent)
        {
            foreach (KeyValuePair<string, IRelationship> pair in GetVisibleRelationships(parent))
            {
                IRelationship relationship = pair.Value;
                if (!String.IsNullOrEmpty(pair.Key))
                {
                    TreeNode node = SetupNode(nodes, relationship);
                    if (node.IsExpanded)
                    {
                        LoadChildNode(relationship);
                    }
                }

            }
        }

        private void LoadChildNode(IRelationship relationship)
        {
            if (relationship == null) return;
            NodeState nodeState = _relationshipNodes[relationship];
            if (!nodeState.IsLoaded)
            {
                TreeNodeCollection nodes = nodeState.Node.Nodes;
                nodes.Clear();
                LoadRelationshipNode(relationship, nodes);
                nodeState.IsLoaded = true;
            }
        }

        private void LoadRelationshipNode(IRelationship relationship, TreeNodeCollection nodes)
        {
            if (relationship is IMultipleRelationship)
            {
                IMultipleRelationship multipleRelationship = (IMultipleRelationship) relationship;
                IBusinessObjectCollection children = multipleRelationship.BusinessObjectCollection;
                foreach (IBusinessObject businessObject in children.Clone())
                {
                    AddBusinessObjectNode(nodes, businessObject);
                }
            } else
            {
                ISingleRelationship singleRelationship = (ISingleRelationship)relationship;
                IBusinessObject businessObject = singleRelationship.GetRelatedObject();
                if (businessObject != null)
                {
                    AddBusinessObjectNode(nodes, businessObject);
                }
            }
        }

        private void AddBusinessObjectNode(TreeNodeCollection nodes, IBusinessObject businessObject)
        {
            TreeNode newNode = SetupNode(nodes, businessObject);
            if (newNode.IsExpanded)
            {
                LoadObjectNode(businessObject);
            }
        }

        private void LoadObjectNode(IBusinessObject businessObject)
        {
            NodeState nodeState = _objectNodes[businessObject];
            if (!nodeState.IsLoaded)
            {
                TreeNodeCollection nodes = nodeState.Node.Nodes;
                nodes.Clear();
                LoadChildrenNodes(nodes, businessObject);
                nodeState.IsLoaded = true;
            }
        }

        private TreeNode SetupNode(TreeNodeCollection nodes, object nodeTag)
        {
            if (nodeTag == null)
            {
                throw new ArgumentNullException("nodeTag");
            }
            IBusinessObject businessObject = nodeTag as IBusinessObject;
            if (businessObject != null) return SetupBusinessObjectNode(businessObject, nodes);

            IRelationship relationship = nodeTag as IRelationship;
            if (relationship != null) return SetupRelationshipNode(relationship, nodes);

            throw new InvalidCastException("'nodeTag' is not of a recognised type.");
        }

        private TreeNode SetupRelationshipNode(IRelationship relationship, TreeNodeCollection nodes)
        {
            bool isNewColTag = !_relationshipNodes.ContainsKey(relationship);
            TreeNode node;
            NodeState nodeState;
            if (isNewColTag)
            {
                node = new TreeNode();
                nodes.Add(node);
                nodeState = new NodeState(node);
                _relationshipNodes.Add(relationship, nodeState);
                SetupRelationshipNodeDummy(relationship, nodeState);
            }
            else
            {
                nodeState = _relationshipNodes[relationship];
                node = nodeState.Node;
            }
            DoSetupNodeWithRelationship(node, relationship);
            node.Tag = relationship;
            return node;
        }

        private void SetupRelationshipNodeDummy(IRelationship relationship, NodeState nodeState)
        {
            int childCount = 0;
            if (relationship is IMultipleRelationship)
            {
                IMultipleRelationship multipleRelationship = (IMultipleRelationship) relationship;
                IBusinessObjectCollection businessObjectCollection = multipleRelationship.BusinessObjectCollection;
                _childCollectionNodes.Add(businessObjectCollection, nodeState);
                RegisterForBusinessObjectCollectionEvents(businessObjectCollection);
                childCount = businessObjectCollection.Count;
            } else
            {
                //TODO: Do something decent with Single Relationship Updated Event
                ISingleRelationship singleRelationship = (ISingleRelationship) relationship;
                if (singleRelationship.HasRelatedObject())
                {
                    childCount = 1;
                }
            }
            UpdateNodeDummy(nodeState, childCount);
        }

        private void DoSetupNodeWithRelationship(TreeNode node, IRelationship relationship)
        {
            node.Text = GetRelationshipDescription(relationship);
            if (SetupNodeWithRelationship != null)
            {
                SetupNodeWithRelationship(node, relationship);
            }
        }

        private TreeNode SetupBusinessObjectNode(IBusinessObject businessObject, TreeNodeCollection nodes)
        {
            bool isNewClassTag = !_objectNodes.ContainsKey(businessObject);
            TreeNode node;
            NodeState nodeState;
            if (isNewClassTag)
            {
                node = new TreeNode();
                nodes.Add(node);
                nodeState = new NodeState(node);
                nodeState.IsLoaded = false;
                UpdateNodeDummy(nodeState, GetVisibleRelationships(businessObject).Count);

                _objectNodes.Add(businessObject, nodeState);
                RegisterForBusinessObjectEvents(businessObject);
            }
            else
            {
                nodeState = _objectNodes[businessObject];
                node = nodeState.Node;
            }
            DoSetupNodeWithBusinessObject(node, businessObject);
            node.Tag = businessObject;
            return node;
        }

        private void DoSetupNodeWithBusinessObject(TreeNode node, IBusinessObject businessObject)
        {
            node.Text = GetClassDescription(businessObject);
            if (SetupNodeWithBusinessObject != null)
            {
                SetupNodeWithBusinessObject(node, businessObject);
            }
        }

        private static void UpdateNodeDummy(NodeState nodeState, int childrenCount)
        {
            if (!nodeState.IsLoaded)
            {
                if (nodeState.Node.Nodes.Count == 0 && 
                    childrenCount > 0)
                {
                    nodeState.Node.Nodes.Add("", "$DUMMY$");
                }
            }
        }

        #endregion //Loading

        #region Node Changes
        
        private void AddBusinessObjectCollectionNode(IBusinessObjectCollection businessObjectCollection, IBusinessObject businessObject)
        {
            if (businessObjectCollection != null && _childCollectionNodes.ContainsKey(businessObjectCollection))
            {
                NodeState nodeState = _childCollectionNodes[businessObjectCollection];
                if (nodeState.IsLoaded)
                {
                    TreeNode node = nodeState.Node;
                    AddBusinessObjectNode(node.Nodes, businessObject);
                }
                else
                {
                    UpdateNodeDummy(nodeState, businessObjectCollection.Count);
                }
            }
        }

        private void RemoveBusinessObjectCollectionNode(IBusinessObjectCollection businessObjectCollection, IBusinessObject businessObject)
        {
            if (businessObjectCollection != null && _childCollectionNodes.ContainsKey(businessObjectCollection))
            {
                NodeState nodeState = _childCollectionNodes[businessObjectCollection];
                if (nodeState.IsLoaded)
                {
                    RemoveBusinessObjectNode(businessObject);
                }
                else
                {
                    UpdateNodeDummy(nodeState, businessObjectCollection.Count);
                }
            }
        }

        private void RefreshBusinessObjectNode(IBusinessObject businessObject)
        {
            if (businessObject != null && _objectNodes.ContainsKey(businessObject))
            {
                NodeState nodeState = _objectNodes[businessObject];
                TreeNode node = nodeState.Node;
                DoSetupNodeWithBusinessObject(node, businessObject);
                //node.Text = GetClassDescription(businessObject);
            }
        }

        private void RemoveBusinessObjectNode(IBusinessObject businessObject)
        {
            if (businessObject != null && _objectNodes.ContainsKey(businessObject))
            {
                NodeState nodeState = _objectNodes[businessObject];
                TreeNode node = nodeState.Node;
                TreeNode parentNode = node.Parent;
                node.Remove();
                UnRegisterForBusinessObjectEvents(businessObject);
                _objectNodes.Remove(businessObject);
                if (parentNode != null)
                {
                    IBusinessObjectCollection businessObjectCollection = parentNode.Tag as IBusinessObjectCollection;
                    if (businessObjectCollection != null)
                    {
                        UpdateNodeDummy(nodeState, businessObjectCollection.Count);
                    }
                }
            }
        }

        #endregion //Node Changes

        #region Node Expansion

        private void ExpandLevels(TreeNodeCollection nodes, int expandLevels)
        {

            if (expandLevels > 0)
            {
                foreach (TreeNode treeNode in nodes)
                {
                    treeNode.Expand();
                    ExpandNode(treeNode);
                    this.ExpandLevels(treeNode.Nodes, expandLevels - 1);
                }
            }
        }

        #endregion //Node Expansion
		
        #region Object Events

        protected virtual void BusinessObjectCollection_ChildAdded(object sender, BOEventArgs e)
        {
            IBusinessObjectCollection businessObjectCollection = sender as IBusinessObjectCollection;
            IBusinessObject businessObject = e.BusinessObject;
            
            AddBusinessObjectCollectionNode(businessObjectCollection, businessObject);
        }
        protected virtual void BusinessObjectCollection_ChildRemoved(object sender, BOEventArgs e)
        {
            IBusinessObjectCollection businessObjectCollection = sender as IBusinessObjectCollection;
            IBusinessObject businessObject = e.BusinessObject;
            RemoveBusinessObjectCollectionNode(businessObjectCollection, businessObject);
        }

        protected virtual void BusinessObject_Updated(object sender, BOEventArgs e)
        {
            UpdateBusinessObject(e.BusinessObject);
        }

        private void UpdateBusinessObject(IBusinessObject businessObject)
        {
            RefreshBusinessObjectNode(businessObject);
        }

        protected virtual void BusinessObject_Deleted(object sender, BOEventArgs e)
        {
            IBusinessObject businessObject = sender as IBusinessObject;
            RemoveBusinessObjectNode(businessObject);
        }
        
        #endregion //Object Events

        #region TreeView Events

        private void TreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode newSelectedNode = _treeView.SelectedNode;
            //SetSelectedNode(newSelectedNode);
            IBusinessObject businessObject = newSelectedNode.Tag as IBusinessObject;
            if (businessObject != null)
            {
                FireBusinessObjectSelected(businessObject);
            } //else if (_selectCollectionsFirstViewableChild)
            //{
            //    IBusinessObjectCollection businessObjectCollection = newSelectedNode.Tag as IBusinessObjectCollection;
            //    if (businessObjectCollection != null && businessObjectCollection.Count > 0)
            //    {
            //        businessObject = businessObjectCollection.ChildrenList()[0];
            //        FireBusinessObjectSelected(businessObject);
            //    }
            //}
        }

        //private void SetSelectedNode(TreeNode newSelectedNode)
        //{
        //    if (_selectedNode != null)
        //    {
        //        _selectedNode.BackColor = _treeView.BackColor;
        //        _selectedNode.ForeColor = _treeView.ForeColor;
        //    }
        //    _selectedNode = newSelectedNode;
        //    if (_selectedNode != null)
        //    {
        //        _selectedNode.BackColor = SystemColors.Highlight;
        //        _selectedNode.ForeColor = SystemColors.HighlightText;
        //    }
        //}

        private void TreeView_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            ExpandNode(e.Node);
        }

        private void ExpandNode(TreeNode node)
        {
            object nodeTag = node.Tag;
            IBusinessObject businessObject = nodeTag as IBusinessObject;
            IRelationship relationship = nodeTag as IRelationship;
            if (relationship != null)
            {
                NodeState nodeState = _relationshipNodes[relationship];
                if (!nodeState.IsLoaded)
                {
                    LoadChildNode(relationship);
                }
            } else if (businessObject != null)
            {
                NodeState nodeState = _objectNodes[businessObject];
                if (!nodeState.IsLoaded)
                {
                    LoadObjectNode(businessObject);
                }
            }
        }

        #endregion //TreeView Events

        private void FireBusinessObjectSelected(IBusinessObject businessObject)
        {
            if (_preventSelectionChanged) return;
            if (BusinessObjectSelected != null)
            {
                BusinessObjectSelected(this, new BOEventArgs(businessObject));
            }
        }

        public void SelectObject(IBusinessObject businessObject)
        {
            if (_objectNodes.ContainsKey(businessObject))
            {
                NodeState nodeState = _objectNodes[businessObject];
                TreeNode node = nodeState.Node;
                if (_treeView.SelectedNode != node)
                {
                    _preventSelectionChanged = true;
                    _treeView.SelectedNode = node;
                    _preventSelectionChanged = false;
                }
            }
        }

        public void CleanUp()
        {
            foreach (KeyValuePair<IBusinessObject, NodeState> objectNode in _objectNodes)
            {
                IBusinessObject businessObject = objectNode.Key;
                UnRegisterForBusinessObjectEvents(businessObject);
            }
            _objectNodes.Clear();
            foreach (KeyValuePair<IBusinessObjectCollection, NodeState> collectionNode in _childCollectionNodes)
            {
                IBusinessObjectCollection businessObjectCollection = collectionNode.Key;
                UnRegisterForBusinessObjectCollectionEvents(businessObjectCollection);
            }
            _childCollectionNodes.Clear();
            _relationshipNodes.Clear();
            _rootNodeBusinessObject = null;
        }

        public TreeNode GetBusinessObjectTreeNode(IBusinessObject businessObject)
        {
            NodeState nodeState = GetBusinessObjectNodeState(businessObject);
            return nodeState != null ? nodeState.Node : null;
        }

        private NodeState GetBusinessObjectNodeState(IBusinessObject businessObject)
        {
            if (_objectNodes.ContainsKey(businessObject))
            {
                return _objectNodes[businessObject];
            }
            return null;
        }

        public void SetVisibility(IBusinessObject businessObject, bool visible)
        {
            NodeState nodeState = GetBusinessObjectNodeState(businessObject);
            //TODO: If null
            if (visible)
            {
                int index = FindPositionIndexOf(businessObject);
                nodeState.Connect(index);
            }
            else
            {
                nodeState.Disconnect();
            }
        }

        private int FindPositionIndexOf(IBusinessObject businessObject)
        {
            NodeState nodeState = GetBusinessObjectNodeState(businessObject);
            TreeNode parentNode = nodeState.ParentNode;
            IBusinessObjectCollection businessObjectCollection = GetNodeCollection(parentNode);
            if (businessObjectCollection == null) return 0;
            int proposedIndex = businessObjectCollection.IndexOf(businessObject);
            int actualIndex = 0;
            for (int i = 0; i < proposedIndex; i++)
            {
                IBusinessObject nodeBo = businessObjectCollection[i];
                NodeState boNodeState = GetBusinessObjectNodeState(nodeBo);
                if (boNodeState.IsConnected()) actualIndex++;
            }
            return actualIndex;
        }

        private IBusinessObjectCollection GetNodeCollection(TreeNode node)
        {
            foreach (KeyValuePair<IBusinessObjectCollection, NodeState> pair in _childCollectionNodes)
            {
                if (pair.Value.Node == node)
                {
                    return pair.Key;
                }
            }
            return null;
        }
    }
}