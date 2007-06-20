using Habanero.Generic;

namespace Chillisoft.Test
{
    /// <summary>
    /// Summary description for ArchitectureTest.
    /// </summary>
    public class ArchitectureTest
    {
        public ArchitectureTest()
        {
            GlobalRegistry.SynchronisationController = new NullSynchronisationController();
            GlobalRegistry.UIExceptionNotifier = new ConsoleExceptionNotifier();
        }
    }
}