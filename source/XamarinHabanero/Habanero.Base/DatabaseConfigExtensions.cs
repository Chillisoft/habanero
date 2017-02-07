using System;

namespace Habanero.Base
{
    /// <summary>
    /// This class exposes extension methods for operations on a Database Config class.
    /// </summary>
    public static class DatabaseConfigExtensions
    {
        /// <summary>
        /// Returns true if the Database Config has an in-memory configuration otherwise it returns false.
        /// Generally this will return true when the vendor contains the word 'memory'.
        /// </summary>
        /// <param name="databaseConfig">The Database Config to check.</param>
        /// <returns>true if the Database Config has an in-memory configuration</returns>
        public static bool IsInMemoryDB(this IDatabaseConfig databaseConfig)
        {
            var vendor = databaseConfig.Vendor;
            return String.IsNullOrEmpty(vendor) || vendor.ToLower().Contains("memory");
        }
    }
}