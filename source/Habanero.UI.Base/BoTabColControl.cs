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
using System.Drawing;
using Habanero.Base;
using Habanero.BO;
using Habanero.UI;

namespace Habanero.UI.Base
{
    /// <summary>
    /// Manages a collection of tab pages that hold business object controls
    /// </summary>
    public class BoTabColControl : IUserControlChilli
    {
        private readonly IControlFactory _controlFactory;
        private readonly ITabControl _tabControl;
        private readonly CollectionTabControlMapper _collectionTabControlMapper;

        /// <summary>
        /// Constructor to initialise a new tab control
        /// </summary>
        public BoTabColControl(IControlFactory controlFactory)
        {
            _controlFactory = controlFactory;
            BorderLayoutManager manager = ControlFactory.CreateBorderLayoutManager(this);
            _tabControl = ControlFactory.CreateTabControl();
            manager.AddControl(_tabControl, BorderLayoutManager.Position.Centre);
            _collectionTabControlMapper = new CollectionTabControlMapper(_tabControl,ControlFactory);
        }

        /// <summary>
        /// Sets the boControl that will be displayed on each tab page.  This must be called
        /// before the BoTabColControl can be used.
        /// </summary>
        /// <param name="boControl">The business object control that is
        /// displaying the business object information in the tab page</param>
        public void SetBusinessObjectControl(IBusinessObjectControl boControl)
        {
            CollectionTabCtlMapper.SetBusinessObjectControl(boControl);
        }

        /// <summary>
        /// Sets the collection of tab pages for the collection of business
        /// objects provided
        /// </summary>
        /// <param name="businessObjectCollection">The business object collection to create tab pages
        /// for</param>
        public void SetCollection(IBusinessObjectCollection businessObjectCollection)
        {
            CollectionTabCtlMapper.SetCollection(businessObjectCollection);
        }

        /// <summary>
        /// Carries out additional steps when the user selects a different tab
        /// </summary>
        public void TabChanged()
        {
            CollectionTabCtlMapper.TabChanged();
        }

        /// <summary>
        /// Returns the TabControl object
        /// </summary>
        public ITabControl TabControl
        {
            get { return _tabControl; }
        }

        /// <summary>
        /// Returns the business object represented in the specified tab page
        /// </summary>
        /// <param name="tabPage">The tab page</param>
        /// <returns>Returns the business object, or null if not available
        /// </returns>
        public IBusinessObject GetBo(ITabPage tabPage)
        {
            return CollectionTabCtlMapper.GetBo(tabPage);
        }

        /// <summary>
        /// Returns the TabPage object that is representing the given
        /// business object
        /// </summary>
        /// <param name="bo">The business object being represented</param>
        /// <returns>Returns the TabPage object, or null if not found</returns>
        public ITabPage GetTabPage(IBusinessObject bo)
        {
            return CollectionTabCtlMapper.GetTabPage(bo);
        }

        /// <summary>
        /// Returns the business object represented in the currently
        /// selected tab page
        /// </summary>
        public IBusinessObject CurrentBusinessObject
        {
            get { return CollectionTabCtlMapper.CurrentBusinessObject; }
            set { CollectionTabCtlMapper.CurrentBusinessObject = value; }
        }

        public event EventHandler Click;
        public event EventHandler DoubleClick;
        public event EventHandler Resize;
        public event EventHandler VisibleChanged;

        public int Width
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public IControlCollection Controls
        {
            get { throw new System.NotImplementedException(); }
        }

        public bool Visible
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public int TabIndex
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public int Height
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public int Top
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public int Bottom
        {
            get { throw new System.NotImplementedException(); }
        }

        public int Left
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public int Right
        {
            get { throw new System.NotImplementedException(); }
        }

        public string Text
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public string Name
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public bool Enabled
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public Color ForeColor
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public Color BackColor
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public bool TabStop
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public Size Size
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public void Select()
        {
            throw new System.NotImplementedException();
        }

        public bool HasChildren
        {
            get { throw new System.NotImplementedException(); }
        }

        public Size MaximumSize
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public Size MinimumSize
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public Font Font
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public IControlFactory ControlFactory
        {
            get { return _controlFactory; }
        }

        public CollectionTabControlMapper CollectionTabCtlMapper
        {
            get { return _collectionTabControlMapper; }
        }

        #region IControlChilli Members


        public void SuspendLayout()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void ResumeLayout(bool performLayout)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Invalidate()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}