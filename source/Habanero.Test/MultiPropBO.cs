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
