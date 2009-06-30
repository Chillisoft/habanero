using System;
using System.Collections.Generic;
using System.Text;
using Habanero.Base;
using Habanero.BO;
using log4net;

namespace Habanero.UI.Base
{
    /// <summary>
    /// A controller used to map an <see cref="IBusinessObjectCollection"/> onto an <see cref="ITreeView"/>. Each <see cref="IBusinessObject"/>
    /// is displayed as a node in the treeview, and the multiple relationships of the <see cref="IBusinessObject"/> are displayed as
    /// subnodes.
    /// </summary>
    public class TreeViewController // : ISelectorController
    {
        /// <summary>
        /// Uses for logging 
        /// </summary>
        protected static readonly ILog log = LogManager.GetLogger("Habanero.UI.Base.TreeViewController");

        private readonly ITreeView _treeView;
        private bool _preventSelectionChanged;
        private IBusinessObject _rootNodeBusinessObject;
        private static int _levelsToDisplay;

        protected Dictionary<IBusinessObject, NodeState> ObjectNodes { get; set; }
        private Dictionary<IRelationship, NodeState> RelationshipNodes { get; set; }
        private Dictionary<IBusinessObjectCollection, NodeState> ChildCollectionNodes { get; set; }

        public event EventHandler<BOEventArgs> BusinessObjectSelected;

        public delegate void SetupNodeWithBusinessObjectDelegate(ITreeNode treeNode, IBusinessObject businessObject);

        public delegate void SetupNodeWithRelationshipDelegate(ITreeNode node, IRelationship relationship);

        public SetupNodeWithBusinessObjectDelegate SetupNodeWithBusinessObject { get; set; }
        public SetupNodeWithRelationshipDelegate SetupNodeWithRelationship { get; set; }

        protected internal class NodeState
        {
            internal ITreeNode Node;
            internal bool IsLoaded;
            internal ITreeNode ParentNode;

            public NodeState(ITreeNode node)
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

        /// <summary>
        /// Constructs the TreeViewController. 
        /// </summary>
        /// <param name="treeView">The <see cref="ITreeView"/> to control/map to</param>
        public TreeViewController(ITreeView treeView)
        {
            _preventSelectionChanged = false;
            ObjectNodes = new Dictionary<IBusinessObject, NodeState>(20);
            RelationshipNodes = new Dictionary<IRelationship, NodeState>(5);
            ChildCollectionNodes = new Dictionary<IBusinessObjectCollection, NodeState>(5);
            _treeView = treeView;
            _treeView.HideSelection = false;
            _treeView.AfterSelect += TreeView_AfterSelect;
            _treeView.BeforeExpand += TreeView_BeforeExpand;
            _levelsToDisplay = -1;
        }

        /// <summary>
        /// Destructor. Removes the event handlers that the controller sets up on the controlled treeview.
        /// </summary>
        ~TreeViewController()
        {
            _treeView.AfterSelect -= TreeView_AfterSelect;
            _treeView.BeforeExpand -= TreeView_BeforeExpand;
            CleanUp();
        }

        public IBusinessObject RootNodeBusinessObject
        {
            get { return _rootNodeBusinessObject; }
        }

        public ITreeView TreeView
        {
            get { return _treeView; }
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
        }

        private string GetRelationshipDescription(IRelationship relationship)
        {
            Habanero.BO.Relationship multipleRelationship = relationship as Habanero.BO.Relationship;
            if (multipleRelationship != null) return multipleRelationship.RelationshipName;
            return "Unknown";
        }

        private IDictionary<string, IRelationship> GetVisibleRelationships(IBusinessObject businessObject)
        {
            Dictionary<string, IRelationship> relationships = new Dictionary<string, IRelationship>(4);
            foreach (IRelationship relationship in businessObject.Relationships)
            {
                if (MustRelationshipBeVisible(relationship))
                {
                    relationships.Add(relationship.RelationshipName, relationship);
                }
            }
            return relationships;
        }

