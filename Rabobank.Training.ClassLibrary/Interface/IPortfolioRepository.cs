// <copyright file="IPortfolioRepository.cs" company="Rabobank">
//  Copyright (c) 2022 Rabobank Training
// </copyright>

using System.Collections.Generic;

namespace Rabobank.Training.ClassLibrary.Interface
{
    using Data;
    using ViewModels;

    /// <summary>
    /// PortfolioRepository Interface will hold the methods and property defintions that needs
    /// to be implemented by the class that implements the following Interface.
    /// </summary>
    public interface IPortfolioRepository
    {
        /// <summary>
        /// Gets the details of list of FundOfMandatesData.
        /// </summary>
        /// <param name="fileName">XML filename from which data needs to be read.</param>
        /// <returns>List of all FundofMandates</returns>
        List<FundOfMandates> GetFundOfMandates(string fileName);

        /// <summary>
        /// Generates a PortfolioVM containing a list of Positions by deserializing 
        /// the Json constant containing an array of positions.
        /// </summary>
        /// <returns>Static PortfolioVM</returns>
        PortfolioVM GetPortfolio();

        /// <summary>
        /// Loads the Portfolio Viewmodel object which is displayed on UI.
        /// Creates a static Portfolio object containing a list of static positions.
        /// Gets the list of FundOfMandates by reading the XML file.
        /// Calculates the mandate value and allocation for each position using the FundOfMandates value and formula.
        /// </summary>
        /// <returns>Returns the Portfolio ViewModel along with Postions and Mandates.</returns>
        PortfolioVM LoadPortfolio();

        /// <summary>
        /// Calculates the Mandate for the position details passed as input.
        /// </summary>
        /// <param name="positionVM">Details of a position in a portfolio</param>
        /// <param name="fundOfMandates">List of fundOfMandates</param>
        /// <returns>Position VM after calculating the mandate.</returns>
        PositionVM CalculateMandate(PositionVM positionVM, List<FundOfMandates> fundOfMandates);

        /// <summary>
        /// Calculates the values of Mandate Allocation and Mandate Value w.r.t Value of the Position.
        /// </summary>
        /// <param name="mandate">Details of a mandate in the position for a portfolio</param>
        /// <param name="positionVM">Value of a position in a portfolio</param>
        /// <returns>The Mandate of a Position in a portfolio.</returns>
        MandateVM CalculateMandateAllocationAndValue(Mandate mandate, PositionVM positionVM);

        /// <summary>
        /// Calculates the values of Equity Mandate of a Position in a portfolio.
        /// For calculating the Value of liquidity mandate we subtract the sum of all the mandates from the total value of position.
        /// </summary>
        /// <param name="positionVM">Details of a mandate in the position for a portfolio</param>
        /// <param name="liquidityAllocation">The value of Liquidity allocation in a FundOfMandates</param>
        /// <returns>The Equity Mandate of a Position in a portfolio.</returns>
        MandateVM CalculateAllocationAndValueForEquity(PositionVM positionVM, decimal liquidityAllocation);
    }
}
