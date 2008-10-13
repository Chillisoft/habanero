using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Habanero.UI.Base
{
    #region TreeViewEventArgs Class

    /// <summary>
    /// Specifies the action that raised a System.Windows.Forms.TreeViewEventArgs event.
    /// </summary>
    //[Serializable()]
    public enum TreeViewAction
    {
        ///<summary>
        /// The event was caused by a keystroke.
        ///</summary>
        ByKeyboard = 1,
        ///<summary>
        /// The event was caused by a mouse operation.
        ///</summary>
        ByMouse = 2,
        ///<summary>
        /// The event was caused by TreeNode collapsing.
        ///</summary>
        Collapse = 3,
        ///<summary>
        /// The event was caused by the TreeNode expanding.
        ///</summary>
        Expand = 4,
        ///<summary>
        /// The action that caused the event is unknown.
        ///</summary>
        Unknown = 0
    }

    ///<summary>
    /// Represents the method that will handle the 
    /// TreeView.AfterCheck, TreeView.AfterCollapse, 
    /// TreeView.AfterExpand, or TreeView.AfterSelect 
    /// event of a TreeView control.
    ///</summary>
    ///<param name="sender">The source of the event.</param>
    ///<param name="e">A TreeViewEventArgs that contains the event data.</param>
    public delegate void TreeViewEventHandler(object sender, TreeViewEventArgs e);

    /// <summary>
    /// Provides data for the TreeView.AfterCheck, TreeView.AfterCollapse, 
    /// TreeView.AfterExpand, or TreeView.AfterSelect events of a TreeView control.
    /// </summary>
    //[Serializable()]
    public class TreeViewEventArgs : EventArgs
    {
        private TreeViewAction action;
        private ITreeNode node;

        #region Contructors

        /// <summary>
        /// Initializes a new instance of the TreeViewEventArgs class for the specified tree node.
        /// </summary>
        /// <param name="node">The TreeNode that the event is responding to.</param>
        public TreeViewEventArgs(ITreeNode node)
            : this(node, TreeViewAction.Unknown)
        { }

        /// <summary>
        /// Initializes a new instance of the TreeViewEventArgs class 
        /// for the specified tree node and with the specified type of action that raised the event.
        /// </summary>
        /// <param name="node">The TreeNode that the event is responding to.</param>
        /// <param name="action">The type of TreeViewAction that raised the event.</param>
        public TreeViewEventArgs(ITreeNode node, TreeViewAction action)
        {
            this.action = action;
            this.node = node;
            this.action = action;
        }

        #endregion // Contructors

        #region Properties

        /// <summary>
        /// Gets the type of action that raised the event. 
        /// </summary>
        public TreeViewAction Action
        {
            get { return this.action; }
        }

        /// <summary>
        /// Gets the tree node that has been checked, expanded, collapsed, or selected.
        /// </summary>
        public ITreeNode Node
        {
            get { return this.node; }
        }

        #endregion // Properties

    }

    #endregion // TreeViewEventArgs Class

    #region TreeViewCancelEventArgs Class

    ///<summary>
    /// Represents the method that will handle the 
    /// TreeView.BeforeCheck, TreeView.BeforeCollapse, 
    /// TreeView.BeforeExpand, or TreeView.BeforeSelect 
    /// event of a TreeView control.
    ///</summary>
    ///<param name="sender">The source of the event.</param>
    ///<param name="e">A TreeViewCancelEventArgs that contains the event data.</param>
    public delegate void TreeViewCancelEventHandler(object sender, TreeViewCancelEventArgs e);

    /// <summary>
    /// Provides data for the TreeView.BeforeCheck, TreeView.BeforeCollapse, 
    /// TreeView.BeforeExpand, and TreeView.BeforeSelect events of a TreeView control.
    /// </summary>
    //[Serializable()]
    public class TreeViewCancelEventArgs : CancelEventArgs
    {
        private TreeViewAction action;
        private ITreeNode node;

        #region Contructors

        /// <summary>
        /// Initializes a new instance of the TreeViewCancelEventArgs class with the specified tree node, 
        /// a value specifying whether the event is to be canceled, 
        /// and the type of tree view action that raised the event.
        /// </summary>
        ///<param name="node">The TreeNode that the event is responding to.</param>
        ///<param name="cancel">true to cancel the event; otherwise, false.</param>
        ///<param name="action">One of the TreeViewAction values indicating the type of action that raised the event.</param>
        public TreeViewCancelEventArgs(ITreeNode node, bool cancel, TreeViewAction action)
            : base(cancel)
        {
            this.node = node;
            this.action = action;
        }


        #endregion // Contructors

        #region Properties

        ///<summary>
        /// Gets the type of TreeView action that raised the event.
        ///</summary>
        public TreeViewAction Action
        {
            get { return this.action; }
        }

        /// <summary>
        /// Gets the tree node to be checked, expanded, collapsed, or selected.
        /// </summary>
        public ITreeNode Node
        {
            get { return this.node; }
        }

        #endregion // Properties

    }

    #endregion // TreeViewCancelEventArgs Class
}
