// <copyright file="PortfolioControllerUnitTest.cs" company="Rabobank">
//  Copyright (c) 2022 Rabobank Training
// </copyright>

using System.Collections.Generic;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Rabobank.Training.WebApp.Tests
{
    using ClassLibrary.Interface;
    using ClassLibrary.ViewModels;
    using Controllers;

    /// <summary>
    /// PortfolioControllerUnitTest class contains all unit test cases for PortfolioController
    /// </summary>
    [TestClass]
    public class PortfolioControllerUnitTest
    {
        /// <summary>
        /// Private field to initialize Mock IPortfolioRepository.
        /// </summary>
        private readonly Mock<IPortfolioRepository> _mockPortfolioRepository;

        /// <summary>
        /// Private field to initialize Mock ILogger.
        /// </summary>
        private readonly Mock<ILogger<PortfolioController>> _mockLogger;

        /// <summary>
        /// Private field to initialize PortfolioController.
        /// </summary>
        private readonly PortfolioController _portfolioController;

        /// <summary>
        /// Constructor to inject/initialize dependencies.
        /// </summary>
        public PortfolioControllerUnitTest()
        {
            _mockLogger = new Mock<ILogger<PortfolioController>>();
            _mockPortfolioRepository = new Mock<IPortfolioRepository>();
            _portfolioController = new PortfolioController(_mockLogger.Object, _mockPortfolioRepository.Object);
        }

        /// <summary>
        /// TestCase to validate the portfolio details loads successfully using Get method of PortfolioController.
        /// </summary>
        [TestMethod]
        [TestCategory("happy-path")]
        public void Get_Portfolio_Loads_Successfully()
        {
            //Arrange 
            var mockMandateVM1 = new MandateVM
            {
                Name = "Test PO-01",
                Allocation = 0.5M,
                Value = 12340
            };
            var mockMandateVM2 = new MandateVM
            {
                Name = "Test PO-02",
                Allocation = 0.5M,
                Value = 12340
            };
            var mockPositionVM = new PositionVM
            {
                Code = "TestCode001",
                Name = "Test PO",
                Value = 24680,
                Mandates = new List<MandateVM>() { mockMandateVM1, mockMandateVM2 }
            };
            var mockPortfolioVM = new PortfolioVM() { Positions = new List<PositionVM>() { mockPositionVM } };

            _mockPortfolioRepository.Setup(config => config.LoadPortfolio()).Returns(mockPortfolioVM);

            //Act
            var portfolio = _portfolioController.Get();

            //Assert
            portfolio.Should().NotBeNull();
            portfolio.Should().BeOfType<PortfolioVM>();
            portfolio.Positions.Should().NotBeNullOrEmpty();
            portfolio.Positions.Should().BeOfType<List<PositionVM>>();
            portfolio.Positions.Should().OnlyContain(prop => prop.Mandates != null);
            portfolio.Positions.Should().OnlyContain(prop => prop.Name != null);
            portfolio.Positions.Should().OnlyContain(prop => prop.Value >= 0);
            portfolio.Positions.Should().OnlyContain(prop => prop.Code != null);
        }

        /// <summary>
        /// TestCase to validate  Get method of PortfolioController, which return null when repository method fails.
        /// </summary>
        [TestMethod]
        [TestCategory("happy-path")]
        public void Get_Portfolio_Returns_Null_OnFailure()
        {
            //Arrange
            PortfolioVM mockPortfolioVM = null;
            _mockPortfolioRepository.Setup(config => config.LoadPortfolio()).Returns(mockPortfolioVM);

            //Act
            var portfolio = _portfolioController.Get();

            //Assert
            portfolio.Should().BeNull();
        }
    }
}
