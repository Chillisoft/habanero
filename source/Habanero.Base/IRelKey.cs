using System.Collections.Generic;

namespace Habanero.Base
{
    public interface IRelKey : IEnumerable<IRelProp>
    {
        int Count
        {
            get;
        }

        /// <summary>
        /// Indexes the array of relprops this relkey contains.
        /// </summary>
        /// <param name="index">The position of the relprop to get</param>
        /// <returns>Returns the RelProp object found with that name</returns>
        IRelProp this[int index]
        {
            get;
        }
    }
}