        /// <summary>
        /// Returns whether the relationship should be shown in the tree view or not.</br>
        /// By default all Composition and Aggregation relationships will be shown in the 
        /// tree. This method can be overriden to only show the relationships that you want.
        /// </summary>
        /// <param name="relationship"></param>
        /// <returns></returns>
        protected virtual bool MustRelationshipBeVisible(IRelationship relationship)
        {
            IRelationshipDef relationshipDef = relationship.RelationshipDef;
            return relationshipDef.RelationshipType == RelationshipType.Composition
                   || relationshipDef.RelationshipType == RelationshipType.Aggregation;
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

        #region Loading

        public void LoadTreeView(IBusinessObject businessObject)
        {
            LoadTreeView(businessObject, 0);
        }

        public void LoadTreeView(IBusinessObject businessObject, int levelsToExpand)
        {
            CleanUp();
            _rootNodeBusinessObject = businessObject;
            //TODO brett 19 Mar 2009: This a hack of casting to a windows control will not work when we port this app to VWG.
           // ControlsHelper.SafeGui
            //    ((Control)_treeView, delegate
              //  {
                    _treeView.BeginUpdate();
                    _treeView.Nodes.Clear();
                    if (businessObject != null)
                        AddBusinessObjectNode(_treeView.Nodes, businessObject);
                    _treeView.EndUpdate();
                    ExpandLevels(_treeView.Nodes, levelsToExpand);
            //    });
           // Application.DoEvents();
        }

        public void LoadTreeView(IBusinessObject businessObject, int levelsToExpand, int levelsToDisplay)
        {
            _levelsToDisplay = levelsToDisplay;
            LoadTreeView(businessObject, levelsToExpand);
        }

        public void LoadTreeView
            (IBusinessObjectCollection businessObjectCollection, int levelsToExpand, int levelsToDisplay)
        {
            _levelsToDisplay = levelsToDisplay;
            LoadTreeView(businessObjectCollection, levelsToExpand);
        }

        public void LoadTreeView(IBusinessObjectCollection businessObjectCollection)
        {
            LoadTreeView(businessObjectCollection, 0);
        }

        public void LoadTreeView(IBusinessObjectCollection businessObjectCollection, int levelsToExpand)
        {
            CleanUp();
           // ControlsHelper.SafeGui
           //     ((IControlHabanero)_treeView, delegate
            //    {
                    _treeView.BeginUpdate();
                    _treeView.Nodes.Clear();
                    if (businessObjectCollection != null)
                    {
                        AddCollectionNode(_treeView.Nodes, businessObjectCollection);
                    }
                    _treeView.EndUpdate();
                    ExpandLevels(_treeView.Nodes, levelsToExpand);
             //   });
        }

        public void LoadTreeView(IRelationship relationship)
        {
            LoadTreeView(relationship, 0);
        }

        public void LoadTreeView(IRelationship relationship, int levelsToExpand)
        {
            CleanUp();
            //ControlsHelper.SafeGui
             //   ((IControlHabanero)_treeView, delegate
             //   {
                    _treeView.BeginUpdate();
                    _treeView.Nodes.Clear();
                    if (relationship != null)
                        AddRelationshipNode(_treeView.Nodes, relationship);
                    _treeView.EndUpdate();
                    ExpandLevels(_treeView.Nodes, levelsToExpand);
            //    });
            //Application.DoEvents();
        }

        public void LoadTreeView(IRelationship relationship, int levelsToExpand, int levelsToDisplay)
        {
            _levelsToDisplay = levelsToDisplay;
            LoadTreeView(relationship, levelsToExpand);
        }

        private void LoadChildrenNodes(ITreeNodeCollection nodes, IBusinessObject parent)
        {
            foreach (KeyValuePair<string, IRelationship> pair in GetVisibleRelationships(parent))
            {
                IRelationship relationship = pair.Value;
                if (!String.IsNullOrEmpty(pair.Key))
                {
                    ITreeNode node = SetupNode(nodes, relationship);
                    if (node == null) continue;
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
            NodeState nodeState = RelationshipNodes[relationship];
            if (nodeState.IsLoaded) return;

            ITreeNodeCollection nodes = nodeState.Node.Nodes;
            nodes.Clear();
            LoadRelationshipNode(relationship, nodes);
            nodeState.IsLoaded = true;
        }

        private void LoadRelationshipNode(IRelationship relationship, ITreeNodeCollection nodes)
        {
            if (relationship is IMultipleRelationship)
            {
                IMultipleRelationship multipleRelationship = (IMultipleRelationship)relationship;
                IBusinessObjectCollection children = multipleRelationship.BusinessObjectCollection;
                foreach (IBusinessObject businessObject in children.Clone())
                {
                    AddBusinessObjectNode(nodes, businessObject);
                }
            }
            else
            {
                ISingleRelationship singleRelationship = (ISingleRelationship)relationship;
                IBusinessObject businessObject = singleRelationship.GetRelatedObject();
                if (businessObject != null)
                {
                    AddBusinessObjectNode(nodes, businessObject);
                }
            }
        }

        private void RemoveRelationshipNode(IRelationship relationship)
        {
            this.RelationshipNodes.Remove(relationship);
            if (relationship is IMultipleRelationship)
            {
                IMultipleRelationship multipleRelationship = (IMultipleRelationship)relationship;
                IBusinessObjectCollection children = multipleRelationship.BusinessObjectCollection;
                ChildCollectionNodes.Remove(children);
                foreach (IBusinessObject businessObject in children.Clone())
                {
                    RemoveBusinessObjectNode(businessObject);
                }
            }
            else
            {
                ISingleRelationship singleRelationship = (ISingleRelationship)relationship;
                IBusinessObject businessObject = singleRelationship.GetRelatedObject();
                if (businessObject != null)
                {
                    RemoveBusinessObjectNode(businessObject);
                }
            }
        }

        private void AddBusinessObjectNode(ITreeNodeCollection nodes, IBusinessObject businessObject)
        {
            ITreeNode newNode = SetupNode(nodes, businessObject);
            if (newNode == null) return;
            if (newNode.IsExpanded)
            {
                LoadObjectNode(businessObject);
            }
        }


        private void AddCollectionNode
            (ITreeNodeCollection nodeCollection, IBusinessObjectCollection businessObjectCollection)
        {
            foreach (IBusinessObject businessObject in businessObjectCollection)
            {
                AddBusinessObjectNode(_treeView.Nodes, businessObject);
            }
        }

        private void AddRelationshipNode(ITreeNodeCollection nodes, IRelationship relationship)
        {
            ITreeNode newNode = SetupNode(nodes, relationship);
            if (newNode == null) return;
            if (newNode.IsExpanded)
            {
                LoadRelationshipNode(relationship, newNode.Nodes);
            }
        }

        private void LoadObjectNode(IBusinessObject businessObject)
        {
            if (businessObject == null) return;
            NodeState nodeState = ObjectNodes[businessObject];
            if (nodeState.IsLoaded) return;
            ITreeNodeCollection nodes = nodeState.Node.Nodes;
            nodes.Clear();
            LoadChildrenNodes(nodes, businessObject);
            nodeState.IsLoaded = true;
        }

        private ITreeNode SetupNode(ITreeNodeCollection nodes, object nodeTag)
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

        private ITreeNode SetupRelationshipNode(IRelationship relationship, ITreeNodeCollection nodes)
        {
            bool isNewColTag = !RelationshipNodes.ContainsKey(relationship);
            ITreeNode node;
            NodeState nodeState;
            if (isNewColTag)
            {
                node = nodes.Add("");
                node.Collapse(false);
                if (_levelsToDisplay > -1 && node.Level > _levelsToDisplay)
                {
                    nodes.Remove(node);
                    return null;
                }
                nodeState = new NodeState(node);
                RelationshipNodes.Add(relationship, nodeState);
                SetupRelationshipNodeDummy(relationship, nodeState);
            }
            else
            {
                nodeState = RelationshipNodes[relationship];
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
                IMultipleRelationship multipleRelationship = (IMultipleRelationship)relationship;
                IBusinessObjectCollection businessObjectCollection = multipleRelationship.BusinessObjectCollection;
                ChildCollectionNodes.Add(businessObjectCollection, nodeState);
                RegisterForBusinessObjectCollectionEvents(businessObjectCollection);
                childCount = businessObjectCollection.Count;
            }
            else
            {
                //TODO: Do something decent with Single Relationship Updated Event
                ISingleRelationship singleRelationship = (ISingleRelationship)relationship;
                if (singleRelationship.HasRelatedObject())
                {
                    childCount = 1;
                }
            }
            UpdateNodeDummy(nodeState, childCount);
        }

        private void DoSetupNodeWithRelationship(ITreeNode node, IRelationship relationship)
        {
            node.Text = GetRelationshipDescription(relationship);
            if (SetupNodeWithRelationship != null)
            {
                SetupNodeWithRelationship(node, relationship);
            }
        }

        private ITreeNode SetupBusinessObjectNode(IBusinessObject businessObject, ITreeNodeCollection nodes)
        {
            ITreeNode node;
            NodeState nodeState;
            if (!ObjectNodes.ContainsKey(businessObject))
            {
                node = nodes.Add("");
                node.Collapse(false);
                if (_levelsToDisplay > -1 && node.Level > _levelsToDisplay)
                {
                    nodes.Remove(node);
                    return null;
                }
                nodeState = new NodeState(node);
                nodeState.IsLoaded = false;
                UpdateNodeDummy(nodeState, GetVisibleRelationships(businessObject).Count);

                //LoadChildrenNodes(nodeState.Node.Nodes, businessObject);
                ObjectNodes.Add(businessObject, nodeState);
                //                LoadObjectNode(businessObject);
                RegisterForBusinessObjectEvents(businessObject);
            }
            else
            {
                nodeState = ObjectNodes[businessObject];
                node = nodeState.Node;
            }
            DoSetupNodeWithBusinessObject(node, businessObject);
            node.Tag = businessObject;
            return node;
        }

        private void DoSetupNodeWithBusinessObject(ITreeNode node, IBusinessObject businessObject)
        {
            string description = GetClassDescription(businessObject);
            log.Debug("Business Object Description : " + description);
            node.Text = description;
            if (SetupNodeWithBusinessObject != null)
            {
                SetupNodeWithBusinessObject(node, businessObject);
            }
        }

        private static void UpdateNodeDummy(NodeState nodeState, int childrenCount)
        {
            if (nodeState.IsLoaded) return;
            //Sets up a dummy node so that the tree view will show a + to expand but 
            // does not load the underlying objects untill required thus implementing lazy loading ;)
            if (nodeState.Node.Nodes.Count == 0 && childrenCount > 0)
            {
                //nodeState.Node.Nodes.Add("", "$DUMMY$");
                ITreeNode node = nodeState.Node.Nodes.Add("", "$DUMMY$");
                if (_levelsToDisplay > -1 && node.Level > _levelsToDisplay)
                {
                    nodeState.Node.Nodes.Remove(node);
                }
            }
        }

        #endregion //Loading

        #region Node Changes

        private void AddBusinessObjectToCollectionNode
            (IBusinessObjectCollection businessObjectCollection, IBusinessObject businessObject)
        {
            if (businessObjectCollection != null && ChildCollectionNodes.ContainsKey(businessObjectCollection))
            {
                NodeState nodeState = ChildCollectionNodes[businessObjectCollection];
                if (nodeState.IsLoaded)
                {
                    ITreeNode node = nodeState.Node;
                    AddBusinessObjectNode(node.Nodes, businessObject);
                }
                else
                {
                    UpdateNodeDummy(nodeState, businessObjectCollection.Count);
                }
            }
        }

        private void RemoveBusinessObjectFromCollectionNode
            (IBusinessObjectCollection businessObjectCollection, IBusinessObject businessObject)
        {
            if (businessObjectCollection != null && ChildCollectionNodes.ContainsKey(businessObjectCollection))
            {
                NodeState nodeState = ChildCollectionNodes[businessObjectCollection];
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
            if (businessObject != null && ObjectNodes.ContainsKey(businessObject))
            {
                NodeState nodeState = ObjectNodes[businessObject];
                ITreeNode node = nodeState.Node;
                DoSetupNodeWithBusinessObject(node, businessObject);
                //node.Text = GetClassDescription(businessObject);
            }
        }

        private void RemoveBusinessObjectNode(IBusinessObject businessObject)
        {
            if (businessObject != null && ObjectNodes.ContainsKey(businessObject))
            {
                NodeState nodeState = ObjectNodes[businessObject];
                ITreeNode node = nodeState.Node;
                ITreeNode parentNode = node.Parent;
                RemoveNode(businessObject, node);

                IDictionary<string, IRelationship> relationships = GetVisibleRelationships(businessObject);
                foreach (KeyValuePair<string, IRelationship> relationshipPair in relationships)
                {
                    IRelationship relationship = relationshipPair.Value;
                    RemoveRelationshipNode(relationship);
                }
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

        private void RemoveNode(IBusinessObject businessObject, ITreeNode node)
        {
            try
            {
                node.Remove();
            }
            catch (System.ObjectDisposedException ex)
            {
                log.Debug
                    ("RemoveNode : cannot remove node for Business Object : " + businessObject.ToString()
                     + Environment.NewLine + " Error :" + ex.Message);
            }
            finally
            {
                try
                {
                    ObjectNodes.Remove(businessObject);
                }
                finally
                {
                    UnRegisterForBusinessObjectEvents(businessObject);
                }
            }
        }

        #endregion //Node Changes

        #region Node Expansion

        private void ExpandLevels(ITreeNodeCollection nodes, int expandLevels)
        {
            if (expandLevels <= 0) return;
            foreach (ITreeNode treeNode in nodes)
            {
                treeNode.Expand();
                ExpandNode(treeNode);
                this.ExpandLevels(treeNode.Nodes, expandLevels - 1);
            }
        }

        #endregion //Node Expansion

        #region Object Events

        protected virtual void BusinessObjectCollection_ChildAdded(object sender, BOEventArgs e)
        {
            try
            {
                IBusinessObjectCollection businessObjectCollection = sender as IBusinessObjectCollection;
                IBusinessObject businessObject = e.BusinessObject;

                AddBusinessObjectToCollectionNode(businessObjectCollection, businessObject);
            }
            catch (Exception ex)
            {
                GlobalRegistry.UIExceptionNotifier.Notify(ex, "", "Error ");
            }
        }

        protected virtual void BusinessObjectCollection_ChildRemoved(object sender, BOEventArgs e)
        {
            try
            {
                IBusinessObjectCollection businessObjectCollection = sender as IBusinessObjectCollection;
                IBusinessObject businessObject = e.BusinessObject;
                _treeView.SelectedNode = null;
                RemoveBusinessObjectFromCollectionNode(businessObjectCollection, businessObject);
            }
            catch (Exception ex)
            {
                GlobalRegistry.UIExceptionNotifier.Notify(ex, "", "Error ");
            }
        }

        protected virtual void BusinessObject_Updated(object sender, BOEventArgs e)
        {
            try
            {
                UpdateBusinessObject(e.BusinessObject);
            }
            catch (Exception ex)
            {
                GlobalRegistry.UIExceptionNotifier.Notify(ex, "", "Error ");
            }
        }

        private void UpdateBusinessObject(IBusinessObject businessObject)
        {
            RefreshBusinessObjectNode(businessObject);
        }

        protected virtual void BusinessObject_Deleted(object sender, BOEventArgs e)
        {
            try
            {
                IBusinessObject businessObject = sender as IBusinessObject;
                RemoveBusinessObjectNode(businessObject);
            }
            catch (Exception ex)
            {
                GlobalRegistry.UIExceptionNotifier.Notify(ex, "", "Error ");
            }
        }

        #endregion //Object Events

        #region TreeView Events

        private void TreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            ITreeNode newSelectedNode = _treeView.SelectedNode;
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

        //private void SetSelectedNode(ITreeNode newSelectedNode)
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

        private void ExpandNode(ITreeNode node)
        {
            object nodeTag = node.Tag;
            IBusinessObject businessObject = nodeTag as IBusinessObject;
            IRelationship relationship = nodeTag as IRelationship;

            if (relationship != null)
            {
                LoadChildNode(relationship);
            }
            else if (businessObject != null)
            {
                LoadObjectNode(businessObject);
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
            if (ObjectNodes.ContainsKey(businessObject))
            {
                NodeState nodeState = ObjectNodes[businessObject];
                ITreeNode node = nodeState.Node;
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
            foreach (KeyValuePair<IBusinessObject, NodeState> objectNode in ObjectNodes)
            {
                IBusinessObject businessObject = objectNode.Key;
                UnRegisterForBusinessObjectEvents(businessObject);
            }
            ObjectNodes.Clear();
            foreach (KeyValuePair<IBusinessObjectCollection, NodeState> collectionNode in ChildCollectionNodes)
            {
                IBusinessObjectCollection businessObjectCollection = collectionNode.Key;
                UnRegisterForBusinessObjectCollectionEvents(businessObjectCollection);
            }
            ChildCollectionNodes.Clear();
            RelationshipNodes.Clear();
            _rootNodeBusinessObject = null;
        }

        public ITreeNode GetBusinessObjectTreeNode(IBusinessObject businessObject)
        {
            NodeState nodeState = GetBusinessObjectNodeState(businessObject);
            return nodeState != null ? nodeState.Node : null;
        }

        private NodeState GetBusinessObjectNodeState(IBusinessObject businessObject)
        {
            if (ObjectNodes.ContainsKey(businessObject))
            {
                return ObjectNodes[businessObject];
            }
            return null;
        }

        public void SetVisibility(IBusinessObject businessObject, bool visible)
        {
            NodeState nodeState = GetBusinessObjectNodeState(businessObject);
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
            ITreeNode parentNode = nodeState.ParentNode;
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

        private IBusinessObjectCollection GetNodeCollection(ITreeNode node)
        {
            foreach (KeyValuePair<IBusinessObjectCollection, NodeState> pair in ChildCollectionNodes)
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
