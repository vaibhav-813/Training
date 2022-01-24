// <copyright file="PortfolioController.cs" company="Rabobank">
//  Copyright (c) 2022 Rabobank Training
// </copyright>

using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Rabobank.Training.WebApp.Controllers
{
    using ClassLibrary.Interface;
    using ClassLibrary.ViewModels;

    /// <summary>
    /// PortfolioController class inherits ControllerBase.
    /// Provides API endpoints Get Portfolio details from the server.
    /// </summary>
    /// <seealso cref="ControllerBase" />
    [ApiController]
    [Route("[controller]")]
    public class PortfolioController : ControllerBase
    {
        /// <summary>
        /// Private field of ILogger.
        /// </summary>
        private readonly ILogger<PortfolioController> _logger;

        /// <summary>
        /// Private field of IPortfolioRepository.
        /// </summary>
        private readonly IPortfolioRepository _portfolioRepository;

        /// <summary>
        /// Constructor to inject dependencies.
        /// </summary>
        /// <param name="logger">Logger instance</param>
        /// <param name="portfolioRepository">Instance of PortfolioRepository class.</param>
        public PortfolioController(ILogger<PortfolioController> logger, IPortfolioRepository portfolioRepository)
        {
            _logger = logger;
            _portfolioRepository = portfolioRepository;
        }

        /// <summary>
        /// API End point to get portfolio details from the server.
        /// </summary>
        /// <returns>Protfolio ViewModel</returns>
        [HttpGet]
        public PortfolioVM Get()
        {
            PortfolioVM portfolio = null;
            try
            {
                portfolio = _portfolioRepository.LoadPortfolio();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return portfolio;
        }
    }
}
