namespace Habanero.Base
{
    ///<summary>
    ///</summary>
    public interface IHabaneroLoggerFactory
    {
        ///<summary>
        ///</summary>
        ///<param name="contextName"></param>
        ///<returns></returns>
        IHabaneroLogger GetLogger(string contextName);
    }
}