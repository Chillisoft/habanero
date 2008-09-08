using Habanero.UI.Base;

namespace Habanero.UI.Win
{
    public class TreeViewGridViewEditorWin : PanelWin, ITreeViewGridViewEditor
    {
        private readonly IControlFactory _controlFactory;

        public TreeViewGridViewEditorWin(IControlFactory controlFactory)
        {
            this._controlFactory = controlFactory;

            ITreeView treeView = _controlFactory.CreateTreeView("TreeView");
            IGridControl gridControl = _controlFactory.CreateEditableGridControl();
            BorderLayoutManager layoutManager = _controlFactory.CreateBorderLayoutManager(this);
            layoutManager.AddControl(treeView, BorderLayoutManager.Position.West);
            layoutManager.AddControl(gridControl, BorderLayoutManager.Position.Centre);


        }

     
    }
}