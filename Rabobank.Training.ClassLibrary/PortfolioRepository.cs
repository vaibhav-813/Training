// <copyright file="PortfolioRepository.cs" company="Rabobank">
//  Copyright (c) 2022 Rabobank Training
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Rabobank.Training.ClassLibrary
{
    using Constants;
    using Data;
    using Interface;
    using ViewModels;

    /// <summary>
    /// PortfolioRepository class contains methods to perform crud operations on Portfolio and Mandates object.
    /// </summary>
    public class PortfolioRepository : IPortfolioRepository
    {
        /// <summary>
        /// Private readonly logger to be initialized within the constructor.
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Private readonly string to be initialized within the constructor.
        /// </summary>
        private readonly string _xmlFileBasePath;

        /// <summary>
        /// Private readonly bool to be initialized within the constructor for getting the base path for XML file.
        /// </summary>
        private readonly bool _isTestService;

        /// <summary>
        /// Constructor to initialize class dependencies.
        /// </summary>
        /// <param name="logger">ILogger object</param>
        public PortfolioRepository(ILogger logger)
        {
            _logger = logger;
            var builder = new ConfigurationBuilder()
                .AddJsonFile(ServiceConstants.ConfigurationFileName, false, true);
            var configuration = builder.Build();
            _xmlFileBasePath = configuration.GetSection(ServiceConstants.XMLDataFilePath).Value;
            _isTestService = Convert.ToBoolean(configuration.GetSection(ServiceConstants.IsTestService).Value ?? false.ToString());
        }

        /// <summary>
        /// Gets the details of list of FundOfMandatesData.
        /// </summary>
        /// <param name="fileName">XML filename from which data needs to be read.</param>
        /// <returns>List of all FundofMandates</returns>
        public List<FundOfMandates> GetFundOfMandates(string fileName)
        {
            List<FundOfMandates> fundOfMandates = null;
            try
            {
                var filePath = _isTestService
                    ? _xmlFileBasePath + fileName
                    : AppDomain.CurrentDomain.BaseDirectory + fileName;

                if (File.Exists(filePath))
                {
                    var serializer = new XmlSerializer(typeof(FundsOfMandatesData));
                    using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
                    {
                        var fundsOfMandatesData = (FundsOfMandatesData)serializer.Deserialize(fileStream);

                        fundOfMandates = fundsOfMandatesData != null
                            ? fundsOfMandatesData.FundsOfMandates : null;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return fundOfMandates;
        }

        /// <summary>
        /// Loads the Portfolio Viewmodel object which is displayed on UI.
        /// Creates a static Portfolio object containing a list of static positions.
        /// Gets the list of FundOfMandates by reading the XML file.
        /// Calculates the mandate value and allocation for each position using the FundOfMandates value and formula.
        /// </summary>
        /// <returns>Returns the Portfolio ViewModel along with Postions and Mandates.</returns>
        public PortfolioVM LoadPortfolio()
        {
            var portfolioVM = new PortfolioVM();
            try
            {
                portfolioVM = GetPortfolio();

                var fundOfMandates = GetFundOfMandates(ServiceConstants.XMLFileName);

                portfolioVM.Positions.ForEach(positionVM =>
                {
                    CalculateMandate(positionVM, fundOfMandates);
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return portfolioVM;
        }

        /// <summary>
        /// Generates a PortfolioVM containing a list of Positions by deserializing 
        /// the Json constant containing an array of positions.
        /// </summary>
        /// <returns>Static PortfolioVM</returns>
        public PortfolioVM GetPortfolio()
        {
            var portfolioVM = new PortfolioVM();
            try
            {
                portfolioVM.Positions = JsonConvert.DeserializeObject<List<PositionVM>>(ServiceConstants.PositionsJson);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return portfolioVM;
        }

        /// <summary>
        /// Calculates the Mandate for the position details passed as input.
        /// </summary>
        /// <param name="positionVM">Details of a position in a portfolio</param>
        /// <param name="fundOfMandates">List of fundOfMandates</param>
        /// <returns>Position VM after calculating the mandate.</returns>
        public PositionVM CalculateMandate(PositionVM positionVM, List<FundOfMandates> fundOfMandates)
        {
            try
            {
                //Getting the FundOfMandate for the Position under execution
                var fundOfMandate = fundOfMandates.Where(prop => prop.InstrumentCode.Equals(
                    positionVM.Code, StringComparison.InvariantCultureIgnoreCase))?.FirstOrDefault();

                if (fundOfMandate != null)
                {
                    positionVM.Mandates = new List<MandateVM>();

                    var totalAllocationForAllMandates = fundOfMandate.Mandates.ToList().Sum(prop => prop.Allocation);
                    //Adding additional check to avoid adding bad data to portfolio positions if any.
                    if (totalAllocationForAllMandates <= 100)
                    {
                        //For each Mandate in FundOfMandate adding the Mandate to Position
                        //after calculating the Mandate allocation and value.
                        fundOfMandate.Mandates.ForEach(mandate =>
                        {
                            var calculatedMandate = CalculateMandateAllocationAndValue(mandate, positionVM);
                            if (calculatedMandate != null)
                                positionVM.Mandates.Add(calculatedMandate);
                        });

                        //Adding a new Mandate with Name as Liquidity if Liquidity allocation is greater than 0.
                        if (fundOfMandate.LiquidityAllocation > 0)
                        {
                            var mandateForLiquidity = CalculateAllocationAndValueForEquity(positionVM, fundOfMandate.LiquidityAllocation);
                            if (mandateForLiquidity != null)
                                positionVM.Mandates.Add(mandateForLiquidity);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return positionVM;
        }

        /// <summary>
        /// Calculates the values of Mandate Allocation and Mandate Value w.r.t Value of the Position.
        /// </summary>
        /// <param name="mandate">Details of a mandate in the position for a portfolio</param>
        /// <param name="positionVM">Value of a position in a portfolio</param>
        /// <returns>The Mandate of a Position in a portfolio.</returns>
        public MandateVM CalculateMandateAllocationAndValue(Mandate mandate, PositionVM positionVM)
        {
            MandateVM mandateVm = null;
            try
            {
                mandateVm = mandate.Allocation <= 100
                    ? new MandateVM()
                    {
                        Allocation = mandate.Allocation / 100,
                        Name = mandate.MandateName,
                        Value = Math.Round((mandate.Allocation / 100) * positionVM.Value, 0, MidpointRounding.AwayFromZero)
                    }
                    : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return mandateVm;
        }

        /// <summary>
        /// Calculates the values of Equity Mandate of a Position in a portfolio.
        /// For calculating the Value of liquidity mandate we subtract the sum of all the mandates from the total value of position.
        /// </summary>
        /// <param name="positionVM">Details of a mandate in the position for a portfolio</param>
        /// <param name="liquidityAllocation">The value of Liquidity allocation in a FundOfMandates</param>
        /// <returns>The Equity Mandate of a Position in a portfolio.</returns>
        public MandateVM CalculateAllocationAndValueForEquity(PositionVM positionVM, decimal liquidityAllocation)
        {
            MandateVM liquidityMandateVM = null;
            try
            {
                var mandateValueForLiquidity = positionVM.Value - positionVM.Mandates.Sum(prop => prop.Value);
                liquidityMandateVM = new MandateVM()
                {
                    Allocation = liquidityAllocation / 100,
                    Name = ServiceConstants.Liquidity,
                    Value = mandateValueForLiquidity
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return liquidityMandateVM;
        }
    }
}
