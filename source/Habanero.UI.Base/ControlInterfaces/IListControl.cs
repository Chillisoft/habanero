using System;
using System.ComponentModel;
using System.Drawing.Design;

namespace Habanero.UI.Base
{
    public interface IListControl:IControlChilli
    {
        /// <summary>Occurs when the <see cref="P:Gizmox.WebGUI.Forms.ListControl.SelectedValue"></see> property changes.</summary>
        /// <filterpriority>1</filterpriority>
        event EventHandler SelectedValueChanged;

        /// <summary>Returns the text representation of the specified item.</summary>
        /// <returns>If the <see cref="P:Gizmox.WebGUI.Forms.ListControl.DisplayMember"></see> property is not specified, the value returned by <see cref="M:Gizmox.WebGUI.Forms.ListControl.GetItemText(System.Object)"></see> is the value of the item's ToString method. Otherwise, the method returns the string value of the member specified in the <see cref="P:Gizmox.WebGUI.Forms.ListControl.DisplayMember"></see> property for the object specified in the item parameter.</returns>
        /// <param name="item">The object from which to get the contents to display. </param>
        /// <filterpriority>1</filterpriority>
        string GetItemText(object item);

        /// <summary>Gets or sets the property to display for this <see cref="T:Gizmox.WebGUI.Forms.ListControl"></see>.</summary>
        /// <returns>A <see cref="T:System.String"></see> specifying the name of an object property that is contained in the collection specified by the <see cref="P:Gizmox.WebGUI.Forms.ListControl.DataSource"></see> property. The default is an empty string (""). </returns>
        /// <filterpriority>1</filterpriority>
        [DefaultValue("")]
        string DisplayMember { get; set; }

        /// <summary>When overridden in a derived class, gets or sets the zero-based index of the currently selected item.</summary>
        /// <returns>A zero-based index of the currently selected item. A value of negative one (-1) is returned if no item is selected.</returns>
        /// <filterpriority>1</filterpriority>
        [System.ComponentModel.Bindable(true)]
        int SelectedIndex { get; set; }

        /// <summary>Gets or sets the value of the member property specified by the <see cref="P:Gizmox.WebGUI.Forms.ListControl.ValueMember"></see> property.</summary>
        /// <returns>An object containing the value of the member of the data source specified by the <see cref="P:Gizmox.WebGUI.Forms.ListControl.ValueMember"></see> property.</returns>
        /// <exception cref="T:System.InvalidOperationException">The assigned value is null or the empty string ("").</exception>
        /// <filterpriority>1</filterpriority>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        object SelectedValue { get; set; }

        /// <summary>Gets or sets the property to use as the actual value for the items in the <see cref="T:Gizmox.WebGUI.Forms.ListControl"></see>.</summary>
        /// <returns>A <see cref="T:System.String"></see> representing the name of an object property that is contained in the collection specified by the <see cref="P:Gizmox.WebGUI.Forms.ListControl.DataSource"></see> property. The default is an empty string ("").</returns>
        /// <exception cref="T:System.ArgumentException">The specified property cannot be found on the object specified by the <see cref="P:Gizmox.WebGUI.Forms.ListControl.DataSource"></see> property. </exception>
        /// <filterpriority>1</filterpriority>
        [DefaultValue(""),
         Editor(
             "System.Windows.Forms.Design.DataMemberFieldEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
             , typeof (UITypeEditor))]
        string ValueMember { get; set; }
    }
}