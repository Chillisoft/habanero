using System;
using System.Collections.Generic;
using System.Text;
using Habanero.BO.ClassDefinition;

namespace Habanero.UI.Base
{
    public interface IStaticDataEditor: IControlHabanero
    {
        void AddSection(string sectionName);
        void AddItem(string itemName, ClassDef classDef);
        void SelectItem(string itemName);
        bool SaveChanges();
        bool RejectChanges();
    }
}
