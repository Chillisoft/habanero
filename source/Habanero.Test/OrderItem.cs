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

using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;
using Habanero.DB;

namespace Habanero.Test
{
    /// <summary>
    /// This is a simple composite key class.  The database table must be
    /// created at run-time and dropped when done.
    /// </summary>
    public class OrderItem : BusinessObject
    {
        public static void CreateTable()
        {
            string sqlCreate = "DROP TABLE IF EXISTS `orderitem`; " +
                               "CREATE TABLE `orderitem` " +
                               "(`OrderNumber` int(10) unsigned NOT NULL default '0', " +
                               "`Product` varchar(100) NOT NULL default '', " +
                               "PRIMARY KEY  (`OrderNumber`,`Product`)) " +
                               "ENGINE=InnoDB DEFAULT CHARSET=utf8;;";
            DatabaseConnection.CurrentConnection.ExecuteRawSql(sqlCreate);
        }

        public static void DropTable()
        {
            string sqlDrop = "DROP TABLE IF EXISTS `orderitem`;";
            DatabaseConnection.CurrentConnection.ExecuteRawSql(sqlDrop);
        }

        public static ClassDef LoadDefaultClassDef()
        {
            XmlClassLoader itsLoader = new XmlClassLoader();
            ClassDef classDef = itsLoader.LoadClass(@"
				<class name=""OrderItem"" assembly=""Habanero.Test"">
					<property name=""OrderNumber"" type=""int"" compulsory=""true"" />
					<property name=""Product"" compulsory=""true"" />
					<primaryKey isObjectID=""false"" >
						<prop name=""OrderNumber"" />
                        <prop name=""Product"" />
					</primaryKey>
					<ui>
						<grid>
							<column property=""OrderNumber"" />
							<column property=""Product"" />
						</grid>
						<form>
							<field property=""OrderNumber"" />
							<field property=""Product"" />
						</form>
					</ui>
				</class>
			");
			ClassDef.ClassDefs.Add(classDef);
			return classDef;
        }

        public static OrderItem AddOrder1Car()
        {
            OrderItem item = new OrderItem();
            item.OrderNumber = 1;
            item.Product = "car";
            item.Save();
            return item;
        }

        public static OrderItem AddOrder2Chair()
        {
            OrderItem item = new OrderItem();
            item.OrderNumber = 2;
            item.Product = "chair";
            item.Save();
            return item;
        }

        public static OrderItem AddOrder3Roof()
        {
            OrderItem item = new OrderItem();
            item.OrderNumber = 3;
            item.Product = "roof";
            item.Save();
            return item;
        }

        public static void ClearTable()
        {
            BusinessObjectCollection<OrderItem> col = new BusinessObjectCollection<OrderItem>();
            col.LoadAll();
            foreach (OrderItem item in col)
            {
                item.Delete();
            }
            col.SaveAll();
        }
    
        public int OrderNumber
        {
            get { return (int) GetPropertyValue("OrderNumber"); }
            set { SetPropertyValue("OrderNumber", value); }
        }

        public string Product
        {
            get { return (string)GetPropertyValue("Product"); }
            set { SetPropertyValue("Product", value); }
        }

        public override string ToString()
        {
            return OrderNumber + " - " + Product;
        }
    }
}