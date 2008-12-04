/// This code is copyright under the terms of the Code Project Open License
/// (C) 2008 Peter Wiles/Chillisoft Solution Services (Pty) Ltd

using System;
using System.Windows.Forms;
using Habanero.BO;
using Habanero.UI.Base;
using Habanero.UI.Win;
using Habanero.Base;
using MachineExample.BO;

namespace MachineExample
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            HabaneroAppTest mainApp = new HabaneroAppTest("MachineExample", "v1.0");
            if (!mainApp.Startup()) return;

            SetupTestDataInMemory();

            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new ProgramForm());
            }
            catch (Exception ex)
            {
                GlobalRegistry.UIExceptionNotifier.Notify(ex,
					"An error has occurred in the application.",
					"Application Error");
            }
        }

        private static void SetupTestDataInMemory()
        {
            MachineType machineType1 = CreateMachineType("Roller");
            CreateMachinePropertyDef(machineType1, "Gauge In", "Int32", true);
            CreateMachinePropertyDef(machineType1, "Gauge Out", "Int32", true);

            MachineType machineType2 = CreateMachineType("Painter");
            CreateMachinePropertyDef(machineType2, "Colour 1", "String", true);
            CreateMachinePropertyDef(machineType2, "Colour 2", "String", false);
            CreateMachinePropertyDef(machineType2, "Colour 3", "String", false);
        }

        private static MachineType CreateMachineType(string name)
        {
            MachineType machineType1 = new MachineType();
            machineType1.Name = name;
            machineType1.Save();
            return machineType1;
        }

        private static void CreateMachinePropertyDef(MachineType machineType, string propertyName, string propertyType, bool isCompulsory)
        {
            MachinePropertyDef def = machineType.MachinePropertyDefs.CreateBusinessObject();
            def.PropertyName = propertyName;
            def.PropertyType = propertyType;
            def.IsCompulsory = isCompulsory;
            def.Save();
        }

        public class HabaneroAppTest : HabaneroAppWin
        {
            public HabaneroAppTest(string appName, string appVersion) : base(appName, appVersion)
            {
            }

            protected override void SetupDatabaseConnection()
            {
                BORegistry.DataAccessor = new DataAccessorInMemory();
            }
        }
    }
}