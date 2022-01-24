// <copyright file="MandateVM.cs" company="Rabobank">
//  Copyright (c) 2022 Rabobank Training
// </copyright>

namespace Rabobank.Training.ClassLibrary.ViewModels
{
    /// <summary>
    /// MandateVM class contains all properties of a MandateVM i.e List of Positions, etc.
    /// </summary>
    public class MandateVM
    {
        /// <summary>
        /// Gets or sets the Name for MandateVM.
        /// </summary>
        /// <value>
        /// Name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Allocation for MandateVM.
        /// </summary>
        /// <value>
        /// Allocation.
        /// </value>
        public decimal Allocation { get; set; }

        /// <summary>
        /// Gets or sets the Value for MandateVM.
        /// </summary>
        /// <value>
        /// Value.
        /// </value>
        public decimal Value { get; set; }
    }
}
