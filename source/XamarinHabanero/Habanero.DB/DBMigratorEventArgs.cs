using System;

namespace Habanero.DB
{
    /// <summary>
    /// Describes how far the DBMigrator is, for us in a progress indicator.
    /// </summary>
    public class DBMigratorEventArgs : EventArgs
    {
        /// <summary>
        /// The step started at
        /// </summary>
        public uint StartingStep { get; protected set; }
        /// <summary>
        /// The step currently being migrated
        /// </summary>
        public uint CurrentStep { get; protected set; }
        /// <summary>
        /// The total steps to be migrated
        /// </summary>
        public uint TotalSteps { get; protected set; }
        /// <summary>
        /// How far the migration is
        /// </summary>
        public decimal PercentageComplete { get; protected set; }
        /// <summary>
        /// Constructor for event args
        /// </summary>
        /// <param name="startingStep">See <see cref="StartingStep"/></param>
        /// <param name="currentStep">See <see cref="CurrentStep"/></param>
        /// <param name="totalSteps">See <see cref="TotalSteps"/></param>
        public DBMigratorEventArgs(uint startingStep, uint currentStep, uint totalSteps)
        {
            this.StartingStep = startingStep;
            this.CurrentStep = currentStep;
            this.TotalSteps = totalSteps;
            if (totalSteps > startingStep)
                this.PercentageComplete = (100M * (currentStep - startingStep + 1)) / (1 + (decimal)totalSteps - (decimal)startingStep);
            else
                this.PercentageComplete = 0;
        }
    }
}
