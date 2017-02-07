#region Licensing Header
// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2011 Chillisoft Solutions
//  
//  This file is part of the Habanero framework.
//  
//      Habanero is a free framework: you can redistribute it and/or modify
//      it under the terms of the GNU Lesser General Public License as published by
//      the Free Software Foundation, either version 3 of the License, or
//      (at your option) any later version.
//  
//      The Habanero framework is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//      GNU Lesser General Public License for more details.
//  
//      You should have received a copy of the GNU Lesser General Public License
//      along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
// ---------------------------------------------------------------------------------
#endregion
using System;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;

namespace Habanero.Test
{
    public class Asset : BusinessObject
    {
        public Guid? AssetID
        {
            get
            {
                return (Guid?)this.GetPropertyValue("AssetID");
            }
            set { this.SetPropertyValue("AssetID", value); }
        }
        public Guid? ParentAssetID
        {
            get
            {
                return (Guid?)this.GetPropertyValue("ParentAssetID");
            }
            set { this.SetPropertyValue("ParentAssetID", value); }
        }
        public virtual Asset Parent
        {
            get
            {
                return Relationships.GetRelatedObject<Asset>("Parent");
            }
            set
            {
                Relationships.SetRelatedObject("Parent", value);
            }
        }

        public static IClassDef LoadClassDef()
        {
            XmlClassLoader itsLoader = CreateXmlClassLoader();
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""Asset"" assembly=""Habanero.Test"">
					<property  name=""AssetID""  type=""Guid"" />
					<property  name=""ParentAssetID""  type=""Guid"" />
					<primaryKey>
						<prop name=""AssetID"" />
					</primaryKey>
                    <relationship name=""Parent"" type=""single"" relatedClass=""Asset"" relatedAssembly=""Habanero.Test"">
						<relatedProperty property=""ParentAssetID"" relatedProperty=""AssetID"" />
					</relationship>
				</class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }

        protected static XmlClassLoader CreateXmlClassLoader()
        {
            return new XmlClassLoader(new DtdLoader(), new DefClassFactory());
        }

    }
}