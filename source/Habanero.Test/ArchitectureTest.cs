using Habanero.Base;
using Habanero.Base.Exceptions;

namespace Habanero.Test
{
    /// <summary>
    /// Summary description for ArchitectureTest.
    /// </summary>
    public class ArchitectureTest
    {
        public ArchitectureTest()
        {
            //GlobalRegistry.SynchronisationController = new NullSynchronisationController();
            GlobalRegistry.UIExceptionNotifier = new ConsoleExceptionNotifier();
        }
    }
}