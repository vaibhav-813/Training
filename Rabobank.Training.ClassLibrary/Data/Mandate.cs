// <copyright file="Mandate.cs" company="Rabobank">
//  Copyright (c) 2022 Rabobank Training
// </copyright>

using System;
using System.Xml.Serialization;

namespace Rabobank.Training.ClassLibrary.Data
{
    using Constants;

    /// <summary>
    /// Mandate class contains all properties of a Mandate.
    /// </summary>
    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = ServiceConstants.XMLNamespace)]
    public class Mandate
    {
        /// <summary>
        /// Gets or sets the MandateId for Mandate.
        /// </summary>
        /// <value>
        /// MandateId.
        /// </value>
        public string MandateId { get; set; }

        /// <summary>
        /// Gets or sets the MandateName for Mandate.
        /// </summary>
        /// <value>
        /// MandateName.
        /// </value>
        public string MandateName { get; set; }

        /// <summary>
        /// Gets or sets the Allocation for Mandate.
        /// </summary>
        /// <value>
        /// Allocation.
        /// </value>
        public decimal Allocation { get; set; }
    }
}