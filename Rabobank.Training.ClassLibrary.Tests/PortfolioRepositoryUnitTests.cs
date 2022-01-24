// <copyright file="PortfolioRepositoryUnitTests.cs" company="Rabobank">
//  Copyright (c) 2022 Rabobank Training
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Rabobank.Training.ClassLibrary.Tests
{
    using Constants;
    using Data;
    using Interface;
    using ViewModels;

    /// <summary>
    /// PortfolioRepositoryUnitTests class contains all unit test cases for PortfolioRepository.
    /// </summary>
    [TestClass]
    public class PortfolioRepositoryUnitTests
    {
        /// <summary>
        /// Private field to initialize IPortfolioRepository
        /// </summary>
        private readonly IPortfolioRepository _portfolioRepository;

        /// <summary>
        /// Private field to initialize ILogger
        /// </summary>
        private readonly Mock<ILogger> _mockLogger;

        /// <summary>
        /// Constructor to initialize dependencies.
        /// </summary>
        public PortfolioRepositoryUnitTests()
        {
            _mockLogger = new Mock<ILogger>();
            _portfolioRepository = new PortfolioRepository(_mockLogger.Object);
        }

        /// <summary>
        /// Testcase to validate GetFundOfMandates() when XML file exists.
        /// </summary>
        [TestMethod]
        [TestCategory("happy-path")]
        public void GetFundOfMandates_Returns_FundOfMandates_Successfully()
        {
            //Act
            var fundOfMandates = _portfolioRepository.GetFundOfMandates(ServiceConstants.XMLFileName);

            //Assert
            Assert.IsNotNull(fundOfMandates);
            fundOfMandates.Should().BeOfType<List<FundOfMandates>>();
            fundOfMandates.Should().NotBeNullOrEmpty();
            fundOfMandates.Should().OnlyContain(prop => prop.Mandates != null);
            fundOfMandates.Should().OnlyContain(prop => prop.InstrumentCode != null);
            fundOfMandates.Should().OnlyContain(prop => prop.InstrumentName != null);
            fundOfMandates.Should().OnlyContain(prop => prop.LiquidityAllocation >= 0);
        }

        /// <summary>
        /// Testcase to validate the output of GetFundOfMandates() when file does not exist.
        /// </summary>
        [TestMethod]
        [TestCategory("fault")]
        public void GetFundOfMandates_Returns_Null_IfFileNotFound()
        {
            //Act
            var fundOfMandates = _portfolioRepository.GetFundOfMandates("FundsOfMandatesData2.xml");

            //Assert
            Assert.IsNull(fundOfMandates);
            fundOfMandates.Should().BeNull();
        }

        /// <summary>
        /// TestCase to validate if Portfolio object is loaded correctlt with positions
        /// after deserializing the static Json string.
        /// </summary>
        [TestMethod]
        [TestCategory("happy-path")]
        public void GetPortfolio_Returns_Portfolio_Successfully()
        {
            //Act
            var fundOfMandates = _portfolioRepository.GetPortfolio();

            //Assert
            Assert.IsNotNull(fundOfMandates);
            fundOfMandates.Should().NotBeNull();
            fundOfMandates.Should().BeOfType<PortfolioVM>();
            fundOfMandates.Positions.Should().NotBeNullOrEmpty();
        }

        /// <summary>
        /// TestCase to Validate the Mandate Allocation and Value Calculated for a Mandate
        /// In this test case liquidity allocation is greater than 0 hence Liquidity
        /// Mandate also needs to be added.
        /// </summary>
        [TestMethod]
        [TestCategory("happy-path")]
        public void CalculateMandate_AddsMandate_WithLiquidity_Successfully()
        {
            //Arrange
            var mockPositionVM = new PositionVM()
            {
                Code = "NL0000292332",
                Name = "Rabobank Core Aandelen Fonds T2",
                Value = 24680,
                Mandates = new List<MandateVM>()
            };
            var mockFundsOfMandates = _portfolioRepository.GetFundOfMandates(ServiceConstants.XMLFileName);

            //Act
            mockPositionVM = _portfolioRepository.CalculateMandate(mockPositionVM, mockFundsOfMandates);

            //Assert
            mockPositionVM.Should().NotBeNull();
            mockPositionVM.Mandates.Should().NotBeNullOrEmpty();
            mockPositionVM.Mandates.Should().HaveCount(mockFundsOfMandates.Where(prop => prop.InstrumentCode
                .Equals(mockPositionVM.Code, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault().Mandates.Count() + 1);
            mockPositionVM.Mandates.Should().ContainSingle(prop => prop.Name.Equals(ServiceConstants.Liquidity));
            mockPositionVM.Mandates.Sum(prop => prop.Value).Should().Be(mockPositionVM.Value);
        }

        /// <summary>
        /// TestCase to Validate the Mandate Allocation and Value Calculated for a Mandate
        /// In this test case liquidity allocation is 0 hence Liquidity Mandate should not to be added.
        /// </summary>
        [TestMethod]
        [TestCategory("happy-path")]
        public void CalculateMandate_AddsMandate_WithOutLiquidity_Successfully()
        {
            //Arrange
            var mockPositionVM = new PositionVM()
            {
                Code = "NL0000440584",
                Name = "Test Fund of Mandates",
                Value = 24680,
                Mandates = new List<MandateVM>()
            };
            var mockFundsOfMandates = _portfolioRepository.GetFundOfMandates(ServiceConstants.XMLFileName);

            //Act
            mockPositionVM = _portfolioRepository.CalculateMandate(mockPositionVM, mockFundsOfMandates);

            //Assert
            mockPositionVM.Should().NotBeNull();
            mockPositionVM.Mandates.Should().NotBeNullOrEmpty();
            mockPositionVM.Mandates.Should().HaveCount(mockFundsOfMandates.Where(prop => prop.InstrumentCode
                .Equals(mockPositionVM.Code, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault().Mandates.Count());
            mockPositionVM.Mandates.Should().NotContain(prop => prop.Name.Equals(ServiceConstants.Liquidity));
            mockPositionVM.Mandates.Sum(prop => prop.Value).Should().Be(mockPositionVM.Value);
        }

        /// <summary>
        /// TestCase to Validate that no mandate should be added to Position VM if no match found.
        /// </summary>
        [TestMethod]
        [TestCategory("fault")]
        public void CalculateMandate_DoesNotAddAnyMandate_WhenNoMatchFound()
        {
            //Arrange
            var mockPositionVM = new PositionVM()
            {
                Code = "TestCode002",
                Name = "Test Funds T2",
                Value = 24680,
                Mandates = new List<MandateVM>()
            };
            var mockFundsOfMandates = _portfolioRepository.GetFundOfMandates(ServiceConstants.XMLFileName);

            //Act
            mockPositionVM = _portfolioRepository.CalculateMandate(mockPositionVM, mockFundsOfMandates);

            //Assert
            mockPositionVM.Should().NotBeNull();
            mockPositionVM.Mandates.Should().NotBeNull();
            mockPositionVM.Mandates.Should().BeEmpty();
        }

        /// <summary>
        /// TestCase to Validate the scenarion when sum of mandate allocation is greater than 100 due to bad data
        /// In this test case liquidity allocation is 0 hence Liquidity Mandate should not to be added.
        /// </summary>
        [TestMethod]
        [TestCategory("fault")]
        public void CalculateMandate_DoesNotAddAnyMandate_When_SumOfAllocation_GreaterThan100()
        {
            //Arrange
            var mockPositionVM = new PositionVM()
            {
                Code = "NL0000440584",
                Name = "Test Fund of Mandates",
                Value = 24680,
                Mandates = new List<MandateVM>()
            };
            var mockFundsOfMandates = _portfolioRepository.GetFundOfMandates(ServiceConstants.XMLFileName);

            //Adding an additional mandate to mock data to test the scenario where sum of mandate allocation > 100 due to bad data.
            mockFundsOfMandates.Where(prop => prop.InstrumentCode.Equals(mockPositionVM.Code))
                .FirstOrDefault()?.Mandates.Add(new Mandate()
                {
                    Allocation = 5,
                    MandateId = "NL0000440584-03",
                    MandateName = "Test of Mandate 03"
                });

            //Act
            mockPositionVM = _portfolioRepository.CalculateMandate(mockPositionVM, mockFundsOfMandates);

            //Assert
            mockPositionVM.Should().NotBeNull();
            mockPositionVM.Mandates.Should().BeOfType<List<MandateVM>>();
            mockPositionVM.Mandates.Should().BeEmpty();
        }

        /// <summary>
        /// TestCase to Validate that Portfolio details loads successfully with position and calculated mandates.
        /// </summary>
        [TestMethod]
        [TestCategory("happy-path")]
        public void LoadPortfolio_Executes_Successfully()
        {
            //Act
            var portfolioVM = _portfolioRepository.LoadPortfolio();

            //Assert
            portfolioVM.Should().NotBeNull();
            portfolioVM.Positions.Should().NotBeNullOrEmpty();
            portfolioVM.Positions.Should().OnlyContain(prop => prop.Mandates != null);
        }

        /// <summary>
        /// TestCase to Validate the Calculations of Mandate Allocation and Value.
        /// </summary>
        [TestMethod]
        [TestCategory("happy-path")]
        public void CalculateMandateAllocationAndValue_Executes_Successfully()
        {
            //Arrange
            var mockMandate = new Mandate
            {
                MandateName = "Robeco Factor Momentum Mandaat",
                Allocation = 35.5M,
                MandateId = "NL0000287100-01"
            };
            var positionVM = new PositionVM
            {
                Code = "NL0000287100",
                Name = "Optimix Fund",
                Value = 23456,
                Mandates = new List<MandateVM>()
            };
            var expectedValueForMandate = Math.Round((mockMandate.Allocation / 100) * positionVM.Value, 0, MidpointRounding.AwayFromZero);
            //Act
            var mandateVM = _portfolioRepository.CalculateMandateAllocationAndValue(mockMandate, positionVM);

            //Assert
            mandateVM.Should().NotBeNull();
            mandateVM.Should().BeOfType<MandateVM>();
            mandateVM.Name.Should().Be(mockMandate.MandateName);
            mandateVM.Allocation.Should().Be(mockMandate.Allocation / 100);
            mandateVM.Value.Should().Be(expectedValueForMandate);
        }

        /// <summary>
        /// TestCase to Validate the Mandate is skipped if allocation in greater than 100 due to bad data.
        /// </summary>
        [TestMethod]
        [TestCategory("fault")]
        public void CalculateMandateAllocationAndValue_ReturnsNull_WhenAllocationGreaterThan100()
        {
            //Arrange
            var mockMandate = new Mandate
            {
                MandateName = "Robeco Factor Momentum Mandaat",
                Allocation = 100.5M,
                MandateId = "NL0000287100-01"
            };
            var positionVM = new PositionVM
            {
                Code = "NL0000287100",
                Name = "Optimix Fund",
                Value = 23456,
                Mandates = new List<MandateVM>()
            };

            //Act
            var mandateVM = _portfolioRepository.CalculateMandateAllocationAndValue(mockMandate, positionVM);

            //Assert
            mandateVM.Should().BeNull();
        }

        /// <summary>
        /// TestCase to Validate the Calculations of Mandate Allocation and Value for Equity Mandate.
        /// </summary>
        [TestMethod]
        [TestCategory("happy-path")]
        public void CalculateAllocationAndValueForEquity_Executes_Successfully()
        {
            //Arrange
            var mockMandateVM = new MandateVM()
            {
                Allocation = 0.99M,
                Name = "Robeco Factor Momentum Mandaat",
                Value = 99
            };
            var positionVM = new PositionVM
            {
                Code = "NL0000287100",
                Name = "Optimix Fund",
                Value = 100,
                Mandates = new List<MandateVM>() { mockMandateVM }
            };

            decimal liquidity = 1;
            var expectedValueForMandate = 1;

            //Act
            var mandateVM = _portfolioRepository.CalculateAllocationAndValueForEquity(positionVM, liquidity);

            //Assert
            mandateVM.Should().NotBeNull();
            mandateVM.Should().BeOfType<MandateVM>();
            mandateVM.Name.Should().Be(ServiceConstants.Liquidity);
            mandateVM.Allocation.Should().Be(liquidity / 100);
            mandateVM.Value.Should().Be(expectedValueForMandate);
        }
    }
}
