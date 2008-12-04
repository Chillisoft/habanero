/// This code is copyright under the terms of the Code Project Open License
/// (C) 2008 Peter Wiles/Chillisoft Solution Services (Pty) Ltd

using System;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;

namespace MachineExample.BO
{
    public partial class Machine
    {
        public Machine(ClassDef def) : base(def)
        {
        }

        public Machine()
        {
        }

        protected override void AfterLoad()
        {
            base.AfterLoad();
            ClassDef = MachineType.GetMachineClassDef();
            foreach (MachineProperty property in MachineProperties)
            {
                string propertyName = property.MachinePropertyDef.PropertyName;
                if (!Props.Contains(propertyName))
                {
                    IBOProp newBOProp = new BOProp(ClassDef.PropDefcol[propertyName]);
                    Props.Add(newBOProp);
                }
                SetPropertyValue(propertyName, property.Value);
            }
            foreach (PropDef propDef in ClassDef.PropDefcol)
            {
                if (!Props.Contains(propDef.PropertyName))
                {
                    IBOProp newBOProp = new BOProp(ClassDef.PropDefcol[propDef.PropertyName]);
                    Props.Add(newBOProp);
                }
            }
        }

        protected override void UpdateObjectBeforePersisting(ITransactionCommitter transactionCommitter)
        {
            SetTransactionLog(new TransactionLogTable(this, "asset_transaction_log"));
            base.UpdateObjectBeforePersisting(transactionCommitter);

            foreach (MachineProperty machineProperty in MachineProperties.CreatedBusinessObjects)
            {
                SynchroniseAssetAttributeAndProperty(transactionCommitter, machineProperty);
            }
            foreach (MachineProperty machineProperty in MachineProperties)
            {
                SynchroniseAssetAttributeAndProperty(transactionCommitter, machineProperty);
            }
        }

        private void SynchroniseAssetAttributeAndProperty(ITransactionCommitter transactionCommitter,
                                                          MachineProperty machineProperty)
        {
            string newValue = Convert.ToString(GetPropertyValue(machineProperty.MachinePropertyDef.PropertyName));
            machineProperty.Value = String.IsNullOrEmpty(newValue) ? null : newValue;
            transactionCommitter.AddBusinessObject(machineProperty);
            SetPropertyValue(machineProperty.MachinePropertyDef.PropertyName, machineProperty.Value);
        }
    }
}