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