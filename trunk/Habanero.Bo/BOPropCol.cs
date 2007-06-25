using System;
using System.Collections;
using System.Text;
using Habanero.Bo.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Bo
{
    /// <summary>
    /// Manages a collection of BOProp objects
    /// </summary>
    public class BOPropCol : DictionaryBase
    {
        /// <summary>
        /// Constructor to initialise a new empty collection
        /// </summary>
        internal BOPropCol() : base()
        {
        }

        /// <summary>
        /// Adds a property to the collection
        /// </summary>
        /// <param name="prop">The property to add</param>
        internal void Add(BOProp prop)
        {
            //TODO_Err: Add sensible error handling if prop already exists etc
            base.Dictionary.Add(prop.PropertyName.ToUpper(), prop);
        }

        /// <summary>
        /// Copies the properties from another collection into this one
        /// </summary>
        /// <param name="propCol">A collection of properties</param>
        internal void Add(BOPropCol propCol)
        {
            foreach (BOProp prop in propCol.Values)
            {
                this.Add(prop);
            }
        }

        /// <summary>
        /// Remove a specified property from the collection
        /// </summary>
        /// <param name="propName">The property name</param>
        internal void Remove(string propName)
        {
            base.Dictionary.Remove(propName.ToUpper());
        }

        /// <summary>
        /// Indicates whether the collection contains the property specified
        /// </summary>
        /// <param name="propName">The property name</param>
        /// <returns>Returns true if found</returns>
        internal bool Contains(string propName)
        {
            return (Dictionary.Contains(propName.ToUpper()));
        }

        /// <summary>
        /// Provides an indexing facility so the contents of the collection
        /// can be accessed with square brackets like an array
        /// </summary>
        /// <param name="propName">The name of the property to access</param>
        /// <returns>Returns the property if found, or null if not</returns>
        public BOProp this[string propName]
        {
            get
            {
                //TODOErr: put appropriate err handling
                return ((BOProp) Dictionary[propName.ToUpper()]);
            }
        }

        /// <summary>
        /// Returns an xml string containing the properties whose values
        /// have changed, along with their old and new values
        /// </summary>
        internal string DirtyXml
        {
            get
            {
                string dirtlyXml = "<Properties>";
                foreach (BOProp prop in this.SortedValues )
                {
                    if (prop.IsDirty)
                    {
                        dirtlyXml += prop.DirtyXml;
                    }
                }
                return dirtlyXml;
            }
        }

        /// <summary>
        /// Restores each of the property values to their PersistedValue
        /// </summary>
        internal void RestorePropertyValues()
        {
            foreach (DictionaryEntry item in this)
            {
                BOProp prop = (BOProp) item.Value;
                prop.RestorePropValue();
            }
        }

        /// <summary>
        /// Copies across each of the properties' current values to their
        /// persisted values
        /// </summary>
        internal void BackupPropertyValues()
        {
            foreach (DictionaryEntry item in this)
            {
                BOProp prop = (BOProp) item.Value;
                prop.BackupPropValue();
            }
        }

        /// <summary>
        /// Indicates whether all of the held property values are valid
        /// </summary>
        /// <param name="invalidReason">A string to alter if one or more
        /// property values are invalid</param>
        /// <returns>Returns true if all the property values are valid, false
        /// if any one is invalid</returns>
        internal bool IsValid(out string invalidReason)
        {
            bool propsValid = true;
            StringBuilder reason = new StringBuilder();
            foreach (DictionaryEntry item in this)
            {
                BOProp prop = (BOProp) item.Value;
                if (!prop.isValid)
                {
                    reason.Append(prop.InvalidReason + Environment.NewLine);
                    propsValid = false;
                }
            }
            invalidReason = reason.ToString();
            return propsValid;
        }

        /// <summary>
        /// Sets the IsObjectNew setting in each property to that specified
        /// </summary>
        /// <param name="bValue">Whether the object is set as new</param>
        internal void setIsObjectNew(bool bValue)
        {
            foreach (DictionaryEntry item in this)
            {
                BOProp prop = (BOProp) item.Value;
                prop.IsObjectNew = bValue;
            }
        }

        /// <summary>
        /// Returns a collection containing all the values being held
        /// </summary>
        public ICollection  Values
        {
            get { return base.Dictionary.Values; }
        }

        public IEnumerable SortedValues {
            get { return new SortedList(base.Dictionary ).Values ;
                }
            }
        
    }


    #region Tests

    [TestFixture]
    public class BOPropColTester
    {
        private PropDef mPropDef;
        private BOProp mProp;
        private BOPropCol mBOPropCol;

        [SetUp]
        public void init()
        {
            mBOPropCol = new BOPropCol();
            mPropDef = new PropDef("PropName", typeof (string), PropReadWriteRule.ReadOnly, null);
            mBOPropCol.Add(mPropDef.CreateBOProp(false));

            mPropDef = new PropDef("Prop2", typeof (string), PropReadWriteRule.ReadOnly, null);
            mPropDef.assignPropRule(new PropRuleString(mPropDef.PropertyName, "Test Message", 1, 10, null, null));
            mBOPropCol.Add(mPropDef.CreateBOProp(false));

            BOPropCol anotherPropCol = new BOPropCol();
            PropDef anotherPropDef =
                new PropDef("TestAddPropCol", typeof (string), PropReadWriteRule.ReadManyWriteMany, null);
            anotherPropCol.Add(anotherPropDef.CreateBOProp(false));

            mBOPropCol.Add(anotherPropCol);
        }

        [Test]
        public void TestSetBOPropValue()
        {
            mProp = mBOPropCol["Prop2"];
            mProp.PropertyValue = "Prop Value";
            Assert.AreEqual("Prop Value", mProp.PropertyValue);

            mProp = mBOPropCol["PropName"];
            mProp.PropertyValue = "Value 2";
            Assert.AreEqual("Value 2", mProp.PropertyValue);
        }

        [Test]
        public void TestPropDefColIsValid()
        {
            mProp = mBOPropCol["Prop2"];
            try
            {
                mProp.PropertyValue = "Prop Value fdfdfdf ff";
            }
            catch (PropertyValueInvalidException)
            {
            }
            mProp = mBOPropCol["PropName"];
            string reason;
            Assert.IsFalse(mBOPropCol.IsValid(out reason));
            Assert.IsTrue(reason.Length > 0);
        }

        [Test]
        public void TestAddBOPropColToBOPropCol()
        {
            Assert.AreEqual(3, mBOPropCol.Count,
                            "There should be 3 items in the BOPropCol after adding the other BOPropCol to it.");
        }

        //		[Test]
        //		public void TestPropDefColIsValid()
        //		{
        //			mProp = _boPropCol["Prop2"];
        //			try
        //			{
        //				mProp.PropertyValue = "Prop Value fdfdfdf ff";
        //			}
        //			catch (Exception e)
        //			{}
        //			mProp = _boPropCol["PropName"];
        //			string reason;
        //			Assert.IsFalse(_boPropCol.IsValid(out reason));
        //			Assert.IsTrue(reason.Length > 0);
        //			
        //		}
        [Test]
        public void TestDirtyXml()
        {
            mProp = mBOPropCol["Prop2"];
            mProp.InitialiseProp("Prop2-Orig");
            mProp.PropertyValue = "Prop2-New";
            Assert.IsTrue(mProp.IsDirty);

            mProp = mBOPropCol["PropName"];
            mProp.InitialiseProp("Propn-Orig");
            mProp.PropertyValue = "PropName-new";
            Assert.IsTrue(mProp.IsDirty);

            mPropDef = new PropDef("Prop3", typeof (string), PropReadWriteRule.ReadOnly, null);
            mPropDef.assignPropRule(new PropRuleString(mPropDef.PropertyName, "Test", 1, 40, null, null));
            mBOPropCol.Add(mPropDef.CreateBOProp(false));
            mProp = mBOPropCol["Prop3"];
            mProp.InitialiseProp("Prop3-new");
            Assert.IsFalse(mProp.IsDirty);
            string dirtyXml =
                "<Properties><Prop2><PreviousValue>Prop2-Orig</PreviousValue><NewValue>Prop2-New</NewValue></Prop2><PropName><PreviousValue>Propn-Orig</PreviousValue><NewValue>PropName-new</NewValue></PropName>";
            Assert.AreEqual(dirtyXml, mBOPropCol.DirtyXml);
        }

        [Test]
        public void TestRemove()
        {
            PropDef propDef = new PropDef("Prop3", typeof (string), PropReadWriteRule.ReadOnly, null);
            BOPropCol propCol = new BOPropCol();
            propCol.Add(propDef.CreateBOProp(false));
            Assert.AreEqual(1, propCol.Count);
            Assert.IsTrue(propCol.Contains("Prop3"), "BOPropCol should contain Prop3 after adding it.");
            propCol.Remove("Prop3");
            Assert.AreEqual(0, propCol.Count, "Remove should remove a BOProp from a BOPropCol");
        }
    }

    #endregion //Tests

}
