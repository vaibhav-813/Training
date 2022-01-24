// <copyright file="FundsOfMandatesData.cs" company="Rabobank">
//  Copyright (c) 2022 Rabobank Training
// </copyright>

using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Rabobank.Training.ClassLibrary.Data
{
    using Constants;

    /// <summary>
    /// FundsOfMandatesData class contains all properties of a FundsOfMandatesData.
    /// </summary>
    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = ServiceConstants.XMLNamespace)]
    [XmlRoot(Namespace = ServiceConstants.XMLNamespace, IsNullable = false)]
    public class FundsOfMandatesData
    {
        /// <summary>
        /// Gets or sets the FundOfMandates for FundsOfMandatesData.
        /// </summary>
        /// <value>
        /// FundOfMandates.
        /// </value>
        [XmlArrayItem(nameof(FundOfMandates), IsNullable = false)]
        public List<FundOfMandates> FundsOfMandates { get; set; }
    }
}

