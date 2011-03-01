namespace Habanero.Base
{
    public interface IHabaneroLogger
    {
        string ContextName { get; }
        void Log(string message);
        void Log(string message, LogCategory logCategory);
    }
}