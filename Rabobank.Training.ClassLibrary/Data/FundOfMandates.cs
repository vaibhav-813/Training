// <copyright file="FundOfMandates.cs" company="Rabobank">
//  Copyright (c) 2022 Rabobank Training
// </copyright>

using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Rabobank.Training.ClassLibrary.Data
{
    using Constants;

    /// <summary>
    /// FundOfMandates class contains all properties of a FundMandate.
    /// </summary>
    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = ServiceConstants.XMLNamespace)]
    public class FundOfMandates
    {
        /// <summary>
        /// Gets or sets the InstrumentCode for FundOfMandates.
        /// </summary>
        /// <value>
        /// InstrumentCode.
        /// </value>
        public string InstrumentCode { get; set; }

        /// <summary>
        /// Gets or sets the InstrumentName for FundOfMandates.
        /// </summary>
        /// <value>
        /// InstrumentName.
        /// </value>
        public string InstrumentName { get; set; }

        /// <summary>
        /// Gets or sets the LiquidityAllocation for FundOfMandates.
        /// </summary>
        /// <value>
        /// LiquidityAllocation.
        /// </value>
        public decimal LiquidityAllocation { get; set; }

        /// <summary>
        /// Gets or sets the Mandates for FundOfMandates.
        /// </summary>
        /// <value>
        /// List of Mandates.
        /// </value>
        [XmlArrayItem(nameof(Mandate), IsNullable = false)]
        public List<Mandate> Mandates { get; set; }
    }
}
