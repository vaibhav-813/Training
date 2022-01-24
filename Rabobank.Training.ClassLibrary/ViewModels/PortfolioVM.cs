// <copyright file="PortfolioVM.cs" company="Rabobank">
//  Copyright (c) 2022 Rabobank Training
// </copyright>

using System.Collections.Generic;

namespace Rabobank.Training.ClassLibrary.ViewModels
{
    /// <summary>
    /// PortfolioVM class contains all properties of a PortfolioVM i.e List of Positions, etc.
    /// </summary>
    public class PortfolioVM
    {
        /// <summary>
        /// Gets or sets the Positions for PortfolioVM.
        /// </summary>
        /// <value>
        /// Positions.
        /// </value>
        public List<PositionVM> Positions { get; set; }
    }
}
