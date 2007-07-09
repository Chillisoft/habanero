using System;
using System.Collections.Generic;
using System.Text;

namespace Habanero.Bo
{
    ///<summary>
    ///</summary>
    public class BOPropEventArgs: EventArgs
    {

        private readonly BOProp _prop;

        public BOPropEventArgs(BOProp prop)
        {
            _prop = prop;
        }

        public BOProp Prop
        {
            get { return _prop; }
        }

    }
}
