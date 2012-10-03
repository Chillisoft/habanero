using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Habanero.DB
{
    public class DBMigratorEventArgs : EventArgs
    {
        public uint StartingStep { get; protected set; }
        public uint CurrentStep { get; protected set; }
        public uint TotalSteps { get; protected set; }
        public decimal PercentageComplete { get; protected set; }
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
