using System;
using System.Collections.Generic;
using System.Text;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;

namespace Habanero.Test
{
    public class MultiPropBO : BusinessObject
    {
        private static ClassDef _newClassDef;

        protected override ClassDef ConstructClassDef()
        {
            return _newClassDef;
        }

        public static ClassDef LoadClassDef()
        {
            XmlClassLoader itsLoader = new XmlClassLoader();
            _newClassDef = itsLoader.LoadClass(@"
			<class name=""MultiPropBO"" assembly=""Habanero.Test"">
                <property name=""MultiPropBOID"" type=""Guid"" />
                <property name=""DateTimeProp"" type=""DateTime"" />
				<property name=""StringProp"" />
                <property name=""IntProp"" type=""int"" />
				<property name=""GuidProp"" type=""Guid"" />
                <property name=""DoubleProp"" type=""double"" />
                <property name=""SingleProp"" type=""Single"" />
                <property name=""TimeSpanProp"" type=""TimeSpan"" />
				<primaryKey>
					<prop name=""MultiPropBOID"" />
				</primaryKey>
			</class>
		    ");
            ClassDef.ClassDefs.Add(_newClassDef);
            return _newClassDef;
        }

        public Guid? MultiPropBOID
        {
            get { return (Guid?) GetPropertyValue("MultiPropBOID"); }
            set { SetPropertyValue("MultiPropBOID", value); }
        }

        public DateTime? DateTimeProp
        {
            get { return (DateTime?)GetPropertyValue("DateTimeProp"); }
            set { SetPropertyValue("DateTimeProp", value); }
        }

        public String StringProp
        {
            get { return (String)GetPropertyValue("StringProp"); }
            set { SetPropertyValue("StringProp", value); }
        }

        public Int32? IntProp
        {
            get { return (Int32?)GetPropertyValue("IntProp"); }
            set { SetPropertyValue("IntProp", value); }
        }

        public Guid? GuidProp
        {
            get { return (Guid?)GetPropertyValue("GuidProp"); }
            set { SetPropertyValue("GuidProp", value); }
        }

        public Double? DoubleProp
        {
            get { return (Double?)GetPropertyValue("DoubleProp"); }
            set { SetPropertyValue("DoubleProp", value); }
        }

        public Single? SingleProp
        {
            get { return (Single?)GetPropertyValue("SingleProp"); }
            set { SetPropertyValue("SingleProp", value); }
        }

        public TimeSpan? TimeSpanProp
        {
            get { return (TimeSpan?)GetPropertyValue("TimeSpanProp"); }
            set { SetPropertyValue("TimeSpanProp", value); }
        }
    }
}
