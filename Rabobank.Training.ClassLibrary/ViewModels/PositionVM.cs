// <copyright file="PositionVM.cs" company="Rabobank">
//  Copyright (c) 2022 Rabobank Training
// </copyright>

using System.Collections.Generic;

namespace Rabobank.Training.ClassLibrary.ViewModels
{
    /// <summary>
    /// PositionVM class contains all properties of a PositionVM i.e Code, Name, Value & List of MAndates, etc.
    /// </summary>
    public class PositionVM
    {
        /// <summary>
        /// Gets or sets the Code for PositionVM.
        /// </summary>
        /// <value>
        /// Code.
        /// </value>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the Name for PositionVM.
        /// </summary>
        /// <value>
        /// Name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Value for PositionVM.
        /// </summary>
        /// <value>
        /// Value.
        /// </value>
        public decimal Value { get; set; }

        /// <summary>
        /// Gets or sets the Mandates for PositionVM.
        /// </summary>
        /// <value>
        /// List of Mandates.
        /// </value>
        public List<MandateVM> Mandates { get; set; }
    }
}
