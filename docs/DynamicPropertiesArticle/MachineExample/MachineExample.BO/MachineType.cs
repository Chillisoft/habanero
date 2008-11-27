/// This code is copyright under the terms of the Code Project Open License
/// (C) 2008 Peter Wiles/Chillisoft Solution Services (Pty) Ltd

using System.Collections;
using Habanero.Base;
using Habanero.BO.ClassDefinition;

namespace MachineExample.BO
{
    public partial class MachineType
    {
        /// <summary>
        /// Creates or retrieves the ClassDef for a Machine of this MachineType. Even if it exists in the cache, this
        /// method will ask for the ClassDef to be recreated. This is intended as a naive solution to the problem of 
        /// changing MachineTypes at run time and not updating the ClassDef.  Using this method, each time the ClassDef
        /// is needed it is recreated from the original Machine ClassDef and the user's new requirements.
        /// </summary>
        /// <returns></returns>
        public ClassDef GetMachineClassDef()
        {
            string machineTypeClassDefName = "Machine_" + Name;
            ClassDef machineClassDef;
            string machineAssemblyName = "MachineExample.BO";
            if (ClassDef.ClassDefs.Contains(machineAssemblyName, machineTypeClassDefName))
            {
                machineClassDef = ClassDef.ClassDefs[machineAssemblyName, machineTypeClassDefName];
                ClassDef.ClassDefs.Remove(machineClassDef);
            }
            machineClassDef = CreateNewMachineClassDef();
            ClassDef.ClassDefs.Add(machineClassDef);

            return machineClassDef;
        }

        /// <summary>
        /// Creates a new ClassDef for this Machine/MachineType combination.  In doing this, the method sets up
        /// the PropDefs, the UIFormFields and the UIGridColumns of the new ClassDef from the MachinePropertyDefs 
        /// collection
        /// </summary>
        /// <returns></returns>
        private ClassDef CreateNewMachineClassDef()
        {
            ClassDef baseMachineClassDef = ClassDef.ClassDefs[typeof (Machine)];
            ClassDef machineClassDef = baseMachineClassDef.Clone();
            machineClassDef.TypeParameter = Name;

            UIDef uiDef = machineClassDef.UIDefCol["default"];
            UIGrid uiGrid = uiDef.UIGrid;
            UIForm form = uiDef.UIForm;
            form.Title = "Add/Edit a Machine";
            UIFormTab tab = form[0];

            UIFormColumn column = tab[0];
            foreach (MachinePropertyDef machinePropertyDef in MachinePropertyDefs)
            {
                var machinePropertyPropDef = new PropDef(machinePropertyDef.PropertyName, "System",
                                                     machinePropertyDef.PropertyType,
                                                     PropReadWriteRule.ReadWrite, "", null,
                                                     machinePropertyDef.IsCompulsory.Value, false);
                machinePropertyPropDef.Persistable = false;
                machineClassDef.PropDefcol.Add(machinePropertyPropDef);

                var uiProperty =
                    new UIFormField(null, machinePropertyDef.PropertyName, "TextBox", "System.Windows.Forms", "", "",
                                    true, "", new Hashtable(), null);
                column.Add(uiProperty);

                uiGrid.Add(new UIGridColumn(machinePropertyDef.PropertyName, machinePropertyDef.PropertyName, "", "",
                                            false, 100, UIGridColumn.PropAlignment.left, new Hashtable()));
            }

            return machineClassDef;
        }


        public override string ToString()
        {
            return Name;
        }

        /// <summary>
        /// Creates a Machine object that has the ClassDef corresponding to this MachineType and Class.  Ensures
        /// that the newly created Machine has all the properties due it from the Machine.
        /// </summary>
        /// <returns></returns>
        public Machine CreateMachine()
        {
            ClassDef machineClassDef = GetMachineClassDef();
            var machine = new Machine(machineClassDef) {MachineType = this};

            foreach (MachinePropertyDef machinePropertyDef in MachinePropertyDefs)
            {
                MachineProperty property = machine.MachineProperties.CreateBusinessObject();
                property.MachinePropertyDef = machinePropertyDef;
            }
            return machine;
        }
    }
}