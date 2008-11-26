
// ------------------------------------------------------------------------------
// This class was auto-generated for use with the Habanero Enterprise Framework.
// ------------------------------------------------------------------------------

using System.Collections;
using Habanero.Base;
using Habanero.BO.ClassDefinition;

namespace MachineExample.BO
{
    using System;
    using Habanero.BO;
    
    
    public partial class MachineType
    {
        public ClassDef GetMachineClassDef()
        {
            string machineTypeClassDefName = "Machine_" + GetMachineTypeName();
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

        private string GetMachineTypeName()
        {
            return this.Name;
        }

        private ClassDef CreateNewMachineClassDef()
        {
            ClassDef baseMachineClassDef = ClassDef.ClassDefs[typeof(Machine)];
            ClassDef machineClassDef = baseMachineClassDef.Clone();
            machineClassDef.TypeParameter = GetMachineTypeName();

            UIDef uiDef = machineClassDef.UIDefCol["default"];
            UIGrid uiGrid = uiDef.UIGrid;
            UIForm form = uiDef.UIForm;
            form.Title = "Add/Edit a Machine";
            UIFormTab tab = form[0];

            UIFormColumn column = tab[0];
            foreach (MachinePropertyDef machinePropertyDef in MachinePropertyDefs)
            {
                PropDef propertyDefPropDef = new PropDef(machinePropertyDef.PropertyName, "System", machinePropertyDef.PropertyType,
                                                         PropReadWriteRule.ReadWrite, "", null, machinePropertyDef.IsCompulsory.Value, false);
                propertyDefPropDef.Persistable = false;
                machineClassDef.PropDefcol.Add(propertyDefPropDef);

                UIFormField uiProperty = 
                    new UIFormField(null, machinePropertyDef.PropertyName, "TextBox", "System.Windows.Forms", "", "", true, "", new Hashtable(), null);
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

        public  Machine CreateMachine()
        {
            ClassDef machineClassDef = GetMachineClassDef();
            Machine newMachine = CreateMachineWithClassDef(machineClassDef);
           
            return newMachine;
        }

        private Machine CreateMachineWithClassDef(ClassDef machineClassDef)
        {
            Machine machine = new Machine(machineClassDef) {MachineType = this};

            foreach (MachinePropertyDef machinePropertyDef in this.MachinePropertyDefs)
            {
                MachineProperty property = machine.MachineProperties.CreateBusinessObject();
                property.MachinePropertyDef = machinePropertyDef;
            }
            return machine;
        }
    }
}
